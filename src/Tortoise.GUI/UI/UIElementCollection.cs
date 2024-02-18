using System.Collections;

namespace Tortoise.GUI.UI;

internal class UIElementCollection : IUIElementCollection
{
    private readonly Dictionary<string, IUIElement> _elements = new();

    public void Add<T>(T element) where T : IUIElement
    {
        string key = typeof(T).Name;
        bool success = _elements.TryAdd(key, element);
        if (!success)
            _elements[key] = element;
    }

    public void Dispose()
    {
        _elements.Clear();
    }

    public IEnumerator<IUIElement> GetEnumerator()
    {
        return _elements.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
