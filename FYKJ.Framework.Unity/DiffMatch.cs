namespace FYKJ.Framework.Utility
{
    public class DiffMatch
    {
        public DiffMatch(int startInOld, int startInNew, int size)
        {
            StartInOld = startInOld;
            StartInNew = startInNew;
            Size = size;
        }

        public int EndInNew => (StartInNew + Size);

        public int EndInOld => (StartInOld + Size);

        public int Size { get; set; }

        public int StartInNew { get; set; }

        public int StartInOld { get; set; }
    }
}

