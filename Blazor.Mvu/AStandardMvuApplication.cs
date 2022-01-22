namespace Blazor.Mvu;

using Microsoft.AspNetCore.Components;

public abstract class AStandardMvuApplication<TState, TUpdater> : AnMvuApplication<TState, TUpdater>
    where TUpdater : IApplicationUpdater<TState>
{
    protected sealed override TUpdater CreateUpdater() => UpdaterFactory();

    [Inject]
    Func<TUpdater> UpdaterFactory { get; set; }
}