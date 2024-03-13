namespace Tortoise.Core;

/**
 * Mailbox piece information uses 4 bits for the
 * piece representation.
 * 
 * 0-2nd bits are piece type.
 * 3th bit is piece color (0=white, 1=black).
 */

public enum PieceType : uint
{
    None = 0,
    Pawn = 1,
    Knight = 2,
    Bishop = 3,
    Rook = 4,
    Queen = 5,
    King = 6,
}

public enum PieceColor : uint
{
    White = 0,
    Black = 8
}

public readonly struct Piece
{
    private readonly uint _piece;

    internal const uint _pieceInfoMask = 0b1111;
    internal const uint _pieceTypeMask = 0b0111;
    internal const uint _pieceColorMask = 0b1000;

    public Piece(uint piece)
    {
        _piece = piece;
    }

    public readonly PieceType PieceType => (PieceType)(_piece & _pieceTypeMask);
    public readonly PieceColor PieceColor => (PieceColor)(_piece & _pieceColorMask);

    public static implicit operator uint(Piece piece) => piece._piece;
    public static explicit operator Piece(uint i) => new(i);

    // Piece types:
    public const uint None = (uint)PieceType.None;     // 0
    public const uint Pawn = (uint)PieceType.Pawn;     // 1
    public const uint Knight = (uint)PieceType.Knight; // 2
    public const uint Bishop = (uint)PieceType.Bishop; // 3
    public const uint Rook = (uint)PieceType.Rook;     // 4
    public const uint Queen = (uint)PieceType.Queen;   // 5
    public const uint King = (uint)PieceType.King;     // 6

    // Piece colors:
    public const uint White = (uint)PieceColor.White;  // 0
    public const uint Black = (uint)PieceColor.Black;  // 8

    // Pieces:
    public const uint WhitePawn = White | Pawn;        // 1
    public const uint WhiteKnight = White | Knight;    // 2
    public const uint WhiteBishop = White | Bishop;    // 3
    public const uint WhiteRook = White | Rook;        // 4
    public const uint WhiteQueen = White | Queen;      // 5
    public const uint WhiteKing = White | King;        // 6
    public const uint BlackPawn = Black | Pawn;        // 9
    public const uint BlackKnight = Black | Knight;    // 10
    public const uint BlackBishop = Black | Bishop;    // 11
    public const uint BlackRook = Black | Rook;        // 12
    public const uint BlackQueen = Black | Queen;      // 13
    public const uint BlackKing = Black | King;        // 14
}
