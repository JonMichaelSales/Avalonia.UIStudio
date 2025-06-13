using Avalonia.Markup.Xaml;

namespace Avalonia.UIStudio.Dialogs.Tests.Fixtures
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}