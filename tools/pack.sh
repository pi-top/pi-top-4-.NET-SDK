#!/bin/bash

DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" >/dev/null 2>&1 && pwd)"

root="$(dirname "${DIR}")"

version="${1:-1.1.1}"
localNugetPath="${2:-/home/pi/localNuget}"

dotnet build "${root}/src"

for project in "${root}/src/"**/*".nuget.csproj"
do
  echo "Packing ${project}"
  dotnet pack "${project}" "/p:PackageVersion=${version}" -o "${localNugetPath}"
done
