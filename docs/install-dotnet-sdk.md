# Install .NET SDK on piTop

Download the [binaries for arm32](https://dotnet.microsoft.com/download/dotnet-core/thank-you/sdk-3.1.301-linux-arm32-binaries).
Make sure to have your environment variables set in .bashrc
```sh
export PATH=$PATH:$HOME/dotnet
export PATH=$PATH:$HOME/.dotnet/tools
export DOTNET_ROOT=$HOME/dotnet
```

Create a folder to use a local nuget source
```sh
pi@pi-top:~ mkdir /home/pi/localNuget
```

Configure the Nuget sources by having a ```Nuget.Config``` file in the folder ```~/.nuget/NuGet```

The file should contain the following sources
```xml

<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <solution>
    <add key="disableSourceControlIntegration" value="true" />
  </solution>
  <packageSources>
    <clear />
    <add key="local" value="/home/pi/localNuget" />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
    <add key="dotnet-eng" value="https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-eng/nuget/v3/index.json" />
    <add key="dotnet-tools" value="https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-tools/nuget/v3/index.json" />
    <add key="dotnet3-dev" value="https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet3.1/nuget/v3/index.json" />
    <add key="dotnet5" value="https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet5/nuget/v3/index.json" />
    <add key="MachineLearning" value="https://pkgs.dev.azure.com/dnceng/public/_packaging/MachineLearning/nuget/v3/index.json" />
    <add key="PSGallery" value="https://www.powershellgallery.com/api/v2/" />
  </packageSources>
</configuration>

```
