namespace Avalonia.UIStudio.Appearance.Tests.Fixtures
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
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                // OR — better: use your own ApplicationWrapper that uses the app instance
            }
        }

        public void Dispose()
        {
            // No teardown needed
        }
    }
}