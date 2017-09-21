using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

namespace FYKJ.Framework.Utility
{
    public class ValidateCode_Style3 : ValidateCodeType
    {
        private Color backgroundColor = Color.White;
        private bool chaos = true;
        private Color chaosColor = Color.FromArgb(170, 170, 0x33);
        private int chaosMode = 1;
        private int contortRange = 4;
        private Color drawColor = Color.FromArgb(50, 0x99, 0xcc);
        private bool fontTextRenderingHint;
        private int imageHeight = 30;
        private int padding = 1;
        private const double PI = 3.1415926535897931;
        private const double PI2 = 6.2831853071795862;
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
            for (int i = 0; i < 3; i++)
            {
                SplitCode(validataCode);
                ImageBmp(out bitmap, validataCode);
                bitmap.Save(stream, ImageFormat.Png);
                encoder.AddFrame(Image.FromStream(stream));
                stream = new MemoryStream();
                bitmap.Dispose();
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
            Brush brush = new SolidBrush(drawColor);
            int maxValue = Math.Max((ImageHeight - validataCodeSize) - 5, 0);
            Random random = new Random();
            for (int i = 0; i < validataCodeLength; i++)
            {
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
            Pen pen = new Pen(DrawColor, 1f);
            Random random = new Random();
            Point[] pointArray = new Point[2];
            if (Chaos)
            {
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

        private void SplitCode(string srcCode)
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
        }

        public Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            Bitmap image = new Bitmap(srcBmp.Width, srcBmp.Height);
            Graphics graphics = Graphics.FromImage(image);
            graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, image.Width, image.Height);
            graphics.Dispose();
            double num = bXDir ? image.Height : image.Width;
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    double a = 0.0;
                    a = bXDir ? ((6.2831853071795862 * j) / num) : ((6.2831853071795862 * i) / num);
                    a += dPhase;
                    double num5 = Math.Sin(a);
                    int x = 0;
                    int y = 0;
                    x = bXDir ? (i + ((int) (num5 * dMultValue))) : i;
                    y = bXDir ? j : (j + ((int) (num5 * dMultValue)));
                    Color pixel = srcBmp.GetPixel(i, j);
                    if (((x >= 0) && (x < image.Width)) && ((y >= 0) && (y < image.Height)))
                    {
                        image.SetPixel(x, y, pixel);
                    }
                }
            }
            return image;
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

        public int ContortRange
        {
            get
            {
                return contortRange;
            }
            set
            {
                contortRange = value;
            }
        }

        public Color DrawColor
        {
            get
            {
                return drawColor;
            }
            set
            {
                drawColor = value;
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
                return "GIF颠簸动画";
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

