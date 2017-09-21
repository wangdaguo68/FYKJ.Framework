namespace FYKJ.Service
{
    using Cache;
    using Framework.Utility;

    public class RefServiceFactory : ServiceFactory
    {
        public override T CreateService<T>() 
        {
            var name = typeof(T).Name;
            return CacheHelper.Get(string.Format("Service_{0}", name), () => AssemblyHelper.FindTypeByInterface<T>());
        }
    }
}

