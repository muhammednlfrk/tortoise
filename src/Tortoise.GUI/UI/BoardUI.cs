using Raylib_cs;

namespace Tortoise.GUI.UI;

internal sealed class BoardUI : UIBase
{
    private readonly float _x;
    private readonly float _y;
    private readonly float _squareSize = 64f;

    public BoardUI(float x, float y)
    {
        _x = x;
        _y = y;
    }

    #region UIBase

    public override void Draw()
    {
        for (int rank = 0; rank < 8; rank++)
        {
            for (int file = 0; file < 8; file++)
            {
                bool isLightSquare = (file + rank) % 2 != 0;
                Color squareColor = isLightSquare ? Color.White : Color.Black;
                Rectangle destinatonRectangle = new(
                    x: _x + _squareSize * file,
                    y: _y + _squareSize * (7 - rank),
                    width: _squareSize,
                    height: _squareSize);
                Raylib.DrawRectangleRec(destinatonRectangle, squareColor);
            }
        }
    }

    #endregion
}
