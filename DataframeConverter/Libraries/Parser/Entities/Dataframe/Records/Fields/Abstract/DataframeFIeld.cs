namespace IE.Entities.Dataframe.Abstract;

abstract class DataframeField
{
    public int Depth { get; set; }
    public string Name { get; set; }
    public int StartPosition { get; set; } = 0;
    public int BytesLength { get; set; } = 0;
    public int OccursCount { get; set; } = 0;
    public DataframeFieldGroup? ParentGroup = null;

    public DataframeField(string name, int depth, int startPosition, int bytesLength, int occursCount)
    {
        Name = name;
        Depth = depth;
        StartPosition = startPosition;
        BytesLength = bytesLength;
        OccursCount = occursCount;
    }
}