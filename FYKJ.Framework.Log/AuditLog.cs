namespace FYKJ.Log
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Framework.Contract;

    [Table("AuditLog")]
    public class AuditLog : ModelBase
    {
        public string EventType { get; set; }

        public int ModelId { get; set; }

        public string ModuleName { get; set; }

        public string NewValues { get; set; }

        public string TableName { get; set; }

        public string UserName { get; set; }
    }
}

