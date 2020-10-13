## Build native runtimes NuGet pacakges

use the nuget.exe located in the `tool` directory on the root of the repo to package the native runtimes 

This is a command line example that builds the package for the onnx runtime
```../tools/nuget.exe pack Microsoft.Onnx.Runtime/1.5.2/pi-top.Onnx.runtime.nuspec -OutputDirectory ../Packages```