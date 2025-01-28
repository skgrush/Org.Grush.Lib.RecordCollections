#!/usr/bin/env bash

thisDir="$(dirname "$0")"
distDir="$thisDir/dist/aot-tests"

sect="==============================="

echo "Publishing AotTests..."
echo $sect

dotnet publish "$thisDir" \
  --use-current-runtime \
  --self-contained \
  --output "$distDir"


statusCodeOfPublish=$?

if [ $statusCodeOfPublish -ne 0 ]; then
  echo $sect
    echo "publish exited with $statusCodeOfPublish"
    exit $statusCodeOfPublish
fi

echo $sect
echo "Running AotTests..."
echo $sect

"$distDir/Org.Grush.Lib.RecordCollections.AotTests" --coverage

statusCodeOfExe=$?

echo $sect

echo "Exe exited with $statusCodeOfExe"

exit $statusCodeOfExe
