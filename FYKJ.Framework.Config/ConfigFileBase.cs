namespace FYKJ.Config
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class ConfigFileBase
    {
        internal virtual void Save()
        {
        }

        internal virtual void UpdateNodeList<T>(List<T> nodeList) where T: ConfigNodeBase
        {
            Func<T, int> selector = null;
            foreach (var local in nodeList.Where(local => local.Id <= 0))
            {
                if (selector == null)
                {
                    selector = n => n.Id;
                }
                local.Id = nodeList.Max(selector) + 1;
            }
        }

        public virtual bool ClusteredByIndex => false;

        public int Id { get; set; }
    }
}

