using System.Text.RegularExpressions;
using IE.Entities.Dataframe;
using IE.Parsers.Processors.FieldBuilding;

namespace IE.Parsers.Processors.StateHandling.States;

class StateRecord : IState
{
    int[] _parameterLengths = { 39, 6, 6, 8, 4, 8, 6, 6, 7 };

    public bool Process(StateProcessParams processParams)
    {
        if (processParams.Dataframes.Count < 1
            || processParams.Dataframes[^1].Records is null)
        {
            return false;
        }

        List<string> parameters = new List<string>();
        int startPos = 0;

        foreach (var len in _parameterLengths)
        {
            string item = "";
            if (startPos < processParams.InputData.Length)
            {
                item = processParams.InputData.Substring(startPos, len);
                startPos += len;
            }
            parameters.Add(item.Trim());
        }

        processParams.Dataframes[^1].Records![^1].QueueField(DataframeFieldBuilder.Build(parameters));

        return true;
    }
}