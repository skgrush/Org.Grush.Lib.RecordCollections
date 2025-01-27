# Tests

## Run tests and view code coverage

```sh
rm -rf ./TestResults/ && \
dotnet test --collect:"XPlat Code Coverage" && \
dotnet tool restore && \
dotnet reportgenerator -reports:$(find ./TestResults/*/coverage.cobertura.xml) -targetdir:coveragereport
```
