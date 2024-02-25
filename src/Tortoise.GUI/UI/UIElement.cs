using Raylib_cs;
using System.Numerics;

namespace Tortoise.GUI.UI;

internal abstract class UIElement : IDisposable
{
    protected MouseState _mouseState;

    public UIElement()
    {
        _mouseState = new MouseState();
    }

    public abstract void Draw();

    public abstract void Dispose();

    public virtual void Update()
    {
        _mouseState.Position = Raylib.GetMousePosition();
        _mouseState.LeftButtonDown = Raylib.IsMouseButtonDown(MouseButton.Left);
        _mouseState.LeftButtonReleased = Raylib.IsMouseButtonReleased(MouseButton.Left);
    }

    public bool IsMouseInRectangle(Rectangle rectangle)
    {
        return _mouseState.Position.X >= rectangle.X &&
               _mouseState.Position.Y >= rectangle.Y &&
               _mouseState.Position.X <= rectangle.X + rectangle.Width &&
               _mouseState.Position.Y <= rectangle.Y + rectangle.Height;
    }
}
