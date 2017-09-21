namespace FYKJ.Framework.Utility
{
    using System;

    public static class ExceptionHelper
    {
        public static bool Is<T>(this Exception source) where T: Exception
        {
            return source is T || ((source.InnerException != null) && source.InnerException.Is<T>());
        }
    }
}

