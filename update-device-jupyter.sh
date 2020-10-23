#!/bin/bash
###############################################################
#                Unofficial 'Bash strict mode'                #
# http://redsymbol.net/articles/unofficial-bash-strict-mode/  #
###############################################################
set -euo pipefail
IFS=$'\n\t'
###############################################################


cd

# Create virtual env
echo "Entering virtualenv..."
source .jupyter_venv/bin/activate

# Inside the virtual env: Install .NET kernel
echo "virtualenv: install .NET kernel..."
dotnet interactive jupyter install

deactivate
