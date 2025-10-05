using IE.Printers.Interfaces;

namespace IE.Printers;

class CobolToFilePrinter : ICodePrinter
{
    string _defaultDirectory;
    bool _isForceCreate;

    public CobolToFilePrinter(string defaultDirectory = "Dataframes", bool isForceCreate = true)
    {
        _defaultDirectory = defaultDirectory;
        _isForceCreate = isForceCreate;
    }

    public void Print(string dataframeName, List<string> dataframeCode)
    {
        if (_defaultDirectory != "" && !Directory.Exists(_defaultDirectory))
        {
            Directory.CreateDirectory(_defaultDirectory);
        }

        string fileName = Path.Combine(_defaultDirectory, $"{dataframeName}.cbl.copy");

        if (File.Exists(fileName))
        {
            if (_isForceCreate)
            {
                File.Delete(fileName);
            }
            else
            {
                throw new ArgumentException($"File already exists, use force-create to overwrite: {fileName}");
            }
        }

        using (var writer = new StreamWriter(fileName))
        {
            foreach (var codeLine in dataframeCode)
            {
                writer.WriteLine(codeLine);
            }
        }
    }

    public void Dispose()
    {
    }
}