#!/bin/bash

# Note: You may need to run this script with sudo

echo "Preparing for SoilMoistureSensorCalibratedSerialESP project"

DIR=$PWD

sudo apt-get update

# curl
if ! type "curl" > /dev/null; then
  sudo apt-get install -y curl
fi

# unzip
if ! type "unzip" > /dev/null; then
  sudo apt-get install -y unzip
fi

# git
if ! type "git" > /dev/null; then
  sudo apt-get install -y git
fi

# python
if ! type "python" > /dev/null; then
  sudo apt-get install -y python python-pip
fi

# platform.io
if ! type "pio" > /dev/null; then
  sudo python -c "$(curl -fsSL https://raw.githubusercontent.com/platformio/platformio/develop/scripts/get-platformio.py)"
fi

# mono
echo "USE_MONO4=$USE_MONO4"
if [ "$USE_MONO4" = 0 ]; then
  if ! type "msbuild" > /dev/null; then
    echo "Installing latest mono"
    VERSION_NAME=$(lsb_release -cs)
  
    sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
    echo "deb http://download.mono-project.com/repo/ubuntu stable-$VERSION_NAME main" | sudo tee /etc/apt/sources.list.d/mono-official-stable.list
  
    sudo apt-get update -qq && sudo apt-get install -y mono-devel mono-complete ca-certificates-mono msbuild
  else
    echo "Mono is already installed. Skipping install."
  fi
else
  if ! type "xbuild" > /dev/null; then
    echo "Using mono4"
    sudo apt-get install -y mono-devel mono-complete ca-certificates-mono
  else
    echo "Mono is already installed. Skipping install."
  fi
fi
echo "Checking mono version..."
mono --version

cd tests/nunit && \
  sh prepare.sh

cd $DIR
