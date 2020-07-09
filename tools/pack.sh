#!/bin/bash
dotnet build ../src
projects=$(find ../src/ -path "*.nuget.csproj" ) 
for project in $projects
do
 echo "poacking " $project
 dotnet pack $project /p:PackageVersion=$1 -o /home/pi/localNuget
done