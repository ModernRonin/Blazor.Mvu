namespace Blazor.Mvu;

using Microsoft.AspNetCore.Components;

public abstract class AStandardMvuPage<TState, TUpdater, TAppState>
    : AnMvuPage<TState, TUpdater, TAppState>
    where TUpdater : IPageUpdater<TState, TAppState>
{
    protected sealed override TUpdater CreateUpdater() => UpdaterFactory();

    [Inject]
    Func<TUpdater> UpdaterFactory { get; set; }
}