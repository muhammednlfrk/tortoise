using Raylib_cs;
using Tortoise.Core;
using Tortoise.Core.Helpers;

namespace Tortoise.GUI.UI.BoardUI;

internal sealed class BoardUI : UIElement
{
    private readonly BoardUIProperties _properties;
    private readonly Board _chessBoard;

    private int _selectedSquareIndex = -1;

    public BoardUI(BoardUIProperties properties)
    {
        _properties = properties;
        _chessBoard = new Board(FenInfo.StandardOpeningPosition);
    }

    #region IUIElement

    public override void Draw()
    {
        int pieceOffset = (_properties.SquareSize - _properties.PieceSize) / 2;

        for (int rank = 0; rank < 8; rank++)
        {
            for (int file = 0; file < 8; file++)
            {
                Rectangle square = drawSquare(file, rank);
                drawPiece(file, rank, square, pieceOffset);
            }
        }
    }

    #endregion

    #region IDisposable

    public override void Dispose() { }

    #endregion

    #region Private Methods

    private Rectangle drawSquare(int file, int rank)
    {
        bool isLightSquare = (file + rank) % 2 != 0;
        Color squareColor = isLightSquare ? _properties.LightSquareColor : _properties.DarkSquareColor;

        Rectangle destinatonRectangle = new(
             x: _properties.Location.X + _properties.SquareSize * file,
             y: _properties.Location.Y + _properties.SquareSize * (7 - rank),
             width: _properties.SquareSize,
             height: _properties.SquareSize);

        Raylib.DrawRectangleRec(destinatonRectangle, squareColor);
        return destinatonRectangle;
    }

    private void drawPiece(int file, int rank, Rectangle targetSquare, int offset)
    {
        int squareIndex = Mailbox.GetSquareIndex(file, rank);
        Piece piece = (Piece)_chessBoard.Mailbox[squareIndex];

        if (piece.IsNone() || squareIndex == _selectedSquareIndex)
            return;

        Raylib.DrawTexture(
            texture: _properties.PieceTextures[piece & 0b1111],
            posX: (int)targetSquare.X + offset,
            posY: (int)targetSquare.Y + offset,
            tint: Color.White);
    }

    #endregion
}
