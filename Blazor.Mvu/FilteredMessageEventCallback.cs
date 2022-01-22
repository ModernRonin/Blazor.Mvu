namespace Blazor.Mvu;

using Microsoft.AspNetCore.Components;

public class FilteredMessageEventCallback<T>
{
    readonly Func<T, bool> _predicate;
    readonly IMessageReceiver _receiver;

    public FilteredMessageEventCallback(IMessageReceiver receiver) : this(receiver, _ => true) { }

    public FilteredMessageEventCallback(IMessageReceiver receiver, Func<T, bool> predicate)
    {
        _receiver = receiver;
        _predicate = predicate;
    }

    public EventCallback<T> Msg(object msg,
        bool doPropagateOutsideComponent = true)
    {
        return new EventCallback<T>(null, send);

        async Task send(T x)
        {
            if (_predicate(x))
            {
                await _receiver.SendMessage(msg, doPropagateOutsideComponent)
                    .InvokeAsync();
            }
        }
    }

    public EventCallback<T> Msg(Func<T, object> msgFactory,
        bool doPropagateOutsideComponent = true)
    {
        return new EventCallback<T>(null, send);

        async Task send(T x)
        {
            if (_predicate(x))
            {
                var msg = msgFactory(x);
                await _receiver.SendMessage(msg, doPropagateOutsideComponent)
                    .InvokeAsync();
            }
        }
    }
}