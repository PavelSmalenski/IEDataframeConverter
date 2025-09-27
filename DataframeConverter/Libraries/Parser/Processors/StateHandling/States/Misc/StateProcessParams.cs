using IE.Entities.Dataframe;

namespace IE.Parsers.Processors.StateHandling;

class StateProcessParams
{
    public List<Dataframe> Dataframes { get; set; } = null!;
    public string InputData { get; set; } = null!;
}