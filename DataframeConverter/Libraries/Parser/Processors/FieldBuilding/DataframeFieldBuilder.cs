using System.Text.RegularExpressions;
using IE.Entities.Dataframe;
using IE.Entities.Dataframe.Abstract;

namespace IE.Parsers.Processors.FieldBuilding;

static class DataframeFieldBuilder
{
    const int DefaultNumericValue = -1;

    public static DataframeField Build(List<string> properties)
    {
        // 0 - Name
        // 1 - StartPos
        // 2 - StartPosHex
        // 3 - BytesLength
        // 4 - Type
        // 5 - Digits
        // 6 - DecimalPos
        // 7 - DateFormat
        // 8 - Occurs

        string name = ParseName(properties[0]);
        var type = ParseType(properties[4], name);

        return type switch
        {
            DataframeFieldType.Group => new DataframeFieldGroup(
                name: name,
                depth: ParseDepth(properties[0]),
                startPosition: ParseStartPos(properties[1]),
                bytesLength: ParseByteLength(properties[3]),
                occursCount: ParseOccurs(properties[8])),
            DataframeFieldType.A_Alphanumeric => new DataframeFieldAlphanumeric(
                name: name,
                depth: ParseDepth(properties[0]),
                startPosition: ParseStartPos(properties[1]),
                bytesLength: ParseByteLength(properties[3]),
                occursCount: ParseOccurs(properties[8])),
            DataframeFieldType.N_PrintDecimalSigned => new DataframeFieldNumeric(
                name: name,
                depth: ParseDepth(properties[0]),
                startPosition: ParseStartPos(properties[1]),
                bytesLength: ParseByteLength(properties[3]),
                digits: ParseDigits(properties[5]),
                decimalPosition: ParseDecimalPos(properties[6]),
                isPacked: false,
                isSigned: true,
                dateFormat: ParseDateFormat(properties[7]),
                occursCount: ParseOccurs(properties[8])),
            DataframeFieldType.O_PrintDecimalUnsigned => new DataframeFieldNumeric(
                name: name,
                depth: ParseDepth(properties[0]),
                startPosition: ParseStartPos(properties[1]),
                bytesLength: ParseByteLength(properties[3]),
                digits: ParseDigits(properties[5]),
                decimalPosition: ParseDecimalPos(properties[6]),
                isPacked: false,
                isSigned: false,
                dateFormat: ParseDateFormat(properties[7]),
                occursCount: ParseOccurs(properties[8])),
            DataframeFieldType.P_PackedDecimalSigned => new DataframeFieldNumeric(
                name: name,
                depth: ParseDepth(properties[0]),
                startPosition: ParseStartPos(properties[1]),
                bytesLength: ParseByteLength(properties[3]),
                digits: ParseDigits(properties[5]),
                decimalPosition: ParseDecimalPos(properties[6]),
                isPacked: true,
                isSigned: true,
                dateFormat: ParseDateFormat(properties[7]),
                occursCount: ParseOccurs(properties[8])),
            DataframeFieldType.Q_PackedDecimalUnsigned => new DataframeFieldNumeric(
                name: name,
                depth: ParseDepth(properties[0]),
                startPosition: ParseStartPos(properties[1]),
                bytesLength: ParseByteLength(properties[3]),
                digits: ParseDigits(properties[5]),
                decimalPosition: ParseDecimalPos(properties[6]),
                isPacked: true,
                isSigned: false,
                dateFormat: ParseDateFormat(properties[7]),
                occursCount: ParseOccurs(properties[8])),
            _ => throw new ArgumentException($"Unknown field type: '{type}'")
        };
    }

    static int ParseDepth(string param)
    {
        return param.Count(c => c == '.');
    }

    static string ParseName(string param)
    {
        return param.TrimStart('.').TrimEnd();
    }

    static int ParseStartPos(string param)
    {
        return int.TryParse(param.Trim(), out int parsedInt) ? parsedInt : DefaultNumericValue;
    }

    static int ParseByteLength(string param)
    {
        return int.TryParse(param.Trim(), out int parsedInt) ? parsedInt : DefaultNumericValue;
    }

    static DataframeFieldType ParseType(string param, string name)
    {
        return param.Trim() switch
        {
            "" => name == "FILLER"
                    ? DataframeFieldType.A_Alphanumeric
                    : DataframeFieldType.Group,
            "A" => DataframeFieldType.A_Alphanumeric,
            "N" => DataframeFieldType.N_PrintDecimalSigned,
            "O" => DataframeFieldType.O_PrintDecimalUnsigned,
            "P" => DataframeFieldType.P_PackedDecimalSigned,
            "Q" => DataframeFieldType.Q_PackedDecimalUnsigned,
            _ => throw new ArgumentException($"Unknown type of variable: \'{param.Trim()}\'")
        };
    }

    static int ParseDigits(string param)
    {
        return int.TryParse(param.Trim(), out int parsedInt) ? parsedInt : DefaultNumericValue;
    }

    static int ParseDecimalPos(string param)
    {
        return int.TryParse(param.Trim(), out int parsedInt) ? parsedInt : DefaultNumericValue;
    }

    static DataframeDateFormat? ParseDateFormat(string param)
    {
        return int.TryParse(param.Trim(), out int parsedInt) ? new DataframeDateFormat(parsedInt) : null;
    }

    static int ParseOccurs(string param)
    {
        return int.TryParse(param.Trim(), out int parsedInt) ? parsedInt : DefaultNumericValue;
    }

    enum DataframeFieldType {
        Group,
        A_Alphanumeric,
        N_PrintDecimalSigned,
        O_PrintDecimalUnsigned,
        P_PackedDecimalSigned,
        Q_PackedDecimalUnsigned
    }
}