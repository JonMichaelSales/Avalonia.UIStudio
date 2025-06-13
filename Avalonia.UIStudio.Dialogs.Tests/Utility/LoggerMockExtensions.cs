using Moq;
using Microsoft.Extensions.Logging;

namespace Avalonia.UIStudio.Dialogs.Tests.Utility
{
    public static class LoggerMockExtensions
    {
        /// <summary>
        /// Verifies that a log entry with the specified level and message fragment was written exactly once.
        /// </summary>
        public static void VerifyLog<T>(this Mock<ILogger<T>> mockLogger, LogLevel level, string messageFragment)
            where T : class
        {
            mockLogger.Verify(x => x.Log(
                    level,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(messageFragment)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        /// <summary>
        /// Verifies that a log entry with the specified level and message fragment was written the specified number of times.
        /// </summary>
        public static void VerifyLog<T>(this Mock<ILogger<T>> mockLogger, LogLevel level, string messageFragment, Times times)
            where T : class
        {
            mockLogger.Verify(x => x.Log(
                    level,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(messageFragment)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                times);
        }
    }
}