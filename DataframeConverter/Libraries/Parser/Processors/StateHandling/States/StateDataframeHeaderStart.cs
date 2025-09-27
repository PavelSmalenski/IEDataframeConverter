using IE.Entities.Dataframe;

namespace IE.Parsers.Processors.StateHandling.States;

class StateDataframeHeaderStart : IState
{
    public bool Process(StateProcessParams processParams)
    {
        processParams.Dataframes.Add(new Dataframe());
        return true;
    }
}