namespace Blazor.Mvu;

public abstract class AnUpdater<TState> : IUpdater<TState>
{
    public virtual Task<TState> UpdateAsync(TState state, object msg) => Task.FromResult(Update(state, msg));
    public virtual TState Update(TState state, object msg) => default;

    protected Task<TState> DontProcess(object msg) =>
        Task.FromResult<TState>(msg switch
        {
            _ => default
        });
}