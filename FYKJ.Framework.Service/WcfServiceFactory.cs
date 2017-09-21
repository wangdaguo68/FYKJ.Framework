namespace FYKJ.Service
{
    using Framework.Utility;

    public class WcfServiceFactory : ServiceFactory
    {
        public override T CreateService<T>() 
        {
            return WcfServiceProxy.CreateServiceProxy<T>(string.Empty);
        }
    }
}

