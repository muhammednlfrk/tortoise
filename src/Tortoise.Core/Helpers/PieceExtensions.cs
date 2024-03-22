namespace Tortoise.Core.Helpers;

public static class PieceExtensions
{
    public static bool IsNone(this Piece piece) => piece == 0u;

    public static bool IsPiece(this Piece piece, uint pieceCode) => (piece & Piece._pieceTypeMask) == pieceCode;
}
