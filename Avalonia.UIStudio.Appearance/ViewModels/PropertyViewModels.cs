using Avalonia.Media;
using Avalonia.UIStudio.Appearance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia.UIStudio.Appearance.ViewModels
{
    public class BoolPropertyViewModel : ReflectedPropertyViewModelBase<bool>
    {
        public BoolPropertyViewModel(object target, PropertyInfo property) : base(target, property)
        {
        }
    }

    public class ColorPropertyViewModel : ReflectedPropertyViewModelBase<Color>
    {
        public ColorPropertyViewModel(object target, PropertyInfo property) : base(target, property)
        {
        }
    }

    public class NumericPropertyViewModel : ReflectedPropertyViewModelBase<double>
    {
        public NumericPropertyViewModel(object target, PropertyInfo property) : base(target, property)
        {
        }
    }

    public class EnumPropertyViewModel : ReflectedPropertyViewModelBase<Enum>
    {
        public EnumPropertyViewModel(object target, PropertyInfo property) : base(target, property)
        {
        }
    }

    public class StringPropertyViewModel : ReflectedPropertyViewModelBase<string>
    {
        public StringPropertyViewModel(object target, PropertyInfo property) : base(target, property)
        {
        }
    }

    public class DateTimePropertyViewModel : ReflectedPropertyViewModelBase<DateTime>
    {
        public DateTimePropertyViewModel(object target, PropertyInfo property) : base(target, property)
        {
        }
    }

    public class TimeSpanPropertyViewModel : ReflectedPropertyViewModelBase<TimeSpan>
    {
        public TimeSpanPropertyViewModel(object target, PropertyInfo property) : base(target, property)
        {
        }
    }

    public class UriPropertyViewModel : ReflectedPropertyViewModelBase<Uri>
    {
        public UriPropertyViewModel(object target, PropertyInfo property) : base(target, property)
        {
        }
    }

    public class FontFamilyPropertyViewModel : ReflectedPropertyViewModelBase<FontFamily>
    {
        public FontFamilyPropertyViewModel(object target, PropertyInfo property) : base(target, property)
        {
        }
    }

    public class ThicknessPropertyViewModel : ReflectedPropertyViewModelBase<Thickness>
    {
        public ThicknessPropertyViewModel(object target, PropertyInfo property) : base(target, property)
        {
        }
    }

    public class TypographyPropertyViewModel : ReflectedPropertyViewModelBase<SerializableTypography>
    {
        public TypographyPropertyViewModel(object target, PropertyInfo property) : base(target, property)
        {
        }
    }

    public class SkinPropertyViewModel : ReflectedPropertyViewModelBase<SerializableSkin>
    {
        public SkinPropertyViewModel(object target, PropertyInfo property) : base(target, property)
        {
        }
    }

    public class StringListPropertyViewModel : ReflectedPropertyViewModelBase<List<string>>
    {
        public StringListPropertyViewModel(object target, PropertyInfo property) : base(target, property)
        {
        }

    }

    public class ObjectListPropertyViewModel : ReflectedPropertyViewModelBase<List<object>>
    {
        public ObjectListPropertyViewModel(object target, PropertyInfo property) : base(target, property)
        {
        }

    }

    public class ObjectPropertyViewModel : ReflectedPropertyViewModelBase<object>
    {
        public ObjectPropertyViewModel(object target, PropertyInfo property) : base(target, property)
        {
        }

    }
}
