using Cobol.Converter;
using IE.Entities.Dataframe;
using IE.Parsers;
using IE.Printers;
using IE.Printers.Interfaces;

namespace DataframeConverter;

class Program
{
    static void Main(string[] args)
    {
        List<Dataframe> dataframes;

        using (FileStream inputFile = new FileStream("dataframesSysprint.txt", FileMode.Open))
        {
            var parser = new DataframeParser();

            dataframes = parser.Parse(inputFile);
        }

        int printedCount = 0;
        ICodePrinter printer = new CobolToFilePrinter();
        foreach (var dataframe in CobolConverter.GenerateConvertedData(dataframes))
        {
            printer.Print(dataframe.Key, dataframe.Value);
            printedCount++;
        }
        System.Console.WriteLine($"Dataframes printed to files: {printedCount}");

        System.Console.ReadLine();
    }
}
