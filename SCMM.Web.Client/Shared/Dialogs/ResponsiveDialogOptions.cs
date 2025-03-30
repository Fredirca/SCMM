using MudBlazor;

namespace SCMM.Web.Client.Shared.Dialogs;

public record ResponsiveDialogOptions : DialogOptions
{
    public Breakpoint FullscreenBreakpoint { get; init; } = Breakpoint.Sm;
}