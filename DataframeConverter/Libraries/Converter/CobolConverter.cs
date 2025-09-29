using Cobol.Converter.Processors;
using IE.Entities.Dataframe;
using IE.Entities.Dataframe.Abstract;

namespace Cobol.Converter;

static class CobolConverter
{
    const string DefaultRootFieldName = "RECORD-ROOT";

    public static IEnumerable<KeyValuePair<string, List<string>>> GenerateConvertedData(List<Dataframe> dataframes)
    {
        foreach (var dataframe in dataframes)
        {
            int startLevel = 1;
            string redefinesVarName = "";

            List<string> dataframePrintData = new List<string>();
            if (dataframe.Records is null)
            {
                continue;
            }

            FillStructureHeaderComments(dataframe, dataframePrintData);

            if (dataframe.Records.Count > 1)
            {
                dataframePrintData.Add(CobolCodeFormatter.GenerateRulerCode());
                dataframePrintData.AddRange(CobolCodeFormatter.GenerateVariablesCode(startLevel, DefaultRootFieldName, CobolTypeFormatter.GenerateType(dataframe.RecordLength)));
                // startLevel++;
                redefinesVarName = DefaultRootFieldName;
            }

            foreach (var record in dataframe.Records)
            {
                FillRecordHeaderComments(record, dataframePrintData);

                if (record.RootGroup is null)
                {
                    continue;
                }

                FillVariables(record.RootGroup, startLevel, dataframePrintData, redefinesVarName);
            }

            yield return new KeyValuePair<string, List<string>>(dataframe.Name, dataframePrintData);
        }
    }

    static void FillStructureHeaderComments(Dataframe dataframe, List<string> printData)
    {
        printData.Add(CobolCodeFormatter.GenerateRulerCode());
        printData.Add(CobolCodeFormatter.GenerateCommentCode($"DATAFRAME = {dataframe.Name}"));

        printData.Add(CobolCodeFormatter.GenerateRulerCode());
        printData.Add(CobolCodeFormatter.GenerateCommentCode($"DDNAME = {dataframe.DatasetName}"));
        printData.Add(CobolCodeFormatter.GenerateCommentCode($"FECFM  = {dataframe.RecordFormat}"));
        printData.Add(CobolCodeFormatter.GenerateCommentCode($"LRECL  = {dataframe.RecordLength}"));

        printData.Add(CobolCodeFormatter.GenerateRulerCode());
        printData.Add(CobolCodeFormatter.GenerateCommentCode($"KEYLEN = {dataframe.KeyLength}"));
        printData.Add(CobolCodeFormatter.GenerateCommentCode($"KEYLOC = {dataframe.KeyLocation}"));

        if (dataframe.OrganizedBy is not null)
        {
            printData.Add(CobolCodeFormatter.GenerateRulerCode());
            printData.Add(CobolCodeFormatter.GenerateCommentCode("ORGANIZED BY:"));
            foreach (var organizedField in dataframe.OrganizedBy)
            {
                printData.Add(CobolCodeFormatter.GenerateCommentCode($"-->{organizedField}"));
            }
        }
    }

    static void FillRecordHeaderComments(DataframeRecord record, List<string> printData)
    {
        printData.Add(CobolCodeFormatter.GenerateRulerCode());
        printData.Add(CobolCodeFormatter.GenerateCommentCode($"RECORD = {record.Name}"));
        printData.Add(CobolCodeFormatter.GenerateRulerCode());

        if (record.Level != "")
        {
            printData.Add(CobolCodeFormatter.GenerateCommentCode($"LEVEL = {record.Level}"));
        }
        if (record.Key != "")
        {
            printData.Add(CobolCodeFormatter.GenerateCommentCode("KEY ="));
            printData.AddRange(CobolCodeFormatter.GenerateCommentsCode(record.Key));
        }
        if (record.IdentifiedBy != "")
        {
            printData.Add(CobolCodeFormatter.GenerateCommentCode("IDENTIFIED ="));
            printData.AddRange(CobolCodeFormatter.GenerateCommentsCode(record.IdentifiedBy));
        }

        printData.Add(CobolCodeFormatter.GenerateRulerCode());
    }

    static void FillVariables(DataframeField field, int level, List<string> printData, string redefinesVarName = "")
    {
        printData.AddRange(CobolCodeFormatter.GenerateVariablesCode(level, field.Name, CobolTypeFormatter.GenerateType(field), redefinesVarName));

        if (field is DataframeFieldGroup group)
        {
            foreach (var groupField in group.ChildFields)
            {
                FillVariables(groupField, level + 1, printData, "");
            }
        }
    }
}