using IE.Entities.Dataframe.Abstract;

namespace IE.Entities.Dataframe;

class DataframeFieldAlphanumeric : DataframeField
{
    public DataframeFieldAlphanumeric(string name, int depth, int startPosition, int bytesLength, int occursCount = 0)
    : base(name, depth, startPosition, bytesLength, occursCount)
    { }
}