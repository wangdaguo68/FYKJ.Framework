using FYKJ.Config;

namespace FYKJ.Log
{
    using System.Data.Entity;
    using Framework.Contract;
    using Framework.DAL;

    public class LogDbContext : DbContextBase, IAuditable
    {
        public LogDbContext() : base(CachedConfigContext.Current.DaoConfig.Log)
        {
            Database.SetInitializer<LogDbContext>(null);
        }

        public void WriteLog(int modelId, string userName, string moduleName, string tableName, string eventType, ModelBase newValues)
        {
        }

        public DbSet<AuditLog> AuditLogs { get; set; }
    }
}

