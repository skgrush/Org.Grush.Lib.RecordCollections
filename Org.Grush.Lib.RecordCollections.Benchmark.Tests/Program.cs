﻿using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

const string noOutputArg = "--no-output";

var filteredArgs = args
  .Where(arg => arg is not noOutputArg)
  .ToArray();


var summaries = BenchmarkSwitcher
  .FromAssembly(typeof(Program).Assembly)
  .Run(filteredArgs);

if (args.Contains(noOutputArg))
{
  Console.WriteLine("\n\nSkipping normal markdown procedure. ");
  return 0;
}

var timestamp = DateTimeOffset.Now;

var file = new FileInfo("../../../Benchmarks.md");
if (file.Exists)
  file.Delete();
await using var writer = new StreamWriter(file.FullName);

writer.Write("# Benchmarks ({0:o})\n", timestamp);
writer.Flush();


var exporter = MarkdownExporter.GitHub;

foreach (Summary summary in summaries)
{
  await writer.WriteAsync($"\n\n## {summary.Title}\n\n");
  await writer.FlushAsync();

  foreach (var line in exporter.ExportToFiles(summary, null))
  {
    await using var reader = new FileStream(line, FileMode.Open);
    await reader.CopyToAsync(writer.BaseStream);
  }
  await writer.FlushAsync();

}

Console.WriteLine($"Wrote compound Markdown to {Path.GetFullPath(file.FullName)}");

return 0;
