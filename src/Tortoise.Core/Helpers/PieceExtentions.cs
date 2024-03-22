namespace Tortoise.Core.Helpers;

public static class PieceExtentions
{
    public static bool IsWhite(this Piece piece) => piece.PieceColor == PieceColor.White;
    public static bool IsBlack(this Piece piece) => piece.PieceColor == PieceColor.Black;

    public static BBPiece AsBBPiece(this Piece piece)
    {
        return (piece & 0b1111) switch
        {
            Piece.White => BBPiece.White,
            Piece.Black => BBPiece.Black,
            Piece.WhitePawn => BBPiece.WhitePawn,
            Piece.BlackPawn => BBPiece.BlackPawn,
            Piece.WhiteKnight => BBPiece.WhiteKnight,
            Piece.BlackKnight => BBPiece.BlackKnight,
            Piece.WhiteBishop => BBPiece.WhiteBishop,
            Piece.BlackBishop => BBPiece.BlackBishop,
            Piece.WhiteRook => BBPiece.WhiteRook,
            Piece.BlackRook => BBPiece.BlackRook,
            Piece.WhiteQueen => BBPiece.WhiteQueen,
            Piece.BlackQueen => BBPiece.BlackQueen,
            Piece.WhiteKing => BBPiece.WhiteKing,
            Piece.BlackKing => BBPiece.BlackKing,
            _ => throw new ArgumentException("Invalid piece code."),
        };
    }

    public static (int index, int colorIndex) GetBBIndexes(this Piece piece)
    {
        int i = (int)piece.AsBBPiece();
        int ci = (int)((Piece)(piece & Piece._pieceColorMask)).AsBBPiece();
        return (i, ci);
    }
}
