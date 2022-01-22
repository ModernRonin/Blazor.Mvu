namespace Blazor.Mvu;

using Microsoft.AspNetCore.Components;

public abstract class ABehaviorlessView<TValue> : ComponentBase
{
    [Parameter]
    public TValue Value { get; set; }
}