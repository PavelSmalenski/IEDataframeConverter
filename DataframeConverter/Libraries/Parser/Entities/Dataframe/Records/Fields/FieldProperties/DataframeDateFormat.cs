namespace IE.Entities.Dataframe;

class DataframeDateFormat
{
    public int FormatId { get; private set; }

    public DataframeDateFormat(int format)
    {
        if (format < 1 || format > 12)
        {
            throw new ArgumentException($"Unknown date format ID - {FormatId}");
        }
        FormatId = format;
    }

    public override string ToString()
    {
        return FormatId switch
        {
            1  => "YYDDD",
            2  => "MMDDYY",
            3  => "DDMMYY",
            4  => "YYMMDD",
            5  => "YYYYDDD",
            6  => "MM/DD/YY",
            7  => "DD/MM/YY",
            8  => "YY/MM/DD",
            9  => "MMDDYYYY",
            10 => "DDMMYYYY",
            11 => "YYYYMMDD",
            12 => "YYYYDDMM",
            _  => throw new ArgumentException($"Unknown date format ID - {FormatId}")
        };
    }
}