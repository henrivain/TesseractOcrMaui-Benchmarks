# TesseractOcrMaui-Benchmarks
This repository contains some quickly built benchmarks to test nuget package TesseractOcrMaui speed.

Note that these benchmarks are using experimental version of the nuget package that is able to be run in non Maui projects on Windows. The same Tesseract libraries are used as in Maui Windows version. Use the package version in `UsedWindowsCompatibleNugetVersion/TesseractOcrMaui.1.2.0.1.nupkg` path.

## Different ways and configurations to recognize with
![Benchmarks_Results_2024-05-12](https://github.com/henrivain/TesseractOcrMaui-Benchmarks/assets/89461562/6f4dc234-9b3a-4a1f-b224-ae58622fe5aa)

See how recognize is called [here in Benchmarks](https://github.com/henrivain/TesseractOcrMaui-Benchmarks/blob/155906b42ded28a2ae202dbe1de5e948a5a04647/Benchmarks.cs)

## Testing different scaled images

![ScaleBenchmarks_Results_2024-05-11](https://github.com/henrivain/TesseractOcrMaui-Benchmarks/assets/89461562/68300a31-9180-426f-b5ad-bf11736edf0f)

To see recognized text from scaled images [look here](https://github.com/henrivain/TesseractOcrMaui-Benchmarks/blob/155906b42ded28a2ae202dbe1de5e948a5a04647/Comparisons/ScaledImageTextOutput.txt)

See how image is scaled [here in Benchmarks_RecognizeScaled](https://github.com/henrivain/TesseractOcrMaui-Benchmarks/blob/155906b42ded28a2ae202dbe1de5e948a5a04647/Benchmarks_RecognizeScaled.cs#L9)


## Input image below

![loremIpsum_Original](https://github.com/henrivain/TesseractOcrMaui-Benchmarks/assets/89461562/0f6acfe3-cf9b-4732-aa42-d5fd0e60d1fc)

## What to test next

- How long different native Tesseract calls take
- Different PageSegmentationModes
- Different device platforms

## Quick conclusion

These benchmarks are not the full truth, because of small data set and only one platform tested outside Maui, so take with a grain of salt.
Most of the time is used inside tesseract's text/layout analyzation. 
### What sped up?
- scaling down image
- using tessdata_fast
