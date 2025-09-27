using Cobol.Converter.Processors;
using IE.Entities.Dataframe;
using IE.Entities.Dataframe.Abstract;

namespace Cobol.Converter;

static class CobolConverter
{
    const string DefaultRootFieldName = "ROOT";

    public static Dictionary<string, List<string>>? Convert(List<Dataframe> dataframes)
    {

        if (dataframes.Count > 0)
        {
            Dictionary<string, List<string>> cobolStructures = new Dictionary<string, List<string>>();

            foreach (var dataframe in dataframes)
            {
                int startLevel = 1;

                List<string> dataframePrintData = new List<string>();
                if (dataframe.Records is null)
                {
                    continue;
                }

                FillStructureHeaderComments(dataframe, dataframePrintData);

                if (dataframe.Records.Count > 1)
                {
                    dataframePrintData.Add(CobolCodeFormatter.GetRulerCode());
                    dataframePrintData.AddRange(CobolCodeFormatter.GetVariablesCode(startLevel, DefaultRootFieldName, CobolTypeFormatter.GetType(dataframe.RecordLength)));
                    startLevel++;
                }

                foreach (var record in dataframe.Records)
                {
                    FillRecordHeaderComments(record, dataframePrintData);

                    if (record.RootGroup is null)
                    {
                        continue;
                    }

                    FillVariables(record.RootGroup, startLevel, dataframePrintData);
                }

                cobolStructures.Add(dataframe.Name, dataframePrintData);
            }

            return cobolStructures;
        }
        else
        {
            return null;
        }
    }

    static void FillStructureHeaderComments(Dataframe dataframe, List<string> printData)
    {
        printData.Add(CobolCodeFormatter.GetRulerCode());
        printData.Add(CobolCodeFormatter.GetCommentCode($"DATAFRAME = {dataframe.Name}"));

        printData.Add(CobolCodeFormatter.GetRulerCode());
        printData.Add(CobolCodeFormatter.GetCommentCode($"DDNAME = {dataframe.DatasetName}"));
        printData.Add(CobolCodeFormatter.GetCommentCode($"FECFM  = {dataframe.RecordFormat}"));
        printData.Add(CobolCodeFormatter.GetCommentCode($"LRECL  = {dataframe.RecordLength}"));

        printData.Add(CobolCodeFormatter.GetRulerCode());
        printData.Add(CobolCodeFormatter.GetCommentCode($"KEYLEN = {dataframe.KeyLength}"));
        printData.Add(CobolCodeFormatter.GetCommentCode($"KEYLOC = {dataframe.KeyLocation}"));

        if (dataframe.OrganizedBy is not null)
        {
            printData.Add(CobolCodeFormatter.GetRulerCode());
            printData.Add(CobolCodeFormatter.GetCommentCode("ORGANIZED BY:"));
            foreach (var organizedField in dataframe.OrganizedBy)
            {
                printData.Add(CobolCodeFormatter.GetCommentCode($"-->{organizedField}"));
            }
        }
    }

    static void FillRecordHeaderComments(DataframeRecord record, List<string> printData)
    {
        printData.Add(CobolCodeFormatter.GetRulerCode());
        printData.Add(CobolCodeFormatter.GetCommentCode($"RECORD = {record.Name}"));
        printData.Add(CobolCodeFormatter.GetRulerCode());

        if (record.Level != "")
        {
            printData.Add(CobolCodeFormatter.GetCommentCode($"LEVEL = {record.Level}"));
        }
        if (record.Key != "")
        {
            printData.Add(CobolCodeFormatter.GetCommentCode("KEY ="));
            printData.AddRange(CobolCodeFormatter.GetCommentsCode(record.Key));
        }
        if (record.IdentifiedBy != "")
        {
            printData.Add(CobolCodeFormatter.GetCommentCode("IDENTIFIED ="));
            printData.AddRange(CobolCodeFormatter.GetCommentsCode(record.IdentifiedBy));
        }

        printData.Add(CobolCodeFormatter.GetRulerCode());
    }

    static void FillVariables(DataframeField field, int level, List<string> printData)
    {
        printData.AddRange(CobolCodeFormatter.GetVariablesCode(level, field.Name, CobolTypeFormatter.GetType(field)));

        if (field is DataframeFieldGroup group)
        {
            foreach (var groupField in group.ChildFields)
            {
                FillVariables(groupField, level + 1, printData);
            }
        }
    }
}