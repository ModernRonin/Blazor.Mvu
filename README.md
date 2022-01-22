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





