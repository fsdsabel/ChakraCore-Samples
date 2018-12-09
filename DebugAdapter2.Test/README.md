# Example for Package ChakraCore.NET.DebugAdapter2.VSCode

This example shows how to use the ChakraCore<i></i>.NET debugger in combination with VS code.

Please note that the project does not currently compile and work out of the box. This is because necessary changes have not made their way back to the main repository. Therefore ChakraCore<i></i>.NET has to be built from the repository at https://github.com/fsdsabel/ChakraCore.NET.

Then reference the DLLs built there.


### Starting the sample

Build the sample with configuration "Debug" and start it. It might be necessary to copy ChakraCore.dll (this contains the JS engine) to the output directory. After the program started, it will wait for the debugger to attach. This is done at the line 

```charp
debugger.WaitForDebugger();
```

### Connection Visual Studio Code

Connect to the sample application using [Visual Studio Code](https://code.visualstudio.com/).

1. Navigate to the Scripts folder in bin and launch Visual Studio Code:

```
> cd <Source folder>\DebugAdapter2.Test\bin\Debug\Scripts
> code .
```

2. Switch to the "Debug (Ctrl+Shift+D)" panel and click the "Configure or Fix 'launch.json'" gear button.
3. Select the "Node.js" environment.
4. Add a new configuration:

```
{
    "type": "node",
    "request": "attach",
    "name": "Attach",
    "port": 5000,
    "protocol": "inspector"
}
```
5. Save the "launch.json" file and click the "Start Debugging" play button.
6. Execution should break on the first line of the script.

