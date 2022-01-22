namespace Blazor.Mvu;

public interface IUpdater<TState>
{
    Task<TState> UpdateAsync(TState state, object msg);
}