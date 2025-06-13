using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.UIStudio.Dialogs.Utility;

namespace Avalonia.UIStudio.Dialogs.Dialogs
{
    /// <summary>
    /// A dialog for requesting user confirmation with customizable buttons.
    /// </summary>
    public partial class ConfirmationDialog : Window
    {
        /// <summary>
        /// Gets or sets the message to display in the confirmation dialog.
        /// </summary>
        public string Message { get; set; } = "";

        /// <summary>
        /// Gets or sets the text for the confirm button.
        /// </summary>
        public string ConfirmText { get; set; } = "Yes";

        /// <summary>
        /// Gets or sets the text for the cancel button.
        /// </summary>
        public string CancelText { get; set; } = "No";

        /// <summary>
        /// Gets the result of the dialog interaction.
        /// </summary>
        public bool? DialogResult { get; private set; }

        /// <summary>
        /// Initializes a new instance of the ConfirmationDialog class.
        /// </summary>
        public ConfirmationDialog()
        {
            InitializeComponent();
            ConfirmButton.Click += ConfirmButton_Click;
            CancelButton.Click += CancelButton_Click;
        }

        /// <summary>
        /// Called when the window is opened to update the dialog content.
        /// </summary>
        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);
            UpdateContent();
        }

        private void UpdateContent()
        {
            MessageText.Text = Message;
            ConfirmButton.Content = ConfirmText;
            CancelButton.Content = CancelText;
        }

        private void ConfirmButton_Click(object? sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close(true);
        }

        private void CancelButton_Click(object? sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close(false);
        }

        /// <summary>
        /// Shows the confirmation dialog and returns the user's choice.
        /// </summary>
        /// <param name="owner">The parent window</param>
        /// <returns>True if confirmed, false if cancelled, null if closed without choice</returns>
        public new async Task<bool?> ShowDialog(Window? owner = null)
        {
            if (owner != null)
            {
                return await ShowDialog<bool?>(owner);
            }
            else
            {
                return await ShowDialog<bool?>(WindowTools.GetMainWindow()!);
            }
        }
    }
}