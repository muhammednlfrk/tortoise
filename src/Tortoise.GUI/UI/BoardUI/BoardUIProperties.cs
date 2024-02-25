using Raylib_cs;
using System.Numerics;

namespace Tortoise.GUI.UI.BoardUI;

internal sealed class BoardUIProperties
{
    public Vector2 Location { get; set; }
    public int SquareSize { get; set; }
    public int PieceSize { get; set; }

    public Color LightSquareColor { get; set; }
    public Color DarkSquareColor { get; set; }

    public IReadOnlyDictionary<uint, Texture2D> PieceTextures { get; set; } = new Dictionary<uint, Texture2D>(0);
}