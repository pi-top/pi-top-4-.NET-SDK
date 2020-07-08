#!/bin/bash

dotnet pack ../src/PiTopMakerArchitecture.Foundation.Psi.nuget/PiTopMakerArchitecture.Foundation.Psi.nuget.csproj /p:PackageVersion=$1 -o /home/pi/localNuget
dotnet pack ../src/PiTopMakerArchitecture.Foundation.nuget/PiTopMakerArchitecture.Foundation.nuget.csproj /p:PackageVersion=$1 -o /home/pi/localNuget
dotnet pack ../src/PiTop.Camera.Psi.nuget/PiTop.Camera.Psi.nuget.csproj /p:PackageVersion=$1 -o /home/pi/localNuget
dotnet pack ../src/PiTop.Camera.nuget/PiTop.Camera.nuget.csproj /p:PackageVersion=$1 -o /home/pi/localNuget
dotnet pack ../src/PiTop.nuget/PiTop.nuget.csproj /p:PackageVersion=$1 -o /home/pi/localNuget