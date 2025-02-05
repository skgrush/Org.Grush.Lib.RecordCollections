#!/usr/bin/env bash

thisDir="$(dirname "$0")"
testResultsDir="./TestResults"

cd $thisDir

sect="==============================="

echo "Clean up existing tests"
rm -rf "$testResultsDir"

echo $sect

echo "Run tests with coverage"
dotnet test --collect:"XPlat Code Coverage"
testStatus=$?

if (( testStatus > 0 )); then
  echo "ERROR: Failed tests"
  cd -
  exit $testStatus
fi

echo $sect


echo "Running tests..."
dotnet tool restore && \
  dotnet reportgenerator -reports:"$(find "$testResultsDir"/*/coverage.cobertura.xml)" -targetdir:coveragereport

reportResult=$?

if (( reportResult > 0 )); then
  echo "ERROR: Reporting failed"
  cd -
fi

#grepResult=$(
#  grep -onE '<coverage line-rate="(.+?)" branch-rate="(.+?)"'  "$testResultsDir"/*/coverage.cobertura.xml
#)
#
#if [[ "$grepResult" != '2:<coverage line-rate="1" branch-rate="1"' ]]; then
#  echo "ERROR: code coverage regression: $grepResult"
#  cd -
#  exit 99
#fi
#
#echo "Success, perfect coverage."
#
#cd -

exit 0
