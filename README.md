##LINQPadSpy

In order to gain a deeper understanding of the C# language, I wrote a plugin for LINQPad which allows me to see compiler generated code.

### Goals

Implment a stable plugin for LINQPad which allows developers to view compiler generated code.

### Getting started and contributing

I haven't really put much thought in to contributions. Frankly I don't even know if anyone else thinks it's useful. Until I recieve some feedback or get some interest, i'll consider it a personal project.

Currently the implementation is a bit of a hack. I've made some small changes to a fork of ILSpy and am re-using the UI text view control and all of ILSpy's built in functionality. I expect the quality of code to improve should there be more interest or I have more spare time available.

Some features I'd like to implement are:

* The ability to click on types which are not part of your assembly and have them also decompiled. ILSpy does currently do this, but due to it's tight coupling and dependence on WPF static classes and properties, a lot of the current functionality doesn't work. Idealy, refactoring the fork of ILSpy in such a way that upstream merges from the main project do not cause merge conflicts would be a preferable approach.
* A forward and back button.
* Decompile when the user presses F5. Currently the addition of the `this.DumpDasm()' is required.
* A tree view displaying all assemblies? Not sure about this one, ILSpy can be quite slow at times and memory consumption is also a worry. Not sure about this.
