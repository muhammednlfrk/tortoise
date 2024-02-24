using Raylib_cs;
using Tortoise.Core;
using Tortoise.GUI.Helpers;

namespace Tortoise.GUI.Resource;

internal sealed class ResourceManager
{
    private readonly string _resurceDirectoryPath;
    private readonly int _squareSize;
    private readonly int _pieceSize;

    public ResourceManager(string resourceDirectoryName, int squareSize, int pieceSize)
    {
        _resurceDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, resourceDirectoryName) + '\\';
        _pieceTextures = new Dictionary<uint, Texture2D>(0);
        _squareSize = squareSize;
        _pieceSize = pieceSize;
    }

    private IReadOnlyDictionary<uint, Texture2D> _pieceTextures;
    public IReadOnlyDictionary<uint, Texture2D> PieceTextures => _pieceTextures;

    public void LoadPieceTextures()
    {
        Dictionary<uint, Texture2D> textures = new(12)
        {
            [Piece.BlackBishop] = loadPieceTexture("black-bishop.png"),
            [Piece.BlackKing] = loadPieceTexture("black-king.png"),
            [Piece.BlackKnight] = loadPieceTexture("black-knight.png"),
            [Piece.BlackPawn] = loadPieceTexture("black-pawn.png"),
            [Piece.BlackQueen] = loadPieceTexture("black-queen.png"),
            [Piece.BlackRook] = loadPieceTexture("black-rook.png"),
            [Piece.WhiteBishop] = loadPieceTexture("white-bishop.png"),
            [Piece.WhiteKing] = loadPieceTexture("white-king.png"),
            [Piece.WhiteKnight] = loadPieceTexture("white-knight.png"),
            [Piece.WhitePawn] = loadPieceTexture("white-pawn.png"),
            [Piece.WhiteQueen] = loadPieceTexture("white-queen.png"),
            [Piece.WhiteRook] = loadPieceTexture("white-rook.png")
        };
        _pieceTextures = textures;
    }

    public Texture2D GetPiecETexture(uint pieceCode)
    {
        return _pieceTextures[pieceCode & 0b1111];
    }

    private Texture2D loadPieceTexture(string fileName)
    {
        string filePath = Path.Combine(_resurceDirectoryPath, fileName);
        return RaylibHelper.LoadImageAsTexture(filePath, _pieceSize, _pieceSize);
    }
}
