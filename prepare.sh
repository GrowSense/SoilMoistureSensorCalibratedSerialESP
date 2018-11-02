#!/bin/bash

# Note: You may need to run this script with sudo

echo "Preparing for project"

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

sh prepare-sketch.sh
sh prepare-tests.sh


cd $DIR
