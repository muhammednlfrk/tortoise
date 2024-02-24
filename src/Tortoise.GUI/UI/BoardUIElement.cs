using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Numerics;
using Tortoise.GUI.Resource;

namespace Tortoise.GUI.UI;

internal sealed class BoardUIElement : IUIElement
{
    private readonly Vector2 _location;
    private readonly int _squareSize;
    private readonly int _pieceSize;
    private readonly ResourceManager _resource;

    public BoardUIElement(ResourceManager resource, Vector2 location, int squareSize, int pieceSize)
    {
        _location = location;
        _squareSize = squareSize;
        _pieceSize = pieceSize;
        _resource = resource;
    }

    #region IUIElement

    public void Draw()
    {
        for (int rank = 0; rank < 8; rank++)
        {
            for (int file = 0; file < 8; file++)
            {
                bool isLightSquare = (file + rank) % 2 != 0;
                Color squareColor = isLightSquare ? Color.White : Color.Black;
                Rectangle destinatonRectangle = new(
                     x: _location.X + _squareSize * file,
                     y: _location.Y + _squareSize * (7 - rank),
                     width: _squareSize,
                     height: _squareSize);
                DrawRectangleRec(destinatonRectangle, squareColor);
            }
        }
    }

    #endregion

    #region IDisposable

    public void Dispose()
    {
    }

    #endregion
}
