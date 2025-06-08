using System.Text;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Avalonia.Accelerate.Dialogs.Dialogs
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ErrorDialog : Window
    {
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public Exception? Exception { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ErrorDialog()
        {
            InitializeComponent();
            CloseButton.Click += CloseButton_Click;
            CopyButton.Click += CopyButton_Click;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);
            UpdateContent();
        }

        private void UpdateContent()
        {
            // Update title
            if (!string.IsNullOrEmpty(Title))
            {
                TitleText.Text = Title;
            }

            // Update message
            MessageText.Text = Message;

            // Update exception details
            if (Exception != null)
            {
                ExceptionExpander.IsVisible = true;
                CopyButton.IsVisible = true;
                ExceptionText.Text = FormatException(Exception);
            }
        }

        private string FormatException(Exception ex)
        {
            var sb = new StringBuilder();

            var currentEx = ex;
            var level = 0;

            while (currentEx != null)
            {
                if (level > 0)
                {
                    sb.AppendLine();
                    sb.AppendLine($"--- Inner Exception {level} ---");
                }

                sb.AppendLine($"Type: {currentEx.GetType().FullName}");
                sb.AppendLine($"Message: {currentEx.Message}");

                if (!string.IsNullOrEmpty(currentEx.StackTrace))
                {
                    sb.AppendLine("Stack Trace:");
                    sb.AppendLine(currentEx.StackTrace);
                }

                currentEx = currentEx.InnerException;
                level++;
            }

            return sb.ToString();
        }

        private void CloseButton_Click(object? sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void CopyButton_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                var clipboard = GetTopLevel(this)?.Clipboard;
                if (clipboard != null)
                {
                    var details = new StringBuilder();
                    details.AppendLine($"Error: {Title}");
                    details.AppendLine($"Message: {Message}");
                    details.AppendLine($"Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

                    if (Exception != null)
                    {
                        details.AppendLine();
                        details.AppendLine("Exception Details:");
                        details.AppendLine(FormatException(Exception));
                    }

                    await clipboard.SetTextAsync(details.ToString());

                    // Briefly show feedback
                    var originalText = CopyButton.Content?.ToString();
                    CopyButton.Content = "Copied!";
                    await Task.Delay(1000);
                    CopyButton.Content = originalText;
                }
            }
            catch
            {
                // Ignore clipboard errors
            }
        }
    }
}