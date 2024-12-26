#!/usr/bin/env bash

thisDir="$(dirname "$0")"
runtime=osx-arm64

sect="==============================="

echo "Publishing AotTests..."
echo $sect

dotnet publish "$thisDir" --runtime $runtime --self-contained

statusCodeOfPublish=$?

if [ $statusCodeOfPublish -ne 0 ]; then
  echo $sect
    echo "publish exited with $statusCodeOfPublish"
    exit $statusCodeOfPublish
fi

echo $sect
echo "Running AotTests..."
echo $sect

"$thisDir/bin/Release/net8.0/$runtime/publish/Org.Grush.Lib.RecordCollections.AotTests" --coverage

statusCodeOfExe=$?

echo $sect

echo "Exe exited with $statusCodeOfExe"
