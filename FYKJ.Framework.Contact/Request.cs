namespace FYKJ.Framework.Contract
{
    public class Request : ModelBase
    {
        public Request()
        {
            PageSize = 10;
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int Top
        {
            set
            {
                PageSize = value;
                PageIndex = 1;
            }
        }
    }
}

