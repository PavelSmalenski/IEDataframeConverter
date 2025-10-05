namespace IE.Entities.Dataframe;

class Dataframe
{
    public string Name { get; set; } = "";

    public string RecordFormat { get; set; } = "";

    public int RecordLength { get; set; } = 0;

    public string DatasetName { get; set; } = "";

    public int KeyLength { get; set; } = 0;

    public int KeyLocation { get; set; } = 0;

    public List<string>? OrganizedBy { get; private set; }

    public List<DataframeRecord>? Records { get; private set; }

    public void AddOrganizedBy(string organizedByString)
    {
        if (OrganizedBy is null)
        {
            OrganizedBy = new List<string>();
        }

        OrganizedBy.Add(organizedByString);
    }

    public void AddRecord(DataframeRecord record)
    {
        if (Records is null)
        {
            Records = new List<DataframeRecord>();
        }

        Records.Add(record);
    }
}