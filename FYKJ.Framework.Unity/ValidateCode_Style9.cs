using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

namespace FYKJ.Framework.Utility.ValidateCode
{
    public class ValidateCode_Style9 : ValidateCodeType
    {
        private Color backgroundColor = Color.White;
        private Color chaosColor = Color.FromArgb(170, 170, 0x33);
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
            Random random = new Random();
            if (fontTextRenderingHint)
            {
                graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
            }
            else
            {
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            }
            Font font = new Font(validateCodeFont, validataCodeSize, FontStyle.Regular);
            int maxValue = Math.Max((ImageHeight - validataCodeSize) - 5, 0);
            for (int i = 0; i < validataCodeLength; i++)
            {
                Color color = DrawColors[random.Next(DrawColors.Length)];
                Brush brush = new SolidBrush(color);
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
            Pen pen = new Pen(ChaosColor, 1f);
            for (int i = 0; i < (validataCodeLength * 10); i++)
            {
                int x = random.Next(bitmap.Width);
                int y = random.Next(bitmap.Height);
                graphics.DrawRectangle(pen, x, y, 1, 1);
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
            int width = (int) ((validataCodeLength * validataCodeSize) * 1.2);
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
                return "噪点干扰(彩色)";
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

