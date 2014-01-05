﻿using System;
using System.Collections.Generic;
using System.Text;

using TPixel = System.Byte;
using TImage = Geb.Image.ImageU8;

namespace Geb.Image
{
    public partial class ImageU8
    {
        public unsafe ImageU8 ApplySobelX()
        {
            int extend = 1;
            TImage maskImage = new TImage(Width + extend * 2, Height + extend * 2);
            maskImage.Fill(0);//这里效率不高。原本只需要填充四周扩大的部分即可

            maskImage.CopyFrom(this, new System.Drawing.Point(0, 0), new System.Drawing.Rectangle(0, 0, this.Width, this.Height), new System.Drawing.Point(extend, extend));

            int width = this.Width;
            int height = this.Height;
            TPixel* start = this.Start;

            // 复制边界像素
            TPixel* dstStart = maskImage.Start + extend;
            int extendWidth = this.Width + extend * 2;
            int extendHeight = this.Height + extend * 2;

            // 复制上方的像素
            for (int y = 0; y < extend; y++)
            {
                TPixel* dstP = dstStart + y * extendWidth;
                TPixel* srcStart = start;
                TPixel* srcEnd = srcStart + width;

                while (srcStart != srcEnd)
                {
                    *dstP = *srcStart;
                    srcStart++;
                    dstP++;
                }
            }

            // 复制下方的像素
            for (int y = height + extend; y < extendHeight; y++)
            {
                TPixel* dstP = dstStart + y * extendWidth;
                TPixel* srcStart = start + (height - 1) * width;
                TPixel* srcEnd = srcStart + width;

                while (srcStart != srcEnd)
                {
                    *dstP = *srcStart;
                    srcStart++;
                    dstP++;
                }
            }

            // 复制左右两侧的像素
            TPixel* dstLine = maskImage.Start + extendWidth * extend;
            TPixel* srcLine = start;
            TPixel p = default(TPixel);
            for (int y = extend; y < height + extend; y++)
            {
                for (int x = 0; x < extend; x++)
                {
                    p = srcLine[0];
                    dstLine[x] = p;
                }

                p = srcLine[width - 1];
                for (int x = width + extend; x < extendWidth; x++)
                {
                    dstLine[x] = p;
                }
                dstLine += extendWidth;
                srcLine += width;
            }

            // 复制四个角落的像素

            // 左上
            p = start[0];
            for (int y = 0; y < extend; y++)
            {
                for (int x = 0; x < extend; x++)
                {
                    maskImage[y, x] = p;
                }
            }

            // 右上
            p = start[width - 1];
            for (int y = 0; y < extend; y++)
            {
                for (int x = width + extend; x < extendWidth; x++)
                {
                    maskImage[y, x] = p;
                }
            }

            // 左下
            p = start[(height - 1) * width];
            for (int y = height + extend; y < extendHeight; y++)
            {
                for (int x = 0; x < extend; x++)
                {
                    maskImage[y, x] = p;
                }
            }

            // 右下
            p = start[height * width - 1];
            for (int y = height + extend; y < extendHeight; y++)
            {
                for (int x = width + extend; x < extendWidth; x++)
                {
                    maskImage[y, x] = p;
                }
            }

            //if (scale == 1)
            //{
            //    for (int h = 0; h < height; h++)
            //    {
            //        for (int w = 0; w < width; w++)
            //        {
            //            TCache val = 0;
            //            for (int kw = 0; kw < kernelWidth; kw++)
            //            {
            //                for (int kh = 0; kh < kernelHeight; kh++)
            //                {
            //                    val += maskImage[h + kh, w + kw] * kernel[kh, kw];
            //                }
            //            }
            //            start[h * width + w] = (TPixel)(val + valShift);
            //        }
            //    }
            //}
            //else
            //{
            //    double factor = 1.0 / scale;
            //    for (int h = 0; h < height; h++)
            //    {
            //        for (int w = 0; w < width; w++)
            //        {
            //            TCache val = 0;
            //            for (int kw = 0; kw < kernelWidth; kw++)
            //            {
            //                for (int kh = 0; kh < kernelHeight; kh++)
            //                {
            //                    val += maskImage[h + kh, w + kw] * kernel[kh, kw];
            //                }
            //            }
            //            start[h * width + w] = (TPixel)(val * factor + valShift);
            //        }
            //    }
            //}
            maskImage.Dispose();
            return this;
        }

        public ImageU8 ApplySobelY()
        {
            return this;
        }

        public ImageU8 ApplySobel()
        {
            return this;
        }
    }
}