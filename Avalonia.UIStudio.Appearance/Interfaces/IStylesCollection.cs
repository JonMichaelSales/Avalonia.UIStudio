using Avalonia.Styling;

namespace Avalonia.UIStudio.Appearance.Interfaces;

/// <summary>
/// </summary>
public interface IStylesCollection : IEnumerable<IStyle>
{
    int Count { get; }
    void Add(IStyle style);
    bool Remove(IStyle style);
    void Clear();
}