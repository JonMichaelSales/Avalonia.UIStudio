namespace Avalonia.Accelerate.Dialogs.Interfaces
{
    /// <summary>
    /// Provides methods for displaying various types of dialog messages, such as errors, warnings, informational messages, 
    /// validation errors, and confirmation prompts, to the user.
    /// </summary>
    public interface IDialogService
    {
        /// <summary>
        /// Displays an error dialog to the user with the specified title, message, and optional exception details.
        /// </summary>
        /// <param name="title">The title of the error dialog.</param>
        /// <param name="message">The message to display in the error dialog.</param>
        /// <param name="exception">An optional exception providing additional details about the error.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ShowErrorAsync(string title, string message, Exception? exception = null);
        /// <summary>
        /// Displays a warning dialog to the user with the specified title and message.
        /// </summary>
        /// <param name="title">The title of the warning dialog.</param>
        /// <param name="message">The message to display in the warning dialog.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ShowWarningAsync(string title, string message);
        /// <summary>
        /// Displays an informational dialog to the user with the specified title and message.
        /// </summary>
        /// <param name="title">The title of the informational dialog.</param>
        /// <param name="message">The message to display in the informational dialog.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ShowInfoAsync(string title, string message);
        /// <summary>
        /// Displays a validation error dialog to the user with the specified title, 
        /// a list of error messages, and a list of warning messages.
        /// </summary>
        /// <param name="title">The title of the validation error dialog.</param>
        /// <param name="errors">A collection of error messages to display in the dialog.</param>
        /// <param name="warnings">A collection of warning messages to display in the dialog.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ShowValidationErrorsAsync(string title, IEnumerable<string> errors, IEnumerable<string> warnings);
        /// <summary>
        /// Displays a confirmation dialog to the user with the specified title, message, and customizable confirmation and cancellation button texts.
        /// </summary>
        /// <param name="title">The title of the confirmation dialog.</param>
        /// <param name="message">The message to display in the confirmation dialog.</param>
        /// <param name="confirmText">The text to display on the confirmation button. Defaults to "Yes".</param>
        /// <param name="cancelText">The text to display on the cancellation button. Defaults to "No".</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the user confirmed (true) or canceled (false) the action.</returns>
        Task<bool> ShowConfirmationAsync(string title, string message, string confirmText = "Yes", string cancelText = "No");
    }
}
