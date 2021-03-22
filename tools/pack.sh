#!/bin/bash
###############################################################
#                Unofficial 'Bash strict mode'                #
# http://redsymbol.net/articles/unofficial-bash-strict-mode/  #
###############################################################
set -euo pipefail
IFS=$'\n\t'
###############################################################


DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" >/dev/null 2>&1 && pwd)"

root="$(dirname "${DIR}")"

# Default to using version 1.1.1 and installing in /home/pi/localNuget
version="${1:-0.0.1-local}"
localNugetPath="${2:-/home/pi/localNuget}"

dotnet build "${root}/src" -c Release
dotnet pack "${root}/src" "/p:PackageVersion=${version}" -o "${localNugetPath}" -c Release
