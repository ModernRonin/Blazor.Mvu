namespace Blazor.Mvu;

using Microsoft.AspNetCore.Components;

public abstract class AStandardComponentMvuView<TState, TUpdater, TValue>
    : AComponentMvuView<TState, TUpdater, TValue>
    where TState : IComponentState<TValue>
    where TUpdater : IComponentUpdater<TState, TValue>
{
    protected sealed override TUpdater CreateUpdater() => UpdaterFactory();

    [Inject]
    Func<TUpdater> UpdaterFactory { get; set; }
}