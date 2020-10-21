#!/bin/bash
set -ex

apt-get install -y \
  zlib1g-dev \
  libjpeg-dev \
  libwebp-dev \
  libpng-dev \
  libtiff5-dev \
  libjasper-dev \
  libopenexr-dev \
  libgdal-dev \
  libdc1394-22-dev \
  libavcodec-dev \
  libavformat-dev \
  libswscale-dev \
  libtheora-dev \
  libvorbis-dev \
  libxvidcore-dev \
  libx264-dev \
  yasm \
  libopencore-amrnb-dev \
  libopencore-amrwb-dev \
  libv4l-dev \
  libxine2-dev \
  libtbb-dev \
  libeigen3-dev
# Download opencv & opencv-contrib
cd ~
mkdir src
cd src
mkdir opencv
cd opencv
wget https://github.com/opencv/opencv/archive/4.2.0.zip
unzip 4.2.0.zip
rm 4.2.0.zip
wget https://github.com/opencv/opencv_contrib/archive/4.2.0.zip
unzip 4.2.0.zip
rm 4.2.0.zip
# Build Opencv
cd opencv-4.2.0/
mkdir build
cd build
cmake -DOPENCV_EXTRA_MODULES_PATH=~/src/opencv/opencv_contrib-4.2.0/modules -D WITH_LIBV4L=ON -D CMAKE_BUILD_TYPE=RELEASE -D WITH_TBB=ON -D ENABLE_NEON=ON ..
time make -j4
sudo make install
sudo ldconfig
# Download Opencvsharp
cd ~/src
git clone https://github.com/shimat/opencvsharp.git
cd opencvsharp/src
# Checkout to correct version
git checkout tags/4.2.0.20200208
# Build Opencvsharp
sed -i.bak '5i\
include_directories("/usr/local/include/")\
' CMakeLists.txt
mkdir build
cd build
cmake ..
time make -j4
sudo make install
sudo ldconfig
##### Install
sudo tar -xzvf opencv_opencvsharp_libs.tar.gz -C /usr/local/lib/
export LD_LIBRARY_PATH=$LD_LIBRARY_PATH:/usr/local/lib
sudo ldconfig
##### Check dependencies in libs
for filename in /usr/local/lib/*; do
    for ((i=0; i<=3; i++)); do
        echo $filename
        ldd $filename | grep "/home/pi/src"
    done
done
