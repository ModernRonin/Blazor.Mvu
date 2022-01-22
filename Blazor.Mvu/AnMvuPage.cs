namespace Blazor.Mvu;

using Microsoft.AspNetCore.Components;

public abstract class AnMvuPage<TState, TUpdater, TAppState> : AnMvuView<TState, TUpdater>
    where TUpdater : IPageUpdater<TState, TAppState>
{
    protected sealed override TState State { get; set; }

    protected sealed override Task<TState> InitializeStateAsync(TUpdater updater) =>
        updater.InitializeAsync(ApplicationState);

    [CascadingParameter]
    public TAppState ApplicationState { get; set; }
}