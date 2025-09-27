namespace IE.Parsers.Processors.StateHandling.States;

interface IState
{
    bool Process(StateProcessParams processParams);
}