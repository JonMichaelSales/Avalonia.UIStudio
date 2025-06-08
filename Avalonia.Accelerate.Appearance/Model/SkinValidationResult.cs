namespace Avalonia.Accelerate.Appearance.Model
{
    /// <summary>
    /// Represents the result of theme validation.
    /// </summary>
    public class SkinValidationResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the theme validation was successful.
        /// </summary>
        /// <value>
        /// <c>true</c> if the theme validation passed without errors; otherwise, <c>false</c>.
        /// </value>
        public bool IsValid { get; set; }
        /// <summary>
        /// Gets or sets the list of error messages encountered during theme validation.
        /// </summary>
        /// <value>
        /// A <see cref="List{T}"/> of <see cref="string"/> representing the validation errors.
        /// </value>
        public List<string> Errors { get; set; } = new();
        /// <summary>
        /// Gets or sets the list of warnings encountered during theme validation.
        /// </summary>
        /// <value>
        /// A list of warning messages that provide additional information about potential issues
        /// in the theme that do not necessarily invalidate it.
        /// </value>
        public List<string> Warnings { get; set; } = new();

        /// <summary>
        /// Adds an error message to the list of validation errors.
        /// </summary>
        /// <param name="error">The error message to add.</param>
        public void AddError(string error) => Errors.Add(error);
        /// <summary>
        /// Adds a warning message to the list of validation warnings.
        /// </summary>
        /// <param name="warning">The warning message to add.</param>
        public void AddWarning(string warning) => Warnings.Add(warning);
    }
}
