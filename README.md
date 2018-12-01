## LINQPadSpy

> This project is no longer maintained. LINQPad now has a keyboard shortcut which launches ILSpy. Not as nice as having it embedded, but good enough.

LINQPadSpy shows decompiled sources from queries which have been compiled by LINQPad allowing one to see any additional compiler generated code. [Download the latest release](http://vincpa.github.io/linqpadspy/).

![LINQPadSpy Screenshot](https://github.com/vincpa/LINQPadSpy/raw/master/LINQPadSpy.JPG)

### Goals

Gain a deeper understanding of the C# language and compiler with LINQPad.

### Getting started and contributing

Currently the implementation is a bit of a hack. I've made some small changes to a fork of ILSpy and am re-using the UI text view control and all of ILSpy's built in functionality. I expect the quality of code to improve should there be more interest or I have more spare time available.

I have a bunch of features I'd like to implement and they're all listed on the project issue listing page, just filter by enhancement. If you'd like to contribute take a look at the issue listing and leave a comment so I know you're working on it.


### Current feature set

* Decompiles source code when the query contains a `this.DumpDasm()` (C#) or `Me.DumpDasm()` (VB.NET) in the execution path. You can also decompile an F# compiled assembly but the decompiled source will be in C#.
* Ability to change the decompiled source language from C# to VB.NET or IL without rerunning your query.
* Display assembly and class heirarchy browser.

### Getting the plugin to work

Unless you plan on making changes and compiling the source, your better off downloading the precompiled binaries.  [Download the latest release here](http://vincpa.github.io/linqpadspy).

1. Clone the repository `git clone --recursive https://github.com/vincpa/LINQPadSpy.git`
2. cd in to the ilspy directory `cd path\to\cloned\repository\LINQPadSpy\ilspy`
3. Run `git submodule foreach git pull origin master`
4. Set the correct HEAD revision for the LINQPadSpy branch `git checkout LINQPadSpy`
5. Open the solution in Visual Studio 2012/2013 and add reference to LINQPad.exe to the the LINQPadSpy.Plugin project. Then compile the solution in Release mode and copy the binaries to the LINQPad plugins directory. Be sure to copy all DLL and EXE files except for LINQPad.exe.

### Compatibility

* .NET 4
* Tested with LINQPad 4.48.01 (Any CPU)

### Known issues

* The first decompilation can be slow but will be quicker thereafter.
