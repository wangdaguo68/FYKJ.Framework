namespace FYKJ.Framework.Utility
{
    public abstract class ValidateCodeType
    {
        public abstract byte[] CreateImage(out string resultCode);

        public abstract string Name { get; }

        public virtual string Tip => "请输入图片中的字符";

        public string Type => GetType().Name;
    }
}

