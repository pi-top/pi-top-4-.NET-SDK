#!/bin/bash -e

################################
### Install .NET interactive ###
################################
cd
rm -rf pi-top-net-api || true
git clone https://github.com/pi-top/pi-top-4-.NET-Core-API.git pi-top-net-api
echo ""
cd pi-top-net-api/tools

dotnetToolList="$(dotnet tool list -g)"

if echo "${dotnetToolList}" | grep -q "microsoft.dotnet-interactive"; then
  echo ".NET Interactive installation found - updating..."
  bash -ex ./update-global-tool.sh
else
  echo ".NET Interactive installation not found - installing..."
  bash -ex ./install-global-tool.sh
fi
echo ""

#############################
### Build pi-top .NET API ###
#############################
echo "Building pi-top .NET API..."
bash -ex ./pack.sh 1.1.1 "/home/pi/localNuget"
echo ""


#######################
### Install Jupyter ###
#######################
cd

# Inside the virtual env: Install .NET kernel
echo "virtualenv: install .NET kernel..."
dotnet interactive jupyter install

deactivate
