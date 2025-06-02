using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Services;

namespace SCMM.Web.Client.Shared.Dialogs;

public abstract class ResponsiveDialog : ComponentBase, IBrowserViewportObserver, IAsyncDisposable
{
    [Inject]
    private IBrowserViewportService BrowserViewportService { get; set; }

    [CascadingParameter]
    protected IMudDialogInstance Dialog { get; set; }

    protected abstract ResponsiveDialogOptions DialogOptions { get; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await Dialog.SetOptionsAsync(DialogOptions);
        await BrowserViewportService.SubscribeAsync(this, fireImmediately: true);
    }

    public async ValueTask DisposeAsync()
    {
        await BrowserViewportService.UnsubscribeAsync(this);
    }

    Guid IBrowserViewportObserver.Id { get; } = Guid.NewGuid();

    ResizeOptions IBrowserViewportObserver.ResizeOptions { get; } = new()
    {
        ReportRate = 250,
        NotifyOnBreakpointOnly = true
    };

    async Task IBrowserViewportObserver.NotifyBrowserViewportChangeAsync(BrowserViewportEventArgs browserViewportEventArgs)
    {
        if (await BrowserViewportService.IsBreakpointWithinReferenceSizeAsync(DialogOptions.FullscreenBreakpoint, browserViewportEventArgs.Breakpoint))
        {
            await Dialog.SetOptionsAsync(Dialog.Options with
            {
                FullScreen = true
            });
        }
        else
        {
            await Dialog.SetOptionsAsync(Dialog.Options with
            {
                FullScreen = false
            });
        }

        await InvokeAsync(StateHasChanged);
    }
}