using System.Runtime.CompilerServices;
using System.Text;
using Cobol.Converter.Extensions;

namespace Cobol.Converter.Processors;

static class CobolCodeFormatter
{
    const char CommentControl = '*';
    const char GenericControl = ' ';
    const int IndentAreaA = 6;
    const int IndentAreaB = 4;
    const int IndentStepCount = 3;
    const int MaxCommentLength = 64;
    const int MaxCodeLength = 64;
    const int CobolLevelStart = 1;
    const int CobolLevelStep = 5;

    public static IEnumerable<string> GenerateCommentsCode(string comment)
    {
        List<string> strings = new List<string>();

        if (comment.Length > MaxCommentLength)
        {
            IEnumerable<string> commentSlices = comment.SplitOnLength(MaxCommentLength);
            foreach (var commentSlice in commentSlices)
            {
                strings.Add(GenerateCommentCode(commentSlice));
            }
        }
        else
        {
            strings.Add(GenerateCommentCode(comment));
        }

        return strings;
    }

    public static string GenerateCommentCode(string comment)
    {
        return $"{new string(' ', IndentAreaA)}{CommentControl} {comment}";
    }

    public static string GenerateRulerCode(char rulerChar = '-')
    {
        return $"{new string(' ', IndentAreaA)}{CommentControl} {new string(rulerChar, MaxCommentLength)}";
    }

    public static IEnumerable<string> GenerateVariablesCode(int level, string name, string type = "", string redefinesVarName = "")
    {
        List<string> resultLines = new List<string>();

        int baseIndentCount = IndentAreaA + 1;
        int relativeIndentCount = level > 1 ? IndentAreaB + (IndentStepCount * (level - 2)) : 0;

        int cobolLevel = level > 1 ? CobolLevelStep * (level - 1) : CobolLevelStart;
        string nameSection = $"{cobolLevel:D2} {name}";

        redefinesVarName = redefinesVarName.Trim();
        string redefinesSection = redefinesVarName.Length > 0 ? $"REDEFINES {redefinesVarName}" : "";

        type = type.Trim();

        //                                             Spacing                       Spacing           Dot
        if (relativeIndentCount + nameSection.Length + 1 + redefinesSection.Length + 1 + type.Length + 1 > MaxCodeLength)
        {
            string fullIndent = new string(' ', baseIndentCount + relativeIndentCount);
            string fullIndentNewLine = new string(' ', baseIndentCount + relativeIndentCount + IndentStepCount);

            resultLines.Add($"{fullIndent}{nameSection}");

            if (redefinesSection.Length > 0)
            {
                resultLines.Add($"{fullIndentNewLine}{redefinesSection}");
            }

            if (type.Length > 0)
            {
                resultLines.Add($"{fullIndentNewLine}{type}");
            }
        }
        else
        {
            if (redefinesSection.Length > 0)
            {
                redefinesSection = $" {redefinesSection}";
            }

            if (type.Length > 0)
            {
                type = $" {type}";
            }

            resultLines.Add($"{new string(' ', IndentAreaA + 1 + relativeIndentCount)}{nameSection}{redefinesSection}{type}");
        }

        // Add statement terminator to the last code-line
        resultLines[^1] += '.';

        return resultLines;
    }
}