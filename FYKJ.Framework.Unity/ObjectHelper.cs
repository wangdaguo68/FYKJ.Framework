namespace FYKJ.Framework.Utility
{
    public class ObjectHelper
    {
        public static F DeepCopy<T, F>(T original)
        {
            return SerializeHelper.JsonDeserialize<F>(SerializeHelper.JsonSerialize(original));
        }

        public static void DeepCopy<T, F>(T original, F desination)
        {
            desination = DeepCopy<T, F>(original);
        }
    }
}

