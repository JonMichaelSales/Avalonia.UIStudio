namespace Avalonia.UIStudio.Appearance.Model;

public class PropertyValidationResult
{
    public string PropertyName { get; set; }
    public object? Value { get; set; }
    public object? SuggestedValue { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsValid { get; set; } = true;
    public bool IsError { get; set; } = false;
}