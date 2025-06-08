namespace Avalonia.Accelerate.Appearance.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class SkinImportResult
    {
        /// <summary>
        /// 
        /// </summary>
        public Skin? Skin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? ErrorMessage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> Warnings { get; set; } = new();
    }
}
