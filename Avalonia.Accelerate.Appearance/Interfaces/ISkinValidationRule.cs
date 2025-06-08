using Avalonia.Accelerate.Appearance.Model;

namespace Avalonia.Accelerate.Appearance.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISkinValidationRule
    {
        /// <summary>
        /// 
        /// </summary>
        SkinValidationResult Validate(Skin skin);
    }
}
