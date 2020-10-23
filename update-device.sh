#!/bin/bash
###############################################################
#                Unofficial 'Bash strict mode'                #
# http://redsymbol.net/articles/unofficial-bash-strict-mode/  #
###############################################################
set -euo pipefail
IFS=$'\n\t'
###############################################################


###############################
### Update .NET interactive ###
###############################
cd
rm -rf pi-top-net-api || true
git clone https://github.com/pi-top/pi-top-4-.NET-Core-API.git pi-top-net-api
echo ""
cd pi-top-net-api/tools

dotnet_tool_list="$(dotnet tool list -g)"

if echo "${dotnet_tool_list}" | grep -q "microsoft.dotnet-interactive"; then
  echo ".NET Interactive installation found - updating..."
  bash -ex ./update-global-tool.sh
else
  echo ".NET Interactive installation not found - installing..."
  bash -ex ./install-global-tool.sh
fi
echo ""

###############################
### Rebuild pi-top .NET API ###
###############################
echo "Building pi-top .NET API..."
bash -ex ./pack.sh 1.1.1 "/home/pi/localNuget"
echo ""
