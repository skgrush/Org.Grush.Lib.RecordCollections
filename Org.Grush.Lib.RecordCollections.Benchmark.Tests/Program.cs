using System.Diagnostics;
using System.Reflection;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

const string noOutputArg = "--no-output";

var filteredArgs = args
  .Where(arg => arg is not noOutputArg)
  .ToArray();


Stopwatch stopwatch = Stopwatch.StartNew();

stopwatch.Start();
var summaries = BenchmarkSwitcher
  .FromAssembly(typeof(Program).Assembly)
  .Run(filteredArgs)
  .ToArray();
stopwatch.Stop();

if (args.Contains(noOutputArg))
{
  Console.WriteLine("\n\nSkipping normal markdown procedure. ");
  return 0;
}

var timestamp = DateTimeOffset.Now;

var benchmarksDir = Path.GetFullPath(Path.Join(AppContext.BaseDirectory, "../../../Benchmarks.md"));
var file = new FileInfo(benchmarksDir);
Console.WriteLine($"Will write to: {file.FullName}");
if (file.Exists)
  file.Delete();
await using var writer = new StreamWriter(file.FullName);

await writer.WriteAsync("# Benchmarks\n");
writer.Write(
  """
  ```
  Finished: {0:o}
  Elapsed: {1}
  ```

  """, timestamp, stopwatch.Elapsed);
await writer.FlushAsync();


var exporter = MarkdownExporter.GitHub;

foreach (Summary summary in summaries)
{
  var (title, subtitle, classPath) = GetTitles(summary);

  if (title is null && subtitle is null)
  {
    await writer.WriteAsync($"\n\n## {summary.Title}\n\n");
  }
  else
  {
    if (title is not null)
      await writer.WriteAsync($"\n\n## {title}\n");
    if (subtitle is not null)
      await writer.WriteAsync($"### {subtitle}\n");
  }

  await writer.WriteLineAsync($"Class: {classPath}\n");
  await writer.FlushAsync();

  foreach (var line in exporter.ExportToFiles(summary, null!))
  {
    await using var reader = new FileStream(line, FileMode.Open);
    await reader.CopyToAsync(writer.BaseStream);
  }
  await writer.FlushAsync();

}

Console.WriteLine($"Wrote compound Markdown to {file.FullName}");

return 0;



static (string? title, string? subtitle, string classPath) GetTitles(Summary summary)
{
  var jobClass = summary.BenchmarksCases.Select(c => c.Descriptor.Type).Distinct().Single();

  var fields = jobClass.GetFields(BindingFlags.Public | BindingFlags.Static).ToDictionary(f => f.Name);

  return (
    title: (string?)fields.GetValueOrDefault("Title")?.GetValue(null),
    subtitle: (string?)fields.GetValueOrDefault("Subtitle")?.GetValue(null),
    classPath: jobClass.FullName!
  );
}
