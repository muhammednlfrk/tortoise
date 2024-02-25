using Raylib_cs;
using System.Numerics;
using Tortoise.Core;
using Tortoise.Core.Helpers;

namespace Tortoise.GUI.UI.BoardUI;

internal sealed class BoardUI : UIElement
{
    private readonly BoardUIProperties _properties;
    private readonly Board _chessBoard;
    private readonly Rectangle _boardRectangle;

    private int _selectedPieceIndex = -1;
    private Piece _selectedPiece = (Piece)Piece.None;
    private Vector2 _selectedPieceOffset = new(0f, 0f);

    public BoardUI(BoardUIProperties properties)
    {
        _properties = properties;
        _chessBoard = new Board(FenInfo.StandardOpeningPosition);

        float boardSize = _properties.SquareSize * 8;
        _boardRectangle = new Rectangle(
            x: _properties.Location.X,
            y: _properties.Location.Y,
            width: boardSize,
            height: boardSize);
    }

    #region UIElement

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

        drawSelectedPiece();
    }

    public override void Update()
    {
        base.Update();

        // If mouse isn't over the board don't do anything.
        if (!IsMouseInRectangle(_boardRectangle))
            return;

        if (_mouseState.LeftButtonReleased)
        {
            int from = _selectedPieceIndex;
            int to = getSquareIndexByLocation(_mouseState.Position.X, _mouseState.Position.Y);
            Raylib.TraceLog(TraceLogLevel.Info, $"F: {from} T: {to}");
            // TODO: Make move.

            _selectedPieceIndex = -1;
            _selectedPiece = (Piece)Piece.None;
            _selectedPieceOffset.X = 0;
            _selectedPieceOffset.Y = 0;
        }
        else if (_mouseState.LeftButtonDown && _selectedPieceIndex == -1)
        {
            float mouseX = _mouseState.Position.X;
            float mouseY = _mouseState.Position.Y;
            _selectedPieceIndex = getSquareIndexByLocation(mouseX, mouseY);
            _selectedPiece = (Piece)_chessBoard.Mailbox[_selectedPieceIndex];
            float boardX = _boardRectangle.X;
            float boardY = _boardRectangle.Y;
            _selectedPieceOffset.X = (mouseX - boardX) % _properties.SquareSize;
            _selectedPieceOffset.Y = (mouseY - boardY) % _properties.SquareSize;
        }
    }

    public override void Dispose() { }

    #endregion

    #region Private Methods

    private int getSquareIndexByLocation(float x, float y)
    {
        int squareSize = _properties.SquareSize;
        float boardX = _boardRectangle.X;
        float boardY = _boardRectangle.Y;
        int file = (int)((x - boardX) / squareSize);
        int rank = (int)(8f - (y - boardY) / squareSize);
        return Mailbox.GetSquareIndex(file, rank);
    }

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

        if (piece.IsNone() || squareIndex == _selectedPieceIndex)
            return;

        Raylib.DrawTexture(
            texture: _properties.PieceTextures[piece & 0b1111],
            posX: (int)targetSquare.X + offset,
            posY: (int)targetSquare.Y + offset,
            tint: Color.White);
    }

    private void drawSelectedPiece()
    {
        if (_selectedPieceIndex == -1)
            return;

        Raylib.DrawTexture(
            texture: _properties.PieceTextures[_selectedPiece],
            posX: (int)(_mouseState.Position.X - _selectedPieceOffset.X),
            posY: (int)(_mouseState.Position.Y - _selectedPieceOffset.Y),
            tint: Color.White);
    }

    #endregion
}
