using Avalonia.Accelerate.Appearance.Model;

namespace Avalonia.Accelerate.Appearance.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISkinValidationRule
    {
        List<SkinValidationMessage> Validate(Skin skin);
    }

}
