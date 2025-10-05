namespace IE.Printers.Interfaces;

interface ICodePrinter : IDisposable
{
    void Print(string dataframeName, List<string> dataframeCode);
}