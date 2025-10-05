using System.Text.RegularExpressions;
using IE.Entities.Dataframe;

namespace IE.Parsers.Processors.StateHandling.States;

class StateDataframeHeaderParams : IState
{
    public bool Process(StateProcessParams processParams)
    {
        if (processParams.Dataframes.Count < 1)
        {
            return false;
        }

        var dfHeaderParams = Regex.Split(processParams.InputData.Trim(), @"\s{3}");
        Dictionary<string, string> keyValDict = dfHeaderParams.Select(val => Regex.Split(val, @": "))
                                                              .ToDictionary((keyVal) => keyVal[0], (keyVal) => keyVal[1]);
        string val;

        if (keyValDict.TryGetValue("RECFM", out val!))
        {
            processParams.Dataframes[^1].RecordFormat = val;
        }
        if (keyValDict.TryGetValue("LRECL", out val!))
        {
            processParams.Dataframes[^1].RecordLength = int.Parse(val);
        }
        if (keyValDict.TryGetValue("DDNAME", out val!))
        {
            processParams.Dataframes[^1].DatasetName = val;
        }
        if (keyValDict.TryGetValue("KEY LENGTH", out val!))
        {
            processParams.Dataframes[^1].KeyLength = int.Parse(val);
        }
        if (keyValDict.TryGetValue("KEY LOCATION", out val!))
        {
            processParams.Dataframes[^1].KeyLocation = int.Parse(val);
        }

        return true;
    }
}