using Raylib_cs;
using Tortoise.Core;

namespace Tortoise.GUI.Resource;

internal sealed class ResourceManager : IDisposable
{
    private readonly string _resurceDirectoryPath;
    private readonly int _pieceSize;

    private Dictionary<uint, Texture2D> _pieceTextures;

    public ResourceManager(string resourceDirectoryName, int pieceSize)
    {
        _resurceDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, resourceDirectoryName) + '\\';
        _pieceTextures = new Dictionary<uint, Texture2D>(0);
        _pieceSize = pieceSize;
    }

    #region Public Methods

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

    public void Dispose()
    {
        foreach (Texture2D pTexture in _pieceTextures.Values)
            Raylib.UnloadTexture(pTexture);
        _pieceTextures.Clear();
    }

    #endregion

    #region Private Methods

    private Texture2D loadPieceTexture(string fileName)
    {
        string path = Path.Combine(_resurceDirectoryPath, fileName);
        Image image = Raylib.LoadImage(path);
        Raylib.ImageResize(ref image, _pieceSize, _pieceSize);
        Texture2D texture = Raylib.LoadTextureFromImage(image);
        Raylib.UnloadImage(image);
        return texture;
    }

    #endregion

    #region Properties

    public IReadOnlyDictionary<uint, Texture2D> PieceTextures => _pieceTextures;

    #endregion
}
