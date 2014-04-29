##LINQPadSpy

LINQPadSpy shows decompiled sources from queries which have been compiled by LINQPad allowing one to see any additional compiler generated code.

![LINQPadSpy Screenshot](https://github.com/vincpa/linqpadspy/raw/master/LINQPadSpy.JPG)

### Goals

Gain a deeper understanding of the C# language and compiler with LINQPad.

### Getting started and contributing

Currently the implementation is a bit of a hack. I've made some small changes to a fork of ILSpy and am re-using the UI text view control and all of ILSpy's built in functionality. I expect the quality of code to improve should there be more interest or I have more spare time available.

I have a bunch of features I'd like to implement and they're all listed on the project issue listing page, just filter by enhancement. If you'd like to contribute take a look at the issue listing and leave a comment so I know you're working on it.


#### Current feature set
* Decompiles source code when the query contains a `this.DumpDasm()` (C#) or `Me.DumpDasm()` (VB.NET) in the execution path. You can also decompile an F# compiled assembly but the decompiled source will be in C#.
* Detects which language is being used in LINQPad and shows the decompiled source in that language. Currently only VB.NET and C# are supported.
* Ability to change the decompiled source language from C# to VB.NET or IL without rerunning your query.
* Display assembly and class heirarchy browser.

### Getting the plugin to work

1. Clone the repository `git clone --recursive https://github.com/vincpa/linqpadspy.git`

2. cd in to the ilspy directory `cd path\to\cloned\repository\linqpadspy\ilspy`

3. Set the correct HEAD revision for the linqpadspy branch `git checkout linqpadspy`

4. Open the solution in Visual Studio 2012/2013 and add reference to LINQPad.exe to the the LinqPadSpy.Plugin project. Then compile the solution in Release mode and copy the binaries to the LINQPad plugins directory. Be sure to copy all DLL and EXE files except for LINQPad.exe.

### Compatibility

* .NET 4
* Tested with LINQPad 4.48.01 (Any CPU)

### Known issues

* On occasion you'll get a COM interop exception.
* Selecting mscorlib from the assembly list will generate an exception.
* The first decompilation can be slow but will be quicker thereafter.
