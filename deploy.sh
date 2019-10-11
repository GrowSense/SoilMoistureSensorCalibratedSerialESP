#!/bin/bash

. ./garden-credentials.sh

BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')

if [ "$BRANCH" = "master" ]
then
  echo "Deploying to host: $GARDEN_HOSTNAME"
  
  # Messy hack to get deployment working. This should be installed by default into the jenkins container but that doesnt seem to be working
#  sudo apt-get update && sudo apt-get -y install sshpass

   sshpass -p "$GARDEN_PASSWORD" ssh -o PreferredAuthentications=password -o PubkeyAuthentication=no -o StrictHostKeyChecking=no -l $GARDEN_USER $GARDEN_HOSTNAME "/bin/echo hello"
#  sshpass -p $GARDEN_PASSWORD ssh -o StrictHostKeyChecking=no -t $GARDEN_USER@$GARDEN_HOSTNAME "bash -c 'cd /home/$GARDEN_USER/workspace/GrowSense/Index/sketches/monitor/SoilMoistureSensorCalibratedSerial && sh pull-build-and-upload.sh'"

else
  echo "Can't deploy dev branch. Switch to master."
fi

