DIR=$PWD

echo "Initializing project"

pio lib install ntpclient

sh init-testing.sh
