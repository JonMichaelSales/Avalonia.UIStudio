using Avalonia.Markup.Xaml;

namespace Avalonia.UIStudio.Appearance.Tests.Fixtures
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}