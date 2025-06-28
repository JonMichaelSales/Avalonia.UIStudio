using Avalonia.UIStudio.Appearance.Interfaces;
using Avalonia.UIStudio.Appearance.Model;
using System.Reflection;

namespace Avalonia.UIStudio.Appearance.Services.ValidationRules
{
    public class SkinCompletenessValidationRule : ISkinValidationRule
    {
        public List<SkinValidationMessage> Validate(Skin skin)
        {
            var messages = new List<SkinValidationMessage>();

            if (skin == null)
            {
                messages.Add(new SkinValidationMessage
                {
                    IsError = true,
                    Message = "Skin object is null",
                    InvolvedProperties = new List<string>()
                });
                return messages;
            }

            ValidateObject(skin, "Skin", messages, new HashSet<object>());
            return messages;
        }

        private void ValidateObject(object obj, string path, List<SkinValidationMessage> messages, HashSet<object> visited)
        {
            if (obj == null || visited.Contains(obj))
                return;

            visited.Add(obj);

            var type = obj.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                if (prop.GetIndexParameters().Length > 0)
                    continue;

                var value = prop.GetValue(obj);
                var propPath = $"{path}.{prop.Name}";

                if (value == null && prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
                {
                    messages.Add(new SkinValidationMessage
                    {
                        IsError = true,
                        Message = $"Property '{propPath}' is null.",
                        InvolvedProperties = new List<string> { propPath },
                        SuggestedValues = new Dictionary<string, object?> { [propPath] = GetDefaultValue(prop.PropertyType) }
                    });
                }
                else if (value != null && IsComplexType(prop.PropertyType))
                {
                    ValidateObject(value, propPath, messages, visited);
                }
            }
        }

        private bool IsComplexType(Type type)
        {
            return type.IsClass && type != typeof(string);
        }

        private object? GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}
