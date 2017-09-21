namespace FYKJ.Framework.Web
{
    using System.Web;

    public class ExplicitException : HttpException
    {
        public ExplicitException()
        {
        }

        public ExplicitException(string message) : base(message)
        {
        }

        public ExplicitException(int httpCode, string message) : base(httpCode, message)
        {
        }
    }
}

