using System.Text.RegularExpressions;
using IE.Entities.Dataframe;

namespace IE.Parsers.Processors.StateHandling.States;

class StateRecordHeaderKey : IState
{
    public bool Process(StateProcessParams processParams)
    {
        if (processParams.Dataframes.Count < 1
            || processParams.Dataframes[^1].Records is null)
        {
            return false;
        }

        var regex = new Regex(@"^(?:\s*KEY IS)?\s+(.*)?$");
        var match = regex.Match(processParams.InputData);

        var record = processParams.Dataframes[^1].Records![^1];

        record.Key = record.Key == string.Empty
                            ? match.Groups[1].Value
                            : string.Join(' ', record.Key, match.Groups[1].Value);

        return true;
    }
}