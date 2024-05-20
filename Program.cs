using Benchmarkoutput;
using System;
using System.Collections.Generic;
#nullable enable



string? ver = TesseractOcrMaui.TessEngine.TryGetVersion();
Console.WriteLine(ver);

var bench = new Benchmarks();
bench.Setup();
//string text = bench.Recognize();
//string text2 = bench.RecognizeWithEngine();
//string text3 = bench.RecognizeConfigured();
//var text4 = bench.Recognize_ITesseract();
//Console.WriteLine(text4);
//var paramBench = new Benchmarks_Parametherized();
//paramBench.Setup();
//string result1 = paramBench.Recognize_Scaled(1);
//string result2 = paramBench.Recognize_Scaled(0.9);
//string result3 = paramBench.Recognize_Scaled(0.75);
//string result4 = paramBench.Recognize_Scaled(0.65);
//string result5 = paramBench.Recognize_Scaled(0.5);
List<string> result6 = bench.Recognize_Scaled_Iterator();
bench.Cleanup();
//string folder = Directory.GetCurrentDirectory();
//var config = DefaultConfig.Instance.WithArtifactsPath(Path.Combine(folder, "results"));
//BenchmarkRunner.Run<Benchmarks>(config, args);
//BenchmarkRunner.Run<Benchmarks_RecognizeScaled>(config, args);

// Use this to select benchmarks from the console:
// var summaries = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);
