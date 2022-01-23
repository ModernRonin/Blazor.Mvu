# Blazor.Mvu
![CI Status](https://github.com/ModernRonin/Blazor.Mvu/actions/workflows/dotnet.yml/badge.svg)
[![NuGet](https://img.shields.io/nuget/v/Blazor.Mvu.svg)](https://www.nuget.org/packages/Blazor.Mvu/)
[![NuGet](https://img.shields.io/nuget/dt/Blazor.Mvu.svg)](https://www.nuget.org/packages/Blazor.Mvu)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](http://makeapullrequest.com) 

## Warning 

This is not production-ready yet. I'm using it in another free-time project of mine and only once that is released, I will consider **Blazor.Mvu** production-ready.

However, I put this public and online in the hopes of finding other people who want to collaborate, for example by
* trying to use this in their (not mission-critical!) Blazor apps and along the way finding things in need of improvement in **Blazor.Mvu**
* improving the convenience of the library via PRs (ideally, after a little bit of discussion first)
* bugfix PRs
* ideas and even better PRs how one could properly test-cover **Blazor.Mvu**
* adding documentation 
* coming up with a logo :-)

## MVU
There are very good explanations of the MVU pattern out there, for example [here](https://guide.elm-lang.org/architecture/), and if you've found this repository, I kinda assume you know what MVU is and are looking for a way to use it with Blazor.

That being said, let me name a few important design goals that this pattern solves:

### Unidirectional data flow
In traditional MVVM binding, you have *one bi-directional data-flow* between view and logic via two-way bindings. While this feels comfortable in the beginning, with increasing complexity of the UI it lets to messy code, unless you are extremely disciplined.

In MVU there are *two uni-directional flows* instead: logic to views via bindings and views to logic via messages. This distinction makes it easier to understand what is going on by looking at views or logic in isolation. 

TODO: flesh this out with an example; something like select entry of a list for editing/removal 

### Immutability
Traditional MVVM requires mutable viewmodels to work. In MVU, we have two separate kinds of data, **state** and **messages**, both of which are immutable. 

Generally, immutable data is easier to reason about, and in multi-threaded scenarios it avoids locking and the myriad of bugs/performance issues coming with that.

Furthermore, immutable state, when combined with deterministic behavior, allows us look at a user's in an application as really just a sequence of states. This enables nifty features like going back in history and automatically replaying user-sessions. 

### Separation of behavior from state
When OO languages started out, everybody felt that encapsulating state (aka data) together with the operations performed on said state was the way to go. I remember myself being rather excited about this when switching to Object Pascal and a bit later C++ in the mid 90ies. 

However, a long time has passed since then (in terms of our understanding the process of creating software, anyway), and along the way our industry has discovered that bundling state and behavior into one type is really suited just for a very specific subset of problem domains, most of them fairly low-level, for example implementing common data structures. Conversely, most other problem domains benefit more from an architecural approach that separates behavior from state. This is the reason why mainstream languages either add more and more functional features (like C# does) or are slowly superseded by more functional cousins (like Java by Scala and Kotlin).

MVU separates behavior from state very strictly by making state immutable and putting behavior into an **update** function.


## Design Goals
Many implementations of MVU describe the view in code. Even the new [.NET MAUI](https://devblogs.microsoft.com/dotnet/introducing-net-multi-platform-app-ui/) implementation does this (much to my surprise).

However, most developers have come to like describing the view in a declarative syntax, usually some XML dialect. There is considerable knowledge and tooling supporting that syntax. Furthermore, in particular when working on web apps, we often work together with designers who understand that kind of declarative syntax very well because they know HTML, but don't understand regular code at all. Describing the view in code throws away all these advantages.

Thus, the probably most important design goal of **Blazor.Mvu** was to keep declarative syntax for views unchanged. In Blazor terms that means all the razor files you know and have continue to exist, with only minor adaptations that pertain to binding. 

So you can continue to use your existing tools and also make use of any future editors for Razor that might come out. You don't have to learn a new DSL just to describe a view. You can seamlessly integrate the myriads of component libraries out there. And your colleague, the designer, will be happy to hear that they still can understand and edit your views.

Another design goal is almost as important: **Blazor.Mvu** should not force you to go all the way if you cannot or do not want to. In other words, it is possible to use **Blazor.Mvu** in three modes:

* you create a greenfield application where all frameworks/components you use integrate easily with Blazor.Mvu: in this case, you just **Blazor.Mvu** for the whole application
* you create a greenfield application, but some of the components you use do not work with **Blazor.Mvu** (the reason for this can be when they depend on your views inheriting from `ComponentBase` instead of just implementing `IComponent` because **Blazor.Mvu** doesn't use `ComponentBase`); in this scenario, you can use **Blazor.Mvu** for all the parts where it makes sense and wrap the problematic components
* you have an existing, brownfield application, you like the MVU concept, but you obviously can't invest the time to switch the whole code-base, so you want to use **Blazor.Mvu** just for the new parts

Another very important goal is that using **Blazor.Mvu** does not require you to switch to a purely functional language like F#. While F# is an awesome language and I'd encourage any .NET developer to learn it and play around with it, simply because learning and understanding F# will make you a better developer in general, the reality for the vast majority of developers is that they simply can't introduce another language into their workplace, no matter how beneficial it might be.  Besides, there are already quite a few plenty of MVU frameworks out there for F#, [Bolero](https://fsbolero.io/) and [Fabulous](https://github.com/fsprojects/Fabulous) probably amongst the more prominent of them.

You can use **Blazor.Mvu** with C#, and it is actually implemented in C#, to ensure usage will always stay idiomatic. However, in the future I hope to add F# wrappers allowing idiomatic use from F#, too. 

And last not least, as with any good framework, there is the design goal that using **Blazor.Mvu** should be comfortable, unsurprising and not require you to repeat yourself. 

## Dependencies
**Blazor.Mvu** runs on NET 6 and for WebAssembly Blazor only. It does not support anything below NET 6 and it won't work on server-hosted Blazor (probably - I actually never tried serrver-hosted Blazor because to my eyes this is more of a stop-gap solution; but even if it does work, that's not something you should rely on staying that way). It has only one dependency that your Blazor project will already have implicitly, too, `Microsoft.AspNetCore.Components.WebAssembly`.

However, **Blazor.Mvu** uses dependency injection a lot and requires the container to be able to resolve `Func<T>`s if `T` is resolvable. The standard container Blazor projects come with, `Microsoft.Extensions.DependencyInjection`, doesn't do this out-of-the-box. And because I personally never use that container, but [Autofac](https://autofac.readthedocs.io/en/latest/) instead (which doesn't have this limitation), you will currenly also have to use `Autofac`. 

In the future, this will be changed (maybe someone who's got more experience with `Microsoft.Extensions.DependencyInjection` wants to help?), and so the dependency on Autofac is kept in a separate package `Blazor.Mvu.Autofac`. While right now you can use `Blazor.Mvu` only together with `Blazor.Mvu.Autofac`, at a later point in time you will be able to use it on its own and maybe there will be integration packages for other popular containers, too, like for example [Castle.Windsor](https://github.com/castleproject/Windsor) or  [Lamar](https://jasperfx.github.io/lamar/).



## Tutorial: How to use it
In the future, there will hopefully be code-samples in a separate repository, a nice documentation site with tutorials and proper XML documentation comments in the library itself. 

For now, though, you will have to make do with this section here. Please be patient, bear in mind that I coded the framework *and* my other project that I use as proof-of-concept within the space of my Christmas vacation and last not least, if you are interested, please get in touch and, if you can, collaborate :-)

First of all, install the packages `Blazor.Mvu` and `Blazor.Mvu.Autofac`. 

In order to enable the use of Autofac, you will also have to modify your `Program.cs` so that you get something like
```csharp
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.ConfigureContainer(
    new AutofacServiceProviderFactory(b => b.RegisterModule(new MyModule())));
```

where `MyModule` is an Autofac module. In that module do your registrations as you normally would, but also add a line

```csharp
builder.AddBlazorMvu(GetType().Assembly);
```
Amongst other things, this line automatically registers all updaters and your implementations of `IApplicationStateHolder<>` and `ISerializer` (see later) for you, so you don't have to do that manually. 

Note that you could also pass multiple assemblies if your views, states, messages and updaters reside in multiple assemblies.

Now in `_Imports.razor`, add the lines
```razor
@using Blazor.Mvu
@using System.Collections.Immutable
```


Next got to your `App.razor` file and add a
```razor
@inherits AStandardMvuApplication<State, Updater>
```

Now go and create `State`. State must be a record with only immutable properties. This includes any collection properties, so instead of using `List<T>` use `ImmutableList<T>` etc. Also, unless you are very sure what you are doing, don't define any non-computed properties outside those in the constructor signature.

Here's an example:
```csharp
public record State(ImmutableArray<Account> Accounts,
    ImmutableArray<Currency> AvailableCurrencies,
    Account CurrentAccount);
```

It will be often beneficial to add a static method for creating an empty/initial state, too. For example:
```csharp
public record State(ImmutableArray<Account> Accounts,
    ImmutableArray<Currency> AvailableCurrencies,
    Account CurrentAccount)
{
    public static State Empty => new(ImmutableArray<Account>.Empty, ImmutableArray<Currency>.Empty, default);
}
```


Next, let's create a few messages:
```csharp
public record LoadAccounts;

public record LoadInitialData;
```

And now the updater:

```csharp
public class Updater : IApplicationUpdater<State>
{
    public Task<State> InitializeAsync() => Task.FromResult(State.Empty);
    public async Task<State> UpdateAsync(State state, object msg)
    {
        return msg switch
        {
            LoadAccounts => state with { Accounts = await LoadAccountsAsync() },
            LoadInitialData => state with
            {
                Accounts = await LoadAccountsAsync(),
                AvailableCurrencies = await LoadCurrenciesAsync()
            },
                 _ => default
        };
    }
}
```
`LoadAccountsAsync` and `LoadCurrenciesAsync` would be regular methods that fetch the data from a web API, for example. Note that you can constructor-inject into your `Updater` to your heart's content, for example some sort of proxy for talking to your backend. 

The critical methods are the two shown in the code-snippet:
* `InitializeAsync` is expected to return a state that can be used immediately when the view (in our current example, the App) is created. While the method is async, you should probably not do anything here that can take a long time because this will block initial rendering. To get initial data, it's better to define a message like `LoadInitialData` and do the heavy lifting when reacting to it. We will come later to where that message is used.
* `UpdateAsync` takes an existing state and an incoming message and outputs the new state. It's called whenever a new message is being processed. Importantly, if you cannot or do not want to handle a message, always return `default`. This allows, in the context of nested components, the message to bubble up through the chain of components to one that can handle it. The `App` updater in our example here is already the top of the chain, so there is no further bubbling up. When a message is encountered that cannot be handled by anyone in the chain, then a warning will be written to the console log of your browser. 

These same two methods, with a minor variation for `InitializeAsync`, you will implement for all your updaters.

Note also that Updater doesn't depend on anything Blazor or view-specific. In principle, it could live in a separate library that knows nothing about your views, let alone the particular tech-stack (Blazor) used by them.

As mentioned before, often you want to perform some potentially lengthy work at application startup without blocking your UI from rendering. The updater above reacts to a `LoadInitialData` to do just that. But who sends that message and when?

Any MVU view has a property `InitMsg`. If that property is set, then this message will be sent exactly once during a view's lifetime, after it has been rendered for the first time. 

Depending on your scenario, you can set that property via bindings `<MyComponent InitMsg="...">` or via code. In the case of `App`, there is nothing that could bind to it, so it has to be code. So you'd append the following to the bottom `App.razor`:

@code
{
    public App() => InitMsg = new LoadInitialData();
}

This is one of the very few scenarios using **Blazor.Mvu** requiring you to write any code (via `@code` directive or code-behind file) for a view. Unfortunately, Razor currently doesn't allow us to define custom directives, but if that feature comes at some point, **Blazor.Mvu** could use that mechanism to allow you to replace the above with a simple
`@initMsg new LoadInitialData()`. 


For now, we saw how to generate the view/state/message/updater tuple for the whole application. 

But **Blazor.Mvu** distinguishes three different entities:
* *applications*, like the one we just did, there's always exactly one per app
* *pages*: they correspond closely to the general Blazor page concept, there can be many in one application, they can contain *components*
* *components*: they correspond to sub-views and custom controls, there can be many in one application, they can contain other *components*

For each of these entities, there is an abstract base view that your view must inherit from. Actually, there are two variations for each, one resolving your updater for you, the other one allowing you to customize that bit. For the purpose of this tutorial, we'll talk only about the first, but if you need customized updater creation, just follow the inheritance chain and you'll find what you need.

*Applications* we already looked at.

Let's look at a more complex example that combines *pages* and *components*. (I'm using the awesome [MudBlazor component library](https://mudblazor.com/), so anything prefixed with `Mud` is just an existing component.)

First create `Sandbox.razor`:
```razor
@page "/sandbox"
@using Logic.Sandbox
@inherits AStandardMvuPage<State, Updater, Logic.App.State>

<h3>Sandbox</h3>
<Settings Value="State.Settings"/>
<MudDivider/>
<Undoable T="decimal" Value="State.OpenVolume.Amount" ValueChanged="Msg<decimal>(v => new UpdateVolume(v))">
    <MudNumericField T="decimal" Label="@($"Open Volume ({State.OpenVolume.Currency.IsoCode})")"
                     Format="@($"0.00 {State.OpenVolume.Currency.IsoCode}")"
                     Value="@context.Value" ValueChanged="@context.HandleChange"/>
</Undoable>
<MudDivider/>
<SandboxResults Value="State.OpenPerUser"/>
<MudDivider/>
<UserManagement Value="State.Users" ValueChanged="Msg<ImmutableArray<string>>(v => new UpdateUsers(v))"/>
<MudDivider/>
```

Note how you reference the state of the application here with `Logic.App.State`. This is because pages define their state as in relation to the application state. This is illustrated by the interface 
```csharp
public interface IPageUpdater<TState, in TAppState> : IUpdater<TState>
{
    Task<TState> InitializeAsync(TAppState appState);
}
``` 
which `Logic.Sandbox.Updater` implements, as we will see in a bit.

But first let us go through the declarations in the view:
* `<Settings Value="State.Settings">` includes a custom *component* and *one-way-binds* a property of `State` to it. 
* `<Undoable ...>` includes another custom *component*, this one more like a custom control that you might write to include in a component library, binds it again against a `State` property and defines which message should be sent when that component tells us it wants to change it's bound value. 
* `<MudNumericField>` is nested in `<Undoable>` and a regular, non-MVU component from MudBlazor; the `ValueChanged` binding interacts with its owning `<Undoable>`
* `<SandboxResults...>` includes a view that doesn't generate any messages (we'll come to that at the end, as it's more of a special case)
* `<UserManagement...>` is another custom *component* that is bound against a property of `State` and translates value changes to a message

Now let us look at the `State`, `Updater`and *messages* for `Logic.Sandbox`:

```csharp
namespace Logic.Sandbox;

public record State(AccountSettings Settings,
    ImmutableArray<string> Users,
    Money OpenVolume)
{
    public IImmutableDictionary<string, Money> OpenPerUser =>
        Settings.Sharing.Share(OpenVolume.Amount, Users, Users.FirstOrDefault())
            .ToImmutableDictionary(kvp => kvp.Key, kvp => new Money(kvp.Value, Currency.Default));
}

public record UpdateVolume(decimal Volume);

public record UpdateUsers(ImmutableArray<string> Users);

public class Updater : IPageUpdater<State, App.State>
{
    public Task<State> InitializeAsync(App.State appState)
    {
        var settings = AccountSettings.Default;
        var users = ImmutableArray<string>.Empty.Add("Bob").Add("Alice").Add("Charlie");
        return Task.FromResult(new State(settings,
            users,
            Money.Zero(settings.Currency)));
    }

    public Task<State> UpdateAsync(State state, object msg) =>
        Task.FromResult(msg switch
        {
            UpdateName m => state with { Settings = state.Settings with { Name = m.Name } },
            UpdateCurrency m => state with
            {
                Settings = state.Settings with { Currency = m.Currency },
                OpenVolume = state.OpenVolume with { Currency = m.Currency }
            },
            UpdateSharing m => state with { Settings = state.Settings with { Sharing = m.Sharing } },
            UpdateVolume m  => state with { OpenVolume = state.OpenVolume with { Amount = m.Volume } },
            UpdateUsers m   => state with { Users = m.Users },
            _               => default
        });
}
```
The `State`and messages are pretty straight-forward (forget about `State.OpenPerUser`'s implementation for now, that's a detail of the domain and has nothing to do with **Blazor.Mvu**).

`Updater` is very similar to the other updater we already saw, but the signature of it's `InitializeAsync` method differs: where the updater for the whole application had no arguments for this method, updaters for *pages* are passed the current application state. Our `Updater`uses that to calculate the initial `Logic.Sandbox.State` for the view. Whenever the application state changes, this method will be called again.

Again, state and message are completely immutable, and the updater holds no state at all (but could have c'tor injected dependencies, for example to send updated state to some backend proxy).

Interestingly, we see only two messages defined here, but `Updater` reacts on a few others. Where do they come from?

They are defined by the *components* referenced in the view, actually all by the `Settings` component which we will look at now.

The `Settings` component consists, like all MVU elements, of 4 parts: a view, a state, and updater and messages.
By the way, how you organize these in terms of namespaces and code-files is up to you. My own current practice is as follows:

* a namespace `Views`, this only contains razor files
* another namespace `Logic` with nested namespaces for the application, every page and every component, so there's `Logic.App`, `Logic.Sandbox`, `Logic.Settings`, `Logic.Undoable`, `Logic.UserManagement` and so on
* in each `Logic.ComponentName` namespace I have 3 code-files: 
    * `State.cs` which contains only the state and any needed additional types, 
    * `Updater.cs` which contains the updater
    * `Messages.cs` which contains records for all messages the component defines - here I use one code-file for all of them because they tend to be rather short; sometimes a component doesn't define any messages, then this file doesn't exist

Here's the view for `Settings`:
```razor
@using Logic.Settings
@inherits AStandardComponentMvuView<State, Updater, AccountSettings>

<Undoable T="string" Value="@State.Value.Name" ValueChanged="Msg<string>(n => new UpdateName(n))">
    <MudTextField Label="Name" Value="@context.Value" ValueChanged="context.HandleChange" 
                  Immediate="true" DebounceInterval="150"/>
</Undoable>
<Undoable T="Currency" Value="@State.Value.Currency" ValueChanged="Msg<Currency>(c=>new UpdateCurrency(c))">
    <MudAutocomplete Label="Home Currency" Value="@context.Value" ValueChanged="context.HandleChange"
                     SearchFunc="p => Task.FromResult(State.FindCurrency(p))" ToStringFunc="c => c.IsoCode">
        <ItemTemplate Context="currency">
            <MudText>
                <Flag Value="@currency.CountryCode"/> @currency.IsoCode
            </MudText>
        </ItemTemplate>
    </MudAutocomplete>
</Undoable>
<Undoable T="ISharingStrategy" Value="@State.Value.Sharing" ValueChanged="Msg<ISharingStrategy>(s => new UpdateSharing(s))" 
          OnValidate="s => s.IsValid">
    <SharingStrategy Value="@context.Value" ValueChanged="context.HandleChange"/>
</Undoable>
```

It contains all kinds of other nested stuff, but for our tutorial only the following lines are relevant:

* the `@inherits` line specifies that this is a *component* - compare this with *page* and *application* declarations we already saw
* the several `<Undoable>`s fire the messages we were missing in the definition of `Sandbox`, for example `UpdateName`

Now let's look at state, messages and updater:
```csharp
namespace Logic.Settings;

public record State(AccountSettings Value, Func<ImmutableArray<Currency>> AvailableCurrencies)
    : IComponentState<AccountSettings>
{
    public IEnumerable<Currency> FindCurrency(string pattern) =>
        AvailableCurrencies()
            .Where(c =>
                c.IsoCode.Contains(pattern, StringComparison.CurrentCultureIgnoreCase));
}

public record UpdateSharing(ISharingStrategy Sharing);

public record UpdateName(string Name);

public record UpdateCurrency(Currency Currency);

public class Updater : IComponentUpdater<State, AccountSettings>
{
    readonly ICurrencyProvider _currencyProvider;

    public Updater(ICurrencyProvider currencyProvider) => _currencyProvider = currencyProvider;

    public Task<State> InitializeAsync(AccountSettings value) =>
        Task.FromResult(new(value ?? AccountSettings.Default,
            () => _currencyProvider.AvailableCurrencies.ToImmutableArray()));

    public override Task<State> UpdateAsync(State state, object msg) =>
        Task.FromResult<TState>(msg switch
        {
            _ => default
        });
}
```

`State` and messages are, as usual, rather straight-forward. The only interesting thing about them is perhaps the function of the `State.FindCurrency` method. It illustrates how you deal with (external) components like `<MudAutoComplete>` that require you to pass in a lambda, but that lambda is dependent on data that might change from outside your view/component, in our case the list of available currencies. 

The list of available currencies is loaded only once, at app startup, from a backend API. That means it is not availanble immediately, but at the same time it is being used by sub-components like `Settings`. So `Settings` must be able to work when there are no currencies loaded yet and also deal with updates to the available currencies.

To do this, our updater c'tor injects an interface `ICurrencyProvider`. The implementation of that interface really has nothing do with MVU. If you wished, you could query an API, a memory cache, whatever. 

Because `Settings` is a *component*, the updater implements the following interface
```csharp
public interface IComponentUpdater<TState, in TValue> : IUpdater<TState>
    where TState : IComponentState<TValue>
{
    Task<TState> InitializeAsync(TValue argument);
}
```

This time, the signature of `InitializeAsync` gets a `TValue` passed in. As you probably have guessed, this corresponds to the `Value` property bindable on *components*. As you can see, `TState` has a constraint to be an `IComponentState<TValue>`. This in turn is defined as
```csharp
public interface IComponentState<out TValue>
{
    TValue Value { get; }
}
```

What's going on here? Every *component* has a bindable `Value` property. When you define the view, you specify the type of that property via the `@inherit` directive. For `Settings`, this was `@inherits AStandardComponentMvuView<State, Updater, AccountSettings>` - the first generic parameter specified the state, as with other views, the second the updater, again nothing new, but the third parameter defined the type of `Value`.

Your updater's `InitializeAsync` method will be called whenever `Value`changes via its binding, giving you the possibility to (re-)initialize your state.

The last thing to note about `Setting`'s updater is that it doesn't handle any messages. Why? Because it doesn't know what to do when, for example, a name changes. Instead, as we saw earlier, we want this to be handled on the higher level of `Sandbox`'s updater.

As stated before, messages not handled by an updater are automatically bubbled up the chain of containing views/updaters until handled. Components bubble up to any surrounding compoents or to the page they are embedded in. Pages bubble up to the App. If even the App doesn't handle the message, then we assume you've simply forgotten or not yet gotten round to implement the handler and issue a warning in the browser console.


With this we've walked through almost everything you need to know to start using **Blazor.Mvu**. The only other thing you absolutely **need** to know about is the interface `IApplicationStateHolder<>`. 

Your top-level state, the state of `App` needs to be held somewhere. While up until here everything was immutable, in the end somewhere we do need a mutable memory location to hold what the current state of the application is. This is where `IApplicationStateHolder<>` comes in. It's definition is fairly simple:
```csharp
public interface IApplicationStateHolder<TState>
{
    TState State { get; set; }
}
```
Your application **must define one implementation of this interface**. (It will automatically be registered as single instance for you by `builder.AddBlazorMvu(GetType().Assembly)`.)

You may wonder: why is this an interface and not a class, so that you wouldn't have to implement it?

Because it is *extremely* likely that eventually you will want to implement more than just this in your application state holder and if it was a class instead of an interface, we'd take away the one base class allowed in C#. 

Why implement other interfaces?

Do you remember when we talked about fetching the list of available currencies once at startup, while making it available to any updater interested?

It turns out the easiest and most elegant way to do this is by making your application state holder implement `ICurrencyProvider`, too, like this:

```csharp
using Logic.App;
public class ApplicationStateHolder : IApplicationStateHolder<State>, ICurrencyProvider,
{
    public State State { get; set; } = State.Empty;
    public IEnumerable<Currency> AvailableCurrencies => State.AvailableCurrencies;
}
```

Your `App` view defined `LoadInitialData` as the message to be fired after initial rendering.`Logic.App.Updater` reacts to that message by, amongst other things, updating your app state with the currencies loaded from some backend. As you app state updates, the property `ApplicationStateHolder.State` gets updated. As this updates, the computed property `AvailableCurrencies` updates, too, and so any consumer of `ICurrencyProvider` will get an up-to-date list of currencies.

## Advanced topics
### Debugging
When using **Blazor.Mvu** together with the awesome hot-reloading capabilities of VS2022, you will find that fairly often you want to look at the current state of a *page* or *component*, sometimes even the *application*.

To make this easier, all MVU views define a bindable property `ShowDebugState` so you can write for example:
```razor
<MyComponent ShowDebugState="true">
```
When this property is true, the state associated with the view will be displayed beneath it. However, for this feature to work, you need to to implement the interface `ISerializer` (if you do, your implementation will be automatically registered by `builder.AddBlazorMvu(GetType().Assembly)`):
```csharp
public interface ISerializer
{
    string Serialize(object obj);
}
```
Debug state display will use the `Serialize` method to generate the debug representation of your state.

In my own usage, I implement that interface using [Json.net](https://www.newtonsoft.com/json). because JSON makes for a very natural and easy-on-the-eye display of typical state objects.

But there are two caveats: 
* if you have state that uses polymorph members - I do -, you'll need the superb extension of json.net [JsonSubTypes](https://github.com/manuc66/JsonSubTypes)
* if you have state that contains lambdas - remember the state of the `Settings` component in the tutorial? - then you want to exclude them from serialization lest you get exceptions

Because all this is a bit of work, I present a more or less ready-made type combining all this here. (It's not included in **Blazor.Mvu** because that would force dependencies on two other libraries on users who maybe don't use Json.net.)

```csharp
using System.Reflection;
using JsonSubTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

public class NewtonsoftJsonSerializer
{
    readonly Lazy<JsonSerializerSettings> _settings;
    public NewtonsoftJsonSerializer() => _settings = new Lazy<JsonSerializerSettings>(Create);

    public void Configure(JsonSerializerSettings settings)
    {
        settings.ContractResolver = new IgnoreDelegateTypesContractResolver();
        foreach (var converter in TypeHierarchyBaseTypes.Select(t => TypeHierarchyConverter(t))
                     .Concat(AdditionalConverters)) settings.Converters.Add(converter);
    }

    public JsonSerializerSettings Settings => _settings.Value;

    JsonSerializerSettings Create()
    {
        var result = new JsonSerializerSettings { Formatting = Formatting.Indented };
        Configure(result);
        return result;
    }

    static JsonConverter TypeHierarchyConverter(Type type, string discriminatorProperty = "type")
    {
        var subTypes = type.Assembly.GetTypes().Where(t => !t.IsAbstract && t.IsAssignableTo(type));
        var builder = JsonSubtypesConverterBuilder.Of(type, discriminatorProperty);
        foreach (var subType in subTypes) builder.RegisterSubtype(subType, subType.Name);
        return builder.SerializeDiscriminatorProperty().Build();
    }

    class IgnoreDelegateTypesContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member,
            MemberSerialization memberSerialization)
        {
            var result = base.CreateProperty(member, memberSerialization);
            if (member is PropertyInfo pi && pi.PropertyType.IsAssignableTo(typeof(Delegate)))
                result.Ignored = true;
            return result;
        }
    }

    protected virtual IEnumerable<JsonConverter> AdditionalConverters { get; } =
        Enumerable.Empty<JsonConverter>();

    protected virtual IEnumerable<Type> TypeHierarchyBaseTypes { get; } = Enumerable.Empty<Type>();
}
```

You can use it to create a general serializer for your app for example like so:
```csharp
public class MyGeneralSerializer : NewtonsoftJsonSerializer
{
    protected override IEnumerable<JsonConverter> AdditionalConverters
    {
        get
        {
            yield return new CurrencyConverter();
            yield return new MoneyConverter();
        }
    }

    protected override IEnumerable<Type> TypeHierarchyBaseTypes
    {
        get
        {
            yield return typeof(ISharingStrategy);
            yield return typeof(ATransaction);
        }
    }

    class CurrencyConverter : JsonConverter<Currency>
    {
        // implementation
    }

    class MoneyConverter : JsonConverter<Money>
    {
        // implementation
    }
}
```

Basically, by overriding `AdditionalConverters` you can specify any additional custom converters you wish and by overriding `TypeHierarchyBaseTypes` you specify the root types of all your polymorph type hierarchies to be serialized.

Once you got this, you can implement **Blazor.Mvu**s `ISerlializer` very simply:

```csharp
public class MyViewStateSerializer : ISerializer
{
    readonly MyGeneralSerializer _serializer;

    public MyViewStateSerializer(MyGeneralSerializer serializer) => _serializer = serializer;

    public string Serialize(object obj) => JsonConvert.SerializeObject(obj, _serializer.Settings);
}
```

### Logging
Sometimes displaying the state is not enough, in particular when developing on the framework itself. For these scenarios there are several different logs you can activate. You do that by injecting `LoggerSettings` (which gets registered by `builder.AddBlazorMvu(GetType().Assembly)` as single-instance) and settings any of its properties to `true`:
* DoLogMessages will log out all messages together when they are processed by an updater together with the state before and after the update
* DoLogParameters will log whenever the parameters of any MVU view are being set by Blazor's engine
* DoLogRender will log whenever a view is being rendered

All log messages are logged to the browser's console.



### Updater abstract helpers
So far, we've always directly implemented the diverse updater interfaces. However, there are a abstract base-classes for each of them that can make your life a little easier. They provide the following features:
* instead of always having to implement initialization and update asynchronously, you can choose which you need, there are two versions of each method to override, one async, one not. It does not make sense at all to override both, but if you did, the async version would be used.
* if you don't want to process any messages in your update method, you can just return `DontProcess(msg)`

As an example, compare the following implementation from the tutorial:
```csharp
public class Updater : IComponentUpdater<State, AccountSettings>
{
    readonly ICurrencyProvider _currencyProvider;

    public Updater(ICurrencyProvider currencyProvider) => _currencyProvider = currencyProvider;

    public Task<State> InitializeAsync(AccountSettings value) =>
        Task.FromResult(new(value ?? AccountSettings.Default,
            () => _currencyProvider.AvailableCurrencies.ToImmutableArray()));

    public override Task<State> UpdateAsync(State state, object msg) =>
        Task.FromResult<TState>(msg switch
        {
            _ => default
        });
}
```
with
```csharp
public class Updater : AComponentUpdater<State, AccountSettings>
{
    readonly ICurrencyProvider _currencyProvider;

    public Updater(ICurrencyProvider currencyProvider) => _currencyProvider = currencyProvider;

    public override State Initialize(AccountSettings value) =>
        new(value ?? AccountSettings.Default,
            () => _currencyProvider.AvailableCurrencies.ToImmutableArray());

    public override Task<State> UpdateAsync(State state, object msg) => DontProcess(msg);
}
```





### Custom Controls
While neither Blazor nor **Blazor.Mvu** distinguish between custom controls and sub-views and they use the same mechanics, markup etc., in practice there are often differences. 

What do I mean with this terms semantically? It's probably easiest to describe with examples:
* a view that would allow to graphically split an amount into two or more parts would be a custom control
* a view that allows entering a monetary amount, ie, a number with certain constraints together with a currency, would be a custom control
* a view that groups together entry of a few form fields, say the contents of one tab in a tabbed settings page, would be a sub-view

While it's hard to define exactly, there are a few heuristics to decide whether a view is a sub-view or a custom control:
* does it allow editing of something that logically, semantically is used as a unit most of the time? 
* is it going to be used in many places in the app?
* does it seem plausible the same element could be used in other applications as well?

Why do we need this distinction? Because custom controls tend to break the pattern of UI frameworks: while sub-views usually should not need any code-behind at all and in rare instances just a tiny bit, custom controls almost always need view-specific code. 

This is true for **Blazor.Mvu** just as it is true for other frameworks, for example MVVM, independent of the platform. For example, in WPF, if you do it right, there is really no necessity to have any code in your views, you can define them exclusively via XAML, making clever use of converters and other static resources. All code can reside in your viewmodels. That is until you find yourself writing a custom control. If, in the example of the amount-divider from above, you want the user be able to drag a mark along a graphical bar representing the total, you will have to react to mouse-events and maybe do some manual drawing, and both are activities that are tightly bound to the technical foundation of your view. As such they have no place in the viewmodel, and will go into the code-behind.

The same applies to **Blazor.Mvu** and in this section we'll see an example for how to implement a custom control with MVU mechanics.

In the tutorial, we often saw an `<Undoable>` element referenced. The idea of this element is to allow the user to edit a field and directly save or discard any changes. So when the value changes, two buttons appear, one for saving the changed value, another for discarding the changes. 
Additionally, in some situations we maybe want to prevent the user from saving values that do not pass certain constraints - so there must be a way to pass a validation function into the control. 
And, last not least, we want the user to be able to save the value not just by clicking the save button, but also by pressing the "Enter" key.

Because Blazor doesn't allow us to add properties to existing elements (like, for example, dependency properties in WPF or custom attributes in Aurelia), we can implement this only by creating a container element wrapping the actual field editor. This forces us however to provide the wrapped editor with a way of notifying our control when a value changes. So we get something like:
```razor
<Undoable T="string" Value="@State.Name" ValueChanged="Msg<string>(n => new UpdateName(n))">
    <MudTextField Label="Name" Value="@context.Value" ValueChanged="context.HandleChange" 
                  Immediate="true" DebounceInterval="150"/>
</Undoable>
```
As we can see, the actual editor (the text field in this case) is wrapped inside an `<Undoable>`. Undoable binds to to the state property `Name` and triggers a message for changing this. The wrapped editor however binds to a value from the context provided to it by `Undoable` and also forwards changes to that context.

So how is this implemented? Let's start with the logic, ie the canonical state, messages and updater:

```csharp
namespace Logic.Undoable;

using Blazor.Mvu;

public record State<T>(T Value, 
                       T Original, 
                       Func<T, bool> Validator, 
                       Func<T, Task> HandleChange)
    : IComponentState<T>
{
    public State<T> Reset() => this with { Value = Original };

    public State<T> Save() => this with { Original = Value };

    public bool IsDirty => !Value.Equals(Original);
    public bool IsInvalid => !Validator(Value);
}

public record Save;

public record Reset;

public record ChangeValue<T>(T ChangedValue);

public class Updater<T> : IComponentUpdater<State<T>, T>
{
    readonly IMessageReceiver _messageReceiver;
    readonly Func<T, bool> _validator;

    public Updater(IMessageReceiver messageReceiver, Func<T, bool> validator)
    {
        _validator = validator;
        _messageReceiver = messageReceiver;
    }

    public Task<State<T>> InitializeAsync(T value)
    {
        return Task.FromResult(new State<T>(value, value, _validator, handleChange));

        Task handleChange(T changedValue) =>
            _messageReceiver.SendMessage(new ChangeValue<T>(changedValue), false).InvokeAsync();
    }

    public Task<State<T>> UpdateAsync(State<T> state, object msg) =>
        Task.FromResult(msg switch
        {
            Save                  => state.Save(),
            Reset                 => state.Reset(),
            ChangeValue<T> change => state with { Value = change.ChangedValue },
            _                     => default
        });
}
```

Our state consist of the value, the original value (so we can compare), the validator function and the callback provided to the wrapped editor for notifying about changed values.

The messages and the updater's update function are self-explanatory.

The updater's init function though is a bit more tricky: it sets `State.HandleChange` to a function that creates a `ChangeValue` message and invokes it via `IMessageReceiver`. What is `IMessageReceiver` and where does it come from? 

The answer to the first question is, every MVU view implements `IMessageReceiver`, so `Undoable` does, too. And the `Undoable` view is exactly who should receive that `ChangeValue` message, so somehow it must get injected into the constructor of `Updater`. How this is done we see in the razor file for `Undoable`:

```razor
@typeparam T
@using Logic.Undoable
@inherits AComponentMvuView<State<T>, Updater<T>, T>

<div @onkeypress="@(Filter<KeyboardEventArgs>(a => a.Key=="Enter").Msg(new Save()))">
@ChildContent(State)
</div>
@if (State.IsDirty)
{
    <MudButtonGroup Size="Size.Small">
        <MudIconButton Icon="@Icons.Filled.Save" OnClick="Msg(new Save())" Disabled="@(State.IsInvalid)"/>
        <MudIconButton Icon="@Icons.Filled.Undo" OnClick="Msg(new Reset(), false)"/>
    </MudButtonGroup>
}

@code
{
    protected override Updater<T> CreateUpdater() => new(this, OnValidate);

    [Parameter]
    public RenderFragment<State<T>> ChildContent { get; set; }

    [Parameter]
    public Func<T, bool> OnValidate { get; set; } = _ => true;

}
```
As you can see (and as promised in the introduction ;-P), this view has substantial code in it. We'll explain all of it, but let's start with the question how the updater gets its message receiver.

Up until now we've only seen views that magically create their updaters themselves, without any code on your part. `Undoable` however needs to pass itself to the updater and so it must customize that creation process. To that end, it inherits from `AComponentMvuView` instead of `AStandardComponentMvuView`. `AComponentMvuView` allows us to freely customize updater creation via the `CreateUpdater` factory method. (`AStandardComponentMvuView` just derives from `AComponentMvuView` and implements `CreateUpdater` using dependency injection.)

This part is rather important. All views implement the interface `IMessageReceiver`. This interface allows you to send a message to the view from outside itself. But this comes at a cost. Let's look at the interface's definition:

```csharp
namespace Blazor.Mvu;

using Microsoft.AspNetCore.Components;

public interface IMessageReceiver
{
    EventCallback SendMessage(object msg, bool doPropagateOutsideComponent);
}
```

As you can see, the interface has a dependency on Blazor, in the form of the `EventCallback` return type. So by making our updater depend on `IMessageReceiver`, we make it depend on Blazor. Up until now, all our updaters, state and message were completely independent of the technology stack used by the view. But if you need the ability to tell generate a message for a view from outside the view itself (or any other view contained within), you lose that independence. This may not be a problem at all if you don't plan on using your logic with any other view stack.

The other interesting bit we see in the view is the use of `Filter`. We want to react on presses of the Enter key, but only that key. `Filter` allows you to filter any event arguments by a predicate of your choice and create a message from them only if the predicate is true. 

This method
* avoids leaking UI concepts like `KeyboardEventArgs` into the logic code, ie updaters
* avoids the need for view code-behind files for these scenarios
* avoids sending messages unnecessarily (as would be the case if the filtering happened in the updater)






### Routing Parameters
Sometimes you want pages to use routing parameters. The reason for this is that you want users be able to bookmark specific items. For example, say users of your app can work with a list of accounts. It makes sense to allow them to bookmark a specific account. In order for this to be possible, you must have a page like `@page "/account/{id}"`.

(Note that you should not use routing parameters for other purposes. Sometimes people use routing parameters without any benefit for the user, merely as a way of passing data between views. If you find yourself doing this, you're doing it wrong. Instead, you should just update state at the right level of your application. Remember: **SPAs are applications more than websites, they just *happen* to run in a browser**.)

How does this play together with **Blazor.Mvu**? How do you react to a routing parameter having been set/changed?

Currently, the way to do this is by overriding the property `ParametersUpdatedMessage` and returning a message that you calculate from the routing parameter. For example, for the account view we mentioned this might look like:

```razor
@using Logic.Account
@inherits AStandardMvuPage<State, Updater, Logic.App.State>
@page "/account/{id}"

<!-- markup -->

@code {
    [Parameter]
    public string Id { get; set; }

    protected override object ParametersUpdatedMessage => new SelectAccount(Id);
}
```

Thus, whenever the route parameter is updated, a corresponding `SelectAccount` message will be fired.

However, this will very likely change in the future. The problem with the current design is that, in the framework, we have no way of knowing which parameter was set. Blazor just doesn't tell us. That is why the property is called "**Parameters**UpdatedMessage". If it's overridden, it will be fired for any parameter change, not just changes to the routing parameter(s). In the given example, this is no problem because there are no other parameters. 

Indeed, situations where it will be a problem ought to be rare: As routing parameters only can be used with pages and pages don't need parameters for bindable properties, the only scenario with  multiple parameters is probably when they all are route parameters. In that case, multiple messages will be generated. 

Considering the nature of MVU, generating these multiple messages will not be a functional problem, but of course it is inefficient and simply unelegant.

If you find yourself in a situation where this creates issues for you, the workaround currently is to turn your parameter properties from auto properties into properties with a backing field and set some flag in the setter. In your `ParametersUpdatedMessage` override you can check then which flag has been set to decide which message to send (and reset the flag.)

Eventually, this problem will go away because I plan to create a custom router implementation for **Blazor.Mvu** that's more in line with MVU design and also frees you from littering your views with code. That implementation will not set any route parameters, but instead directly send messages. So the razor file above will change to:

```razor
@using Logic.Account
@inherits AStandardMvuPage<State, Updater, Logic.App.State>
@page "/account"

<!-- markup -->
```
and somewhere in code (unrelated to any view) you'd define something like:
```csharp
OnRoute("/account/{id}").Send(id => new SelectAccount(Id)).To<Logic.Account>();
```

Replacing the existing router component in such a way as to not lose functionality or duplicate a lot of code is not easy, though, so it'll take some time until this feature comes. (Or maybe you want to help? :-))


