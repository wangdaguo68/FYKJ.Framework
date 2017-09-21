namespace FYKJ.Framework.Utility
{
    public class DiffOperation
    {
        public DiffOperation(DiffAction action, int startInOld, int endInOld, int startInNew, int endInNew)
        {
            Action = action;
            StartInOld = startInOld;
            EndInOld = endInOld;
            StartInNew = startInNew;
            EndInNew = endInNew;
        }

        public DiffAction Action { get; set; }

        public int EndInNew { get; set; }

        public int EndInOld { get; set; }

        public int StartInNew { get; set; }

        public int StartInOld { get; set; }
    }
}

