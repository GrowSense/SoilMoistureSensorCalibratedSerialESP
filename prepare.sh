#!/bin/bash

# Note: You may need to run this script with sudo

echo "Preparing this system for the project"

DIR=$PWD

sudo apt-get update -qq || exit 1

# curl
if ! type "curl" > /dev/null; then
  sudo apt-get install -qq -y curl || exit 1
fi

# unzip
if ! type "unzip" > /dev/null; then
  sudo apt-get install -qq -y unzip || exit 1
fi

# git
if ! type "git" > /dev/null; then
  sudo apt-get install -qq -y git || exit 1
fi

sh prepare-sketch.sh && \
sh prepare-tests.sh && \


cd $DIR && \

echo "Finished preparing the system for your project."
