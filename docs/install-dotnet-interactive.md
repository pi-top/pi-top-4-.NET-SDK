# Install .NET interactive on piTop

Make sure the .NET 3.1 sdk is installed
```sh
pi@pi-top:~ dotnet --list-sdks
3.1.102 [/home/pi/dotnet/sdk]
3.1.201 [/home/pi/dotnet/sdk]
3.1.300 [/home/pi/dotnet/sdk]
3.1.301 [/home/pi/dotnet/sdk]
```

If SDK 3.1 is not present follow the [install instructions](./install-dotnet-sdk.md)


You can now install latest dotnet interactive
```sh
 pi@pi-top:~ dotnet tool install -g --add-source "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-tools/nuget/v3/index.json" Microsoft.dotnet-interactive
```

And later on update
```sh
 pi@pi-top:~ dotnet tool update -g --add-source "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-tools/nuget/v3/index.json" Microsoft.dotnet-interactive
```

You can use the ```install-global-tool.sh``` script contained in the ```tools``` folder of the repo.
