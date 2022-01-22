namespace Blazor.Mvu;

public abstract class APageUpdater<TState, TAppState> : AnUpdater<TState>, IPageUpdater<TState, TAppState>
{
    public Task<TState> InitializeAsync(TAppState appState) => Task.FromResult(Initialize(appState));
    public virtual TState Initialize(TAppState appState) => default;
}