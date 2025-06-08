using Avalonia.Styling;

namespace Avalonia.Accelerate.Appearance.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IStylesCollection : IEnumerable<IStyle>
    {
        void Add(IStyle style);
        bool Remove(IStyle style);
        void Clear();
        int Count { get; }
    }
}
