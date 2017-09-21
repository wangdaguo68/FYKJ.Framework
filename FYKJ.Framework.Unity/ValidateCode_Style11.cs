using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

namespace FYKJ.Framework.Utility.ValidateCode
{
    public class ValidateCode_Style11 : ValidateCodeType
    {
        private Color backgroundColor = Color.White;
        private bool chaos = true;
        private Color chaosColor = Color.FromArgb(170, 170, 0x33);
        private int chaosMode = 1;
        private readonly List<Color> colors = new List<Color>();
        private Color[] drawColors = { Color.FromArgb(0x6b, 0x42, 0x26), Color.FromArgb(0x4f, 0x2f, 0x4f), Color.FromArgb(50, 0x99, 0xcc), Color.FromArgb(0xcd, 0x7f, 50), Color.FromArgb(0x23, 0x23, 0x8e), Color.FromArgb(0x70, 0xdb, 0x93), Color.Red, Color.FromArgb(0xbc, 0x8f, 0x8e) };
        private bool fontTextRenderingHint;
        private int imageHeight = 30;
        private int padding = 1;
        private int validataCodeLength = 4;
        private int validataCodeSize = 0x10;
        private string validateCodeFont = "Arial";

        public override byte[] CreateImage(out string validataCode)
        {
            Bitmap bitmap;
            string formatString = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            GetRandom(formatString, ValidataCodeLength, out validataCode);
            MemoryStream stream = new MemoryStream();
            AnimatedGifEncoder encoder = new AnimatedGifEncoder();
            encoder.Start();
            encoder.SetDelay(1);
            encoder.SetRepeat(0);
            Random random = new Random();
            for (int i = 0; i < validataCode.Length; i++)
            {
                colors.Add(DrawColors[random.Next(DrawColors.Length)]);
            }
            for (int j = 0; j < 3; j++)
            {
                string[] strArray = SplitCode(validataCode);
                for (int k = 0; k < 2; k++)
                {
                    if (k == 0)
                    {
                        ImageBmp(out bitmap, strArray[0]);
                    }
                    else
                    {
                        ImageBmp(out bitmap, strArray[1]);
                    }
                    bitmap.Save(stream, ImageFormat.Png);
                    encoder.AddFrame(Image.FromStream(stream));
                    stream = new MemoryStream();
                    bitmap.Dispose();
                }
            }
            encoder.OutPut(ref stream);
            bitmap = null;
            stream.Close();
            stream.Dispose();
            return stream.GetBuffer();
        }

        private void CreateImageBmp(ref Bitmap bitMap, string validateCode)
        {
            Graphics graphics = Graphics.FromImage(bitMap);
            if (fontTextRenderingHint)
            {
                graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
            }
            else
            {
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            }
            Font font = new Font(validateCodeFont, validataCodeSize, FontStyle.Regular);
            int maxValue = Math.Max((ImageHeight - validataCodeSize) - 4, 0);
            Random random = new Random();
            for (int i = 0; i < validataCodeLength; i++)
            {
                Brush brush = new SolidBrush(colors[i]);
                int[] numArray = { ((i * validataCodeSize) + random.Next(1)) + 3, random.Next(maxValue) - 4 };
                Point point = new Point(numArray[0], numArray[1]);
                graphics.DrawString(validateCode[i].ToString(), font, brush, point);
            }
            graphics.Dispose();
        }

        private void DisposeImageBmp(ref Bitmap bitmap)
        {
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            Random random = new Random();
            Point[] pointArray = new Point[2];
            if (Chaos)
            {
                Pen pen;
                switch (chaosMode)
                {
                    case 1:
                        pen = new Pen(ChaosColor, 1f);
                        for (int i = 0; i < (validataCodeLength * 10); i++)
                        {
                            int x = random.Next(bitmap.Width);
                            int y = random.Next(bitmap.Height);
                            graphics.DrawRectangle(pen, x, y, 1, 1);
                        }
                        break;

                    case 2:
                        pen = new Pen(ChaosColor, validataCodeLength * 4);
                        for (int j = 0; j < (validataCodeLength * 10); j++)
                        {
                            int num5 = random.Next(bitmap.Width);
                            int num6 = random.Next(bitmap.Height);
                            graphics.DrawRectangle(pen, num5, num6, 1, 1);
                        }
                        break;

                    case 3:
                        pen = new Pen(ChaosColor, 1f);
                        for (int k = 0; k < (validataCodeLength * 2); k++)
                        {
                            pointArray[0] = new Point(random.Next(bitmap.Width), random.Next(bitmap.Height));
                            pointArray[1] = new Point(random.Next(bitmap.Width), random.Next(bitmap.Height));
                            graphics.DrawLine(pen, pointArray[0], pointArray[1]);
                        }
                        break;

                    default:
                        pen = new Pen(ChaosColor, 1f);
                        for (int m = 0; m < (validataCodeLength * 10); m++)
                        {
                            int num9 = random.Next(bitmap.Width);
                            int num10 = random.Next(bitmap.Height);
                            graphics.DrawRectangle(pen, num9, num10, 1, 1);
                        }
                        break;
                }
            }
            graphics.Dispose();
        }

        private static void GetRandom(string formatString, int len, out string codeString)
        {
            codeString = string.Empty;
            string[] strArray = formatString.Split(',');
            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                int index = random.Next(0x186a0) % strArray.Length;
                codeString = codeString + strArray[index];
            }
        }

        private void ImageBmp(out Bitmap bitMap, string validataCode)
        {
            int width = (int) (((validataCodeLength * validataCodeSize) * 1.3) + 4.0);
            bitMap = new Bitmap(width, ImageHeight);
            DisposeImageBmp(ref bitMap);
            CreateImageBmp(ref bitMap, validataCode);
        }

        private string[] SplitCode(string srcCode)
        {
            Random random = new Random();
            string[] strArray = new string[2];
            foreach (char ch in srcCode)
            {
                if ((random.Next(Math.Abs((int) DateTime.Now.Ticks)) % 2) == 0)
                {
                    string[] strArray2;
                    string[] strArray3;
                    (strArray2 = strArray)[0] = strArray2[0] + ch.ToString();
                    (strArray3 = strArray)[1] = strArray3[1] + " ";
                }
                else
                {
                    string[] strArray4;
                    string[] strArray5;
                    (strArray4 = strArray)[1] = strArray4[1] + ch.ToString();
                    (strArray5 = strArray)[0] = strArray5[0] + " ";
                }
            }
            return strArray;
        }

        public Color BackgroundColor
        {
            get
            {
                return backgroundColor;
            }
            set
            {
                backgroundColor = value;
            }
        }

        public bool Chaos
        {
            get
            {
                return chaos;
            }
            set
            {
                chaos = value;
            }
        }

        public Color ChaosColor
        {
            get
            {
                return chaosColor;
            }
            set
            {
                chaosColor = value;
            }
        }

        public int ChaosMode
        {
            get
            {
                return chaosMode;
            }
            set
            {
                chaosMode = value;
            }
        }

        public Color[] DrawColors
        {
            get
            {
                return drawColors;
            }
            set
            {
                drawColors = value;
            }
        }

        private bool FontTextRenderingHint
        {
            get
            {
                return fontTextRenderingHint;
            }
            set
            {
                fontTextRenderingHint = value;
            }
        }

        public int ImageHeight
        {
            get
            {
                return imageHeight;
            }
            set
            {
                imageHeight = value;
            }
        }

        public override string Name
        {
            get
            {
                return "GIF闪烁动画(彩色)";
            }
        }

        public int Padding
        {
            get
            {
                return padding;
            }
            set
            {
                padding = value;
            }
        }

        public int ValidataCodeLength
        {
            get
            {
                return validataCodeLength;
            }
            set
            {
                validataCodeLength = value;
            }
        }

        public int ValidataCodeSize
        {
            get
            {
                return validataCodeSize;
            }
            set
            {
                validataCodeSize = value;
            }
        }

        public string ValidateCodeFont
        {
            get
            {
                return validateCodeFont;
            }
            set
            {
                validateCodeFont = value;
            }
        }
    }
}

