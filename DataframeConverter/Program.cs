using Cobol.Converter;
using FluentFTP.Exceptions;
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

        printedCount = 0;
        try
        {
            using (ICodePrinter ftpPrinter = new CobolMvsPrinter("<host>", "<user>", "<pwd>", "<TARGET.PDS.NAME>"))
            {
                foreach (var dataframe in CobolConverter.GenerateConvertedData(dataframes))
                {
                    ftpPrinter.Print(dataframe.Key, dataframe.Value);
                    printedCount++;
                    System.Console.WriteLine($"Printed dataframe #{printedCount:D3}: {dataframe.Key}");
                }
                System.Console.WriteLine($"Dataframes printed to mainframe: {printedCount}");
            }
        }
        catch (FtpCommandException e)
        {
            System.Console.WriteLine($"FTP command error: {e.Message}");
        }
        catch (FtpException e)
        {
            // In some cases, common FtpException may be caused by PDS being out of space
            System.Console.WriteLine($"FTP error: {e.Message}");
            if(e.InnerException != null)
            {
                System.Console.WriteLine($"Inner exception: {e.InnerException.Message}");
            }
        }

        System.Console.ReadLine();
    }
}
