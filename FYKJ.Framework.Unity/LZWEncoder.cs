using System;
using System.IO;

namespace FYKJ.Framework.Utility
{
    public class LZWEncoder
    {
        private int a_count;
        private readonly byte[] accum = new byte[0x100];
        private static readonly int BITS = 12;
        private bool clear_flg;
        private int ClearCode;
        private readonly int[] codetab = new int[HSIZE];
        private int cur_accum;
        private int cur_bits;
        private int curPixel;
        private static readonly int EOF = -1;
        private int EOFCode;
        private int free_ent;
        private int g_init_bits;
        private readonly int hsize = HSIZE;
        private static readonly int HSIZE = 0x138b;
        private readonly int[] htab = new int[HSIZE];
        private readonly int imgH;
        private readonly int imgW;
        private readonly int initCodeSize;
        private readonly int[] masks = { 
            0, 1, 3, 7, 15, 0x1f, 0x3f, 0x7f, 0xff, 0x1ff, 0x3ff, 0x7ff, 0xfff, 0x1fff, 0x3fff, 0x7fff, 
            0xffff
         };
        private readonly int maxbits = BITS;
        private int maxcode;
        private readonly int maxmaxcode = (1 << BITS);
        private int n_bits;
        private readonly byte[] pixAry;
        private int remaining;

        public LZWEncoder(int width, int height, byte[] pixels, int color_depth)
        {
            imgW = width;
            imgH = height;
            pixAry = pixels;
            initCodeSize = Math.Max(2, color_depth);
        }

        private void Add(byte c, Stream outs)
        {
            accum[a_count++] = c;
            if (a_count >= 0xfe)
            {
                Flush(outs);
            }
        }

        private void ClearTable(Stream outs)
        {
            ResetCodeTable(hsize);
            free_ent = ClearCode + 2;
            clear_flg = true;
            Output(ClearCode, outs);
        }

        private void Compress(int init_bits, Stream outs)
        {
            int num;
            g_init_bits = init_bits;
            clear_flg = false;
            n_bits = g_init_bits;
            maxcode = MaxCode(n_bits);
            ClearCode = 1 << (init_bits - 1);
            EOFCode = ClearCode + 1;
            free_ent = ClearCode + 2;
            a_count = 0;
            int code = NextPixel();
            int num3 = 0;
            int hsize = this.hsize;
            while (hsize < 0x10000)
            {
                num3++;
                hsize *= 2;
            }
            num3 = 8 - num3;
            int num5 = this.hsize;
            ResetCodeTable(num5);
            Output(ClearCode, outs);
            while ((num = NextPixel()) != EOF)
            {
                hsize = (num << maxbits) + code;
                int index = (num << num3) ^ code;
                if (htab[index] == hsize)
                {
                    code = codetab[index];
                }
                else
                {
                    if (htab[index] >= 0)
                    {
                        int num7 = num5 - index;
                        if (index == 0)
                        {
                            num7 = 1;
                        }
                        do
                        {
                            index -= num7;
                            if (index < 0)
                            {
                                index += num5;
                            }
                            if (htab[index] == hsize)
                            {
                                code = codetab[index];
                            }
                        }
                        while (htab[index] >= 0);
                    }
                    Output(code, outs);
                    code = num;
                    if (free_ent < maxmaxcode)
                    {
                        codetab[index] = free_ent++;
                        htab[index] = hsize;
                    }
                    else
                    {
                        ClearTable(outs);
                    }
                }
            }
            Output(code, outs);
            Output(EOFCode, outs);
        }

        public void Encode(Stream os)
        {
            os.WriteByte(Convert.ToByte(initCodeSize));
            remaining = imgW * imgH;
            curPixel = 0;
            Compress(initCodeSize + 1, os);
            os.WriteByte(0);
        }

        private void Flush(Stream outs)
        {
            if (a_count > 0)
            {
                outs.WriteByte(Convert.ToByte(a_count));
                outs.Write(accum, 0, a_count);
                a_count = 0;
            }
        }

        private int MaxCode(int n_bit)
        {
            return ((1 << n_bit) - 1);
        }

        private int NextPixel()
        {
            if (remaining == 0)
            {
                return EOF;
            }
            remaining--;
            int num = curPixel + 1;
            if (num < pixAry.GetUpperBound(0))
            {
                byte num2 = pixAry[curPixel++];
                return (num2 & 0xff);
            }
            return 0xff;
        }

        private void Output(int code, Stream outs)
        {
            cur_accum &= masks[cur_bits];
            if (cur_bits > 0)
            {
                cur_accum |= code << cur_bits;
            }
            else
            {
                cur_accum = code;
            }
            cur_bits += n_bits;
            while (cur_bits >= 8)
            {
                Add((byte) (cur_accum & 0xff), outs);
                cur_accum = cur_accum >> 8;
                cur_bits -= 8;
            }
            if ((free_ent > maxcode) || clear_flg)
            {
                if (clear_flg)
                {
                    maxcode = MaxCode(n_bits = g_init_bits);
                    clear_flg = false;
                }
                else
                {
                    n_bits++;
                    if (n_bits == maxbits)
                    {
                        maxcode = maxmaxcode;
                    }
                    else
                    {
                        maxcode = MaxCode(n_bits);
                    }
                }
            }
            if (code == EOFCode)
            {
                while (cur_bits > 0)
                {
                    Add((byte) (cur_accum & 0xff), outs);
                    cur_accum = cur_accum >> 8;
                    cur_bits -= 8;
                }
                Flush(outs);
            }
        }

        private void ResetCodeTable(int size)
        {
            for (int i = 0; i < size; i++)
            {
                htab[i] = -1;
            }
        }
    }
}

