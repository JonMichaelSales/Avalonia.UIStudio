using Avalonia.UIStudio.Appearance.Model;

namespace Avalonia.UIStudio.Appearance.Interfaces
{
    public interface ISkinImportExportService
    {
        Task<bool> ExportSkinAsync(Skin skin, string filePath, string? description = null, string? author = null);
        Task<bool> ExportAdvancedSkinAsync(Skin skin, string filePath, string? description = null, string? author = null);
        Task<bool> ExportInheritableSkinAsync(InheritableSkin skin, string filePath, string? description = null, string? author = null);
        Task<SkinImportResult> ImportSkinAsync(string filePath);
        Task<Skin?> ImportAdvancedSkinAsync(string filePath);
        Task<InheritableSkin?> ImportInheritableSkinAsync(string filePath);
        Task<SkinValidationResult> ValidateSkinFileAsync(string filePath);
        Task<bool> ExportSkinPackAsync(Dictionary<string, Skin> skins, string filePath, string packName, string? description = null);
    }
}