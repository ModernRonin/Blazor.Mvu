namespace Blazor.Mvu;

public abstract class AComponentUpdater<TState, TValue> : AnUpdater<TState>, IComponentUpdater<TState, TValue>
    where TState : IComponentState<TValue>
{
    public Task<TState> InitializeAsync(TValue argument) => Task.FromResult(Initialize(argument));
    public virtual TState Initialize(TValue argument) => default;
}