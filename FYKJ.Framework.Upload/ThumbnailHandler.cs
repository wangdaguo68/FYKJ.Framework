using System.Globalization;

namespace FYKJ.Upload
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Web;
    using Config;

    public class ThumbnailHandler : IHttpHandler
    {
        private string GetImageType(string ext)
        {
            switch (ext.ToLower())
            {
                case "gif":
                    return "image/gif";

                case "jpg":
                case "jpe":
                case "jpeg":
                    return "image/jpeg";

                case "bmp":
                    return "image/bmp";

                case "tif":
                case "tiff":
                    return "image/tiff";

                case "eps":
                    return "application/postscript";
            }
            return null;
        }

        public void ProcessRequest(HttpContext context)
        {
            if (!string.IsNullOrEmpty(context.Request.Headers["If-Modified-Since"]))
            {
                context.Response.StatusCode = 0x130;
                context.Response.StatusDescription = "Not Modified";
            }
            else
            {
                string currentExecutionFilePath = context.Request.CurrentExecutionFilePath;
                if (currentExecutionFilePath.EndsWith(".axd") || currentExecutionFilePath.StartsWith("/Upload", StringComparison.OrdinalIgnoreCase))
                {
                    Match match = Regex.Match(currentExecutionFilePath, @"upload/(.+)/(day_\d+)/thumb/(\d+)_(\d+)_(\d+)\.([A-Za-z]+)\.axd$", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        var str2 = match.Groups[1].Value;
                        var str3 = match.Groups[2].Value;
                        var str4 = match.Groups[3].Value;
                        var str5 = match.Groups[4].Value;
                        var str6 = match.Groups[5].Value;
                        var ext = match.Groups[6].Value;
                        var key = string.Format("{0}_{1}_{2}", str2, str5, str6).ToLower();
                        if (UploadConfigContext.ThumbnailConfigDic.ContainsKey(key) && (UploadConfigContext.ThumbnailConfigDic[key].Timming == Timming.OnDemand))
                        {
                            string str9 = string.Format(@"{0}\{1}\Thumb\{2}_{4}_{5}.{3}", new object[] { str2, str3, str4, ext, str5, str6 });
                            str9 = Path.Combine(UploadConfigContext.UploadPath, str9);
                            string str10 = string.Format(@"{0}\{1}\{2}.{3}", new object[] { str2, str3, str4, ext });
                            str10 = Path.Combine(UploadConfigContext.UploadPath, str10);
                            if (File.Exists(str10))
                            {
                                if (!File.Exists(str9))
                                {
                                    string str11 = string.Format(@"{0}\{1}\Thumb", str2, str3);
                                    str11 = Path.Combine(UploadConfigContext.UploadPath, str11);
                                    if (!Directory.Exists(str11))
                                    {
                                        Directory.CreateDirectory(str11);
                                    }
                                    ThumbnailHelper.MakeThumbnail(str10, str9, UploadConfigContext.ThumbnailConfigDic[key]);
                                }
                                context.Response.Clear();
                                context.Response.ContentType = GetImageType(ext);
                                var buffer = File.ReadAllBytes(str9);
                                context.Response.BinaryWrite(buffer);
                                Set304Cache(context);
                                context.Response.Flush();
                            }
                        }
                    }
                }
            }
        }

        private void Set304Cache(HttpContext context)
        {
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.SetLastModified(DateTime.UtcNow);
            context.Response.AddHeader("If-Modified-Since", DateTime.UtcNow.ToString(CultureInfo.CurrentCulture));
            const int seconds = 0x127500;
            context.Response.Cache.SetExpires(DateTime.Now.AddSeconds(seconds));
            context.Response.Cache.SetMaxAge(new TimeSpan(0, 0, seconds));
            context.Response.CacheControl = "private";
            context.Response.Cache.SetValidUntilExpires(true);
        }

        public bool IsReusable => false;
    }
}

