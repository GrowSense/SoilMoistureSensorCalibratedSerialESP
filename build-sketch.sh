#!/bin/bash

echo "Building sketch..."

pio run || exit 1

echo "Finished building sketch."

