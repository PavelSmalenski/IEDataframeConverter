using Microsoft.Extensions.Configuration;
using DataframeConverter.Libraries.MvsFtp.Entities;

namespace DataframeConverter.Libraries.MvsFtp;

class MvsFtpSettingsBuilder
{
    IConfigurationRoot? _config;

    public MvsFtpSettings GetMvsFtpSettings(string path, bool isForceCreate = false)
    {
        if (_config is null || isForceCreate)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path, optional: false, reloadOnChange: true);
            _config = builder.Build();
        }

        var ftpSettings = _config.GetSection("FtpSettings").Get<MvsFtpSettings>();

        if (ftpSettings == null)
        {
            throw new ArgumentException("Failed to load FTP settings from configuration.");
        }
        else if (string.IsNullOrWhiteSpace(ftpSettings.Host) ||
                 string.IsNullOrWhiteSpace(ftpSettings.Username) ||
                 string.IsNullOrWhiteSpace(ftpSettings.Password) ||
                 string.IsNullOrWhiteSpace(ftpSettings.TargetPdsName))
        {
            throw new ArgumentException("One or more MVS FTP settings are missing or empty in configuration.");
        }

        return ftpSettings;
    }
}