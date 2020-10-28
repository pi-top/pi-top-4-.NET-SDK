#!/bin/bash
###############################################################
#                Unofficial 'Bash strict mode'                #
# http://redsymbol.net/articles/unofficial-bash-strict-mode/  #
###############################################################
set -euo pipefail
IFS=$'\n\t'
###############################################################


if echo "${dotnet_tool_list}" | grep -q "microsoft.dotnet-interactive"; then
  echo ".NET Interactive installation found - updating..."
  command="update"
else
  echo ".NET Interactive installation not found - installing..."
  command="install"
fi

dotnet tool "${command}" -g --add-source "https://dotnet.myget.org/F/dotnet-try/api/v3/index.json" Microsoft.dotnet-interactive
