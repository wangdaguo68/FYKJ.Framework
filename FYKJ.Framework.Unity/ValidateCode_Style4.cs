using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

namespace FYKJ.Framework.Utility
{
    public class ValidateCode_Style4 : ValidateCodeType
    {
        private Color backgroundColor = Color.White;
        private bool chaos = true;
        private Color chaosColor = Color.FromArgb(170, 170, 0x33);
        private Color drawColor = Color.FromArgb(50, 0x99, 0xcc);
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
            ImageBmp(out bitmap, validataCode);
            bitmap.Save(stream, ImageFormat.Png);
            bitmap.Dispose();
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
            new Random();
            Point[] pointArray = new Point[2];
            Random random = new Random();
            if (Chaos)
            {
                pen = new Pen(ChaosColor, 1f);
                for (int i = 0; i < (validataCodeLength * 2); i++)
                {
                    pointArray[0] = new Point(random.Next(bitmap.Width), random.Next(bitmap.Height));
                    pointArray[1] = new Point(random.Next(bitmap.Width), random.Next(bitmap.Height));
                    graphics.DrawLine(pen, pointArray[0], pointArray[1]);
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
                return "线条干扰(蓝色)";
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

