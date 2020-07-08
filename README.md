# pi-top
.NET core api for [pi-top4](https://www.pi-top.com/products/pi-top-4)

The src directory contains the code for libraries you can use to create .NET core app for the amazing [pi-top4 platform](https://www.pi-top.com/products/pi-top-4). Get one and get creative.

Requires [.NET Core sdk 3.1 LTS for ARM32](https://dotnet.microsoft.com/download/dotnet-core/thank-you/sdk-3.1.201-linux-arm32-binaries)

The libraries comes along with [dotnet interactive](https://github.com/dotnet/interactive/) intergration so you can use notebooks to explore the power of pi-top.

To use the notebook sample you will need jupyter and the dotnet interactive tool installed, if you don't have them follow this instructions

 * install the dotnet interactive tool as shown [here](https://github.com/dotnet/interactive/)
 * [install jupyter and jupyter lab module](./docs/install-jupyter.md)
 * install [Camera library dependencies](./docs/install-camera-depednencies.md)

Build the libraries and packages

 * build the project ```>dotnet build```
 * if you do not have it, create the folder ```/home/pi/localNuget```
 * pack the proejcts with ```> dotnet pack /p:PackageVersion=1.1.1 -o /home/pi/localNuget```
  

Look at this example.
  
![image](https://user-images.githubusercontent.com/375556/80700336-71322400-8ad5-11ea-8eb1-6122c9cac554.png)
