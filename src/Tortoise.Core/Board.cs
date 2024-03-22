using Tortoise.Core.Helpers;

namespace Tortoise.Core;

public enum BBPiece : int
{
    White = 0,
    Black = 1,
    WhitePawn = 2,
    BlackPawn = 3,
    WhiteKnight = 4,
    BlackKnight = 5,
    WhiteBishop = 6,
    BlackBishop = 7,
    WhiteRook = 8,
    BlackRook = 9,
    WhiteQueen = 10,
    BlackQueen = 11,
    WhiteKing = 12,
    BlackKing = 13,
}

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
    private readonly Stack<Move> _playedMoves;

    // Bitboards.
    private Bitboard[] _pieceBB;
    private Bitboard _emptyBB;
    private Bitboard _occupiedBB;
    private Bitboard _whiteAttackMap;
    private Bitboard _blackAttackMap;

    public Board(FenInfo fenInfo)
    {
        _pieceBB = new Bitboard[14];
        _emptyBB = new Bitboard();
        _occupiedBB = new Bitboard();
        _whiteAttackMap = new Bitboard();
        _blackAttackMap = new Bitboard();

        _playedMoves = new Stack<Move>();

        SetPosition(fenInfo);
    }

    #region Public Methods

    public void MakeMove(Move move)
    {
        uint fromP = _mailbox[move.From] & Piece._pieceInfoMask;
        uint fromC = fromP & Piece._pieceColorMask;

        uint toP = _mailbox[move.To] & Piece._pieceInfoMask;
        uint toC = toP & Piece._pieceColorMask;

        Piece from = new(_mailbox[move.From]);
        Piece to = new(_mailbox[move.To]);
        uint sideToMove = _isWhiteToMove ? Piece.White : Piece.Black;

        // Check from square.
        if (fromP == Piece.None)
            throw new InvalidChessMoveException("There is no piece at from square.");

        // Check target square piece color.
        if (toP != Piece.None && fromC == toC)
            throw new InvalidChessMoveException("The target square contains friendly piece.");

        // Check side to move.
        if (fromC != sideToMove)
            throw new InvalidChessMoveException("Invalid side to move.");

        // Check half move clock.
        if (_halfMoveClock >= 50)
            throw new InvalidChessMoveException("The game already over. (50 move rule.)");

        // Check move flags
        if (move.HasUnusedFlags())
            throw new InvalidChessMoveException("Invalid move flag.");

        // Update bitboards.
        (int fbbi, int fcbbi) = from.GetBBIndexes();
        ulong fromBB = 1ul << move.From;
        ulong toBB = 1ul << move.To;
        ulong fromToBB = fromBB ^ toBB;
        _pieceBB[fbbi] ^= fromToBB;
        _pieceBB[fcbbi] ^= fromToBB;

        // Handle quiet moves.
        if (move.IsQuietMove())
        {
            if (!to.IsNone())
                throw new InvalidChessMoveException("Invalid capture move. There is a target.");

            // Update mailbox.
            _mailbox[move.To] = _mailbox[move.From];

            // Update bitboards.
            _occupiedBB ^= fromToBB;
            _emptyBB ^= fromToBB;
        }

        // Handle captures.
        else if (move.IsCapture())
        {
            if (to.IsNone())
                throw new InvalidChessMoveException("Invalid capture move. There is no target.");

            // Update bitboards.
            (int tbbi, int tbbbi) = to.GetBBIndexes();
            _pieceBB[tbbi] ^= toBB;
            _pieceBB[tbbbi] ^= toBB;
            _occupiedBB ^= fromBB;
            _emptyBB ^= fromBB;

            // Update mailbox.
            _mailbox[move.To] = _mailbox[move.From];
        }

        // Handle en-passant.
        else if (move.IsEnPassant())
        {
            if (_epIndex < 0)
                throw new InvalidChessMoveException("There is no en-passant move.");

            int epCaptureIndex = move.To + (_isWhiteToMove ? -8 : 8);

            if (epCaptureIndex != _epIndex)
                throw new InvalidChessMoveException("Invalid en-passant square index.");

            // Update bitboards.
            (int capbbi, int capcbbi) = ((Piece)_mailbox[epCaptureIndex]).GetBBIndexes();
            ulong capBB = 1ul << epCaptureIndex;
            _pieceBB[capbbi] ^= capBB;
            _pieceBB[capcbbi] ^= capBB;
            _occupiedBB ^= fromBB;
            _emptyBB ^= fromBB;

            // Update mailbox.
            _mailbox[epCaptureIndex] = Piece.None;
            _mailbox[move.To] = _mailbox[move.From];
        }

        // Handle promotion.
        else if (move.IsPromotion())
        {
            // TODO: Check promotion square.

            // Update bitboards.
            Piece promotionPiece = new(sideToMove | move.GetPromotionPieceType());
            (int pbbi, int pcbbi) = promotionPiece.GetBBIndexes();
            _pieceBB[pbbi] ^= toBB;
            _pieceBB[pcbbi] ^= toBB;
            _occupiedBB ^= fromToBB;
            _emptyBB ^= fromToBB;

            // Update mailbox.
            _mailbox[move.To] = promotionPiece;
        }

        // Handle castling.
        else if (move.IsCastling())
        {
            // Check castling rights.
            bool isKingSide = move.Flags == Move.KingCastleFlag;
            bool hasKingSideRights = _isWhiteToMove ? _castlingRights.WhiteKingSide : _castlingRights.BlackKingSide;
            bool hasQueenSideRights = _isWhiteToMove ? _castlingRights.WhiteQueenSide : _castlingRights.BlackQueenSide;
            bool hasRights = isKingSide ? hasKingSideRights : hasQueenSideRights;
            if (!hasRights)
                throw new InvalidChessMoveException($"The king don't have {(isKingSide ? "king" : "queen")} side castling rights.");

            // Check there is there a rook.
            int rookFromIndex = isKingSide ? move.To + 1 : move.To - 2;
            if (_mailbox[rookFromIndex] != new Piece(Piece.Rook, sideToMove))
                throw new InvalidChessMoveException("There is no same color rook!");

            // Update castling rights.
            if (_isWhiteToMove)
            {
                _castlingRights.WhiteQueenSide = false;
                _castlingRights.WhiteKingSide = false;
            }
            else
            {
                _castlingRights.BlackQueenSide = false;
                _castlingRights.BlackKingSide = false;
            }

            // Update bitboards.
            int rookToIndex = isKingSide ? move.To - 1 : move.To - 2;
            (int rbbi, int rcbbi) = ((Piece)_mailbox[rookFromIndex]).GetBBIndexes();
            ulong rookFromBB = 1ul << rookFromIndex;
            ulong rookToBB = 1ul << rookToIndex;
            ulong rookFromToBB = rookFromBB ^ rookToBB;
            _pieceBB[rbbi] ^= rookFromToBB;
            _pieceBB[rcbbi] ^= rookFromToBB;
            _occupiedBB ^= rookFromToBB;
            _emptyBB ^= rookFromToBB;
            _occupiedBB ^= fromToBB;
            _emptyBB ^= fromToBB;

            // Update mailbox.
            _mailbox[move.To] = _mailbox[move.From];
            _mailbox[rookToIndex] = _mailbox[rookFromIndex];
            _mailbox[rookFromIndex] = Piece.None;
        }

        // Update king's square index and rights.
        else if (from.IsPiece(Piece.King))
        {
            // TODO: Check is kings are goint to side by side.

            if (_isWhiteToMove)
            {
                _castlingRights.WhiteKingSide = false;
                _castlingRights.WhiteQueenSide = false;
            }
            else
            {
                _castlingRights.BlackKingSide = false;
                _castlingRights.BlackQueenSide = false;
            }

            // Update mailbox.
            _mailbox[move.To] = _mailbox[move.From];

            // Update bitboards.
            _occupiedBB ^= fromToBB;
            _emptyBB ^= fromToBB;
        }

        // Update en-passant target square index.
        else if (from.IsPiece(Piece.Pawn))
        {
            if (move.IsDoublePawnPush())
                _epIndex = move.To + (_isWhiteToMove ? -8 : 8);
            else
                _epIndex = -1;

            // Update mailbox
            _mailbox[move.To] = _mailbox[move.From];

            // Update bitboards.
            _occupiedBB ^= fromToBB;
            _emptyBB ^= fromToBB;
        }

        // Update castling rights.
        Piece rook = new(sideToMove, Piece.Rook);
        int targetIndex = -1;
        if (rook == from)
            targetIndex = move.From;
        else if (rook == to)
            targetIndex = move.To;
        switch (targetIndex)
        {
            /*A1*/
            case 0: _castlingRights.WhiteQueenSide = false; break;
            /*A8*/
            case 56: _castlingRights.BlackQueenSide = false; break;
            /*H1*/
            case 7: _castlingRights.WhiteKingSide = false; break;
            /*H1*/
            case 63: _castlingRights.BlackKingSide = false; break;
        }

        // Update half move clock and move counter.
        if (!_isWhiteToMove)
        {
            _fullMoveCounter++;

            // If there is any captures or pawn move, reset the half move clock.
            if (from.IsPiece(Piece.Pawn) || move.IsCaptureAny())
                _halfMoveClock = 0;

            // Else increse the half move clock.
            else
                _halfMoveClock++;
        }

        // Remove piece at the from index. Becouse whatewer move you make always from index is empty.
        _mailbox[move.From] = Piece.None;

        // Update side to move.
        _isWhiteToMove = !_isWhiteToMove;

        // Save the move.
        _playedMoves.Push(move);
    }

    public void SetPosition(FenInfo fenInfo)
    {
        _pieceBB = new Bitboard[14];
        _occupiedBB = new Bitboard();
        _whiteAttackMap = new Bitboard();
        _blackAttackMap = new Bitboard();

        _playedMoves.Clear();

        _mailbox = fenInfo.Mailbox;
        _castlingRights = fenInfo.CastlingRights;
        _isWhiteToMove = fenInfo.IsWhiteToMove;
        _halfMoveClock = fenInfo.HalfMoveClock;
        _fullMoveCounter = fenInfo.FullMoveCounter;
        _epIndex = fenInfo.EnPassantTargetSquareIndex;

        for (int i = 0; i < 64; i++)
        {
            Piece piece = new(_mailbox[i]);
            if ((uint)piece == Piece.None) continue;
            int bbi = (int)piece.AsBBPiece(); // piece BB index/
            int bbci = (int)((Piece)(piece & Piece._pieceColorMask)).AsBBPiece(); // piece color BB index/
            ulong pieceBB = 1ul << i;
            _pieceBB[bbi] |= pieceBB;
            _pieceBB[bbci] |= pieceBB;
            _occupiedBB |= pieceBB;
        }
        _emptyBB = ~_occupiedBB;
    }

    #endregion

    #region Internal Methods

    internal Bitboard getPieceSet(BBPiece piece) => _pieceBB[(int)piece];

    internal Bitboard setPieceSet(BBPiece piece, Bitboard bb) => _pieceBB[(int)piece] = bb;

    internal Bitboard getPieceSet(uint piece) => getPieceSet(((Piece)piece).AsBBPiece());

    internal Bitboard setPieceSet(uint piece, Bitboard bb) => _pieceBB[(int)((Piece)piece).AsBBPiece()] = bb;

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
    public Bitboard[] PieceBitboards => _pieceBB;
    public Bitboard EmptyBitboard => _emptyBB;
    public Bitboard OccupiedBitboard => _occupiedBB;
    public Bitboard WhiteAttachMap => _whiteAttackMap;
    public Bitboard BlackAttackMap => _blackAttackMap;

    #endregion
}
