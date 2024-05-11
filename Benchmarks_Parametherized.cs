using BenchmarkDotNet.Attributes;
using SkiaSharp;
using System.IO;
using TesseractOcrMaui;
using TesseractOcrMaui.Tessdata;

namespace Benchmarkoutput;
[MemoryDiagnoser]
public class Benchmarks_Parametherized
{
    ITessDataProvider _provider;
    const string _imagePath = @"C:\Users\henri\Downloads\loremIpsum.png";

    Pix _preloadedImage;

    [Params(1.0, 0.9, 0.75, 0.65, 0.5)]
    public double Scales { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _provider = new DummyProvider(Directory.GetCurrentDirectory(), "eng.traineddata");
        _preloadedImage = Pix.LoadFromFile(_imagePath);
    }


    [GlobalCleanup]
    public void Cleanup()
    {
        _preloadedImage.Dispose();
    }

    [Benchmark]
    public string Recognize_Scaled(double? scale = null)
    {
        SKData outputData = ScaleDown(_imagePath, scale ?? Scales);
        byte[] outputImage = outputData.AsSpan().ToArray();

        //SaveImageBytes(outputData, $"{_imagePath}_out_{scale ?? Scales}.png");

        using var engine = new TessEngine(_provider.GetLanguagesString(), _provider.TessDataFolder);
        using Pix pix = Pix.LoadFromMemory(outputImage);
        using TessPage page = engine.ProcessImage(pix);
        return page.GetText();
    }

    public static SKData ScaleDown(string path, double multiplier)
    {
        byte[] bytes = File.ReadAllBytes(path);
        using MemoryStream memory = new(bytes);

        using SKImage image = SKImage.FromEncodedData(memory);
        using SKBitmap bitmap = SKBitmap.FromImage(image);

        int targetHeight = (int)(bitmap.Height * multiplier);
        int targetWidth = (int)(bitmap.Width * multiplier);

        SKImageInfo scaleInfo = new(targetWidth, targetHeight);
        using SKBitmap output = bitmap.Resize(scaleInfo, SKFilterQuality.Medium);


        return output.Encode(SKEncodedImageFormat.Png, 70);

    }

    public static void SaveImageBytes(SKData data, string path)
    {
        using FileStream stream = File.Create(path);
        data.AsStream().CopyTo(stream);
    }
}
