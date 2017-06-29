using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.ImageLibHelper
{
    public class ImageHelper
    {
        #region 在图片上画框
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
        #endregion

        #region 在图片上画椭圆
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
        #endregion

        #region 设置文字
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
        #endregion

        #region 剪裁，用GDI+
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
        #endregion

        #region 图像灰度化
        /// <summary>
        /// 图像灰度化
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static Bitmap ToGray(Bitmap bmp)
        {
            for (var i = 0; i < bmp.Width; i++)
            {
                for (var j = 0; j < bmp.Height; j++)
                {
                    // 获取该点的像素的RGB的颜色
                    var color = bmp.GetPixel(i, j);

                    // 利用公式计算灰度值
                    var gray = (int)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
                    var newColor = Color.FromArgb(gray, gray, gray);
                    bmp.SetPixel(i, j, newColor);
                }
            }
            return bmp;


            /*
             * 图像的灰度化处理可用两种方法来实现。
             * 第一种方法使求出每个像素点的R、G、B三个分量的平均值，然后将这个平均值赋予给这个像素的三个分量。
             * 第二种方法是根据YUV的颜色空间中，Y的分量的物理意义是点的亮度，由该值反映亮度等级，根据RGB和YUV颜色空间的变化关系可建立亮度Y与R、G、B三个颜色分量的对应：Y=0.3R+0.59G+0.11B，以这个亮度值表达图像的灰度值。
             */
        }
        #endregion

        #region 图像灰度反转
        /// <summary>
        /// 图像灰度反转;
        /// 把每个像素点的R、G、B三个分量的值0的设为255，255的设为0.
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static Bitmap GrayReverse(Bitmap bmp)
        {
            for (var i = 0; i < bmp.Width; i++)
            {
                for (var j = 0; j < bmp.Height; j++)
                {
                    // 获取该点的像素的RGB的颜色
                    var color = bmp.GetPixel(i, j);
                    var newColor = Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B);
                    bmp.SetPixel(i, j, newColor);
                }
            }
            return bmp;
        }
        #endregion

        #region 灰度图像二值化1
        /// <summary>
        /// 灰度图像二值化1：取图片的平均灰度作为阈值，低于该值的全都为0，高于该值的全都为255
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static Bitmap ConvertTo1Bpp1(Bitmap bmp)
        {
            var average = 0;
            for (var i = 0; i < bmp.Width; i++)
            {
                for (var j = 0; j < bmp.Height; j++)
                {
                    var color = bmp.GetPixel(i, j);
                    average += color.B;
                }
            }
            average = (int)average / (bmp.Width * bmp.Height);

            for (var i = 0; i < bmp.Width; i++)
            {
                for (var j = 0; j < bmp.Height; j++)
                {
                    // 获取该点的像素的RGB的颜色
                    var color = bmp.GetPixel(i, j);
                    var value = 255 - color.B;
                    var newColor = value > average ? Color.FromArgb(0, 0, 0) : Color.FromArgb(255, 255, 255);

                    bmp.SetPixel(i, j, newColor);
                }
            }
            return bmp;

            /*
             * 在进行了灰度化处理之后，图像中的每个象素只有一个值，那就是象素的灰度值。
             * 它的大小决定了象素的亮暗程度。为了更加便利的开展下面的图像处理操作，
             * 还需要对已经得到的灰度图像做一个二值化处理。
             * 图像的二值化就是把图像中的象素根据一定的标准分化成两种颜色。
             * 在系统中是根据象素的灰度值处理成黑白两种颜色。
             * 和灰度化相似的，图像的二值化也有很多成熟的算法。
             * 它可以采用自适应阀值法，也可以采用给定阀值法。
             */
        }
        #endregion

        #region 图像二值化2
        /// <summary>
        /// 图像二值化2
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static Bitmap ConvertTo1Bpp2(Bitmap img)
        {
            var w = img.Width;
            var h = img.Height;
            var bmp = new Bitmap(w, h, PixelFormat.Format1bppIndexed);
            var data = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, PixelFormat.Format1bppIndexed);
            for (var y = 0; y < h; y++)
            {
                var scan = new byte[(w + 7) / 8];
                for (var x = 0; x < w; x++)
                {
                    var c = img.GetPixel(x, y);
                    if (c.GetBrightness() >= 0.5) scan[x / 8] |= (byte)(0x80 >> (x % 8));
                }
                Marshal.Copy(scan, 0, (IntPtr)((int)data.Scan0 + data.Stride * y), scan.Length);
            }
            return bmp;
        }
        #endregion

        #region 图片旋转第一种方法，顺时针旋转
        /// <summary>
        /// 根据角度旋转图片
        /// </summary>
        /// <param name="img"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Image RotateImg(Image img, float angle)
        {
            // 通过Png图片设置图片透明，修改旋转图片变黑问题。  
            var width = img.Width;
            var height = img.Height;

            // 角度  
            var mtrx = new Matrix();
            var i = width / 2;
            var f = height / 2;
            mtrx.RotateAt(angle, new PointF(x: i, y: f), MatrixOrder.Append);

            // 得到旋转后的矩形  
            var path = new GraphicsPath();
            path.AddRectangle(new RectangleF(0f, 0f, width, height));
            var rct = path.GetBounds(mtrx);

            // 生成目标位图  
            var devImage = new Bitmap((int)(rct.Width), (int)(rct.Height));
            var g = Graphics.FromImage(devImage);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            // 计算偏移量  
            var offset = new Point((int)(rct.Width - width) / 2, (int)(rct.Height - height) / 2);

            // 构造图像显示区域：让图像的中心与窗口的中心点一致  
            var rect = new Rectangle(offset.X, offset.Y, (int)width, (int)height);
            var center = new Point((int)(rect.X + rect.Width / 2), (int)(rect.Y + rect.Height / 2));
            g.TranslateTransform(center.X, center.Y);
            g.RotateTransform(angle);

            // 恢复图像在水平和垂直方向的平移  
            g.TranslateTransform(-center.X, -center.Y);
            g.DrawImage(img, rect);

            // 重至绘图的所有变换  
            g.ResetTransform();
            g.Save();
            g.Dispose();
            path.Dispose();
            return devImage;
        }
        #endregion

        #region 图片旋转第二种方法，逆时针旋转
        public static Image RotateImg2(Image b, float angle)
        {
            angle = angle % 360;            // 弧度转换  
            var radian = angle * Math.PI / 180.0;
            var cos = Math.Cos(radian);
            var sin = Math.Sin(radian);

            // 原图的宽和高  
            var w = b.Width;
            var h = b.Height;
            var w2 = (int)(Math.Max(Math.Abs(w * cos - h * sin), Math.Abs(w * cos + h * sin)));
            var h2 = (int)(Math.Max(Math.Abs(w * sin - h * cos), Math.Abs(w * sin + h * cos)));

            // 目标位图  
            Image dsImage = new Bitmap(w2, h2);
            var g = System.Drawing.Graphics.FromImage(dsImage);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            // 计算偏移量  
            var offset = new Point((w2 - w) / 2, (h2 - h) / 2);
            // 构造图像显示区域：让图像的中心与窗口的中心点一致  
            var rect = new Rectangle(offset.X, offset.Y, w, h);
            var center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            g.TranslateTransform(center.X, center.Y);
            g.RotateTransform(360 - angle);

            // 恢复图像在水平和垂直方向的平移  
            g.TranslateTransform(-center.X, -center.Y);
            g.DrawImage(b, rect);

            // 重至绘图的所有变换  
            g.ResetTransform();
            g.Save();
            g.Dispose();
            return dsImage;
        }
        #endregion

        #region 获取图片的某个区域
        /// <summary>
        /// 获取图片的某个区域，优化过内存；
        /// </summary>
        /// <param name="pPath"></param>
        /// <param name="pPartWidth"></param>
        /// <param name="pPartHeight"></param>
        /// <param name="pOrigStartPointX"></param>
        /// <param name="pOrigStartPointY"></param>
        /// <returns></returns>
        public static System.Drawing.Bitmap GetImagePart(string pPath, int pPartWidth, int pPartHeight, int pOrigStartPointX, int pOrigStartPointY)
        {
            const int pPartStartPointX = 0;
            const int pPartStartPointY = 0;

            var originalImg = System.Drawing.Image.FromFile(pPath);

            var destRect = new System.Drawing.Rectangle(new System.Drawing.Point(pPartStartPointX, pPartStartPointY), new System.Drawing.Size(pPartWidth, pPartHeight)); // 目标位置
            var origRect = new System.Drawing.Rectangle(new System.Drawing.Point(pOrigStartPointX, pOrigStartPointY), new System.Drawing.Size(pPartWidth, pPartHeight)); // 原图位置（默认从原图中截取的图片大小等于目标图片的大小）

            var bitmap = new Bitmap(originalImg, originalImg.Size);
            var cloneBitmap = bitmap.Clone(origRect, PixelFormat.DontCare);
            var g = Graphics.FromImage(cloneBitmap);
            g.DrawImage(cloneBitmap, destRect);

            bitmap.Dispose();
            originalImg.Dispose();

            return cloneBitmap;
        }
        #endregion

        #region bitmap转byte[]
        /// <summary>
        /// bitmap转byte[]
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static byte[] Bitmap2Byte(Bitmap bitmap)
        {
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Jpeg);
                var data = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(data, 0, Convert.ToInt32(stream.Length));
                return data;
            }
        }
        #endregion

        #region 用白色遮住图片指定区域，批量多个区域
        public static string ImageMaskByList(string path, List<RectangleF> list)
        {
            var tempPath = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + "_mask" + Path.GetExtension(path);
            if (File.Exists(tempPath))
            {
                return tempPath;
            }

            using (var image = Image.FromFile(path))
            {
                if (IsPixelFormatIndexed(image.PixelFormat))
                {
                    using (var bmp = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb))
                    {
                        using (var g = Graphics.FromImage(bmp))
                        {
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                            g.DrawImage(image, 0, 0, image.Width, image.Height);
                            foreach (var item in list)
                            {
                                g.FillRectangle(Brushes.White, item);
                            }
                            bmp.Save(tempPath);
                            return tempPath;
                        }
                    }
                }
                else
                {
                    using (var g = Graphics.FromImage(image))
                    {
                        foreach (var item in list)
                        {
                            g.FillRectangle(Brushes.White, item);
                        }
                        image.Save(tempPath);
                        return tempPath;
                    }
                }
            }
        }
        #endregion

        #region ImageCommon
        /// <summary>
        /// 会产生graphics异常的PixelFormat
        /// </summary>
        private static readonly PixelFormat[] IndexedPixelFormats =
        {
            PixelFormat.Undefined,
            PixelFormat.DontCare,
            PixelFormat.Format16bppArgb1555,
            PixelFormat.Format1bppIndexed,
            PixelFormat.Format4bppIndexed,
            PixelFormat.Format8bppIndexed
        };

        /// <summary>
        /// 判断图片的PixelFormat 是否在 引发异常的 PixelFormat 之中
        /// </summary>
        /// <param name="imgPixelFormat">原图片的PixelFormat</param>
        /// <returns></returns>
        public static bool IsPixelFormatIndexed(PixelFormat imgPixelFormat)
        {
            foreach (var pf in IndexedPixelFormats)
            {
                if (pf.Equals(imgPixelFormat)) return true;
            }

            return false;
        }
        #endregion

        #region Obsolete Methods

        #region 用白色遮住图片指定区域，单个区域
        public static string ImageMask(string path, int x, int y, int width, int height)
        {
            var tempPath = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + "_mask" + Path.GetExtension(path);
            if (File.Exists(tempPath))
            {
                return tempPath;
            }

            using (var image = Image.FromFile(path))
            {
                if (IsPixelFormatIndexed(image.PixelFormat))
                {
                    using (var bmp = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb))
                    {
                        using (var g = Graphics.FromImage(bmp))
                        {
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                            g.DrawImage(image, 0, 0, image.Width, image.Height);
                            g.FillRectangle(Brushes.White, new RectangleF(x, y, width, height));
                            bmp.Save(tempPath);
                            return tempPath;
                        }
                    }
                }
                else
                {
                    using (var g = Graphics.FromImage(image))
                    {
                        g.FillRectangle(Brushes.White, new RectangleF(x, y, width, height));
                        image.Save(tempPath);
                        return tempPath;
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// 图片转byte[]
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static byte[] BitmapToBytes(Bitmap bitmap)
        {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                bitmap.Save(ms, bitmap.RawFormat);
                var byteImage = ms.ToArray();
                return byteImage;
            }
            finally
            {
                ms?.Close();
            }
        }

        /// <summary>
        /// byte[] 转图片
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Bitmap BytesToBitmap(byte[] bytes)
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(bytes);
                return new Bitmap((Image)new Bitmap(stream));
            }
            finally
            {
                stream?.Close();
            }
        }

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
            var pen = new Pen(brush, lineWidth) { DashStyle = ds };

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
