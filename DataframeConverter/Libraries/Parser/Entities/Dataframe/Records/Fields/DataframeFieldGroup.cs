using IE.Entities.Dataframe.Abstract;

namespace IE.Entities.Dataframe;

class DataframeFieldGroup : DataframeField
{
    public List<DataframeField> ChildFields { get; private set; } = null!;

    public DataframeFieldGroup(string name, int depth, int startPosition, int bytesLength, int occursCount = 0)
    : base(name, depth, startPosition, bytesLength, occursCount)
    {
        ChildFields = new List<DataframeField>();
    }
}