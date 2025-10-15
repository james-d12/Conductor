using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Conductor.Portal.Web.Components.Layout;

public partial class MainLayout
{
    private bool _isDarkMode = true;
    private MudTheme? _theme;

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        _theme = new MudTheme
        {
            PaletteLight = _lightPalette,
            PaletteDark = _darkPalette,
            LayoutProperties = new LayoutProperties()
        };
    }

    private void DarkModeToggle()
    {
        _isDarkMode = !_isDarkMode;
    }

    private readonly PaletteLight _lightPalette = new()
    {
        Primary = "#001e58",
        Secondary = "#001e58",
        Tertiary = "#ff4081",
        Black = "#110e2d",
        AppbarText = "#424242",
        AppbarBackground = "rgba(255,255,255,0.8)",
        DrawerBackground = "#ffffff",
        GrayLight = "#e8e8e8",
        GrayLighter = "#f9f9f9",
    };

    private readonly PaletteDark _darkPalette = new()
    {
        Primary = "#111827",
        Secondary = "#fed120",
        Tertiary = "#ff4081",
        Surface = "#1f2937",
        Background = "#171e2c",
        BackgroundGray = "#0f172a",
        AppbarText = "#f3f4f6",
        AppbarBackground = "rgba(31,41,55,0.9)",
        DrawerBackground = "#1f2937",
        ActionDefault = "#9ca3af",
        ActionDisabled = "#64748b",
        ActionDisabledBackground = "#374151",
        TextPrimary = "#ffffff",
        TextSecondary = "#e5e7eb",
        TextDisabled = "#94a3b8",
        DrawerIcon = "#e5e7eb",
        DrawerText = "#f3f4f6",
        GrayLight = "#374151",
        GrayLighter = "#4b5563",
        Info = "#3b82f6",
        Success = "#10b981",
        Warning = "#f59e0b",
        Error = "#ef4444",
        LinesDefault = "#374151",
        TableLines = "#374151",
        Divider = "#4b5563",
        OverlayLight = "#11182780",
    };

    public string DarkLightModeButtonIcon => _isDarkMode switch
    {
        true => Icons.Material.Filled.LightMode,
        false => Icons.Material.Filled.DarkMode,
    };
}