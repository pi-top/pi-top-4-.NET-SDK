#!/bin/bash -e

##########################################################
### Install System.Drawing dependencies and virtualenv ###
##########################################################
echo "Installing dependencies..."
sudo apt install -y libgdiplus virtualenv
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
export LD_LIBRARY_PATH=${LD_LIBRARY_PATH}:/usr/local/lib
EOF
fi
export PATH=${PATH}:/home/pi/.dotnet
export PATH=${PATH}:/home/pi/.dotnet/tools
export DOTNET_ROOT=/home/pi/.dotnet
export LD_LIBRARY_PATH=${LD_LIBRARY_PATH}:/usr/local/lib
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
add_nuget_src https://dotnet.myget.org/F/dotnet-try/api/v3/index.json dotnet-try
add_nuget_src https://dotnet.myget.org/F/roslyn/api/v3/index.json roslyn
add_nuget_src https://dotnet.myget.org/F/dotnet-corefxlab/api/v3/index.json dotnet-corefxlab
add_nuget_src https://www.powershellgallery.com/api/v2/ PSGallery
echo ""


################################
### Install .NET interactive ###
################################
cd
rm -rf pi-top-net-api || true
git clone https://github.com/colombod/pi-top.git pi-top-net-api
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

##########################
### Install PowerShell ###
##########################

cd ~/pi-top-net-api/tools

dotnetToolList="$(dotnet tool list -g)"

if echo "${dotnetToolList}" | grep -q "powershell"; then
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
sudo tar -xzf ../libs/opencv_opencvsharp_libs.tar.gz -C /usr/local/lib/
echo ".NET OpenCVSharp: Configuring dynamic linker run-time bindings..."
sudo ldconfig
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

# Create virtual env
echo "Installing Jupyter: creating virtualenv..."
rm -rf .jupyter_venv || true
virtualenv .jupyter_venv -p python3
source .jupyter_venv/bin/activate

# Inside the virtual env: Install jupyter
echo "virtualenv: pip install jupyter jupyterlab..."
pip3 install jupyter jupyterlab

# Inside the virtual env: Install .NET kernel
echo "virtualenv: install .NET kernel..."
dotnet interactive jupyter install

deactivate
