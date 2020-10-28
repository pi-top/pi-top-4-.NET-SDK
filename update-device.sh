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

################################
### Install .NET OpenCVSharp ###
################################
echo "Installing .NET OpenCVSharp..."
echo ".NET OpenCVSharp: Extracting..."
sudo unzip -d /usr/local/lib/ ../libs/opencv-dotnet-4.5.0.zip
echo ".NET OpenCVSharp: Configuring dynamic linker run-time bindings..."
sudo ldconfig
echo ""

################################
### Install ONNX Runtimes ###
################################
echo "Installing ONNX Runtimes..."
echo "ONNX Runtimes: Extracting..."
sudo unzip -d /usr/local/lib/ ../libs/onnxruntime-1.5.2.zip
echo "ONNX Runtimes: Configuring dynamic linker run-time bindings..."
sudo ldconfig
echo ""


###############################
### Rebuild pi-top .NET API ###
###############################
echo "Building pi-top .NET API..."
bash -ex ./pack.sh 0.0.1 "/home/pi/localNuget"
echo ""
