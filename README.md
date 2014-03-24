##LINQPadSpy

LINQPadSpy shows decompiled sources from queries which have been compiled by LINQPad allowing one to see any additional compiler generated code.

![LINQPadSpy Screenshot](https://github.com/vincpa/linqpadspy/raw/master/LINQPadSpy.JPG)

### Goals

Gain a deeper understanding of the C# language and compiler with LINQPad.

### Getting started and contributing

I haven't really put much thought in to contributions. Frankly I don't even know if anyone else thinks it's useful. Until I recieve some feedback or recieve some interest, i'll consider it a personal project.

Currently the implementation is a bit of a hack. I've made some small changes to a fork of ILSpy and am re-using the UI text view control and all of ILSpy's built in functionality. I expect the quality of code to improve should there be more interest or I have more spare time available.

Some features I'd like to implement are:

* The ability to click on types which are not part of the LINQPad generated assembly and have them decompiled. ILSpy does currently do this, but due to it's tight coupling and dependence on WPF's static classes and properties, a lot of the current functionality doesn't work. Idealy, refactoring the fork of ILSpy in such a way that upstream merges from the main project do not cause merge conflicts would be a preferable approach.
* Ability to 'detect' which language in LINQPad was used so as to show the same decompiled language. Currently any code written in VB.NET is decompiled to C#.
* A forward and back button.
* Decompile when the user presses F5. With the current implementation you need to add `this.DumpDasm()' somewhere in your main method.
* Merge all assemblies with ILMerge in order to make deployment easier and to not polute the LINQPad plugins directory.
* A tree view displaying all assemblies? Not sure about this one, ILSpy can be quite slow at times and memory consumption is also a worry. The screen real estate is also limited. Consider this a note.

### Getting the plugin to work

First you'll need to open the solution up in Visual Studio 2012/2013 and add reference to the LinqPadSpy project which points to the LINQPad.exe executable. Then compile the solution and copy the binaries to the LINQPad plugins directory. Be sure to copy all DLL and EXE files except for LINQPad.exe.

### Compatibility

* .NET 4
* Tested with LINQPad 4.48.01 (Any CPU)

### Known issues

* On occasion you'll get a COM interop exception.
* The first decompilation can be slow but will be quicker thereafter.
