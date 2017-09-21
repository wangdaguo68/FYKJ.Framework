namespace FYKJ.Config
{
    using System;
    using System.IO;

    public class FileConfigService : IConfigService
    {
        private readonly string configFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");

        public string GetConfig(string fileName)
        {
            if (!Directory.Exists(configFolder))
            {
                Directory.CreateDirectory(configFolder);
            }
            var filePath = GetFilePath(fileName);
            return !File.Exists(filePath) ? null : File.ReadAllText(filePath);
        }

        public string GetFilePath(string fileName)
        {
            return string.Format(@"{0}\{1}.xml", configFolder, fileName);
        }

        public void SaveConfig(string fileName, string content)
        {
            File.WriteAllText(GetFilePath(fileName), content);
        }
    }
}

