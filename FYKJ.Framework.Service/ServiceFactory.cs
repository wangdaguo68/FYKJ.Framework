namespace FYKJ.Service
{
    public abstract class ServiceFactory
    {
        public abstract T CreateService<T>() where T: class;
    }
}

