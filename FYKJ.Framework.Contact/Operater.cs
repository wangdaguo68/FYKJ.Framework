namespace FYKJ.Framework.Contract
{
    using System;

    public class Operater
    {
        public Operater()
        {
            Name = "匿名";
        }

        public string IP { get; set; }

        public string Method { get; set; }

        public string Name { get; set; }

        public DateTime Time { get; set; }

        public Guid Token { get; set; }

        public int UserId { get; set; }
    }
}

