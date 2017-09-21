namespace FYKJ.Upload
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using Config;
    using Framework.Utility;

    public class UploadConfigContext
    {
        private static readonly object olock = new object();
        private static Dictionary<string, ThumbnailSize> thumbnailConfigDic;
        public static UploadConfig UploadConfig = CachedConfigContext.Current.UploadConfig;
        public static string uploadPath;

        public static Dictionary<string, ThumbnailSize> ThumbnailConfigDic
        {
            get
            {
                if (thumbnailConfigDic == null)
                {
                    lock (olock)
                    {
                        if (thumbnailConfigDic == null)
                        {
                            thumbnailConfigDic = new Dictionary<string, ThumbnailSize>();
                            foreach (var folder in UploadConfig.UploadFolders)
                            {
                                foreach (var size in folder.ThumbnailSizes)
                                {
                                    var key = string.Format("{0}_{1}_{2}", folder.Path, size.Width, size.Height).ToLower();
                                    if (!thumbnailConfigDic.ContainsKey(key))
                                    {
                                        thumbnailConfigDic.Add(key, size);
                                    }
                                }
                            }
                        }
                    }
                }
                return thumbnailConfigDic;
            }
        }

        public static string UploadPath
        {
            get
            {
                if (uploadPath == null)
                {
                    lock (olock)
                    {
                        if (uploadPath == null)
                        {
                            uploadPath = CachedConfigContext.Current.UploadConfig.UploadPath ?? string.Empty;
                            if ((HttpContext.Current != null) && (((Fetch.ServerDomain.IndexOf("cnsaas", StringComparison.OrdinalIgnoreCase) < 0) || string.IsNullOrEmpty(UploadConfig.UploadPath)) || !Directory.Exists(UploadConfig.UploadPath)))
                            {
                                uploadPath = HttpContext.Current.Server.MapPath("~/Upload");
                            }
                        }
                    }
                }
                return uploadPath;
            }
        }
    }
}

