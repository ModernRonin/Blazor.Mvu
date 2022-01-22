namespace Blazor.Mvu;

public interface IApplicationStateHolder<TState>
{
    TState State { get; set; }
}