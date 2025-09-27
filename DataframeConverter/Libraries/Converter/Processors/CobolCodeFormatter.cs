using System.Runtime.CompilerServices;
using Cobol.Converter.Extensions;

namespace Cobol.Converter.Processors;

static class CobolCodeFormatter
{
    const char CommentControl = '*';
    const char GenericControl = ' ';
    const int IndentAreaA = 6;
    const int IndentAreaB = 4;
    const int IndentGeneral = 4;
    const int MaxCommentLength = 64;
    const int MaxCodeLength = 64;
    const int CobolLevelStart = 1;
    const int CobolLevelStep = 5;

    public static IEnumerable<string> GetCommentsCode(string comment)
    {
        List<string> strings = new List<string>();

        if (comment.Length > MaxCommentLength)
        {
            IEnumerable<string> commentSlices = comment.SplitOnLength(MaxCommentLength);
            foreach (var commentSlice in commentSlices)
            {
                strings.Add(GetCommentCode(commentSlice));
            }
        }
        else
        {
            strings.Add(GetCommentCode(comment));
        }

        return strings;
    }

    public static string GetCommentCode(string comment)
    {
        return $"{new string(' ', IndentAreaA)}{CommentControl} {comment}";
    }

    public static string GetRulerCode(char rulerChar = '-')
    {
        return $"{new string(' ', IndentAreaA)}{CommentControl} {new string(rulerChar, MaxCommentLength)}";
    }

    public static IEnumerable<string> GetVariablesCode(int level, string name, string type = "", string overridesName = "")
    {
        List<string> result = new List<string>();

        int indentCount = 0;
        if (level > 1)
        {
            indentCount += IndentAreaB + (IndentGeneral * (level - 1));
        }

        int cobolLevel = level > 1 ? CobolLevelStep * (level - 1) : CobolLevelStart;

        if (type == "")
        {
            result.Add($"{new string(' ', IndentAreaA + 1 + indentCount)}{cobolLevel:D2} {name}.");
            return result;
        }

        //                Level             Spacing           Dot
        if (indentCount + 3 + name.Length + 1 + type.Length + 1 > MaxCodeLength)
        {
            result.Add($"{new string(' ', IndentAreaA + 1 + indentCount)}{cobolLevel:D2} {name}");
            result.Add($"{new string(' ', IndentAreaA + 1 + indentCount + 1)}{type}.");
        }
        else
        {
            result.Add($"{new string(' ', IndentAreaA + 1 + indentCount)}{cobolLevel:D2} {name} {type}.");
        }

        return result;
    }
}