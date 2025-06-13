using Avalonia.UIStudio.Appearance.Model;

namespace Avalonia.UIStudio.Appearance.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISkinValidationRule
    {
        List<SkinValidationMessage> Validate(Skin skin);
    }

}
