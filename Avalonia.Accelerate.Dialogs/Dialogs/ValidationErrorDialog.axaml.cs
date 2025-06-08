using Avalonia.Accelerate.Dialogs.Utility;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Avalonia.Accelerate.Dialogs.Dialogs
{
    /// <summary>
    /// A dialog for displaying validation errors and warnings in a structured format.
    /// </summary>
    public partial class ValidationErrorDialog : Window
    {
        /// <summary>
        /// Gets or sets the list of error messages to display.
        /// </summary>
        public List<string> Errors { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of warning messages to display.
        /// </summary>
        public List<string> Warnings { get; set; } = new();

        /// <summary>
        /// Initializes a new instance of the ValidationErrorDialog class.
        /// </summary>
        public ValidationErrorDialog()
        {
            InitializeComponent();
            CloseButton.Click += CloseButton_Click;
        }

        /// <summary>
        /// Called when the window is opened to populate the dialog with error and warning data.
        /// </summary>
        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);
            PopulateValidationResults();
        }

        private void PopulateValidationResults()
        {
            // Update counts
            ErrorCountText.Text = $"{Errors.Count} Error{(Errors.Count != 1 ? "s" : "")}";
            WarningCountText.Text = $"{Warnings.Count} Warning{(Warnings.Count != 1 ? "s" : "")}";

            // Populate errors
            if (Errors.Any())
            {
                ErrorsList.ItemsSource = Errors;
                ErrorsSection.IsVisible = true;
            }
            else
            {
                ErrorsSection.IsVisible = false;
            }

            // Populate warnings
            if (Warnings.Any())
            {
                WarningsList.ItemsSource = Warnings;
                WarningsSection.IsVisible = true;
            }
            else
            {
                WarningsSection.IsVisible = false;
            }

            // Update title based on content
            if (Errors.Any() && Warnings.Any())
            {
                Title = "Validation Errors and Warnings";
                TitleText.Text = "Validation Issues";
            }
            else if (Errors.Any())
            {
                Title = "Validation Errors";
                TitleText.Text = "Validation Errors";
            }
            else if (Warnings.Any())
            {
                Title = "Validation Warnings";
                TitleText.Text = "Validation Warnings";
                // Change header color for warnings-only dialog
                var headerBorder = this.FindControl<Border>("HeaderBorder");
                if (headerBorder != null)
                {
                    headerBorder.Background = WindowTools.GetMainWindow()!.FindResource("WarningBrush") as Avalonia.Media.IBrush;
                }
            }
            else
            {
                Title = "Validation Complete";
                TitleText.Text = "No Issues Found";
                // Change header color for success
                var headerBorder = this.FindControl<Border>("HeaderBorder");
                if (headerBorder != null)
                {
                    headerBorder.Background = WindowTools.GetMainWindow()!.FindResource("SuccessBrush") as Avalonia.Media.IBrush;
                }
            }
        }

        private void CloseButton_Click(object? sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Sets the validation results and updates the dialog display.
        /// </summary>
        /// <param name="errors">List of error messages</param>
        /// <param name="warnings">List of warning messages</param>
        public void SetValidationResults(IEnumerable<string> errors, IEnumerable<string> warnings)
        {
            Errors = errors.ToList();
            Warnings = warnings.ToList();

            if (IsVisible)
            {
                PopulateValidationResults();
            }
        }
    }
}