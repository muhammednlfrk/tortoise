using System.Text;
using Tortoise.Core.Helpers;

namespace Tortoise.Core;

public struct FenInfo
{
    private string _fen;
    private readonly Mailbox _mailbox;
    private bool _isWhiteToMove;
    private CastlingRights _castlingRights;
    private int _halfMoveClock;
    private int _fullMoveCounter;
    private int _epTargetIndex;

    private static readonly IReadOnlyDictionary<char, uint> _fenPieceCodes =
        new Dictionary<char, uint>(12)
    {
        { 'P', Piece.WhitePawn },
        { 'N', Piece.WhiteKnight },
        { 'B', Piece.WhiteBishop },
        { 'R', Piece.WhiteRook },
        { 'Q', Piece.WhiteQueen },
        { 'K', Piece.WhiteKing },
        { 'p', Piece.BlackPawn },
        { 'n', Piece.BlackKnight },
        { 'b', Piece.BlackBishop},
        { 'r', Piece.BlackRook },
        { 'q', Piece.BlackQueen },
        { 'k', Piece.BlackKing }
    };

    private static readonly IReadOnlyDictionary<uint, char> _pieceFenCodes =
        new Dictionary<uint, char>(12)
    {
        { Piece.WhitePawn, 'P' },
        { Piece.WhiteKnight, 'N' },
        { Piece.WhiteBishop, 'B' },
        { Piece.WhiteRook, 'R' },
        { Piece.WhiteQueen, 'Q' },
        { Piece.WhiteKing, 'K' },
        { Piece.BlackPawn, 'p' },
        { Piece.BlackKnight, 'n' },
        { Piece.BlackBishop, 'b' },
        { Piece.BlackRook, 'r' },
        { Piece.BlackQueen, 'q' },
        { Piece.BlackKing, 'k' }
    };

    public FenInfo(string fen)
    {
        _fen = fen;
        _mailbox = new Mailbox();
        _isWhiteToMove = false;
        _castlingRights = new CastlingRights();
        _halfMoveClock = 0;
        _fullMoveCounter = 0;
        _epTargetIndex = 0;
        initialize();
    }

    public FenInfo(
        Mailbox mailbox,
        bool isWhiteToMove,
        CastlingRights castlingRights,
        int halfMoveClock,
        int fullMoveCounter,
        int epTargetIndex)
    {
        _fen = string.Empty;
        _mailbox = mailbox;
        _isWhiteToMove = isWhiteToMove;
        _castlingRights = castlingRights;
        _halfMoveClock = halfMoveClock;
        _fullMoveCounter = fullMoveCounter;
        _epTargetIndex = epTargetIndex;
        generate();
    }

    private void initialize()
    {
        string[] fields = _fen.Split(' ');

        // Initialize piece locations.
        string[] rankPlacements = fields[0].Split('/');
        int file = 0, rank = 7;
        for (int i = 0; i < 8; i++)
        {
            string placement = rankPlacements[i];
            for (int j = 0; j < placement.Length; j++)
            {
                if (char.IsNumber(placement[j]))
                {
                    file += (int)char.GetNumericValue(placement[j]);
                    continue;
                }

                uint pieceCode = _fenPieceCodes[placement[j]];
                int squareIndex = MailboxExtensions.GetSquareIndex(file, rank);
                _mailbox[squareIndex] = pieceCode;
                file++;
            }
            if (file != 8)
                throw new ArgumentException("Invalid FEN", nameof(_fen));
            file = 0;
            rank--;
        }

        // Initialize side to move.
        _isWhiteToMove = fields[1] switch
        {
            "w" or "W" => true,
            "b" or "B" => false,
            _ => throw new ArgumentException("Invalid FEN", nameof(_fen)),
        };

        // Initialize catling rights.
        if (fields[2] != "-")
        {
            foreach (char key in fields[2])
            {
                switch (key)
                {
                    case 'K': _castlingRights.WhiteKingSide = true; break;
                    case 'Q': _castlingRights.WhiteQueenSide = true; break;
                    case 'k': _castlingRights.BlackKingSide = true; break;
                    case 'q': _castlingRights.BlackQueenSide = true; break;
                    default: throw new ArgumentException("Invalid FEN", nameof(_fen));
                }
            }
        }

        // Initialize ep target square.
        int epFile = fields[3][0] switch
        {
            '-' => -1,
            'a' => 0,
            'b' => 1,
            'c' => 2,
            'd' => 3,
            'e' => 4,
            'f' => 5,
            'g' => 6,
            'h' => 7,
            _ => throw new ArgumentException("Invalid argument", nameof(_fen))
        };
        if (epFile != -1)
        {
            int epRankIndex = ((int)char.GetNumericValue(fields[3][1])) - 1;
            _epTargetIndex = MailboxExtensions.GetSquareIndex(epFile, epRankIndex);
        }

        // Initialize half move clock and full move counter.
        _halfMoveClock = Convert.ToInt32(fields[4]);
        _fullMoveCounter = Convert.ToInt32(fields[5]);
    }

    private void generate()
    {
        StringBuilder fenBuilder = new();

        for (int rankIndex = 0; rankIndex < 8; rankIndex++)
        {
            int emptySquareCount = 0;

            for (int fileIndex = 0; fileIndex < 8; fileIndex++)
            {
                int squareIndex = rankIndex * 8 + fileIndex;
                uint pieceCode = _mailbox[squareIndex];

                if (pieceCode == 0)
                {
                    emptySquareCount++;
                }
                else
                {
                    if (emptySquareCount != 0)
                    {
                        fenBuilder.Append(emptySquareCount);
                        emptySquareCount = 0;
                    }

                    fenBuilder.Append(_pieceFenCodes[pieceCode]);
                }
            }
            fenBuilder.Append('/');
        }
        fenBuilder.Append(' ');

        fenBuilder.Append(IsWhiteToMove ? 'w' : 'b');
        fenBuilder.Append(' ');

        if (_castlingRights.Rights == 0)
        {
            fenBuilder.Append('-');
        }
        else
        {
            if (CastlingRights.WhiteKingSide)
                fenBuilder.Append('K');
            if (CastlingRights.WhiteQueenSide)
                fenBuilder.Append('Q');
            if (CastlingRights.BlackKingSide)
                fenBuilder.Append('k');
            if (CastlingRights.BlackQueenSide)
                fenBuilder.Append('q');
        }
        fenBuilder.Append(' ');

        if (EnPassantTargetSquareIndex == -1)
            fenBuilder.Append('-');
        else
            fenBuilder.Append(MailboxExtensions.GetSquareName(EnPassantTargetSquareIndex));
        fenBuilder.Append(' ');

        fenBuilder.Append(HalfMoveClock);
        fenBuilder.Append(' ');

        fenBuilder.Append(FullMoveCounter);
        fenBuilder.Append(' ');

        _fen = fenBuilder.ToString();
    }

    public readonly string FEN => _fen;
    public readonly Mailbox Mailbox => _mailbox;
    public readonly bool IsWhiteToMove => _isWhiteToMove;
    public readonly CastlingRights CastlingRights => _castlingRights;
    public readonly int HalfMoveClock => _halfMoveClock;
    public readonly int FullMoveCounter => _fullMoveCounter;
    public readonly int EnPassantTargetSquareIndex => _epTargetIndex;

    public static readonly FenInfo StandardOpeningPosition = new("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
}
