using System.Numerics;

namespace Tortoise.GUI.UI;

internal class MouseState
{
    public Vector2 Position { get; set; }
    public bool LeftButtonDown { get; set; }
    public bool LeftButtonReleased { get; set; }
#if DEBUG
    public override string ToString()
    {
        return $"MOUSE STATUS -> POS: {Position.X}, {Position.Y}; LBD: {LeftButtonDown}; LBR: {LeftButtonReleased};";
    }
#endif
}
