# pi-top
.NET Core API for [pi-top4](https://www.pi-top.com/products/pi-top-4), check out the [dotnet/iot](https://github.com/dotnet/iot) repo for loads of device bindings!

[![Build status](https://ci.appveyor.com/api/projects/status/85dvsfxd4lw4xp2c?svg=true)](//ci.appveyor.com/project/colombod/pi-top/branch/master) [![Join the chat at https://gitter.im/pi-top-dotnet/community](https://badges.gitter.im/pi-top-dotnet/community.svg)](https://gitter.im/pi-top-dotnet/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

## Experimental!!
Set up all by running this from your pitop
```sh
pi@pi-top:~ curl -L https://raw.githubusercontent.com/colombod/pi-top/master/setup-device.sh | bash -e
```
Once executed you will have this repo cloned at
```sh
/home/pi/pi-top-net-api
```

The code is compiled and the NuGet packages version 1.1.1 are located at 
```sh
/home/pi/localNuget
```

You can activate the Python environment using the command
```sh
pi@pi-top:~ source .jupyter_venv/bin/activate
```

Later on can jsut update using the command
```sh
pi@pi-top:~ curl -L https://raw.githubusercontent.com/colombod/pi-top/master/update-device.sh | bash -e
```

## Manual installation steps

The `src` directory contains the code for libraries you can use to create .NET Core apps for the amazing [pi-top4 platform](https://www.pi-top.com/products/pi-top-4). Get one and get creative.

Requires [.NET Core SDK 3.1 LTS for ARM32](./docs/install-dotnet-sdk.md)

The libraries comes along with [`dotnet interactive`](https://github.com/dotnet/interactive/) integration so you can use notebooks to explore the power of pi-top.

 * install the `dotnet interactive` tool as shown [here](./docs/install-dotnet-interactive.md) 
 * install [Camera library dependencies](./docs/install-camera-dependencies.md)

To use the notebook examples in the folder `examples/notebooks` you will need jupyter lab 
* install [jupyter and jupyter lab modules](./docs/install-jupyter.md)

To use the notebook sampples in the folder `examples/vs-code` you will need vs-code and vs-code extension on your local machine
* install [vscode extension](./docs/vscode-extension.md)

Build the libraries and packages

 * build the project `> dotnet build`
 * if you do not have it, create the folder `/home/pi/localNuget`
 * pack the projects with `> sh tools/pack.sh 1.1.1` it will package the project into the `/home/pi/localnuget` using version 1.1.1


Look at this example.
  
![image](https://user-images.githubusercontent.com/375556/80700336-71322400-8ad5-11ea-8eb1-6122c9cac554.png)
