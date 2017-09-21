using System.Linq;

namespace FYKJ.Framework.Utility
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Description;
    using System.Xml;

    public class WcfServiceProxy
    {
        private const int maxReceivedMessageSize = 0x7fffffff;
        private static readonly TimeSpan timeout = TimeSpan.FromMinutes(10.0);

        public static T CreateServiceProxy<T>(string uri)
        {
            string name = string.Format("{0} - {1}", typeof(T), uri);
            if (Caching.Get(name) != null)
            {
                return (T) Caching.Get(name);
            }
            BasicHttpBinding binding = new BasicHttpBinding {
                MaxReceivedMessageSize = 0x7fffffffL,
                ReaderQuotas = new XmlDictionaryReaderQuotas()
            };
            binding.ReaderQuotas.MaxStringContentLength = 0x7fffffff;
            binding.ReaderQuotas.MaxArrayLength = 0x7fffffff;
            binding.ReaderQuotas.MaxBytesPerRead = 0x7fffffff;
            binding.OpenTimeout = timeout;
            binding.ReceiveTimeout = timeout;
            binding.SendTimeout = timeout;
            ChannelFactory<T> factory = new ChannelFactory<T>(binding, new EndpointAddress(uri));
            foreach (DataContractSerializerOperationBehavior behavior in factory.Endpoint.Contract.Operations.Select(description => description.Behaviors.Find<DataContractSerializerOperationBehavior>()).Where(behavior => behavior != null))
            {
                behavior.MaxItemsInObjectGraph = 0x7fffffff;
            }
            factory.Open();
            T local = factory.CreateChannel();
            Caching.Set(name, local);
            return local;
        }
    }
}

