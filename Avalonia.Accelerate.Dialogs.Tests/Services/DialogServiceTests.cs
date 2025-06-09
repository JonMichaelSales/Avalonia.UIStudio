using Avalonia.Headless.XUnit;
using Avalonia.Accelerate.Dialogs.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using Avalonia.Accelerate.Dialogs.Tests.Utility;

namespace Avalonia.Accelerate.Dialogs.Tests.Services
{
    public class DialogServiceTests
    {
        private readonly DialogService _dialogService;
        private readonly Mock<ILogger<DialogService>> _loggerMock;

        public DialogServiceTests()
        {
            _loggerMock = new Mock<ILogger<DialogService>>();
            _loggerMock.Setup(x => x.IsEnabled(It.IsAny<LogLevel>())).Returns(true);

            _dialogService = new DialogService(_loggerMock.Object, testAutoCloseDialogs: true);

            // Ensure MainWindow is visible for headless testing of dialogs
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (desktop.MainWindow == null)
                {
                    desktop.MainWindow = new Window();
                    desktop.MainWindow.Show();
                }
            }
        }

        [AvaloniaFact]
        public async Task ShowErrorAsync_Should_Display_ErrorDialog()
        {
            await _dialogService.ShowErrorAsync("Test Error", "This is a test error message", new Exception("Test Exception"));

            _loggerMock.VerifyLog(LogLevel.Error, "Error dialog shown");
        }

        [AvaloniaFact]
        public async Task ShowWarningAsync_Should_Display_WarningDialog()
        {
            await _dialogService.ShowWarningAsync("Test Warning", "This is a test warning message");

            _loggerMock.VerifyLog(LogLevel.Warning, "Warning dialog shown");
        }

        [AvaloniaFact]
        public async Task ShowInfoAsync_Should_Display_InfoDialog()
        {
            await _dialogService.ShowInfoAsync("Test Info", "This is a test info message");

            _loggerMock.VerifyLog(LogLevel.Information, "Info dialog shown");
        }

        [AvaloniaFact]
        public async Task ShowValidationErrorsAsync_Should_Display_ValidationErrorDialog()
        {
            var errors = new List<string> { "Error 1", "Error 2" };
            var warnings = new List<string> { "Warning 1" };

            await _dialogService.ShowValidationErrorsAsync("Validation Test", errors, warnings);

            _loggerMock.VerifyLog(LogLevel.Warning, "Validation dialog shown");
        }

        [AvaloniaFact]
        public async Task ShowConfirmationAsync_Should_Return_True_When_Confirmed()
        {
            bool result = await _dialogService.ShowConfirmationAsync("Confirm Test", "Do you want to proceed?");

            Assert.True(result);

            _loggerMock.VerifyLog(LogLevel.Debug, "Confirmation dialog shown");
        }
    }
}
