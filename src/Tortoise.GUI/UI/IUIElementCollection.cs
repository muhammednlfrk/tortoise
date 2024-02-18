namespace Tortoise.GUI.UI;

internal interface IUIElementCollection : IDisposable, IEnumerable<IUIElement>
{
    void Add<T>(T element) where T : IUIElement;
}
