#!/bin/bash
###############################################################
#                Unofficial 'Bash strict mode'                #
# http://redsymbol.net/articles/unofficial-bash-strict-mode/  #
###############################################################
set -euo pipefail
IFS=$'\n\t'
###############################################################

# Modified from https://github.com/shimat/opencvsharp/issues/388#issuecomment-338617593

set -x

VERSION="4.5.0"
OPENCVSHARP_VERSION_REL_DATE="20201013"

install_build_deps() {
  sudo apt-get install -y \
    cmake \
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
}

go_to_fresh_dir() {
  local dir="${1}"
  rm -rf "${dir}" || true
  mkdir "${dir}"
  cd "${dir}"
}

get_from_github() {
  local username="${1}"
  local repo="${2}"
  local version="${3}"
  local file
  file="${repo}-${version}.zip"
  wget "https://github.com/${username}/${repo}/archive/${version}.zip" -O "${file}"
  unzip "${file}"
  rm "${file}"
}

build_and_install() {
  local cmake_args="${1:-}"
  go_to_fresh_dir build

  # Prepare build
  cmake "${cmake_args}" ..

  # Build
  time make "-j$(nproc)"

  # Install
  sudo make install

  # Update config
  sudo ldconfig
}

install_build_deps

################
# Download src #
################
go_to_fresh_dir "${HOME}/opencv-dotnet"

get_from_github "opencv" "opencv" "${VERSION}"                                       # creates opencv-${VERSION}
get_from_github "opencv" "opencv_contrib" "${VERSION}"                               # creates opencv_contrib-${VERSION}
get_from_github "shimat" "opencvsharp" "${VERSION}.${OPENCVSHARP_VERSION_REL_DATE}"  # creates opencvsharp-${VERSION}.${OPENCVSHARP_VERSION_REL_DATE}

##########
# OpenCV #
##########
(
  cd "opencv-${VERSION}"

  # Takes approx. 2 hours on Pi 4
  build_and_install "-DOPENCV_EXTRA_MODULES_PATH=\"${HOME}/opencv-dotnet/opencv_contrib-${VERSION}/modules\" \
    -D WITH_LIBV4L=ON \
    -D CMAKE_BUILD_TYPE=RELEASE \
    -D WITH_TBB=ON \
    -D ENABLE_NEON=ON"
)

###############
# OpenCvSharp #
###############
(
  cd "opencvsharp-${VERSION}.${OPENCVSHARP_VERSION_REL_DATE}/src"

  # Patch to include directory
  grep -q "/usr/local/include/" CMakeLists.txt || sed -i.bak '5i\
include_directories("/usr/local/include/")\
' CMakeLists.txt

  # Takes approx. 2 mins on Pi 4
  build_and_install
)

####################
# Package into zip #
####################
(
  cd /usr/local/lib
  zip "${HOME}/opencv-dotnet/opencv-dotnet-${VERSION}.zip" lib[Oo]pen[Cc]v*
)
