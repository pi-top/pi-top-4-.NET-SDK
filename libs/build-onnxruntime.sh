#!/bin/bash
set -ex

#######################################################################################
# Modified from                                                                       #
# https://github.com/microsoft/onnxruntime/blob/master/dockerfiles/Dockerfile.arm32v7 #
#######################################################################################

# Write to file
readonly LOG_FILE="~/run.log"
exec 1>$LOG_FILE
exec 2>&1


# Set up build
if [[ "$(arch)" == "armv7l" ]];
  PLATFORM="arm"
else
  PLATFORM="arm64"
fi
BUILDTYPE="MinSizeRel"

# Use latest backported packages
sudo rm /etc/apt/sources.list.d/buster-backports.list || true
echo "deb http://deb.debian.org/debian buster-backports main" | sudo tee /etc/apt/sources.list.d/buster-backports.list

# sudo apt install -y debian-ports-archive-keyring
# sudo apt-key adv --keyserver keyserver.ubuntu.com --recv-keys 04EE7237B7D453EC 648ACFD622F3D138
sudo apt update
sudo apt -t buster-backports install -y build-essential curl libcurl4-openssl-dev libssl-dev wget python3 python3-pip \
   python3-dev git tar libatlas-base-dev cmake protobuf-compiler
sudo -H pip3 install --upgrade pip setuptools wheel numpy flake8

# Download
cd ~
rm -rf onnxruntime || true
git clone --depth=1 --single-branch --branch master --recursive https://github.com/Microsoft/onnxruntime onnxruntime

# Patch: https://qiita.com/linyixian/items/bd7b8378da8c1fc9d24d
echo "set(CMAKE_CXX_LINK_FLAGS \"${CMAKE_CXX_LINK_FLAGS} -latomic\")" >> ~/onnxruntime/cmake/CMakeLists.txt

# Build
cd ~/onnxruntime
./build.sh --use_openmp --config "${BUILDTYPE}" "--${PLATFORM}" --update --build --build_shared_lib --build_wheel

# Show Build Output
ls -l ~/onnxruntime/build/Linux/${BUILDTYPE}/*.so
ls -l ~/onnxruntime/build/Linux/${BUILDTYPE}/dist/*.whl
