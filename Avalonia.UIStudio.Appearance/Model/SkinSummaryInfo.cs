using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia.UIStudio.Appearance.Model
{

    /// <summary>
    /// Represents information about a skin, including its name, description, and a preview color.
    /// </summary>
    /// <remarks>
    /// This class is used to encapsulate the details of a skin, which can be displayed in the UI
    /// or used for skin management purposes within the application.
    /// </remarks>
    public class SkinSummaryInfo
    {
        /// <summary>
        /// Gets or sets the name of the skin.
        /// </summary>
        /// <remarks>
        /// The name uniquely identifies the skin and is used for selection and application purposes.
        /// </remarks>
        public string Name { get; set; } = "";
        /// <summary>
        /// Gets or sets the description of the skin.
        /// </summary>
        /// <remarks>
        /// This property provides a textual description of the skin, which can be displayed in the user interface
        /// to give users more context about the skin's purpose or appearance.
        /// </remarks>
        public string Description { get; set; } = "";
        /// <summary>
        /// Gets or sets the brush used to represent the preview color of the skin.
        /// </summary>
        /// <remarks>
        /// This property is typically used to display a visual representation of the skin's accent color
        /// in the user interface, such as in skin selection controls.
        /// </remarks>
        public IBrush PreviewColor { get; set; } = Brushes.Transparent;
    }
}
