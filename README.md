# Blazor.Mvu
![CI Status](https://github.com/ModernRonin/Blazor.Mvu/actions/workflows/dotnet.yml/badge.svg)
[![NuGet](https://img.shields.io/nuget/v/Blazor.Mvu.svg)](https://www.nuget.org/packages/Blazor.Mvu/)
[![NuGet](https://img.shields.io/nuget/dt/Blazor.Mvu.svg)](https://www.nuget.org/packages/Blazor.Mvu)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](http://makeapullrequest.com) 

## Warning 

This is not production-ready yet. I'm using it in another free-time project of mine and only once that is released, I will consider **Blazor.Mvu** production-ready.

However, I put this public and online in the hopes of finding other people who want to collaborate, for example by
* trying to use this in their (not mission-critical!) Blazor apps and thus finding stuff that needs improvement in **Blazor.Mvu**
* improving the convenience of the library via PRs (ideally, after a little bit of discussion first)
* bugfix PRs
* ideas and even better PRs how one could properly test-cover **Blazor.Mvu**
* adding documentation 
* coming up with a logo :-)

## MVU
There are very good explanations of the MVU pattern out there, for example [here](https://guide.elm-lang.org/architecture/), and if you've found this repository, I kinda assume you know what MVU is and are looking for a way to use it with Blazor.

That being said, let me name a few important design goals that this pattern solves:

> **Unidirectional data flow**
> 
> In traditional MVVM binding, you have one bi-directional data-flow between view and logic via two-way bindings. While this feels very nice in the beginning, with increasing complexity of the UI this very quickly starts to get messy, unless you are extremely disciplined.
>
>In MVU there are *two uni-directional flows* instead: logic to views via bindings and views to logic via messages. This distinction makes it much easier to understand what is going on by looking at views or logic in isolation. 

TODO: flesh this out with an example; something like select entry of a list for editing/removal 

> **Immutability**
>
>Traditional MVVM requires mutable viewmodels to work. In MVU, we have two separate kinds of data, **state** and **messages**, both of which are immutable. 
>
>Generally, immutable data is easier to reason about, and in multi-threaded scenarios it avoids locking and the myriad of bugs/performance issues that go with that.
>
>Furthermore, it immutable state, when combined with deterministic behavior, allow us look at a user-session with an application as really just a sequence of states. This enables nifty features like going back in history and automatically replaying user-sessions. 

>**Separation of behavior from state**
>
>When OO languages started out, everybody felt that encapsulating state (aka data) together with the operations performed on said state was the way to go. I remember myself being rather excited about this when first encountering C++ in the mid 90ies. 
>
>However, a long time has passed since then (in terms of our understanding the process of creating software, anyway), and along the way our industry has been discovering that bundling state and behavior into one type is really suited just for a very specific subset of problem domains, most of them fairly low-level, for example implementing common data structures. Conversely, most other problem domains benefit more from an architecural approach that separates behavior from state. This is the reason why mainstream languages either add more and more functional features (like C# does) or are slowly superseded by more functional cousins (like Java by Scala and Kotlin).
>
>MVU separates behavior from state very strictly by making state immutable and putting behavior into an **update** function.


## Design Goals
Many implementations of MVU describe the view in code. Even the new [.NET MAUI](https://devblogs.microsoft.com/dotnet/introducing-net-multi-platform-app-ui/) implementation does this (much to my consternation, I must say).

However, many developers have come to like describing the view in a declarative syntax, usually some XML dialect. There is considerable knowledge and tooling supporting that syntax. Furthermore, in particular when working on web apps, we often work together with designers who understand that kind of declarative syntax very well because they know HTML, but don't understand regular code at all. Describing the view in code throws away all these advantages.

Thus, the probably most important design goal of **Blazor.Mvu** was and is to keep declarative syntax for views unchanged. In Blazor terms that means all the razor files you know and have continue to exist, with only minor adaptations that pertain to binding. 

So, you can continue to use your existing tools and also make use of any future editors for Razor that might come out. You don't have to learn a new DSL just to describe a view. You can seamlessly integrate the myriads of component libraries out there. And your colleague, the designer, will be happy to hear that they still can understand and edit your views.

Another design goal is almost as important: **Blazor.Mvu** should not force you to go all the way if you cannot or don't want to. In other words, it is possible to use **Blazor.Mvu** in three modes:

* you create a greenfield application where all frameworks/components you use integrate easily with Blazor.Mvu: in this case, you just **Blazor.Mvu** for the whole application, including any custom controls you might create.
* you create a greenfield application, but some of the components you use do not work with **Blazor.Mvu** (the reason for this can be when they somehow depend on your views inheriting from `ComponentBase` instead of another implementation of `IComponent` because **Blazor.Mvu** doesn't use `ComponentBase`); in this scenario, you can use **Blazor.Mvu** for all the parts where it makes sense and wrap the problematic components
* you have an existing application and want to use **Blazor.Mvu** just for the new parts

Another very important goal is that using **Blazor.Mvu** does not require you to switch to a purely functional language like F#. While F# is super interesting and there are already very interesting MVU frameworks out for it, for example the excellent [Bolero](https://fsbolero.io/), the reality for 90% of developers is that they simply can't introduce another language into their workplace, no matter how beneficial it might be. 

So, you can use **Blazor.Mvu** with C#, and it is actually implemented in C#, to ensure that usage of it will always stay idiomatic. However, in the future I hope to add F# wrappers that allow idiomatic use from F#, too. 

And last not least, as with any good framework, there is the design goal that using **Blazor.Mvu** should be comfortable, unsurprising and not require you to repeat yourself. 

## Dependencies
**Blazor.Mvu** runs on NET 6 and for WebAssembly Blazor only. It does not support anything below NET 6 and it won't work on server-hosted Blazor (probably - I never tried, but even if it does, it would probably be not very performant). It has only one dependency that your Blazor project will have in any case, `Microsoft.AspNetCore.Components.WebAssembly`.

However, **Blazor.Mvu** used dependency injection a lot and requires that the container be able to resolve `Func<T>`s if `T` is resolvable. The standard container Blazor projects come with, `Microsoft.Extensions.DependencyInjection`, doesn't do this out-of-the-box. And because I personally never use that container, but [Autofac](https://autofac.readthedocs.io/en/latest/) instead (which doesn't have this limitation), you will currenly also have to use `Autofac`. 

In the future, this will be changed (maybe someone who's got more experience with `Microsoft.Extensions.DependencyInjection` wants to help?), and so the dependency on Autofac is kept in a separate package `Blazor.Mvu.Autofac`. While currently, you can use `Blazor.Mvu` only together with `Blazor.Mvu.Autofac`, at a later point in time you will be able to use it on its own and maybe there will be also integration packages for other popular containers like [Castle.Windsor](https://github.com/castleproject/Windsor) or  [Lamar](https://jasperfx.github.io/lamar/).



## How to use it
In the future, there will hopefully be code-samples in a separate repository, a nice documentation site with tutorials and proper XML documentation comments in the library itself. 

For now, though, you will have to make do with a this section here. Please be patient, bear in mind that I coded the framework *and* my other project that I use as proof-of-concept within the space of my Christmas vacation and last not least, if you are interested, please get in touch and, if you can, collaborate :-)

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

For now, we saw how to generate the view/state/message/updater tuple for the whole application. 

But **Blazor.Mvu** distinguishes three different entities:
* *applications*, like the one we just did, there's always exactly one per app
* *pages*: they correspond closely to the general Blazor page concept, there can be many in one application, they can contain *components*
* *components*: they correspond to sub-views and custom controls, there can be many in one application, they can contain other *components*

For each of these entities, there is an abstract base view that your view must inherit from. Actually, there are two variations for each, one resolving your updater for you, the other one allowing you to customize that bit. For the purpose of this tutorial, we'll talk only about the first, but if you need customized updater creation, just follow the inheritance chain and you'll find what you need.

*Applications* we already looked at.

Let's look at a more complex example that combines *pages* and *components*:

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

Note how you reference the state of the application here with `Logic.App.State`. This is because pages define their state as in relation to a source/parent state. 

