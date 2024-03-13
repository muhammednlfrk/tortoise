using System.ComponentModel.Design;
using System.Text.RegularExpressions;
using Tortoise.Core.Helpers;

namespace Tortoise.Core;

public sealed class Board
{
    // Mailbox board representation.
    private Mailbox _mailbox;

    // Castling rights information of the current state of the board.
    private CastlingRights _castlingRights;

    // Side to move.
    private bool _isWhiteToMove;

    // A clock for 50 move rule. (any black's moves)
    private int _halfMoveClock;

    // A counter for how many moves played. (any black's moves)
    private int _fullMoveCounter;

    // The target square index for en passant.
    private int _epIndex;

    // List of the played moves.
    private Stack<Move> _playedMoves;

    // Bitboard representations and attack maps.
    // 0: Pawn
    // 1: Knight
    // 2: Bishop
    // 3: Rook
    // 4: Queen
    // 5: King
    internal Bitboard[] _whiteBBs;
    internal Bitboard _whiteBB;
    internal Bitboard[] _blackBBs;
    internal Bitboard _blackBB;
    internal Bitboard _whiteAttackMap;
    internal Bitboard _blackAttackMap;
    internal Bitboard _occupiedBB;
    internal Bitboard _emptyBB;

    public Board(FenInfo fenInfo)
    {
        _playedMoves = new Stack<Move>();
        _whiteBBs = new Bitboard[6];
        _blackBBs = new Bitboard[6];
        _whiteAttackMap = new Bitboard();
        _blackAttackMap = new Bitboard();
        _occupiedBB = new Bitboard();
        _emptyBB = new Bitboard();
        SetPosition(fenInfo);
    }

    #region Public Methods

    public void MakeMove(Move move)
    {
        uint fromP = _mailbox[move.From] & Piece._pieceInfoMask;
        uint fromC = fromP & Piece._pieceColorMask;

        uint toP = _mailbox[move.To] & Piece._pieceInfoMask;
        uint toC = toP & Piece._pieceColorMask;

        // Check from square.
        if (fromP == Piece.None)
            throw new InvalidChessMoveException("There is no piece at from square.");

        // Check target square piece color.
        if (toP != Piece.None && fromC == toC)
            throw new InvalidChessMoveException("The target square contains friendly piece.");

        // Check side to move.
        if (!((int)fromC == Piece.White && _isWhiteToMove))
            throw new InvalidChessMoveException("Invalid side to move.");

        // Check half move clock.
        if (_halfMoveClock >= 50)
            throw new InvalidChessMoveException("The game already over. (50 move rule.)");

        // TODO: Implement the rules.
        _mailbox[move.To] = _mailbox[move.From];
        _mailbox[move.From] = Piece.None;

        // Update bitboards.
        updateBBsFromMove(move);

        _isWhiteToMove = !_isWhiteToMove;
        _fullMoveCounter++;
        _playedMoves.Push(move);
    }

    public void SetPosition(FenInfo fenInfo)
    {
        _mailbox = fenInfo.Mailbox;
        _castlingRights = fenInfo.CastlingRights;
        _isWhiteToMove = fenInfo.IsWhiteToMove;
        _halfMoveClock = fenInfo.HalfMoveClock;
        _fullMoveCounter = fenInfo.FullMoveCounter;
        _epIndex = fenInfo.EnPassantTargetSquareIndex;
        _playedMoves.Clear();

        for (int i = 0; i < 64; i++)
        {
            uint piece = _mailbox[i] & Piece._pieceInfoMask;
            if (piece == Piece.None)
                continue;

            int bbi = (int)(piece & Piece._pieceTypeMask) - 1;
            bool isWhite = (piece & Piece._pieceColorMask) == Piece.White;

            ulong pieceBB = 1ul << i;

            if (isWhite)
            {
                _whiteBBs[bbi] |= pieceBB;
                _whiteBB |= pieceBB;
            }
            else
            {
                _blackBBs[bbi] |= pieceBB;
                _blackBB |= pieceBB;
            }

            _occupiedBB |= pieceBB;
        }
        _emptyBB = ~_occupiedBB;
    }

    #endregion

    #region Private Methods

    private void updateBBsFromMove(Move move)
    {
        int fromBBi = (int)(_mailbox[move.From] & Piece._pieceTypeMask);
        int toBBi = (int)(_mailbox[move.To] & Piece._pieceTypeMask);

        ulong fromBB = 1ul << move.From;
        ulong toBB = 1ul << move.To;
        ulong fromToBB = fromBB ^ toBB;

        if (_isWhiteToMove)
        {
            _whiteBBs[fromBBi] ^= fromToBB;
            _whiteBB ^= fromToBB;
        }
        else
        {
            _blackBBs[fromBBi] ^= fromToBB;
            _blackBB ^= fromToBB;
        }

        if (move.IsCapture())
        {
            if (_isWhiteToMove)
            {
                _blackBBs[toBBi] ^= toBB;
                _blackBB ^= toBB;
            }
            else
            {
                _whiteBBs[toBBi] ^= toBB;
                _whiteBB ^= toBB;
            }

            _occupiedBB ^= fromBB;
            _emptyBB ^= fromBB;
        }
        else
        {
            _occupiedBB ^= fromToBB;
            _emptyBB ^= fromToBB;
        }
    }

    #endregion

    #region Properties

    public uint this[int index] => _mailbox[index];
    public PieceColor SideToMove => _isWhiteToMove ? PieceColor.White : PieceColor.Black;
    public CastlingRights CastlingRights => _castlingRights;
    public int HalfMoveClock => _halfMoveClock;
    public int FullMoveCounter => _fullMoveCounter;
    public int AllMoveCounter => _playedMoves.Count;
    public IReadOnlyCollection<Move> PlayedMoves => _playedMoves;
    public int EnPassantTargetSquareIndex => _epIndex;

    #endregion
}
