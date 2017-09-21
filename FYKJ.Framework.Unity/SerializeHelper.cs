namespace FYKJ.Framework.Utility
{
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;

    public class SerializeHelper
    {
        public static T JsonDeserialize<T>(string json)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json.ToCharArray()));
            T local = (T) serializer.ReadObject(stream);
            stream.Close();
            return local;
        }

        public static string JsonSerialize<T>(T obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, obj);
            stream.Position = 0L;
            StreamReader reader = new StreamReader(stream);
            string str = reader.ReadToEnd();
            reader.Close();
            stream.Close();
            return str;
        }

        public static T XmlDeserialize<T>(string xml)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml.ToCharArray()));
            T local = (T) serializer.ReadObject(stream);
            stream.Close();
            return local;
        }

        public static string XmlSerialize<T>(T obj)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, obj);
            stream.Position = 0L;
            StreamReader reader = new StreamReader(stream);
            string str = reader.ReadToEnd();
            reader.Close();
            stream.Close();
            return str;
        }
    }
}

