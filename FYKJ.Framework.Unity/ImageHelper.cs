using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace FYKJ.Framework.Utility
{
    public class ImageHelper
    {
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode = "Cut", bool isaddwatermark = false, int quality = 0x4b)
        {
            MakeThumbnail(originalImagePath, thumbnailPath, width, height, mode, isaddwatermark, ImagePosition.Default, null, quality);
        }

        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode, bool isaddwatermark, ImagePosition imagePosition, string waterImage = null, int quality = 0x4b)
        {
            string str;
            Image image = Image.FromFile(originalImagePath);
            int num = width;
            int num2 = height;
            int x = 0;
            int y = 0;
            int num5 = image.Width;
            int num6 = image.Height;
            if (((str = mode) != null) && (str != "HW"))
            {
                if (str != "W")
                {
                    switch (str)
                    {
                        case "H":
                            num = (image.Width * height) / image.Height;
                            break;
                        case "Cut":
                            if ((image.Width >= num) && (image.Height >= num2))
                            {
                                if ((image.Width / ((double) image.Height)) > (num / ((double) num2)))
                                {
                                    num6 = image.Height;
                                    num5 = (image.Height * num) / num2;
                                    y = 0;
                                    x = (image.Width - num5) / 2;
                                }
                                else
                                {
                                    num5 = image.Width;
                                    num6 = (image.Width * height) / num;
                                    x = 0;
                                    y = (image.Height - num6) / 2;
                                }
                            }
                            else
                            {
                                x = (image.Width - num) / 2;
                                y = (image.Height - num2) / 2;
                                num5 = num;
                                num6 = num2;
                            }
                            break;
                        case "Fit":
                            if ((image.Width > num) && (image.Height > num2))
                            {
                                if ((image.Width / ((double) image.Height)) > (num / ((double) num2)))
                                {
                                    num2 = (image.Height * width) / image.Width;
                                }
                                else
                                {
                                    num = (image.Width * height) / image.Height;
                                }
                            }
                            else if (image.Width > num)
                            {
                                num2 = (image.Height * width) / image.Width;
                            }
                            else if (image.Height > num2)
                            {
                                num = (image.Width * height) / image.Height;
                            }
                            else
                            {
                                num = image.Width;
                                num2 = image.Height;
                                num5 = num;
                                num6 = num2;
                            }
                            break;
                    }
                }
                else
                {
                    num2 = (image.Height * width) / image.Width;
                }
            }
            Image image2 = new Bitmap(num, num2);
            Graphics graphics = Graphics.FromImage(image2);
            graphics.InterpolationMode = InterpolationMode.High;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.Clear(Color.White);
            graphics.DrawImage(image, new Rectangle(0, 0, num, num2), new Rectangle(x, y, num5, num6), GraphicsUnit.Pixel);
            if (isaddwatermark)
            {
                int num7;
                int num8;
                if (string.IsNullOrEmpty(waterImage))
                {
                    waterImage = "watermarker.png";
                }
                Image image3 = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, waterImage));
                int num9 = image3.Height;
                int num10 = image3.Width;
                int num11 = num2;
                int num12 = num;
                switch (imagePosition)
                {
                    case ImagePosition.LeftTop:
                        num7 = 70;
                        num8 = -70;
                        break;

                    case ImagePosition.LeftBottom:
                        num7 = 70;
                        num8 = (num11 - num9) - 70;
                        break;

                    case ImagePosition.RightTop:
                        num7 = (num12 - num10) - 70;
                        num8 = -70;
                        break;

                    case ImagePosition.RigthBottom:
                        num7 = (num12 - num10) - 70;
                        num8 = (num11 - num9) - 70;
                        break;

                    default:
                        num7 = 10;
                        num8 = 0;
                        break;
                }
                graphics.DrawImage(image3, new Rectangle(num7, num8, image3.Width, image3.Height), 0, 0, image3.Width, image3.Height, GraphicsUnit.Pixel);
            }
            EncoderParameters encoderParams = new EncoderParameters();
            long[] numArray = { quality };
            EncoderParameter parameter = new EncoderParameter(Encoder.Quality, numArray);
            encoderParams.Param[0] = parameter;
            ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo encoder = imageEncoders.FirstOrDefault(t => t.FormatDescription.Equals("JPEG"));
            try
            {
                if (encoder != null)
                {
                    image2.Save(thumbnailPath, encoder, encoderParams);
                }
                else
                {
                    image2.Save(thumbnailPath, ImageFormat.Jpeg);
                }
            }
            finally
            {
                image.Dispose();
                image2.Dispose();
                graphics.Dispose();
            }
        }
    }
}

