namespace FYKJ.Config
{
    public interface IConfigService
    {
        string GetConfig(string name);
        string GetFilePath(string name);
        void SaveConfig(string name, string content);
    }
}

