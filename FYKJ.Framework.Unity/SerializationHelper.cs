using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;

namespace FYKJ.Framework.Utility
{
    [Serializable]
    public class SerializationHelper
    {
        private SerializationHelper()
        {
        }

        public static object DataContractDeserialize(Type type, string xmlStr)
        {
            if ((xmlStr == null) || (xmlStr.Trim() == ""))
            {
                return null;
            }
            DataContractSerializer serializer = new DataContractSerializer(type);
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlStr));
            return serializer.ReadObject(stream);
        }

        public static string DataContractSerialize(object o)
        {
            if (o == null)
            {
                return null;
            }
            MemoryStream stream = new MemoryStream();
            new DataContractSerializer(o.GetType()).WriteObject(stream, o);
            return Encoding.UTF8.GetString(stream.ToArray());
        }

        public static T JsonDeserialize<T>(string json)
        {
            return (T) JsonDeserialize(typeof(T), json);
        }

        public static object JsonDeserialize(Type type, string json)
        {
            if (json == null)
            {
                return null;
            }
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(type);
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json.ToCharArray()));
            object obj2 = serializer.ReadObject(stream);
            stream.Close();
            return obj2;
        }

        public static string JsonSerialize<T>(T obj)
        {
            return JsonSerialize(obj, Encoding.UTF8);
        }

        public static string JsonSerialize(object obj, Encoding encoding)
        {
            if (obj == null)
            {
                return null;
            }
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, obj);
            stream.Position = 0L;
            StreamReader reader = new StreamReader(stream, encoding);
            string str = reader.ReadToEnd();
            reader.Close();
            stream.Close();
            return str;
        }

        public static object LoadFromBinaryBytes(byte[] bytes)
        {
            object obj2 = null;
            BinaryFormatter formatter = new BinaryFormatter();
            if (bytes != null)
            {
                MemoryStream serializationStream = new MemoryStream(bytes);
                obj2 = formatter.Deserialize(serializationStream);
            }
            return obj2;
        }

        public static byte[] SaveToBinaryBytes(object obj)
        {
            MemoryStream serializationStream = new MemoryStream();
            new BinaryFormatter().Serialize(serializationStream, obj);
            return serializationStream.ToArray();
        }

        public static MemoryStream SaveToMemoryStream(object obj)
        {
            MemoryStream stream = new MemoryStream();
            new BufferedStream(stream);
            new BinaryFormatter().Serialize(stream, obj);
            return stream;
        }

        public static object XmlDeserialize(Type type, string xmlStr)
        {
            if ((xmlStr == null) || (xmlStr.Trim() == ""))
            {
                return null;
            }
            XmlSerializer serializer = new XmlSerializer(type);
            StringReader textReader = new StringReader(xmlStr);
            return serializer.Deserialize(textReader);
        }

        public static object XmlDeserializeFromFile(Type type, string filename)
        {
            FileStream stream = null;
            object obj2;
            try
            {
                stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                obj2 = new XmlSerializer(type).Deserialize(stream);
            }
            finally
            {
                stream?.Close();
            }
            return obj2;
        }

        public static string XmlSerialize(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, obj);
            return writer.ToString();
        }

        public static void XmlSerialize(object obj, string filename)
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                new XmlSerializer(obj.GetType()).Serialize(stream, obj);
            }
            finally
            {
                stream?.Close();
            }
        }

        public static string XmlSerialize(object obj, Encoding ecoding)
        {
            if (obj == null)
            {
                return null;
            }
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            MemoryStream stream = new MemoryStream();
            StreamWriter textWriter = new StreamWriter(stream, ecoding);
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            serializer.Serialize(textWriter, obj, namespaces);
            string str = ecoding.GetString(stream.ToArray());
            stream.Close();
            return str;
        }
    }
}

