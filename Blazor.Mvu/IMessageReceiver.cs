namespace Blazor.Mvu;

using Microsoft.AspNetCore.Components;

public interface IMessageReceiver
{
    EventCallback SendMessage(object msg, bool doPropagateOutsideComponent);
}