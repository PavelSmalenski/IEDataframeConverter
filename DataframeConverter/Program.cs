using Cobol.Converter;
using IE.Entities.Dataframe;
using IE.Parsers;

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

        var data = CobolConverter.Convert(dataframes);

        System.Console.WriteLine(dataframes.Count);
        System.Console.ReadLine();
    }
}
