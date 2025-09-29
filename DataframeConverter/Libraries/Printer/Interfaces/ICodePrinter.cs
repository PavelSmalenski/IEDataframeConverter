namespace IE.Printers.Interfaces;

interface ICodePrinter
{
    void Print(string dataframeName, List<string> dataframeCode);
}