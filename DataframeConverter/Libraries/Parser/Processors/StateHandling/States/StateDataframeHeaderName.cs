using IE.Entities.Dataframe;

namespace IE.Parsers.Processors.StateHandling.States;

class StateDataframeHeaderName : IState
{
    public bool Process(StateProcessParams processParams)
    {
        string[] splittedData = processParams.InputData.Trim()
                                                       .Split(" ")
                                                       .Where(s => s.Trim() != string.Empty)
                                                       .ToArray();

        if (processParams.Dataframes.Count < 1 ||
            splittedData.Length < 6)
        {
            return false;
        }

        processParams.Dataframes[^1].Name = splittedData[5];
        return true;
    }
}