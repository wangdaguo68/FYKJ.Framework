﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

namespace FYKJ.Framework.Utility.ValidateCode
{
    public class ValidateCode_Style14 : ValidateCodeType
    {
        private Color backgroundColor = Color.White;
        private Color chaosColor = Color.FromArgb(170, 170, 0x33);
        private Color[] drawColors = { Color.FromArgb(0x6b, 0x42, 0x26), Color.FromArgb(0x4f, 0x2f, 0x4f), Color.FromArgb(50, 0x99, 0xcc), Color.FromArgb(0xcd, 0x7f, 50), Color.FromArgb(0x23, 0x23, 0x8e), Color.FromArgb(0x70, 0xdb, 0x93), Color.Red, Color.FromArgb(0xbc, 0x8f, 0x8e) };
        private bool fontTextRenderingHint;
        private int imageHeight = 30;
        private int padding = 1;
        private int validataCodeLength = 5;
        private int validataCodeSize = 0x10;
        private string validateCodeFont = "Arial";

        public override byte[] CreateImage(out string resultCode)
        {
            string str;
            Bitmap bitmap;
            string formatString = "1,2,3,4,5,6,7,8,9,0";
            GetRandom(formatString, out str, out resultCode);
            MemoryStream stream = new MemoryStream();
            ImageBmp(out bitmap, str);
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
            int maxValue = Math.Max((ImageHeight - validataCodeSize) - 5, 0);
            Random random = new Random();
            for (int i = 0; i < validataCodeLength; i++)
            {
                Brush brush = new SolidBrush(drawColors[random.Next(drawColors.Length)]);
                int[] numArray = { (i * validataCodeSize) + (i * 5), random.Next(maxValue) };
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

        private static void GetRandom(string formatString, out string codeString, out string resultCode)
        {
            Random random = new Random();
            string s = string.Empty;
            string str2 = string.Empty;
            string[] strArray = formatString.Split(',');
            for (int i = 0; i < 2; i++)
            {
                int index = random.Next(strArray.Length);
                if ((i == 0) && (strArray[index] == "0"))
                {
                    i--;
                }
                else
                {
                    s = s + strArray[index];
                }
            }
            for (int j = 0; j < 2; j++)
            {
                int num4 = random.Next(strArray.Length);
                if ((j == 0) && (strArray[num4] == "0"))
                {
                    j--;
                }
                else
                {
                    str2 = str2 + strArray[num4];
                }
            }
            if ((random.Next(100) % 2) == 1)
            {
                codeString = s + "+" + str2;
                resultCode = (int.Parse(s) + int.Parse(str2)).ToString();
            }
            else
            {
                if (int.Parse(s) > int.Parse(str2))
                {
                    codeString = s + "─" + str2;
                }
                else
                {
                    codeString = str2 + "─" + s;
                }
                resultCode = Math.Abs(int.Parse(s) - int.Parse(str2)).ToString();
            }
        }

        private void ImageBmp(out Bitmap bitMap, string validataCode)
        {
            int width = (int) (((validataCodeLength * validataCodeSize) * 1.3) + 10.0);
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
                return "2年级算术(彩色)";
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

        public override string Tip
        {
            get
            {
                return "请输入计算结果";
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

