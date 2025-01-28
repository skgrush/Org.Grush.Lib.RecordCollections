# Benchmarks

Benchmarks comparing `RecordCollection<T>` to `List<T>` in some common record use caes.

## Running the benchmarks.

Requires .NET 8 SDK.

To execute all benchmarks and update the Benchmarks.*.md file, run:

```sh
# sudo allows setting high priority process
sudo dotnet run -c Release -- -f '*'
```

To run interactively without updating the markdown file, run:

```sh
# sudo allows setting high priority process
sudo dotnet run -c Release -- --no-output
```
