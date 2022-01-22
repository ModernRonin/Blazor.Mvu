namespace Blazor.Mvu;

public interface IComponentState<out TValue>
{
    TValue Value { get; }
}