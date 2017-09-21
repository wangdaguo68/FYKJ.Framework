namespace FYKJ.Framework.Contract
{
    using System;

    public  class ModelBase
    {
        public virtual DateTime CreateTime { get; set; } = DateTime.Now;

        public virtual int ID { get; set; }
    }
}

