using System.Text.RegularExpressions;
using IE.Entities.Dataframe;

namespace IE.Parsers.Processors.StateHandling.States;

class StateDataframeHeaderOrganized : IState
{
    public bool Process(StateProcessParams processParams)
    {
        if (processParams.Dataframes.Count < 1)
        {
            return false;
        }

        var regex = new Regex(@"^\s*(?:ORGANIZED BY)?\s*([\S]+)\s*$");
        var match = regex.Match(processParams.InputData);
        processParams.Dataframes[^1].AddOrganizedBy(match.Groups[1].Value);

        return true;
    }
}