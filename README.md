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
There are very good explanations of the MVU pattern out there, for example [here](https://thomasbandt.com/model-view-update), and if you've found this repository, I kinda assume you know what MVU is and are looking for a way to use it with Blazor.

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


## Blazor.Mvu Design Goals
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



## How to use it



