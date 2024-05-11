using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TesseractOcrMaui.Results;
using TesseractOcrMaui.Tessdata;

namespace Benchmarkoutput;
internal class DummyProvider(string path, params string[] files) : ITessDataProvider
{
    public string[] AvailableLanguages { get; } = files.Select(x => Path.Combine(path, x)).ToArray();

    public bool IsAllDataLoaded => true;

    public string TessDataFolder { get; } = path;


    readonly string _languages = string.Join('+', files.Select(Path.GetFileNameWithoutExtension));

    public string[] GetAllFileNames() => files;

    public string GetLanguagesString() => _languages;

    public Task<DataLoadResult> LoadFromPackagesAsync() => Task.FromResult(new DataLoadResult { State = TessDataState.AllValid });
}
