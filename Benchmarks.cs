using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Diagnostics.Tracing.Parsers.AspNet;
using Microsoft.Diagnostics.Tracing.Parsers.FrameworkEventSource;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TesseractOcrMaui;
using TesseractOcrMaui.Enums;
using TesseractOcrMaui.Imaging;
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
    public void LoadImage_FromFile()
    {
        using var image = Pix.LoadFromFile(_imagePath);
    }

    [Benchmark]
    public void LoadImage_Bytes()
    {
        byte[] bytes = File.ReadAllBytes(_imagePath);
        using var image = Pix.LoadFromMemory(bytes);
    }


    [Benchmark]
    public string Recognize_Engine()
    {
        using var engine = new TessEngine(_provider.GetLanguagesString(), _provider.TessDataFolder);
        using var page = engine.ProcessImage(_preloadedImage);
        return page.GetText();
    }


    [Benchmark]
    public string Recognize_EngineFastTraineddata()
    {
        using var engine = new TessEngine(_providerFast.GetLanguagesString(), _providerFast.TessDataFolder);

        using var page = engine.ProcessImage(_preloadedImage);
        return page.GetText();
    }

    [Benchmark]
    public List<string> Recognize_SymbolsOnlyIterator()
    {
        List<string> result = [];
        var iterator = new ResultIterable(_preloadedImage, _provider, PageIteratorLevel.Symbol);
        foreach (var item in iterator)
        {
            result.Add(item.Text);
        }
        return result;
    }

    [Benchmark]
    public string Recognize_EngineConfigured()
    {
        using var engine = new TessEngine(_provider.GetLanguagesString(), _provider.TessDataFolder);
        if (engine.TryGetBoolVar("tessedit_do_invert", out bool value1))
        {
            Console.WriteLine(value1);
        }

        engine.SetVariable("tessedit_do_invert", "FALSE");

        bool isSet = engine.TryGetBoolVar("tessedit_do_invert", out bool value2);
        if (isSet is false)
        {
            throw new Exception();
        }
        if (value2 is true)
        {
            throw new Exception();
        }

        using var page = engine.ProcessImage(_preloadedImage);
        return page.GetText();
    }

    [Benchmark]
    public List<string> Recognize_Iterator()
    {
        List<string> result = [];
        var iterator = new ResultIterable(_preloadedImage, _provider);
        foreach (var item in iterator)
        {
            result.Add(item.Text);
        }
        return result;
    }


    public List<BoundingBox> Recognize_CoordinatesOnly_TextLine()
    {
        List<BoundingBox> result = [];
        var iterator = new PageIterable(_preloadedImage, _provider, PageIteratorLevel.TextLine);
        foreach (var item in iterator)
        {
            result.Add(item.Box);
        }

        // You can then get the image with this size and process it
        return result;
    }

    [Benchmark]
    public List<BoundingBox> Recognize_CoordinatesOnly_Word()
    {
        List<BoundingBox> result = [];
        var iterator = new PageIterable(_preloadedImage, _provider, PageIteratorLevel.Word);
        foreach (var item in iterator)
        {
            result.Add(item.Box);
        }

        // You can then get the image with this size and process it
        return result;
    }


    [GlobalCleanup]
    public void Cleanup()
    {
        _preloadedImage.Dispose();
    }
}
