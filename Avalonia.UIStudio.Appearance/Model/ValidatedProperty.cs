using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace Avalonia.UIStudio.Appearance.Model
{
    public class ValidatedProperty
    {
        public SkinValidationMessage OriginalMessage { get; set; }
        public string Name { get; set; }
        public bool IsValid { get; set; }
        public object Value { get; set; }
        
        public object SuggestedValue { get; set; }
        
        public string Message { get; set; }

    }
    

    public class ValidatableProperty<T> : ReactiveObject
    {
        public PropertyValidationResult ValidationResult { get; } = new();

        private T _value;
        public T Value
        {
            get => _value;
            set
            {
                this.RaiseAndSetIfChanged(ref _value, value);
                ValidationResult.Value = value;
            }
        }

        public ValidatableProperty(string propertyName, T initialValue)
        {
            ValidationResult.PropertyName = propertyName;
            _value = initialValue;
            ValidationResult.Value = initialValue;
        }
    }

}
