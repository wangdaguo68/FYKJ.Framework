using System.Linq;

namespace FYKJ.Upload
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Threading;
    using Config;

    public class ThumbnailService
    {
        public static void HandleImmediateThumbnail(string filePath, Timming timming = (Timming)2)
        {
            var match = Regex.Match(filePath, @"^(.*)\\upload\\(.+)\\(day_\d+)\\(\d+)(\.[A-Za-z]+)$", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                var str = match.Groups[1].Value;
                var folder = match.Groups[2].Value;
                var str2 = match.Groups[3].Value;
                var str3 = match.Groups[4].Value;
                var str4 = match.Groups[5].Value;
                foreach (KeyValuePair<string, ThumbnailSize> pair in from t in UploadConfigContext.ThumbnailConfigDic
                    where t.Key.StartsWith(folder.ToLower() + "_") && (t.Value.Timming == timming)
                    select t)
                {
                    var size = pair.Value;
                    var path = string.Format(@"{0}\upload\{1}\{2}\thumb", str, folder, str2);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var thumbnailPath = string.Format(@"{0}\upload\{1}\{2}\thumb\{3}_{4}_{5}{6}", new object[] { str, folder, str2, str3, size.Width, size.Height, str4 });
                    ThumbnailHelper.MakeThumbnail(filePath, thumbnailPath, size);
                }
            }
        }

        public static void HandlerLazyThumbnail()
        {
            foreach (var folder in UploadConfigContext.UploadConfig.UploadFolders)
            {
                var path = Path.Combine(UploadConfigContext.UploadPath, folder.Path);
                if (Directory.Exists(path))
                {
                    foreach (var str2 in Directory.GetDirectories(path))
                    {
                        foreach (var str3 in Directory.GetFiles(str2))
                        {
                            var match = Regex.Match(str3, @"^(.+\\day_\d+)\\(\d+)(\.[A-Za-z]+)$", RegexOptions.IgnoreCase);
                            if (match.Success)
                            {
                                var str4 = match.Groups[1].Value;
                                var str5 = match.Groups[2].Value;
                                var str6 = match.Groups[3].Value;
                                var str7 = Path.Combine(str2, "Thumb");
                                if (!Directory.Exists(str7))
                                {
                                    Directory.CreateDirectory(str7);
                                }
                                foreach (var size in folder.ThumbnailSizes)
                                {
                                    if (size.Timming == Timming.Lazy)
                                    {
                                        var str8 = string.Format(@"{0}\thumb\{1}_{2}_{3}{4}", str4, str5, size.Width, size.Height, str6);
                                        if (File.Exists(str8) && size.IsReplace)
                                        {
                                            File.Delete(str8);
                                        }
                                        if (!File.Exists(str8))
                                        {
                                            ThumbnailHelper.MakeThumbnail(str3, str8, size);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void HandlerLazyThumbnail(int intervalMunites)
        {
            var watcher = new FileSystemWatcher(UploadConfigContext.UploadPath) {
                IncludeSubdirectories = true
            };
            watcher.Created += (s, e) => HandleImmediateThumbnail(e.FullPath, Timming.Lazy);
            watcher.EnableRaisingEvents = true;
            while (true)
            {
                HandlerLazyThumbnail();
                GC.Collect();
                Console.WriteLine("等待 {0} 分钟再重新扫描...........", intervalMunites);
                Thread.Sleep((intervalMunites * 60) * 0x3e8);
            }
        }
    }
}

