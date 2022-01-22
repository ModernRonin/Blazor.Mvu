namespace Blazor.Mvu;

public interface IPageUpdater<TState, in TAppState> : IUpdater<TState>
{
    Task<TState> InitializeAsync(TAppState appState);
}