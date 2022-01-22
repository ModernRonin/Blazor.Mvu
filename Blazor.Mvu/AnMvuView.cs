namespace Blazor.Mvu;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

public abstract class AnMvuView<TState, TUpdater> : IComponent, IHandleAfterRender, IMvuView
    where TUpdater : IUpdater<TState>
{
    readonly RenderFragment _renderFragment;
    bool _hasPendingQueuedRender;
    bool _hasRenderedAtLeastOnce;
    Logger _logger;
    RenderHandle _renderHandle;
    TUpdater _updater;

    protected AnMvuView(bool doAddCascadingValueForState = false)
    {
        const int sequenceForRazorContent = -800;
        _renderFragment = builder =>
        {
            Logger.LogRender("start");
            _hasPendingQueuedRender = false;
            var typeForThis = typeof(CascadingValue<>).MakeGenericType(GetType());
            builder.OpenRegion(0);
            builder.OpenComponent(-1000, typeForThis);
            builder.AddAttribute(-999, "Value", this);
            if (doAddCascadingValueForState) renderTreeWithoutState(builder);
            else renderRazorContent(builder);

            builder.CloseComponent();
            builder.CloseRegion();
            if (ShowDebugState)
            {
                builder.OpenElement(1, "pre");
                builder.AddContent(2, Serializer?.Serialize(State));
                builder.CloseElement();
            }

            Logger.LogRender("finish");
        };

        void renderTreeWithoutState(RenderTreeBuilder builder)
        {
            Logger.LogRender("renderTreeWithoutState");
            builder.AddAttribute(-998, "ChildContent", (RenderFragment)renderTreeWithState);
        }

        void renderTreeWithState(RenderTreeBuilder builder)
        {
            Logger.LogRender("renderTreeWithState");
            var typeForState = typeof(CascadingValue<>).MakeGenericType(typeof(TState));
            builder.OpenComponent(-997, typeForState);
            builder.AddAttribute(-996, "Value", State);
            renderRazorContent(builder);
            builder.CloseComponent();
        }

        void renderRazorContent(RenderTreeBuilder builder) =>
            builder.AddAttribute(sequenceForRazorContent, "ChildContent", (RenderFragment)BuildRenderTree);
    }

    /// <summary>
    ///     Adjusted from <see cref="ComponentBase" />.
    /// </summary>
    void IComponent.Attach(RenderHandle renderHandle)
    {
        if (_renderHandle.IsInitialized)
        {
            throw new InvalidOperationException(
                $"The render handle is already set. Cannot initialize a {nameof(AnMvuView<TState, TUpdater>)} more than once.");
        }

        _renderHandle = renderHandle;
    }

    async Task IHandleAfterRender.OnAfterRenderAsync()
    {
        if (_hasRenderedAtLeastOnce) return;
        _hasRenderedAtLeastOnce = true;
        if (InitMsg != null) await Msg(InitMsg).InvokeAsync();
    }

    public EventCallback SendMessage(object msg, bool doPropagateOutsideComponent)
    {
        return new EventCallback(null, send);

        async Task send()
        {
            var newState = await _updater.UpdateAsync(State, msg);
            Logger.LogMessage(msg, State, newState);
            if (newState is null)
            {
                if (Owner != default)
                {
                    await Owner.SendMessage(msg, doPropagateOutsideComponent)
                        .InvokeAsync();
                }
                else Logger.Warning($"unhandled message {msg.GetType().Name}: {Serializer.Serialize(msg)}");
            }
            else if (State.Equals(newState))
            {
                // do nothing
            }
            else
            {
                State = newState;
                ReRender();
                if (doPropagateOutsideComponent) await OnStateChangedAsync();
            }
        }
    }

    async Task IComponent.SetParametersAsync(ParameterView parameters)
    {
        Logger.LogParameters(parameters);
        parameters.SetParameterProperties(this);
        _updater = CreateUpdater();
        State = await InitializeStateAsync(_updater);
        if (ParametersUpdatedMessage is not null)
            await SendMessage(ParametersUpdatedMessage, true).InvokeAsync();
        ReRender();
    }

    [Parameter]
    public object InitMsg { get; set; }

    [Parameter]
    public bool ShowDebugState { get; set; }

    /// <summary>
    ///     Adjusted from <see cref="ComponentBase.StateHasChanged" />
    /// </summary>
    void ReRender()
    {
        if (_hasPendingQueuedRender) return;

        _hasPendingQueuedRender = true;
        try
        {
            _renderHandle.Render(_renderFragment);
        }
        finally
        {
            _hasPendingQueuedRender = false;
        }
    }

    [Inject]
    Logger Logger
    {
        get => _logger;
        set
        {
            _logger = value;
            _logger.Type = GetType();
        }
    }

    [CascadingParameter]
    IMvuView Owner { get; set; }

    [Inject]
    ISerializer Serializer { get; set; }

    protected virtual Task OnStateChangedAsync() => Task.CompletedTask;

    protected EventCallback Msg<TMessage>(bool doPropagateOutsideComponent = true) where TMessage : new() =>
        SendMessage(new TMessage(), doPropagateOutsideComponent);

    protected EventCallback Msg(object msg,
        bool doPropagateOutsideComponent = true) =>
        SendMessage(msg, doPropagateOutsideComponent);

    protected EventCallback<T> Msg<T>(Func<T, object> msgFactory,
        bool doPropagateOutsideComponent = true) =>
        new FilteredMessageEventCallback<T>(this).Msg(msgFactory,
            doPropagateOutsideComponent);

    protected FilteredMessageEventCallback<T> Filter<T>(Func<T, bool> predicate) => new(this, predicate);

    protected abstract TState State { get; set; }
    protected abstract TUpdater CreateUpdater();
    protected abstract Task<TState> InitializeStateAsync(TUpdater updater);
    protected virtual object ParametersUpdatedMessage => null;

    /// <summary>
    ///     Will be filled out by the Razor source generator.
    /// </summary>
    protected virtual void BuildRenderTree(RenderTreeBuilder builder) { }
}