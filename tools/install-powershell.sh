#!/bin/bash
###############################################################
#                Unofficial 'Bash strict mode'                #
# http://redsymbol.net/articles/unofficial-bash-strict-mode/  #
###############################################################
set -euo pipefail
IFS=$'\n\t'
###############################################################


if echo "$(dotnet tool list -g)" | grep -q "powershell"; then
  echo "PowerShell installation found - updating..."
  command="update"
else
  echo "PowerShell installation not found - installing..."
  command="install"
fi

dotnet tool "${command}" -g powershell
