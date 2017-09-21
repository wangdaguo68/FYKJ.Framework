namespace FYKJ.Config
{
    using System;

    [Flags]
    public enum Timming
    {
        Immediate = 2,
        Lazy = 1,
        OnDemand = 4
    }
}

