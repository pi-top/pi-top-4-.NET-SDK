#!/bin/bash
###############################################################
#                Unofficial 'Bash strict mode'                #
# http://redsymbol.net/articles/unofficial-bash-strict-mode/  #
###############################################################
set -euo pipefail
IFS=$'\n\t'
###############################################################


#############################
### Install dependencies  ###
#############################
echo "Installing dependencies..."
sudo apt install -y libgdiplus libffi-dev zlib1g
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


#######################
### Get source repo ###
#######################
cd
if [[ -d "~/pi-top-4-.NET-SDK" ]]; then
  cd ~/pi-top-4-.NET-SDK
  git pull
else
  git clone https://github.com/pi-top/pi-top-4-.NET-SDK.git
  cd ~/pi-top-4-.NET-SDK
fi
echo ""

################################
### Install .NET interactive ###
################################
~/pi-top-4-.NET-SDK/tools/install-global-tool.sh
echo ""

##########################
### Install PowerShell ###
##########################
~/pi-top-4-.NET-SDK/tools/install-powershell.sh
echo ""

################################
### Install .NET OpenCVSharp ###
################################
echo "Installing .NET OpenCVSharp..."
echo ".NET OpenCVSharp: Extracting..."
sudo unzip -d /usr/local/lib/ ~/pi-top-4-.NET-SDK/libs/opencv-dotnet-4.5.0.zip
echo ".NET OpenCVSharp: Configuring dynamic linker run-time bindings..."
sudo ldconfig
echo ""

################################
### Install ONNX Runtimes ###
################################
echo "Installing ONNX Runtimes..."
echo "ONNX Runtimes: Extracting..."
sudo unzip -d /usr/local/lib/ ~/pi-top-4-.NET-SDK/libs/onnxruntime-1.5.2.zip
echo "ONNX Runtimes: Configuring dynamic linker run-time bindings..."
sudo ldconfig
echo ""


#############################
### Build pi-top .NET API ###
#############################
echo "Building pi-top .NET API..."
~/pi-top-4-.NET-SDK/tools/pack.sh 0.0.1-local "/home/pi/localNuget"
echo ""

##############
### Finish ###
##############
echo "pi-top .NET API is installed."
echo "To install add Jupyter notebook supportPlease run \`setup-device-jupyter.sh\`."
echo "Note, this will install jupyterlab and .NET kernel inside a Python virtualenv (~/.jupyter_venv)"
