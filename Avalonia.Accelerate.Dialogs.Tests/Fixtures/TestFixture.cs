using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace Avalonia.Accelerate.Dialogs.Tests.Fixtures
{
    public class TestAppFixture : IDisposable
    {
        public TestAppFixture()
        {
            if (Application.Current == null)
            {
                var app = new Application();
                try
                {
                    AppBuilder.Configure(() => app)
                        .UsePlatformDetect()
                        .UseSkia()
                        .SetupWithoutStarting();  // SAFE, no Dispatcher loop started
                    var lifetime = (IClassicDesktopStyleApplicationLifetime)app.ApplicationLifetime;
                    lifetime.MainWindow = new MainWindow();
                    app.RunWithMainWindow<MainWindow>();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }
        }

        public void Dispose()
        {
            // No teardown needed
        }
    }


    public static class TestRunnerTool
    {
        public static void RunOnUIThread(Action action)
        {
            Threading.Dispatcher.UIThread.InvokeAsync(action).GetAwaiter().GetResult();
        }
    }
}