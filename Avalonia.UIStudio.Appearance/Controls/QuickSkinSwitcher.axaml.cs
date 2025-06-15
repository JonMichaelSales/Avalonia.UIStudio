using Avalonia.Controls;
using Avalonia.UIStudio.Appearance.Extensions;
using Avalonia.UIStudio.Appearance.Interfaces;
using Avalonia.UIStudio.Appearance.ViewModels;

namespace Avalonia.UIStudio.Appearance.Controls;

/// <summary>
///     Represents a user control that provides a quick skin switching functionality
///     for Avalonia applications. This control is designed to integrate seamlessly
///     with the Avalonia UI framework and is backed by the <see cref="QuickSkinSwitcherViewModel" />.
/// </summary>
public partial class QuickSkinSwitcher : UserControl
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="QuickSkinSwitcher" /> class.
    ///     This constructor sets up the control by initializing its components and
    ///     assigning a new instance of <see cref="QuickSkinSwitcherViewModel" /> as its data context.
    /// </summary>
    public QuickSkinSwitcher()
    {
        InitializeComponent();
        DataContext = AppBuilderExtensions.GetRequiredService<IQuickSkinSwitcherViewModel>();
    }

    public QuickSkinSwitcher(ISkinManager skinManager)
    {
        InitializeComponent();
        DataContext = new QuickSkinSwitcherViewModel(skinManager);
    }

    public QuickSkinSwitcher(IQuickSkinSwitcherViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}