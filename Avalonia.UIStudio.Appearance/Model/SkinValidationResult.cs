namespace Avalonia.UIStudio.Appearance.Model
{
    /// <summary>
    /// Represents the result of skin validation.
    /// </summary>
    public class SkinValidationResult
    {
        public bool IsValid { get; set; } = true;

        public List<SkinValidationMessage> ValidationMessages { get; set; } = new();

        public List<string> Errors => ValidationMessages
            .Where(v => v.IsError)
            .Select(v => v.Message)
            .ToList();

        public List<string> Warnings => ValidationMessages
            .Where(v => !v.IsError)
            .Select(v => v.Message)
            .ToList();
    }
}
