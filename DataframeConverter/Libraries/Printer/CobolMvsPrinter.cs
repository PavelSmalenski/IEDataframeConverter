using FluentFTP;
using IE.Printers.Interfaces;

namespace IE.Printers;

class CobolMvsPrinter : ICodePrinter, IDisposable
{
    string _host;
    string _user;
    string _password;
    string _targetPds;
    FtpClient? _ftpClient;

    public CobolMvsPrinter(string host, string user, string password, string targetPds)
    {
        _host = host;
        _user = user;
        _password = password;
        _targetPds = VerifyPdsName(targetPds) ? targetPds : throw new ArgumentException($"Invalid PDS name: {targetPds}");

        _ftpClient = new FtpClient(_host, _user, _password, config: new FtpConfig
        {
            UploadDataType = FtpDataType.ASCII,
            ConnectTimeout = 10000
        });
        _ftpClient.Connect();
    }

    public void Print(string dataframeName, List<string> dataframeCode)
    {
        if (_ftpClient == null || !_ftpClient.IsConnected)
        {
            throw new InvalidOperationException("FTP client is not connected.");
        }

        string content = string.Join(Environment.NewLine, dataframeCode);
        using (var stream = _ftpClient.OpenWrite($"'{_targetPds}({dataframeName})'", FtpDataType.ASCII))
        using (var writer = new StreamWriter(stream))
        {
            writer.Write(content);
        }
    }

    bool VerifyPdsName(string pdsName)
    {
        if (string.IsNullOrEmpty(pdsName) || pdsName.Length > 44)
        {
            return false;
        }

        var subQualifiers = pdsName.Split('.');
        foreach (var qualifier in subQualifiers)
        {
            if (string.IsNullOrEmpty(qualifier) || qualifier.Length < 1 || qualifier.Length > 8)
            {
                return false;
            }
            else
            {
                if (qualifier[0] == '-')
                {
                    return false;
                }

                if (qualifier.FirstOrDefault(c => !char.IsLetterOrDigit(c) && !(c == '$' || c == '#' || c == '@' || c == '-')) != char.MinValue)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void Dispose()
    {
        _ftpClient?.Dispose();
    }
}