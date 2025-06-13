
// Icons/ApplicationIcons.cs - Enhanced version

using Avalonia.Media;
using Avalonia.UIStudio.Icons.Interfaces;

namespace Avalonia.UIStudio.Icons
{
    /// <summary>
    /// Centralized repository for SVG path data used in Path controls throughout the application.
    /// Provides extensive icon collection for file types, UI elements, and status indicators.
    /// </summary>
    public class ApplicationIcons : IIconProvider
    {
        private static readonly Dictionary<string, string> _iconCache = new();

        static ApplicationIcons()
        {
            InitializeIconCache();
        }

        #region Core System Icons
        /// <summary>
        /// Gets the SVG path data for the "File" icon, representing a generic file.
        /// </summary>
        public const string File = "M6,2C4.89,2 4,2.89 4,4V20A2,2 0 0,0 6,22H18A2,2 0 0,0 20,20V8L14,2H6Z M13,3.5L18.5,9H13V3.5Z";
        /// <summary>
        /// Gets the SVG path data for the "Folder" icon, representing a generic folder structure.
        /// </summary>
        public const string Folder = "M10,4H4C2.89,4 2,4.89 2,6V18A2,2 0 0,0 4,20H20A2,2 0 0,0 22,18V8C22,6.89 21.1,6 20,6H12L10,4Z";
        /// <summary>
        /// Gets the SVG path data for the "Folder Open" icon, representing an open folder structure.
        /// </summary>
        public const string FolderOpen = "M19,20H4C2.89,20 2,19.1 2,18V6C2,4.89 2.89,4 4,4H10L12,6H19A2,2 0 0,1 21,8H21L4,8V18L6.14,10H23.21L20.93,18.5C20.7,19.37 19.92,20 19,20Z";
        /// <summary>
        /// Gets the SVG path data for the "Lock" icon, representing a locked folder or file.
        /// </summary>
        public const string Lock = "M12,17A2,2 0 0,0 14,15C14,13.89 13.1,13 12,13A2,2 0 0,0 10,15A2,2 0 0,0 12,17M18,8A2,2 0 0,1 20,10V20A2,2 0 0,1 18,22H6A2,2 0 0,1 4,20V10C4,8.89 4.9,8 6,8H7V6A5,5 0 0,1 12,1A5,5 0 0,1 17,6V8H18M12,3A3,3 0 0,0 9,6V8H15V6A3,3 0 0,0 12,3Z";
        #endregion

        #region UI Navigation Icons
        /// <summary>
        /// Gets the SVG path data for the "Search" icon.
        /// </summary>
        public const string Search = "M9.5,3A6.5,6.5 0 0,1 16,9.5C16,11.11 15.41,12.59 14.44,13.73L14.71,14H15.5L20.5,19L19,20.5L14,15.5V14.71L13.73,14.44C12.59,15.41 11.11,16 9.5,16A6.5,6.5 0 0,1 3,9.5A6.5,6.5 0 0,1 9.5,3M9.5,5C7,5 5,7 5,9.5C5,12 7,14 9.5,14C12,14 14,12 14,9.5C14,7 12,5 9.5,5Z";
        /// <summary>
        /// Gets the SVG path data for the "Settings" icon.
        /// </summary>
        public const string Settings = "M12,15.5A3.5,3.5 0 0,1 8.5,12A3.5,3.5 0 0,1 12,8.5A3.5,3.5 0 0,1 15.5,12A3.5,3.5 0 0,1 12,15.5M19.43,12.97C19.47,12.65 19.5,12.33 19.5,12C19.5,11.67 19.47,11.34 19.43,11L21.54,9.37C21.73,9.22 21.78,8.95 21.66,8.73L19.66,5.27C19.54,5.05 19.27,4.96 19.05,5.05L16.56,6.05C16.04,5.66 15.5,5.32 14.87,5.07L14.5,2.42C14.46,2.18 14.25,2 14,2H10C9.75,2 9.54,2.18 9.5,2.42L9.13,5.07C8.5,5.32 7.96,5.66 7.44,6.05L4.95,5.05C4.73,4.96 4.46,5.05 4.34,5.27L2.34,8.73C2.22,8.95 2.27,9.22 2.46,9.37L4.57,11C4.53,11.34 4.5,11.67 4.5,12C4.5,12.33 4.53,12.65 4.57,12.97L2.46,14.63C2.27,14.78 2.22,15.05 2.34,15.27L4.34,18.73C4.46,18.95 4.73,19.03 4.95,18.95L7.44,17.94C7.96,18.34 8.5,18.68 9.13,18.93L9.5,21.58C9.54,21.82 9.75,22 10,22H14C14.25,22 14.46,21.82 14.5,21.58L14.87,18.93C15.5,18.68 16.04,18.34 16.56,17.94L19.05,18.95C19.27,19.03 19.54,18.95 19.66,18.73L21.66,15.27C21.78,15.05 21.73,14.78 21.54,14.63L19.43,12.97Z";
        /// <summary>
        /// Gets the SVG path data for the "Refresh" icon.
        /// </summary>
        public const string Refresh = "M17.65,6.35C16.2,4.9 14.21,4 12,4A8,8 0 0,0 4,12A8,8 0 0,0 12,20C15.73,20 18.84,17.45 19.73,14H17.65C16.83,16.33 14.61,18 12,18A6,6 0 0,1 6,12A6,6 0 0,1 12,6C13.66,6 15.14,6.69 16.22,7.78L13,11H20V4L17.65,6.35Z";
        /// <summary>
        /// Gets the SVG path data for the "Browse" icon.
        /// </summary>
        public const string Browse = "M10,4H4C2.89,4 2,4.89 2,6V18A2,2 0 0,0 4,20H20A2,2 0 0,0 22,18V8C22,6.89 21.1,6 20,6H12L10,4Z";
        /// <summary>
        /// Gets the SVG path data for the "Delete" icon.
        /// </summary>
        public const string Delete = "M19,4H15.5L14.5,3H9.5L8.5,4H5V6H19M6,19A2,2 0 0,0 8,21H16A2,2 0 0,0 18,19V7H6V19Z";
        #endregion

        #region Status Icons
        /// <summary>
        /// Gets the SVG path data for the "Information" icon.
        /// </summary>
        public const string Information = "M11,9H13V7H11M12,20C7.59,20 4,16.41 4,12C4,7.59 7.59,4 12,4C16.41,4 20,7.59 20,12C20,16.41 16.41,20 12,20M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2M11,17H13V11H11V17Z";
        /// <summary>
        /// Gets the SVG path data for the "Warning" icon.
        /// </summary>
        public const string Warning = "M13,14H11V10H13M13,18H11V16H13M1,21H23L12,2L1,21Z";
        /// <summary>
        /// Gets the SVG path data for the "Error" icon.
        /// </summary>
        public const string Error = "M12,2C17.53,2 22,6.47 22,12C22,17.53 17.53,22 12,22C6.47,22 2,17.53 2,12C2,6.47 6.47,2 12,2M15.59,7L12,10.59L8.41,7L7,8.41L10.59,12L7,15.59L8.41,17L12,13.41L15.59,17L17,15.59L13.41,12L17,8.41L15.59,7Z";
        /// <summary>
        /// Gets the SVG path data for the "Success" icon.
        /// </summary>
        public const string Success = "M12,2A10,10 0 0,1 22,12A10,10 0 0,1 12,22A10,10 0 0,1 2,12A10,10 0 0,1 12,2M11,16.5L18,9.5L16.59,8.09L11,13.67L7.91,10.59L6.5,12L11,16.5Z";
        #endregion

        #region Media and File Types
        /// <summary>
        /// Gets the SVG path data for the "Image" icon.
        /// </summary>
        public const string Image = "M8.5,13.5L11,16.5L14.5,12L19,18H5M21,19V5C21,3.89 20.1,3 19,3H5A2,2 0 0,0 3,5V19A2,2 0 0,0 5,21H19A2,2 0 0,0 21,19Z";
        /// <summary>
        /// Gets the SVG path data for the "Video" icon.
        /// </summary>
        public const string Video = "M17,10.5V7A1,1 0 0,0 16,6H4A1,1 0 0,0 3,7V17A1,1 0 0,0 4,18H16A1,1 0 0,0 17,17V13.5L21,17.5V6.5L17,10.5Z";
        /// <summary>
        /// Gets the SVG path data for the "Audio" icon.
        /// </summary>
        public const string Audio = "M12,3V13.55C11.41,13.21 10.73,13 10,13A4,4 0 0,0 6,17A4,4 0 0,0 10,21A4,4 0 0,0 14,17V7H18V3H12Z";
        /// <summary>
        /// Gets the SVG path data for the "Archive" icon.
        /// </summary>
        public const string Archive = "M14,17H12V15H10V13H12V15H14M14,9H12V7H14M10,9H12V11H10M10,13H12V11H14V13H12V15H10M8,9H10V7H8M16,9H14V11H16M16,15H14V13H16M16,17H14V15H16V17M14,2H6A2,2 0 0,0 4,4V20A2,2 0 0,0 6,22H18A2,2 0 0,0 20,20V8L14,2Z";
        /// <summary>
        /// Gets the SVG path data for the "Code" icon, representing code files or programming-related content.
        /// </summary>
        public const string Code = "M14.6,16.6L19.2,12L14.6,7.4L16,6L22,12L16,18L14.6,16.6M9.4,16.6L4.8,12L9.4,7.4L8,6L2,12L8,18L9.4,16.6Z";
        #endregion

        #region Document Types
        /// <summary>
        /// Gets the SVG path data for the "Word Document" icon, representing Microsoft Word documents.
        /// </summary>
        public const string WordDocument = "M6,2A2,2 0 0,0 4,4V20A2,2 0 0,0 6,22H18A2,2 0 0,0 20,20V8L14,2M13,3.5L18.5,9H13M8.5,11H10.36L11.13,16.21L12,11H13.67L14.54,16.21L15.31,11H17.17L15.54,19H13.67L12.84,14.5L12,19H10.13";
        /// <summary>
        /// Gets the SVG path data for the "Excel Document" icon, representing Microsoft Excel documents.
        /// </summary>
        public const string ExcelDocument = "M6,2A2,2 0 0,0 4,4V20A2,2 0 0,0 6,22H18A2,2 0 0,0 20,20V8L14,2M13,3.5L18.5,9H13M8,11V13H16V11M8,15V17H11V15M13,15V17H16V15M8,19V21H11V19M13,19V21H16V19";
        /// <summary>
        /// Gets the SVG path data for the "PowerPoint Document" icon, representing Microsoft PowerPoint documents.
        /// </summary>
        public const string PowerPointDocument = "M6,2A2,2 0 0,0 4,4V20A2,2 0 0,0 6,22H18A2,2 0 0,0 20,20V8L14,2M13,3.5L18.5,9H13M8,11H12A2,2 0 0,1 14,13V15A2,2 0 0,1 12,17H10V19H8M10,13V15H12V13";
        /// <summary>
        /// Gets the SVG path data for the "PDF Document" icon, representing PDF files.
        /// </summary>
        public const string PdfDocument = "M6,2A2,2 0 0,0 4,4V20A2,2 0 0,0 6,22H18A2,2 0 0,0 20,20V8L14,2M13,3.5L18.5,9H13M8,11H10A2,2 0 0,1 12,13V15A2,2 0 0,1 10,17H8M9,12V16H10A1,1 0 0,0 11,15V13A1,1 0 0,0 10,12M13,11H15V12H14V14H15V15H13V17H16V11H13V11Z";
        #endregion

        /// <summary>
        /// Initialize the icon cache with all available icons
        /// </summary>
        private static void InitializeIconCache()
        {
            // File system icons
            _iconCache["File"] = File;
            _iconCache["Folder"] = Folder;
            _iconCache["FolderOpen"] = FolderOpen;
            _iconCache["Lock"] = Lock;

            // Navigation icons
            _iconCache["Search"] = Search;
            _iconCache["Settings"] = Settings;
            _iconCache["Refresh"] = Refresh;
            _iconCache["Browse"] = Browse;
            _iconCache["Delete"] = Delete;

            // Status icons
            _iconCache["Information"] = Information;
            _iconCache["Warning"] = Warning;
            _iconCache["Error"] = Error;
            _iconCache["Success"] = Success;

            // Media icons
            _iconCache["Image"] = Image;
            _iconCache["Video"] = Video;
            _iconCache["Audio"] = Audio;
            _iconCache["Archive"] = Archive;
            _iconCache["Code"] = Code;

            // Document icons
            _iconCache["WordDocument"] = WordDocument;
            _iconCache["ExcelDocument"] = ExcelDocument;
            _iconCache["PowerPointDocument"] = PowerPointDocument;
            _iconCache["PdfDocument"] = PdfDocument;
        }

        #region IIconProvider Implementation
        /// <summary>
        /// Retrieves the geometry representation of an icon based on its name.
        /// </summary>
        /// <param name="iconName">
        /// The name of the icon to retrieve. If the icon is not found, a default icon is returned.
        /// </param>
        /// <returns>
        /// A <see cref="Geometry"/> object representing the specified icon.
        /// </returns>
        public Geometry GetIcon(string iconName)
        {
            if (_iconCache.TryGetValue(iconName, out string? pathData))
            {
                return Geometry.Parse(pathData);
            }
            return Geometry.Parse(File); // Default fallback
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Geometry GetFileTypeIcon(string fileName)
        {
            var pathData = GetFileTypePathData(fileName);
            return Geometry.Parse(pathData);
        }

        /// <summary>
        /// Retrieves the geometry representation of a folder icon.
        /// </summary>
        /// <param name="isAccessible">
        /// A boolean value indicating whether the folder is accessible. 
        /// If <c>true</c>, the accessible folder icon is returned; otherwise, a locked folder icon is returned.
        /// </param>
        /// <returns>
        /// A <see cref="Geometry"/> object representing the folder icon.
        /// </returns>
        public Geometry GetFolderIcon(bool isAccessible = true)
        {
            var pathData = isAccessible ? Folder : Lock;
            return Geometry.Parse(pathData);
        }

        /// <summary>
        /// Retrieves a collection of available icon names.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> of <see cref="string"/> containing the names of all available icons.
        /// </returns>
        public IEnumerable<string> GetAvailableIcons()
        {
            return _iconCache.Keys;
        }

        /// <summary>
        /// Determines whether an icon with the specified name exists in the icon cache.
        /// </summary>
        /// <param name="iconName">The name of the icon to check for existence.</param>
        /// <returns>
        /// <c>true</c> if the icon with the specified name exists in the cache; otherwise, <c>false</c>.
        /// </returns>
        public bool HasIcon(string iconName)
        {
            return _iconCache.ContainsKey(iconName);
        }
        #endregion

        #region Direct Geometry Properties (For XAML Binding)
        /// <summary>
        /// Gets the geometry representation of the file icon.
        /// </summary>
        /// <value>
        /// A <see cref="Geometry"/> object that represents the file icon.
        /// </value>
        /// <remarks>
        /// This property parses the predefined file path data into a <see cref="Geometry"/> object
        /// to be used in UI components.
        /// </remarks>
        public static Geometry FileGeometry => Geometry.Parse(File);
        /// <summary>
        /// Gets the geometry representation of a folder icon.
        /// </summary>
        /// <remarks>
        /// This property provides a parsed <see cref="Geometry"/> object for the folder icon.
        /// It is based on the predefined path data string for a folder.
        /// </remarks>
        public static Geometry FolderGeometry => Geometry.Parse(Folder);
        /// <summary>
        /// Gets the geometry representation of the "Browse" icon.
        /// </summary>
        /// <remarks>
        /// This property provides a parsed <see cref="Geometry"/> object for the "Browse" icon,
        /// which can be used in UI components to render the corresponding visual representation.
        /// </remarks>
        public static Geometry BrowseGeometry => Geometry.Parse(Browse);
        /// <summary>
        /// Gets the geometry representation of a lock icon.
        /// </summary>
        /// <remarks>
        /// This property provides a parsed <see cref="Geometry"/> object for the lock icon,
        /// which can be used in UI elements requiring vector graphics.
        /// </remarks>
        public static Geometry LockGeometry => Geometry.Parse(Lock);
        /// <summary>
        /// Gets the geometry representation of the "Information" icon.
        /// </summary>
        /// <value>
        /// A <see cref="Geometry"/> object representing the "Information" icon.
        /// </value>
        public static Geometry InformationGeometry => Geometry.Parse(Information);
        /// <summary>
        /// Gets the geometry representing a warning icon.
        /// </summary>
        /// <remarks>
        /// This property provides a pre-defined <see cref="Geometry"/> object that can be used
        /// to render a warning icon in the application.
        /// </remarks>
        public static Geometry WarningGeometry => Geometry.Parse(Warning);
        /// <summary>
        /// Gets the geometry representation of the "Error" icon.
        /// </summary>
        /// <remarks>
        /// This property provides a parsed <see cref="Geometry"/> object representing the "Error" icon.
        /// It can be used in UI elements to visually indicate an error state.
        /// </remarks>
        public static Geometry ErrorGeometry => Geometry.Parse(Error);
        /// <summary>
        /// Gets the geometry representing a "Success" icon.
        /// </summary>
        /// <value>
        /// A <see cref="Geometry"/> object that defines the shape of the "Success" icon.
        /// </value>
        public static Geometry SuccessGeometry => Geometry.Parse(Success);
        public static Geometry VideoGeometry => Geometry.Parse(Video);
        public static Geometry AudioGeometry => Geometry.Parse(Audio);
        /// <summary>
        /// Gets the <see cref="Geometry"/> representation of the "Refresh" icon.
        /// </summary>
        /// <value>
        /// A <see cref="Geometry"/> object representing the "Refresh" icon.
        /// </value>
        /// <remarks>
        /// This property parses the predefined path data for the "Refresh" icon into a <see cref="Geometry"/> object.
        /// </remarks>
        public static Geometry ImageGeometry => Geometry.Parse(Image);
        public static Geometry ArchiveGeometry => Geometry.Parse(Archive);
        public static Geometry CodeGeometry => Geometry.Parse(Code);
        public static Geometry WordDocumentGeometry => Geometry.Parse(WordDocument);
        public static Geometry ExcelDocumentGeometry => Geometry.Parse(ExcelDocument);
        public static Geometry PowerPointDocumentGeometry => Geometry.Parse(PowerPointDocument);
        public static Geometry PdfDocumentGeometry => Geometry.Parse(PdfDocument);
        public static Geometry RefreshGeometry => Geometry.Parse(Refresh);
        /// <summary>
        /// Gets the geometry representation of the "Search" icon.
        /// </summary>
        /// <value>
        /// A <see cref="Geometry"/> object that represents the "Search" icon.
        /// </value>
        public static Geometry SearchGeometry => Geometry.Parse(Search);
        /// <summary>
        /// Gets the geometry representation of the "Delete" icon.
        /// </summary>
        /// <value>
        /// A <see cref="Geometry"/> object that represents the "Delete" icon.
        /// </value>
        /// <remarks>
        /// This property provides a parsed geometry for the "Delete" icon, which can be used 
        /// in UI components such as paths or shapes in Avalonia applications.
        /// </remarks>
        public static Geometry DeleteGeometry => Geometry.Parse(Delete);
        /// <summary>
        /// Gets the geometry data representing the "Settings" icon.
        /// </summary>
        /// <remarks>
        /// This property provides a parsed <see cref="Geometry"/> object for the "Settings" icon.
        /// It can be used in UI components, such as <see cref="Avalonia.Controls.Shapes.Path"/>, 
        /// to render the "Settings" icon.
        /// </remarks>
        /// <value>
        /// A <see cref="Geometry"/> object representing the "Settings" icon.
        /// </value>
        public static Geometry SettingsGeometry => Geometry.Parse(Settings);
        #endregion

        /// <summary>
        /// Gets the SVG path data string for file type-specific icons
        /// </summary>
        /// <param name="fileName">The file name including extension</param>
        /// <returns>SVG path data string for the appropriate file type icon</returns>
        public static string GetFileTypePathData(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return File;

            var extension = Path.GetExtension(fileName).ToLowerInvariant();

            return extension switch
            {
                // Microsoft Office Documents
                ".docx" or ".doc" or ".dotx" or ".dot" => WordDocument,
                ".xlsx" or ".xls" or ".xlsm" or ".xltx" or ".xlt" => ExcelDocument,
                ".pptx" or ".ppt" or ".potx" or ".pot" => PowerPointDocument,
                ".pdf" => PdfDocument,

                // Media Files
                ".jpg" or ".jpeg" or ".png" or ".gif" or ".bmp" or ".tiff" or ".svg" or ".webp" or ".ico" => Image,
                ".mp4" or ".avi" or ".mkv" or ".mov" or ".wmv" or ".flv" or ".webm" or ".m4v" => Video,
                ".mp3" or ".wav" or ".flac" or ".aac" or ".ogg" or ".m4a" or ".wma" => Audio,

                // Archives
                ".zip" or ".rar" or ".7z" or ".tar" or ".gz" or ".bz2" or ".xz" => Archive,

                // Code Files
                ".cs" or ".js" or ".ts" or ".html" or ".css" or ".json" or ".xml" or ".xaml" or
                ".py" or ".java" or ".cpp" or ".c" or ".h" or ".php" or ".rb" or ".go" or ".rs" => Code,

                // Default fallback
                _ => File
            };
        }

        /// <summary>
        /// Create a styled Path control with the specified icon
        /// </summary>
        /// <param name="iconName">Name of the icon</param>
        /// <param name="size">Size of the icon (default: 16)</param>
        /// <param name="brush">Brush for the icon (default: null - uses skin brush)</param>
        /// <returns>Configured Path control</returns>
        public Avalonia.Controls.Shapes.Path CreateIconPath(string iconName, double size = 16, IBrush? brush = null)
        {
            return new Avalonia.Controls.Shapes.Path
            {
                Data = GetIcon(iconName),
                Width = size,
                Height = size,
                Stretch = Stretch.Uniform,
                Fill = brush ?? new SolidColorBrush(Avalonia.Media.Colors.White), // Default to black if no brush is provided
            };
        }
    }
}