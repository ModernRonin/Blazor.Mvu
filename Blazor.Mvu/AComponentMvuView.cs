namespace Blazor.Mvu;

using Microsoft.AspNetCore.Components;

public abstract class AComponentMvuView<TState, TUpdater, TValue> : AnMvuView<TState, TUpdater>
    where TState : IComponentState<TValue>
    where TUpdater : IComponentUpdater<TState, TValue>
{
    protected sealed override TState State { get; set; }
    protected sealed override Task OnStateChangedAsync() => ValueChanged.InvokeAsync(State.Value);

    protected sealed override Task<TState> InitializeStateAsync(TUpdater updater) =>
        updater.InitializeAsync(Value);

    [Parameter]
    public TValue Value { get; set; }

    [Parameter]
    public EventCallback<TValue> ValueChanged { get; set; }
}