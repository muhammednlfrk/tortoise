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
    private List<Move> _playedMoves;

    public Board(FenInfo fenInfo)
    {
        _playedMoves = new List<Move>();
        SetPosition(fenInfo);
    }

    #region Public Methods

    public void MakeMove(Move move)
    {
        throw new NotImplementedException();
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
    }

    #endregion

    #region Properties

    public Mailbox Mailbox => _mailbox;
    public uint this[int index] => _mailbox[index];
    public PieceColor SideToMove => _isWhiteToMove ? PieceColor.White : PieceColor.Black;
    public CastlingRights CastlingRights => _castlingRights;
    public int HalfMoveClock => _halfMoveClock;
    public int FullMoveCounter => _fullMoveCounter;
    public int AllMoveCounter => _playedMoves.Count;
    public IReadOnlyList<Move> PlayedMoves => _playedMoves;
    public int EnPassantTargetSquareIndex => _epIndex;

    #endregion
}
