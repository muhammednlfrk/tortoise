namespace Tortoise.GUI.UI;

internal abstract class UIElement : IDisposable
{
    public abstract void Dispose();
    public abstract void Draw();
}
