using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;

namespace Avalonia.UIStudio.Appearance.Model
{
    public class SkinValidationMessage
    {
        public bool IsError { get; set; }
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Names of properties involved in this validation message.
        /// </summary>
        public List<string> InvolvedProperties { get; set; } = new();

        /// <summary>
        /// Suggested values for involved properties (if applicable).
        /// Example:
        /// { "PrimaryTextColor" => Color }
        /// { "FontSizeSmall" => 12.0 }
        /// { "Name" => "My Skin Name" }
        /// </summary>
        public Dictionary<string, object?> SuggestedValues { get; set; } = new();
    }


}
