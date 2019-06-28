#!/bin/bash

CATEGORY=$1

if [ -z "$CATEGORY" ]; then
    CATEGORY="Unit"
fi

echo "Testing project"
echo "  Dir: $PWD"
echo "  Category: $CATEGORY"

mono lib/NUnit.Runners/tools/nunit-console.exe bin/Release/*.dll --include=$CATEGORY $2 $3 $4 $5
