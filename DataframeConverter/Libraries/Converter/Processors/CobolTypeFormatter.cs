using IE.Entities.Dataframe;
using IE.Entities.Dataframe.Abstract;

namespace Cobol.Converter.Processors;

static class CobolTypeFormatter
{
    public static string GetType(int length)
    {
        return $"PIC X({length})";
    }

    public static string GetType(DataframeField dataframeField)
    {
        string occurs = dataframeField.OccursCount > 0 ? $" OCCURS {dataframeField.OccursCount} TIMES" : "";

        if (dataframeField is DataframeFieldGroup group)
        {
            return occurs.TrimStart();
        }
        else if (dataframeField is DataframeFieldAlphanumeric alphanumeric)
        {
            return $" PIC X({alphanumeric.BytesLength}){occurs}";
        }
        else if (dataframeField is DataframeFieldNumeric numeric)
        {
            string sign = numeric.IsSigned ? "S" : "";
            string num = numeric.DecimalPosition > 0
                            ? $"9({numeric.Digits - numeric.DecimalPosition})V9({numeric.DecimalPosition})"
                            : $"9({numeric.Digits})";
            string comp = numeric.IsPacked ? " COMP-3" : "";
            return $"PIC {sign}{num}{comp}{occurs}";
        }
        else
        {
            throw new ArgumentException($"Unknown row type for field {dataframeField.Name}");
        }
    }
}