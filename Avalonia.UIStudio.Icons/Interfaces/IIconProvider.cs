// Icons/IIconProvider.cs

using Geometry = Avalonia.Media.Geometry;

namespace Avalonia.UIStudio.Icons.Interfaces
{
    /// <summary>
    /// Defines a contract for providing icons in the form of <see cref="Avalonia.Media.Geometry"/>.
    /// </summary>
    public interface IIconProvider
    {
        /// <summary>
        /// Retrieves the geometry representation of an icon based on its name.
        /// </summary>
        /// <param name="iconName">
        /// The name of the icon to retrieve.
        /// </param>
        /// <returns>
        /// A <see cref="Avalonia.Media.Geometry"/> object representing the specified icon.
        /// </returns>
        Geometry GetIcon(string iconName);
        /// <summary>
        /// Retrieves the geometry representation of an icon based on the file type of the specified file name.
        /// </summary>
        /// <param name="fileName">
        /// The name of the file for which the file type icon is to be retrieved. 
        /// This can include the file extension to determine the appropriate icon.
        /// </param>
        /// <returns>
        /// A <see cref="Avalonia.Media.Geometry"/> object representing the icon associated with the file type.
        /// </returns>0
        Geometry GetFileTypeIcon(string fileName);
        /// <summary>
        /// Retrieves the geometry representation of a folder icon.
        /// </summary>
        /// <param name="isAccessible">
        /// A boolean value indicating whether the folder is accessible. 
        /// If <c>true</c>, the accessible folder icon is returned; otherwise, a locked folder icon is returned.
        /// </param>
        /// <returns>
        /// A <see cref="Avalonia.Media.Geometry"/> object representing the folder icon.
        /// </returns>
        Geometry GetFolderIcon(bool isAccessible = true);
        /// <summary>
        /// Retrieves a collection of available icon names.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> of <see cref="string"/> containing the names of all available icons.
        /// </returns>
        IEnumerable<string> GetAvailableIcons();
        /// <summary>
        /// Determines whether an icon with the specified name exists.
        /// </summary>
        /// <param name="iconName">The name of the icon to check for existence.</param>
        /// <returns>
        /// <c>true</c> if an icon with the specified name exists; otherwise, <c>false</c>.
        /// </returns>
        bool HasIcon(string iconName);
    }
}