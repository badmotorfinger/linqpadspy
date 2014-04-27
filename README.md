##LINQPadSpy

LINQPadSpy shows decompiled sources from queries which have been compiled by LINQPad allowing one to see any additional compiler generated code.

![LINQPadSpy Screenshot](https://github.com/vincpa/linqpadspy/raw/master/LINQPadSpy.JPG)

### Goals

Gain a deeper understanding of the C# language and compiler with LINQPad.

### Getting started and contributing

Currently the implementation is a bit of a hack. I've made some small changes to a fork of ILSpy and am re-using the UI text view control and all of ILSpy's built in functionality. I expect the quality of code to improve should there be more interest or I have more spare time available.

If you'd like to contribute to the project then grab a task from the [public Trello board](https://trello.com/b/l9qDZ4t9/linqpadspy) and leave a comment on the card so I know you're working on it.

#### Current feature set
* Decompiles source code when the query contains a `this.DumpDasm()` (C#) or `Me.DumpDasm()` (VB) in the execution path. You can also decompile an F# compiled assembly but the decompiled source will be in C#.
* Detects which language is being used in LINQPad and shows the decompiled source in that language. Currently only VB.NET and C# are supported.
* Displays tooltips when the mouse hovers over a type.
* Ability to change the decompiled source language from C# to VB.NET or IL without rerunning your query.
* Remembers position of split pane (not really a feature :/)

#### Features I'd like to implement:

* The ability to click on types which are not part of the LINQPad generated assembly and have them decompiled. ILSpy does currently do this, but due to it's tight coupling and dependence on WPF's static classes and properties, a lot of the current functionality doesn't work. Idealy, refactoring the fork of ILSpy in such a way that upstream merges from the main project do not cause merge conflicts would be a preferable approach.
* A forward and back button.
* Decompile when the user presses F5. With the current implementation you need to add `this.DumpDasm()' somewhere in your main method.
* Merge all assemblies with ILMerge in order to make deployment easier and to not polute the LINQPad plugins directory.
* A tree view displaying all assemblies? Not sure about this one, ILSpy can be quite slow at times and memory consumption is also a worry. The screen real estate is also limited. Consider this a note.

### Getting the plugin to work

1. Clone the repository `git clone --recursive https://github.com/vincpa/linqpadspy.git`

2. cd in to the ilspy directory `cd path\to\cloned\repository\ilspy`

3. Set the correct HEAD revision for the linqpadspy branch `git checkout linqpadspy`

4. Open the solution in Visual Studio 2012/2013 and add reference to LINQPad.exe to the the LinqPadSpy project. Then compile the solution in Release mode and copy the binaries to the LINQPad plugins directory. Be sure to copy all DLL and EXE files except for LINQPad.exe.

### Compatibility

* .NET 4
* Tested with LINQPad 4.48.01 (Any CPU)

### Known issues

* On occasion you'll get a COM interop exception.
* Selecting mscorlib from the assembly list will generate an exception.
* The first decompilation can be slow but will be quicker thereafter.
