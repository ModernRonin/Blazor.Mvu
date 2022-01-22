namespace Blazor.Mvu;

using Microsoft.AspNetCore.Components;

public abstract class AnMvuApplication<TState, TUpdater> : AnMvuView<TState, TUpdater>
    where TUpdater : IApplicationUpdater<TState>
{
    protected AnMvuApplication() : base(true) { }

    protected sealed override TState State
    {
        get => ApplicationStateHolder.State;
        set => ApplicationStateHolder.State = value;
    }

    protected sealed override Task<TState> InitializeStateAsync(TUpdater updater) =>
        updater.InitializeAsync();

    [Inject]
    IApplicationStateHolder<TState> ApplicationStateHolder { get; set; }
}