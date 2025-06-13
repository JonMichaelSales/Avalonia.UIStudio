using System.Collections;
using Avalonia.Styling;
using Avalonia.UIStudio.Appearance.Interfaces;

namespace Avalonia.UIStudio.Appearance.Model
{
    public class AvaloniaStylesWrapper : IStylesCollection
    {
        private readonly Styles _styles;

        public AvaloniaStylesWrapper(Styles styles)
        {
            _styles = styles ?? throw new ArgumentNullException(nameof(styles));
        }

        public void Add(IStyle style)
        {
            _styles.Add(style);
        }

        public bool Remove(IStyle style)
        {
            return _styles.Remove(style);
        }

        public void Clear()
        {
            _styles.Clear();
        }

        // Fix: Make this a property, not a method, and delegate to the actual Styles collection
        public int Count => _styles.Count;

        public IEnumerator<IStyle> GetEnumerator()
        {
            return _styles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}