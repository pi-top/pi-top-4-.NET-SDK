# Using vscode extensions

You can use vscode from your computer and connect to the piTop running ```.NET interactive```.
Follow the instructions [here](https://github.com/dotnet/interactive/blob/master/src/dotnet-interactive-vscode/README.md) to get the extension, then open the ```sample/vs-code``` folder.

You need ot start dotnet interactive in http mode, type this in your piTop
```sh
pi@pi-top:~ dotnet interactive [vscode] http --http-port 1024
```
or you can use the script ```start-dotnet-interactive.sh``` in the tools folder.