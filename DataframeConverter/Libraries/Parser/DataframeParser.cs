using System.Text.RegularExpressions;
using IE.Entities.Dataframe;
using IE.Entities.Dataframe.Abstract;
using IE.Parsers.Processors.StateHandling;

namespace IE.Parsers;


class DataframeParser
{
    public List<Dataframe> Parse(Stream inputStream)
    {
        int rowsReadCount = 0;
        var dataframes = new List<Dataframe>();
        var stateProcessParams = new StateProcessParams() { Dataframes = dataframes };
        var stateHandler = new StateHandler();

        using (StreamReader reader = new StreamReader(inputStream))
        {
            string? inputLine = reader.ReadLine();
            while (inputLine != null)
            {
                rowsReadCount++;

                char controlCharacter = inputLine[0];
                string inputData = inputLine.Substring(1);
                

                var command = GenerateCommand(controlCharacter, inputData);
                stateProcessParams.InputData = inputData;

                if (!stateHandler.Transit(command) ||
                    !stateHandler.CurrentState.Process(stateProcessParams))
                {
                    throw new Exception($"ERROR-ROW: {rowsReadCount} '{inputLine}'");
                }

                inputLine = reader.ReadLine();
            }

            stateHandler.Transit(Command.Exit);
        }

        return dataframes;
    }

    Command GenerateCommand(char controlCharacter, string inputData)
    {
        string[] splittedData = inputData.Trim()
                                         .Split(" ")
                                         .Where(s => s.Trim() != string.Empty)
                                         .ToArray();

        if (splittedData.Count() > 0)
        {
            if (splittedData[0] == "ORGANIZED" && splittedData[1] == "BY")
            {
                return Command.DataframeOrganized;
            }
            if (splittedData[0] == "IDENTIFIED" && splittedData[1] == "BY")
            {
                return Command.RecordIdentified;
            }
            if (splittedData[0] == "KEY" && splittedData[1] == "IS")
            {
                return Command.RecordKey;
            }
            if (controlCharacter == '1' && splittedData[0].Substring(0, 5) == "-----")
            {
                return Command.RecordHeading;
            }
        }

        return controlCharacter switch
        {
            '1' => Command.NewPage,
            ' ' => Command.Advance1,
            '0' => Command.Advance2,
            '-' => Command.Advance3,
            _ => throw new ArgumentException($"Invalid control character: '{controlCharacter}'")
        };
    }
}