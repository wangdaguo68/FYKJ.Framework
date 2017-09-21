namespace FYKJ.Cache
{
    using System;

    public interface ICacheProvider
    {
        void Clear(string keyRegex);
        object Get(string key);
        void Remove(string key);
        void Set(string key, object value, int minutes, bool isAbsoluteExpiration, Action<string, object, string> onRemove);
    }
}

