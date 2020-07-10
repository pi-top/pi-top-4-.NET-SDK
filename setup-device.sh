#!/bin/bash -e

####################
### Install .NET ###
####################
curl -L https://dot.net/v1/dotnet-install.sh | bash -e
cat << \EOF >> ~/.bashrc
# .NET Core SDK tools
export PATH=${PATH}:${HOME}/.dotnet
export PATH=${PATH}:${HOME}/.dotnet/tools
export DOTNET_ROOT=${HOME}/.dotnet
export LD_LIBRARY_PATH=${LD_LIBRARY_PATH}:/usr/local/lib
EOF
source ~/.bashrc
mkdir -p ~/localNuget
mkdir -p ~/.nuget/NuGet/
cat << \EOF > ~/.nuget/NuGet/Nuget.Config
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <solution>
    <add key="disableSourceControlIntegration" value="true" />
  </solution>
  <packageSources>
    <clear />
    <add key="local" value="/home/pi/localNuget" />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />            
    <add key="dotnet-try" value="https://dotnet.myget.org/F/dotnet-try/api/v3/index.json" />
    <add key="roslyn" value="https://dotnet.myget.org/F/roslyn/api/v3/index.json" />
    <add key="dotnet-corefxlab" value="https://dotnet.myget.org/F/dotnet-corefxlab/api/v3/index.json" />    
    <add key="PSGallery" value="https://www.powershellgallery.com/api/v2/" />
  </packageSources>
</configuration>
EOF
################################
### Install .NET interactive ###
################################
cd
git clone https://github.com/colombod/pi-top.git pi-top-net-api
cd pi-top-net-api/tools
bash -e ./install-global-tool.sh
###########################################
### Install System.Drawing dependencies ###
###########################################
sudo apt install -y libc6-dev libgdiplus
##############################
### Build .NET OpenCVSharp ###
##############################
sudo tar -xzvf ../libs/opencv_opencvsharp_libs.tar.gz -C /usr/local/lib/
sudo ldconfig
#############################
### Build pi-top .NET API ###
#############################
bash -e ./pack.sh 1.0.0
#######################
### Install Jupyter ###
#######################
cd
# Create virtual env
sudo apt install virtualenv -y
virtualenv .jupyter_venv -p python3
source .jupyter_venv/bin/activate
# Install jupyter
pip3 install jupyter jupyterlab
### Install the .NET kernel
dotnet interactive jupyter install
deactivate