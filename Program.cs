using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Benchmarkoutput;
using System;
using System.IO;
#nullable enable



string? ver = TesseractOcrMaui.TessEngine.TryGetVersion();
Console.WriteLine(ver);

//Console.WriteLine(File.Exists("eng.traineddata"));
//var bench = new Benchmarks();
//bench.Setup();
//string text = bench.Recognize();
//string text2 = bench.RecognizeWithEngine();


//var paramBench = new Benchmarks_Parametherized();
//paramBench.Setup();
//string result1 = paramBench.Recognize_Scaled(1);
//string result2 = paramBench.Recognize_Scaled(0.9);
//string result3 = paramBench.Recognize_Scaled(0.75);
//string result4 = paramBench.Recognize_Scaled(0.65);
//string result5 = paramBench.Recognize_Scaled(0.5);



string folder = Directory.GetCurrentDirectory();
var config = DefaultConfig.Instance.WithArtifactsPath(Path.Combine(folder, "results"));
BenchmarkRunner.Run<Benchmarks>(config, args);
BenchmarkRunner.Run<Benchmarks_Parametherized>(config, args);

// Use this to select benchmarks from the console:
// var summaries = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);
