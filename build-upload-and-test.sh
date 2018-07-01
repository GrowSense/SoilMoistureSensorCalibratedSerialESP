PORT=$1

if [ ! $PORT ]; then
  echo "Specify the port as an argument."
  exit 1
fi

build.sh && \
upload.sh $PORT && \
sh test.sh
