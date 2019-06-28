echo "Getting library files..."
echo "  Dir: $PWD"

bash install-package-from-libs-repository.sh GreenSense NUnit 2.6.4 || exit 1
bash install-package-from-libs-repository.sh GreenSense NUnit.Runners 2.6.4 || exit 1
bash install-package-from-libs-repository.sh GreenSense Newtonsoft.Json 11.0.2 || exit 1
bash install-package-from-libs-repository.sh GreenSense M2Mqtt 4.3.0.0 || exit 1

bash install-package-from-github-release.sh CompulsiveCoder ArduinoSerialControllerClient 1.1.1.11 || exit 1

echo "Finished getting library files."
