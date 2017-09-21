namespace FYKJ.Upload
{
    using System;
    using System.Text.RegularExpressions;
    using Config;
    using Framework.Utility;

    public class ThumbnailHelper
    {
        public static string GetThumbnailUrl(string rawUrl, int width, int height)
        {
            Match match = Regex.Match(rawUrl, @"^(.*)/upload/(.+)/(day_\d+)/(\d+)(\.[A-Za-z]+)$", RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                return string.Empty;
            }
            var str = match.Groups[1].Value;
            var str2 = match.Groups[2].Value;
            var str3 = match.Groups[3].Value;
            var str4 = match.Groups[4].Value;
            var str5 = match.Groups[5].Value;
            var key = string.Format("{0}_{1}_{2}", str2, width, height).ToLower();
            if (UploadConfigContext.ThumbnailConfigDic.ContainsKey(key) && (UploadConfigContext.ThumbnailConfigDic[key].Timming == Timming.OnDemand))
            {
                str5 = str5 + ".axd";
            }
            return string.Format("{0}/upload/{1}/{2}/thumb/{3}_{4}_{5}{6}", str, str2, str3, str4, width, height, str5);
        }

        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, ThumbnailSize size)
        {
            try
            {
                ImageHelper.MakeThumbnail(originalImagePath, thumbnailPath, size.Width, size.Height, size.Mode, size.AddWaterMarker, size.WaterMarkerPosition, size.WaterMarkerPath, size.Quality);
                Console.WriteLine("生成成功:{0}", thumbnailPath);
            }
            catch
            {
                Console.WriteLine("生成失败，非标准图片:{0}", thumbnailPath);
            }
        }

        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode = "Cut", bool isaddwatermark = false, int quality = 0x58)
        {
            var size = new ThumbnailSize {
                Width = width,
                Height = height,
                Mode = mode,
                AddWaterMarker = isaddwatermark,
                Quality = quality
            };
            MakeThumbnail(originalImagePath, thumbnailPath, size);
        }
    }
}

