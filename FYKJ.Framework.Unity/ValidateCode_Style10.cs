using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

namespace FYKJ.Framework.Utility.ValidateCode
{
    public class ValidateCode_Style10 : ValidateCodeType
    {
        public ValidateCode_Style10(bool fontTextRenderingHint)
        {
            FontTextRenderingHint = fontTextRenderingHint;
        }

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
            graphics.TextRenderingHint = FontTextRenderingHint ? TextRenderingHint.SingleBitPerPixel : TextRenderingHint.AntiAlias;
            Font font = new Font(ValidateCodeFont, ValidataCodeSize, FontStyle.Regular);
            int maxValue = Math.Max((ImageHeight - ValidataCodeSize) - 5, 0);
            for (int i = 0; i < ValidataCodeLength; i++)
            {
                Color color = DrawColors[random.Next(DrawColors.Length)];
                Brush brush = new SolidBrush(color);
                int[] numArray = { ((i * ValidataCodeSize) + random.Next(1)) + 3, random.Next(maxValue) - 4 };
                Point point = new Point(numArray[0], numArray[1]);
                graphics.DrawString(validateCode[i].ToString(), font, brush, point);
            }
            graphics.Dispose();
        }

        private void DisposeImageBmp(ref Bitmap bitmap)
        {
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            new Random();
            Point[] pointArray = new Point[2];
            Random random = new Random();
            for (int i = 0; i < (ValidataCodeLength * 2); i++)
            {
                Pen pen = new Pen(DrawColors[random.Next(DrawColors.Length)], 1f);
                pointArray[0] = new Point(random.Next(bitmap.Width), random.Next(bitmap.Height));
                pointArray[1] = new Point(random.Next(bitmap.Width), random.Next(bitmap.Height));
                graphics.DrawLine(pen, pointArray[0], pointArray[1]);
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
            int width = (int) ((ValidataCodeLength * ValidataCodeSize) * 1.2);
            bitMap = new Bitmap(width, ImageHeight);
            DisposeImageBmp(ref bitMap);
            CreateImageBmp(ref bitMap, validataCode);
        }

        public Color BackgroundColor { get; set; } = Color.White;

        public Color ChaosColor { get; set; } = Color.FromArgb(170, 170, 0x33);

        public Color[] DrawColors { get; set; } = { Color.FromArgb(0x6b, 0x42, 0x26), Color.FromArgb(0x4f, 0x2f, 0x4f), Color.FromArgb(50, 0x99, 0xcc), Color.FromArgb(0xcd, 0x7f, 50), Color.FromArgb(0x23, 0x23, 0x8e), Color.FromArgb(0x70, 0xdb, 0x93), Color.Red, Color.FromArgb(0xbc, 0x8f, 0x8e) };

        private bool FontTextRenderingHint { get; }

        public int ImageHeight { get; set; } = 30;

        public override string Name => "线条干扰(彩色)";

        public int Padding { get; set; } = 1;

        public int ValidataCodeLength { get; set; } = 4;

        public int ValidataCodeSize { get; set; } = 0x10;

        public string ValidateCodeFont { get; set; } = "Arial";
    }
}

