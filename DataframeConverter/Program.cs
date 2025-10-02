using Cobol.Converter;
using FluentFTP.Exceptions;
using IE.Entities.Dataframe;
using IE.Parsers;
using IE.Printers;
using IE.Printers.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DataframeConverter;

class Program
{
    const string MvsFtpSettingsPath = "Configs/mvsFtpSettings.json";

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
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(MvsFtpSettingsPath, optional: false, reloadOnChange: true);
            var configuration = builder.Build();

            var ftpSettings = configuration.GetSection("FtpSettings").Get<MvsFtpSettings>();

            if (ftpSettings == null)
            {
                throw new FtpException("Failed to load FTP settings from configuration.");
            }
            else if (string.IsNullOrWhiteSpace(ftpSettings.Host) ||
                     string.IsNullOrWhiteSpace(ftpSettings.Username) ||
                     string.IsNullOrWhiteSpace(ftpSettings.Password) ||
                     string.IsNullOrWhiteSpace(ftpSettings.TargetPdsName))
            {
                throw new FtpException("One or more MVS FTP settings are missing or empty in configuration.");
            }

            using (ICodePrinter ftpPrinter = new CobolMvsPrinter(ftpSettings.Host, ftpSettings.Username, ftpSettings.Password, ftpSettings.TargetPdsName))
            {
                foreach (var dataframe in CobolConverter.GenerateConvertedData(dataframes))
                {
                    System.Console.Write($"Printing dataframe ({printedCount + 1:D3}/{dataframes.Count:D3}): {dataframe.Key,8} ... ");
                    ftpPrinter.Print(dataframe.Key, dataframe.Value);
                    printedCount++;
                    System.Console.WriteLine($"Done");
                }
                System.Console.WriteLine($"Dataframes printed to mainframe: {printedCount}");
            }
        }
        catch (FtpCommandException e)
        {
            System.Console.WriteLine();
            System.Console.WriteLine($"FTP command error: {e.Message}");
        }
        catch (FtpException e)
        {
            System.Console.WriteLine();
            // In some cases, common FtpException may be caused by PDS being out of space
            System.Console.WriteLine($"FTP error: {e.Message}");
            if (e.InnerException != null)
            {
                System.Console.WriteLine($"Inner exception: {e.InnerException.Message}");
            }
        }

        System.Console.ReadLine();
    }

    class MvsFtpSettings
    {
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string TargetPdsName { get; set; }
    }
}
