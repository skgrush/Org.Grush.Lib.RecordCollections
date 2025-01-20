# Benchmarks

To execute all benchmarks and update the [Benchmarks.md](./Benchmarks.md) file, run:

```sh
# sudo allows setting high priority process
sudo dotnet run -c Release -- -f '*'
```

To run interactively without updating the markdown file, run:

```sh
dotnet run -c Release -- --no-output
```
