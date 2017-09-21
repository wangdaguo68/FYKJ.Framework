namespace FYKJ.Service
{
    using Castle.DynamicProxy;

    public class ServiceHelper
    {
        public static ServiceFactory serviceFactory = new RefServiceFactory();

        public static T CreateService<T>() where T: class
        {
            var target = serviceFactory.CreateService<T>();
            var generator = new ProxyGenerator();
            return generator.CreateInterfaceProxyWithTargetInterface(target, new InvokeInterceptor());
        }
    }
}

