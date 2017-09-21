namespace FYKJ.Framework.Contract
{
    using System;

    public class BusinessException : Exception
    {
        public BusinessException() : this(string.Empty)
        {
        }

        public BusinessException(string message) : this("错误", message)
        {
        }

        public BusinessException(string message, Enum errorCode) : this("错误", message, errorCode)
        {
        }

        public BusinessException(string name, string message) : base(message)
        {
            Name = name;
        }

        public BusinessException(string name, string message, Enum errorCode) : base(message)
        {
            Name = name;
            ErrorCode = errorCode;
        }

        public Enum ErrorCode { get; set; }

        public string Name { get; set; }
    }
}

