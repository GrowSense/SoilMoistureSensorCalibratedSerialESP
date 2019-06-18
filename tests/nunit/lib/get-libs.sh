echo "Getting library files..."
echo "  Dir: $PWD"

bash install-package.sh NUnit 2.6.4
bash install-package.sh NUnit.Runners 2.6.4
bash install-package.sh Newtonsoft.Json 11.0.2
bash install-package.sh ArduinoSerialControllerClient 1.1.1.5
bash install-package.sh M2Mqtt 4.3.0.0

echo "Finished getting library files."
