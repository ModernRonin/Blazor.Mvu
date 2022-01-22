namespace Blazor.Mvu;

public interface IApplicationUpdater<TState> : IUpdater<TState>
{
    Task<TState> InitializeAsync();
}