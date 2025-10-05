using System.Text.RegularExpressions;
using IE.Entities.Dataframe;

namespace IE.Parsers.Processors.StateHandling.States;

class StateRecordHeaderName : IState
{
    public bool Process(StateProcessParams processParams)
    {
        if (processParams.Dataframes.Count < 1)
        {
            return false;
        }

        var regex = new Regex(@"^RECORD NAME - (\S+)(?:\s+\( LEVEL = (\S+) \))?");
        var match = regex.Match(processParams.InputData);

        processParams.Dataframes[^1].AddRecord(
            new DataframeRecord()
            {
                Name = match.Groups[1].Value,
                Level = match.Groups.Count > 2 ? match.Groups[2].Value : ""
            });

        return true;
    }
}