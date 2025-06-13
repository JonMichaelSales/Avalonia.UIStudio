using Microsoft.Extensions.DependencyInjection;

namespace Avalonia.UIStudio.Dialogs.Services
{
    public static class DialogServiceLocator
    {
        public static IServiceProvider? ServiceProvider { get; set; }

        public static T GetService<T>() where T : notnull =>
            (T)(ServiceProvider?.GetService(typeof(T))
                ?? throw new InvalidOperationException($"Service {typeof(T).FullName} not registered."));

        public static T GetRequiredService<T>() where T : notnull =>
            (T)(ServiceProvider?.GetRequiredService(typeof(T))
                ?? throw new InvalidOperationException($"Required service {typeof(T).FullName} not registered."));
    }
}
