namespace Blazor.Mvu;

public interface IComponentUpdater<TState, in TValue> : IUpdater<TState>
    where TState : IComponentState<TValue>
{
    Task<TState> InitializeAsync(TValue argument);
}