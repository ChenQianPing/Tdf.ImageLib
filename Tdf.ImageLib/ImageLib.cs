using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.ImageLib
{
    public class ImageLib
    {
        /// <summary>
        /// Lib #1.在图片上画框
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Bitmap DrawRectangleInPicture(Bitmap bmp, int x, int y, int width, int height)
        {
            if (bmp == null) return null;

            var g = Graphics.FromImage(bmp);

            g.DrawRectangle(Pens.Red, new Rectangle(x, y, width, height));

            g.Dispose();

            return bmp;
        }

        /// <summary>
        /// Lib #2.在图片上画椭圆
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Bitmap DrawRoundInPicture(Bitmap bmp, int x, int y, int width, int height)
        {
            if (bmp == null) return null;

            var g = Graphics.FromImage(bmp);

            g.DrawEllipse(Pens.Red, new Rectangle(x, y, width, height));

            g.Dispose();

            return bmp;
        }

        /// <summary>
        /// Lib #3.设置文字
        /// </summary>
        /// <param name="b"></param>
        /// <param name="txt"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Bitmap KiSetText(Bitmap b, string txt, int x, int y)
        {
            if (b == null)
            {
                return null;
            }

            var g = Graphics.FromImage(b);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            // 作为演示，我们用Arial字体，大小为32，红色。
            var fm = new FontFamily("Arial");
            var font = new Font(fm, 32, FontStyle.Regular, GraphicsUnit.Pixel);
            var sb = new SolidBrush(Color.Red);

            g.DrawString(txt, font, sb, new PointF(x, y));
            g.Dispose();

            return b;
        }

        /// <summary>
        /// Lib #4.剪裁，用GDI+
        /// </summary>
        /// <param name="b"></param>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="iWidth"></param>
        /// <param name="iHeight"></param>
        /// <returns></returns>
        public static Bitmap KiCut(Bitmap b, int startX, int startY, int iWidth, int iHeight)
        {
            if (b == null)
            {
                return null;
            }

            var w = b.Width;
            var h = b.Height;

            if (startX >= w || startY >= h)
            {
                return null;
            }

            if (startX + iWidth > w)
            {
                iWidth = w - startX;
            }

            if (startY + iHeight > h)
            {
                iHeight = h - startY;
            }

            try
            {
                var bmpOut = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb);

                var g = Graphics.FromImage(bmpOut);
                g.DrawImage(b, new Rectangle(0, 0, iWidth, iHeight), new Rectangle(startX, startY, iWidth, iHeight), GraphicsUnit.Pixel);
                g.Dispose();

                return bmpOut;
            }
            catch
            {
                return null;
            }
        }



        #region Methods
        /// <summary>
        /// 在图片上画框
        /// </summary>
        /// <param name="bmp">原始图</param>
        /// <param name="p0">起始点</param>
        /// <param name="p1">终止点</param>
        /// <param name="rectColor">矩形框颜色</param>
        /// <param name="lineWidth">矩形框边界</param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static Bitmap DrawRectangleInPicture(Bitmap bmp, Point p0, Point p1, Color rectColor, int lineWidth,
            DashStyle ds)
        {
            if (bmp == null) return null;


            var g = Graphics.FromImage(bmp);

            Brush brush = new SolidBrush(rectColor);
            var pen = new Pen(brush, lineWidth) { DashStyle = ds };

            g.DrawRectangle(pen, new Rectangle(p0.X, p0.Y, Math.Abs(p0.X - p1.X), Math.Abs(p0.Y - p1.Y)));

            g.Dispose();

            return bmp;


        }

        /// <summary>
        /// 在图片上画椭圆
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="rectColor"></param>
        /// <param name="lineWidth"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static Bitmap DrawRoundInPicture(Bitmap bmp, Point p0, Point p1, Color rectColor, int lineWidth,
            DashStyle ds)
        {
            if (bmp == null) return null;

            var g = Graphics.FromImage(bmp);

            Brush brush = new SolidBrush(rectColor);
            var pen = new Pen(brush, lineWidth) {DashStyle = ds};

            g.DrawEllipse(pen, new Rectangle(p0.X, p0.Y, Math.Abs(p0.X - p1.X), Math.Abs(p0.Y - p1.Y)));

            g.Dispose();

            return bmp;
        }

        public static Bitmap KiSetText(Bitmap b, string txt, string textFont, int textSize, Color c, int x, int y)
        {
            if (b == null)
            {
                return null;
            }

            var g = Graphics.FromImage(b);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            var fm = new FontFamily(textFont);
            var font = new Font(fm, textSize, FontStyle.Regular, GraphicsUnit.Pixel);
            var sb = new SolidBrush(c);

            g.DrawString(txt, font, sb, new PointF(x, y));
            g.Dispose();

            return b;
        }

        public static Bitmap KiSetText(Bitmap b, string txt, Font font, Color c, int x, int y)
        {
            if (b == null)
            {
                return null;
            }

            var g = Graphics.FromImage(b);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            var sb = new SolidBrush(c);

            g.DrawString(txt, font, sb, new PointF(x, y));
            g.Dispose();

            return b;
        }

        #endregion
    }
}
