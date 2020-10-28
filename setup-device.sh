#!/bin/bash
###############################################################
#                Unofficial 'Bash strict mode'                #
# http://redsymbol.net/articles/unofficial-bash-strict-mode/  #
###############################################################
set -euo pipefail
IFS=$'\n\t'
###############################################################


##########################################################
### Install dependencies and virtualenv ###
##########################################################
echo "Installing dependencies..."
sudo apt install -y libgdiplus virtualenv libffi-dev zlib1g
echo ""


####################
### Install .NET ###
####################
echo "Installing .NET..."
curl -L https://dot.net/v1/dotnet-install.sh | bash -e
echo ""

echo "Updating PATH, DOTNET_ROOT and LD_LIBRARY_PATH environment variables..."
if ! grep -q ".NET Core SDK tools" "/home/pi/.bashrc"; then
  cat << \EOF >> "/home/pi/.bashrc"
# .NET Core SDK tools
export PATH=${PATH}:/home/pi/.dotnet
export PATH=${PATH}:/home/pi/.dotnet/tools
export DOTNET_ROOT=/home/pi/.dotnet
export LD_LIBRARY_PATH=${LD_LIBRARY_PATH:-}:/usr/local/lib
EOF
fi
export PATH=${PATH}:/home/pi/.dotnet
export PATH=${PATH}:/home/pi/.dotnet/tools
export DOTNET_ROOT=/home/pi/.dotnet
export LD_LIBRARY_PATH=${LD_LIBRARY_PATH:-}:/usr/local/lib
echo ""


echo "Installing NuGet sources..."
nugetListSource="$(dotnet nuget list source)"

add_nuget_src() {
  local packageSourcePath="${1}"
  local name="${2}"

  if echo "${nugetListSource}" | grep -q "${packageSourcePath}"; then
    dotnet nuget update source "${name}" -s "${packageSourcePath}"
  else
    dotnet nuget add source "${packageSourcePath}" -n "${name}"
  fi
}

mkdir -p "/home/pi/localNuget"
add_nuget_src "/home/pi/localNuget" local
add_nuget_src https://api.nuget.org/v3/index.json nuget.org
add_nuget_src https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-eng/nuget/v3/index.json dotnet-eng
add_nuget_src https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-tools/nuget/v3/index.json dotnet-tools
add_nuget_src https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet3.1/nuget/v3/index.json dotnet3-dev
add_nuget_src https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet5/nuget/v3/index.json dotnet5
add_nuget_src https://pkgs.dev.azure.com/dnceng/public/_packaging/MachineLearning/nuget/v3/index.json MachineLearning
add_nuget_src https://www.powershellgallery.com/api/v2/ PSGallery
echo ""


################################
### Install .NET interactive ###
################################
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

##########################
### Install PowerShell ###
##########################

cd ~/pi-top-net-api/tools

dotnet_tool_list="$(dotnet tool list -g)"

if echo "${dotnet_tool_list}" | grep -q "powershell"; then
  echo "PowerShell installation found - updating..."
  bash -ex ./update-powershell.sh
else
  echo "PowerShell installation not found - installing..."
  bash -ex ./install-powershell.sh
fi
echo ""

################################
### Install .NET OpenCVSharp ###
################################
echo "Installing .NET OpenCVSharp..."
echo ".NET OpenCVSharp: Extracting..."
sudo unzip -d /usr/local/lib/ ../libs/opencv-dotnet-4.5.0.zip
echo ".NET OpenCVSharp: Configuring dynamic linker run-time bindings..."
sudo ldconfig
echo ""


#############################
### Build pi-top .NET API ###
#############################
echo "Building pi-top .NET API..."
bash -ex ./pack.sh 0.0.1 "/home/pi/localNuget"
echo ""
echo "pi-top .NET API is installed."
echo "Please run \`setup-device-jupyter.sh\` to install add Jupyter notebook support."
echo "Note, this will install jupyterlab and .NET kernel inside a Python virtualenv (~/.jupyter_venv)"
