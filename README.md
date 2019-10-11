# SoilMoistureSensorCalibratedSerialESP
An ESP8266 WiFi sketch for a calibrated soil moisture sensor which outputs data via serial.

## Status

|    | master | dev |
| ------------- | ------------- | ------------- |
| Travis CI Build  | [![Build Status](https://travis-ci.org/GrowSense/SoilMoistureSensorCalibratedSerialESP.svg?branch=master)](https://travis-ci.org/GrowSense/SoilMoistureSensorCalibratedSerialESP)  | [![Build Status](https://travis-ci.org/GrowSense/SoilMoistureSensorCalibratedSerialESP.svg?branch=dev)](https://travis-ci.org/GrowSense/SoilMoistureSensorCalibratedSerialESP)  |
| Jenkins Hardware Test  | [![Build Status](http://greensense.hopto.org:8080/job/GrowSense/job/SoilMoistureSensorCalibratedSerialESP/job/master/badge/icon)](http:/greensense.hopto.org:8080/job/GrowSense/job/SoilMoistureSensorCalibratedSerialESP/job/master/)  | [![Build Status](http://greensense.hopto.org:8080/job/GrowSense/job/SoilMoistureSensorCalibratedSerialESP/job/dev/badge/icon)](http:/greensense.hopto.org:8080/job/GrowSense/job/SoilMoistureSensorCalibratedSerialESP/job/dev/)  |

## Clone the Index
If you intend to use more than one software component from the GrowSense group it is recommended you clone the entire index.
This repository is included as a submodule along with a number of others you may need, or find useful.

You can find it here:
https://github.com/GrowSense/Index

To clone the index run:

```
git clone --recursive git://github.com/GrowSense/Index.git GrowSense/Index
```

Then navigate to the directory:
```
cd GrowSense/Index/sketches/monitor/SoilMoistureSensorCalibratedSerialESP
```

## Clone this Repository Only
To clone this repository only choose either...

### Using the same directory structure as the index (recommended):

```
git clone https://github.com/GrowSense/SoilMoistureSensorCalibratedSerialESP.git GrowSense/Index/sketches/monitor/SoilMoistureSensorCalibratedSerialESP
```
Then navigate to the directory:
```
cd GrowSense/Index/sketches/monitor/SoilMoistureSensorCalibratedSerialESP
```

### Using the current directory:

```
git clone https://github.com/GrowSense/SoilMoistureSensorCalibratedSerialESP.git
```

## Prepare and Initialize
Note: This only has to be done once before the first build. It doesn't need to be repeated unless there has been an update.

Run the prepare script to install required system software:

```
sh prepare.sh
```

Run the init script to get required project libraries:

```
sh init.sh
```

## Build and Upload
Run the build script to compile the sketch:

```
sh build.sh
```

Run the upload script to upload the sketch to the attached arduino based device:

```
sh upload.sh
```

## Monitor Serial Output

Run the monitor serial output sketch to view the data coming from the device:

```
sh monitor-serial.sh
```

Press CTRL+C to close the serial monitor.

## Run Automated Tests
With the automated test hardware connected run the test script:

```
sudo sh test.sh
```


