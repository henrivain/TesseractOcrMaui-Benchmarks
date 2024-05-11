using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Diagnostics.Tracing.Parsers.AspNet;
using SkiaSharp;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TesseractOcrMaui;
using TesseractOcrMaui.Iterables;
using TesseractOcrMaui.Results;
using TesseractOcrMaui.Tessdata;

#nullable enable

namespace Benchmarkoutput;

[MemoryDiagnoser]
public class Benchmarks
{
    ITessDataProvider _provider;
    ITessDataProvider _providerFast;
    const string _imagePath = @"C:\Users\henri\Downloads\loremIpsum.png";

    Pix _preloadedImage;


    [GlobalSetup]
    public void Setup()
    {
        _provider = new DummyProvider(Directory.GetCurrentDirectory(), "eng.traineddata");
        _providerFast = new DummyProvider(Directory.GetCurrentDirectory(), "eng_fast.traineddata");
        _preloadedImage = Pix.LoadFromFile(_imagePath);
    }





    [Benchmark]
    public void LoadImage()
    {
        using var image = Pix.LoadFromFile(_imagePath);
    }

    [Benchmark]
    public void LoadImageBytes()
    {
        byte[] bytes = File.ReadAllBytes(_imagePath);
        using var image = Pix.LoadFromMemory(bytes);
    }

   


    [Benchmark]
    public string RecognizeWithEngine()
    {
        using var engine = new TessEngine(_provider.GetLanguagesString(), _provider.TessDataFolder);
        using var page = engine.ProcessImage(_preloadedImage);
        return page.GetText();
    }
    
    [Benchmark]
    public string RecognizeWithEngine_Fast()
    {
        using var engine = new TessEngine(_providerFast.GetLanguagesString(), _providerFast.TessDataFolder);
        using var page = engine.ProcessImage(_preloadedImage);
        return page.GetText();
    }

    [Benchmark]
    public List<string> RecognizeWithIterator()
    {
        List<string> result = [];
        var iterator = new ResultIterable(_preloadedImage, _provider);
        foreach (var item in iterator)
        {
            result.Add(item.Text);
        }
        return result;
    }


    [GlobalCleanup]
    public void Cleanup()
    {
        _preloadedImage.Dispose();
    }
}
