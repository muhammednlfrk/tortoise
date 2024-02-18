using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Numerics;

namespace Tortoise.GUI.UI;

internal sealed class BoardUIElement : IUIElement
{
    private readonly Vector2 _location;
    private readonly float _squareSize;
    private readonly float _pieceSize;

    public BoardUIElement(Vector2 location, float squareSize, float pieceSize)
    {
        _location = location;
        _squareSize = squareSize;
        _pieceSize = pieceSize;
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
