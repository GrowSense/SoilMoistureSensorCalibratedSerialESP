PORT=$1

if [ ! $PORT ]; then
  echo "Specify the port as an argument."
  exit 1
fi

sh build-all.sh && \
sh upload.sh $PORT && \
sh test.sh
