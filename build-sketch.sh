#!/bin/bash

echo "Building sketch..."

pio run -s && \

echo "Finished building sketch." || \

(echo "Failed building sketch." && exit 1)


