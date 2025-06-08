using Avalonia.Accelerate.Dialogs.Models;
using Avalonia.Accelerate.Dialogs.Utility;
using Avalonia.Accelerate.Icons;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace Avalonia.Accelerate.Dialogs.Dialogs
{
    /// <summary>
    /// A dialog for displaying notification messages with different types (Information, Warning, Error).
    /// </summary>
    public partial class NotificationDialog : Window
    {
        public const string ErrorText = "Error";
        public const string WarningText = "Warning";
        public const string InformationText = "Information";
        
        
        /// <summary>
        /// Gets or sets the message to display in the dialog.
        /// </summary>
        public string Message { get; set; } = "";
        
        ///StyledProperty for AdditionalContent
        
        public static readonly StyledProperty<Control?> AdditionalContentProperty =
            AvaloniaProperty.Register<NotificationDialog, Control?>(nameof(AdditionalContent));
        
        /// <summary>
        /// Gets or sets the type of notification dialog.
        /// </summary>
        public NotificationDialogType DialogType { get; set; } = NotificationDialogType.Information;

        /// <summary>
        /// Initializes a new instance of the NotificationDialog class.
        /// </summary>
        public NotificationDialog()
        {
            InitializeComponent();
            OkButton.Click += OkButton_Click;
        }

        /// <summary>
        /// Called when the window is opened to configure the dialog based on its type.
        /// </summary>
        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);
            ConfigureDialogType();
            UpdateContent();
        }

        private void ConfigureDialogType()
        {
            switch (DialogType)
            {
                case NotificationDialogType.Information:
                    Title = TitleText.Text = InformationText;
                    HeaderIcon.Data = ApplicationIcons.InformationGeometry;
                    HeaderBorder.Background = WindowTools.GetMainWindow()!.FindResource("AccentBlueBrush") as IBrush;
                    break;

                case NotificationDialogType.Warning:
                    Title = TitleText.Text = WarningText;
                    HeaderIcon.Data = ApplicationIcons.WarningGeometry;
                    HeaderBorder.Background = WindowTools.GetMainWindow()!.FindResource("WarningBrush") as IBrush;
                    break;

                case NotificationDialogType.Error:
                    Title = TitleText.Text = ErrorText;
                    HeaderIcon.Data = ApplicationIcons.ErrorGeometry;
                    HeaderBorder.Background = WindowTools.GetMainWindow()!.FindResource("ErrorBrush") as IBrush;
                    break;
            }
        }

        private void UpdateContent()
        {
            MessageText.Text = Message;
        }

        private void OkButton_Click(object? sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}