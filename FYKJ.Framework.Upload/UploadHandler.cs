namespace FYKJ.Upload
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using Config;

    public abstract class UploadHandler : IHttpHandler
    {
        public abstract string GetResult(string localFileName, string uploadFilePath, string err);
        public abstract void OnUploaded(HttpContext context, string filePath);
        public void ProcessRequest(HttpContext context)
        {
            byte[] buffer;
            context.Response.Charset = "UTF-8";
            string localFileName;
            var err = string.Empty;
            var str3 = string.Empty;
            var path = string.Empty;
            var input = context.Request.ServerVariables["HTTP_CONTENT_DISPOSITION"];
            if (input != null)
            {
                buffer = context.Request.BinaryRead(context.Request.TotalBytes);
                localFileName = Regex.Match(input, "filename=\"(.+?)\"").Groups[1].Value;
            }
            else
            {
                var file = context.Request.Files.Get(FileInputName);
                localFileName = Path.GetFileName(file.FileName);
                buffer = new byte[file.ContentLength];
                var inputStream = file.InputStream;
                inputStream.Read(buffer, 0, file.ContentLength);
                inputStream.Close();
            }
            var str7 = localFileName.Substring(localFileName.LastIndexOf('.') + 1).ToLower();
            if (buffer.Length == 0)
            {
                err = "无数据提交";
            }
            else if (buffer.Length > MaxFilesize)
            {
                err = "文件大小超过" + MaxFilesize + "字节";
            }
            else if (!AllowExt.Contains(str7))
            {
                err = "上传文件扩展名必需为：" + string.Join(",", AllowExt);
            }
            else
            {
                var folder = context.Request["subfolder"] ?? "default";
                var Ufolder = UploadConfigContext.UploadConfig.UploadFolders.FirstOrDefault(u => string.Equals(folder, u.Path, StringComparison.OrdinalIgnoreCase));
                switch ((Ufolder.DirType))
                {
                    case DirType.Day:
                        str3 = "day_" + DateTime.Now.ToString("yyMMdd");
                        break;

                    case DirType.Month:
                        str3 = "month_" + DateTime.Now.ToString("yyMM");
                        break;

                    case DirType.Ext:
                        str3 = "ext_" + str7;
                        break;
                }
                var str4 = Path.Combine(UploadConfigContext.UploadPath, folder, str3);
                path = Path.Combine(str4, string.Format("{0}{1}.{2}", DateTime.Now.ToString("yyyyMMddhhmmss"), new Random(DateTime.Now.Millisecond).Next(0x2710), str7));
                if (!Directory.Exists(str4))
                {
                    Directory.CreateDirectory(str4);
                }
                var stream2 = new FileStream(path, FileMode.Create, FileAccess.Write);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.Flush();
                stream2.Close();
                if (ImageExt.Contains(str7))
                {
                    ThumbnailService.HandleImmediateThumbnail(path);
                }
                OnUploaded(context, path);
            }
            context.Response.Write(GetResult(localFileName, path, err));
            context.Response.End();
        }

        public virtual string[] AllowExt => new[] { "txt", "rar", "zip", "jpg", "jpeg", "gif", "png", "swf" };

        public virtual string FileInputName => "filedata";

        public virtual string[] ImageExt => new[] { "jpg", "jpeg", "gif", "png" };

        public bool IsReusable => false;

        public int MaxFilesize => 0xa76980;

        public string UploadPath => UploadConfigContext.UploadPath;
    }
}

