namespace FYKJ.Framework.Contract
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Remoting.Messaging;

    [Serializable]
    public class WCFContext : Dictionary<string, object>
    {
        private const string CallContextKey = "__CallContext";
        internal const string ContextHeaderLocalName = "__CallContext";
        internal const string ContextHeaderNamespace = "urn:cnsaas.com";

        private void EnsureSerializable(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (!value.GetType().IsSerializable)
            {
                throw new ArgumentException(string.Format("此类型参数 \"{0}\" 不能序列化！", value.GetType().FullName));
            }
        }

        public static WCFContext Current
        {
            get
            {
                if (CallContext.GetData("__CallContext") == null)
                {
                    CallContext.SetData("__CallContext", new WCFContext());
                }
                return (CallContext.GetData("__CallContext") as WCFContext);
            }
            set
            {
                CallContext.SetData("__CallContext", value);
            }
        }

        public new object this[string key]
        {
            get
            {
                return base[key];
            }
            set
            {
                EnsureSerializable(value);
                base[key] = value;
            }
        }

        public Operater Operater
        {
            get
            {
                return JsonConvert.DeserializeObject<Operater>(this["__Operater"].ToString());
            }
            set
            {
                this["__Operater"] = JsonConvert.SerializeObject(value);
            }
        }
    }
}

