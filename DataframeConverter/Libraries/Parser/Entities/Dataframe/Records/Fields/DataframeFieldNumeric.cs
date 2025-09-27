using IE.Entities.Dataframe.Abstract;

namespace IE.Entities.Dataframe;

class DataframeFieldNumeric : DataframeField
{
    public int Digits { get; set; } = 0;
    public int DecimalPosition { get; set; } = 0;
    public DataframeDateFormat? DateFormat { get; set; }
    public bool IsDate { get { return DateFormat == null; } }
    public bool IsPacked { get; private set; }
    public bool IsSigned { get; private set; }

    public DataframeFieldNumeric(string name, int depth, int startPosition, int bytesLength, int digits, int decimalPosition = 0, bool isPacked = false, bool isSigned = false, DataframeDateFormat? dateFormat = null, int occursCount = 0)
    : base(name, depth, startPosition, bytesLength, occursCount)
    {
        Digits = digits;
        DecimalPosition = decimalPosition;
        if (dateFormat is not null)
        {
            DateFormat = dateFormat;
        }
        IsPacked = isPacked;
        IsSigned = isSigned;
    }
}