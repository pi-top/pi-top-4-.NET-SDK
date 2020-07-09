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

Configure the Nuget soruces by having a ```Nuget.Config``` file in the folder ```~/.nuget/NuGet```

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
    <add key="dotnet-try" value="https://dotnet.myget.org/F/dotnet-try/api/v3/index.json" />
    <add key="roslyn" value="https://dotnet.myget.org/F/roslyn/api/v3/index.json" />
    <add key="dotnet-corefxlab" value="https://dotnet.myget.org/F/dotnet-corefxlab/api/v3/index.json" />    
    <add key="PSGallery" value="https://www.powershellgallery.com/api/v2/" />
  </packageSources>
</configuration>

```