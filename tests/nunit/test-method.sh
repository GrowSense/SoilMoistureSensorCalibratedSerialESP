#!/bin/bash

TEST_NAME=$1

if [ ! "$TEST_NAME" ]; then
  echo "Specify a test name as an argument."
  exit 1
fi

echo "Testing project"
echo "  Dir: $PWD"
echo "  Test name: $TEST_NAME"

mono lib/NUnit.Runners.2.6.4/tools/nunit-console.exe bin/Release/*.dll --run=$TEST_NAME
