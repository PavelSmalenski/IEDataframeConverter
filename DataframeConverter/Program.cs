using Cobol.Converter;
using FluentFTP.Exceptions;
using IE.Entities.Dataframe;
using IE.Parsers;
using IE.Printers;
using IE.Printers.Interfaces;
using DataframeConverter.Libraries.MvsFtp;
using Microsoft.Extensions.Configuration;
using DataframeConverter.Libraries.Menu;
using DataframeConverter.Libraries.Menu.Entities;

namespace DataframeConverter;

class Program
{
    const string DataframesSysptintPath = "dataframesSysprint.txt";
    const string MvsFtpSettingsPath = "Configs/mvsFtpSettings.json";


    static void Main(string[] args)
    {
        MenuController menuController = new MenuController();
        RegisterMenuActions(menuController);

        menuController.HandleUserInput();

        Console.WriteLine("Exiting program.");
    }

    public static void RegisterMenuActions(MenuController menuController)
    {
        // Parse to files:
        menuController.AddMenuOption(
            new MenuOption("Parse dataframes to files", () =>
            {
                List<Dataframe> dataframes;
                using (FileStream inputFile = new FileStream(DataframesSysptintPath, FileMode.Open))
                {
                    dataframes = new DataframeParser().Parse(inputFile);
                }

                int printedCount = 0;
                ICodePrinter printer = new CobolToFilePrinter();
                foreach (var dataframe in CobolConverter.GenerateConvertedData(dataframes))
                {
                    System.Console.Write($"Printing dataframe ({printedCount + 1:D3}/{dataframes.Count:D3}): {dataframe.Key,8} ... ");
                    printer.Print(dataframe.Key, dataframe.Value);
                    printedCount++;
                    System.Console.WriteLine($"Done");
                }
            }));

        // Parse to MVS:
        menuController.AddMenuOption(
            new MenuOption("Parse dataframes to MVS (FTP)", () =>
            {
                int printedCount = 0;
                try
                {
                    List<Dataframe> dataframes;
                    using (FileStream inputFile = new FileStream(DataframesSysptintPath, FileMode.Open))
                    {
                        dataframes = new DataframeParser().Parse(inputFile);
                    }

                    var mvsFtpSettingsBuilder = new MvsFtpSettingsBuilder();
                    var ftpSettings = mvsFtpSettingsBuilder.GetMvsFtpSettings(MvsFtpSettingsPath);

                    using (ICodePrinter ftpPrinter = new CobolMvsPrinter(ftpSettings.Host!, ftpSettings.Username!, ftpSettings.Password!, ftpSettings.TargetPdsName!))
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
            }));
    }
}
