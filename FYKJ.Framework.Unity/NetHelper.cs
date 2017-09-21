using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace FYKJ.Framework.Utility
{
    public class NetHelper
    {
        public static string HttpGet(string uri)
        {
            StringBuilder builder = new StringBuilder();
            HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            byte[] buffer = new byte[0x2000];
            Stream responseStream = response.GetResponseStream();
            int count = 0;
            do
            {
                count = responseStream.Read(buffer, 0, buffer.Length);
                if (count != 0)
                {
                    builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
                }
            }
            while (count > 0);
            return builder.ToString();
        }

        public static T HttpGet<T>(string uri, SerializationType serializationType)
        {
            string xmlStr = HttpGet(uri);
            T local = default(T);
            if (serializationType == SerializationType.Xml)
            {
                return (T) SerializationHelper.XmlDeserialize(typeof(T), xmlStr);
            }
            if (serializationType == SerializationType.Json)
            {
                local = (T) SerializationHelper.JsonDeserialize(typeof(T), xmlStr);
            }
            return local;
        }

        public static string HttpPost(string uri, NameValueCollection data)
        {
            byte[] bytes = new CNNWebClient { Encoding = Encoding.UTF8, Timeout = 300 }.UploadValues(uri, "POST", data);
            return Encoding.UTF8.GetString(bytes);
        }

        public static string HttpPost(string uri, object data, SerializationType serializationType)
        {
            HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            string xmlStr = string.Empty;
            if (data is string)
            {
                xmlStr = (string) data;
            }
            else if (serializationType == SerializationType.Xml)
            {
                xmlStr = SerializationHelper.XmlSerialize(data);
                SerializationHelper.XmlDeserialize(data.GetType(), xmlStr);
            }
            else if (serializationType == SerializationType.Json)
            {
                xmlStr = SerializationHelper.JsonSerialize(data);
            }
            byte[] bytes = new CNNWebClient { Timeout = 300 }.UploadData(uri, "POST", Encoding.UTF8.GetBytes(xmlStr));
            return Encoding.UTF8.GetString(bytes);
        }

        public static T HttpPost<T>(string uri, object data, SerializationType serializationType)
        {
            string xmlStr = HttpPost(uri, data, serializationType);
            T local = default(T);
            if (serializationType == SerializationType.Xml)
            {
                return (T) SerializationHelper.XmlDeserialize(typeof(T), xmlStr);
            }
            if (serializationType == SerializationType.Json)
            {
                local = (T) SerializationHelper.JsonDeserialize(typeof(T), xmlStr);
            }
            return local;
        }

        public class CNNWebClient : WebClient
        {
            private int _timeOut = 200;

            protected override WebRequest GetWebRequest(Uri address)
            {
                HttpWebRequest webRequest = (HttpWebRequest) base.GetWebRequest(address);
                webRequest.Timeout = 0x3e8 * Timeout;
                webRequest.ReadWriteTimeout = 0x3e8 * Timeout;
                return webRequest;
            }

            public int Timeout
            {
                get
                {
                    return _timeOut;
                }
                set
                {
                    if (value <= 0)
                    {
                        _timeOut = 200;
                    }
                    _timeOut = value;
                }
            }
        }
    }
}

