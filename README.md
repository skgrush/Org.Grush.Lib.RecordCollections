# Org.Grush.Lib.RecordCollections

[Org.Grush.Lib.RecordCollections](./Org.Grush.Lib.RecordCollections) core library.

[Org.Grush.Lib.RecordCollections.Newtonsoft](./Org.Grush.Lib.RecordCollections.Newtonsoft) Newtonsoft de/serialization library, if using Newtonsoft.

[Org.Grush.Lib.RecordCollections.Benchmark](./Org.Grush.Lib.RecordCollections.Benchmark.Tests) benchmark info.


## Development

This section is more of a devblog than a guide, documenting what how I've done it rather than recommending anything.

### macOS setup

TODO: On macOS, I use JetBrains Rider and have for long enough that I don't remember when/were I installed .NET!
But during the development of this project I used .NET SDK 8.0.301.

### Ubuntu setup

On my Ubuntu Server test bench, I'm lazy so I just used apt-get for installing .NET:

```sh
sudo apt-get update && \
  sudo apt-get install -y dotnet-sdk-8.0 dotnet-runtime-8.0

mkdir repos && cd repos # or whatever
git clone https://github.com/skgrush/Org.Grush.Lib.RecordCollections.git
cd Org.Grush.Lib.RecordCollections
```

When running remote benchmarks, I run
```sh
screen
cd Org.Grush.Lib.RecordCollections.Benchmark.Tests
dotnet run -c Release -- -f '*'
# ctrl+A D   to detach the screen

# later come back
screen -r # to reattach to the screen
```
