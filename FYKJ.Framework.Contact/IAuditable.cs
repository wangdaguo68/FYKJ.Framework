namespace FYKJ.Framework.Contract
{
    public interface IAuditable
    {
        void WriteLog(int modelId, string userName, string moduleName, string tableName, string eventType, ModelBase newValues);
    }
}

