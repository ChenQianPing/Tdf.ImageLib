using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tdf.ImageLibHelper;

namespace Tdf.ImageLib
{
    public class TestImageLibHelper
    {
        public void TestMethod1()
        {
            Console.Write("Start ...");

            #region Mock Data
            var pPath = @"E:\10001003\1d.jpg";
            var pSavedPath = @"E:\10001003\";

            var f1 = new Frame
            {
                X = 75,
                Y = 430,
                Width = 970,
                Height = 277
            };

            var f2 = new Frame
            {
                X = 608,
                Y = 499,
                Width = 388,
                Height = 147
            };

            var f3 = new Frame
            {
                X = 81,
                Y = 977,
                Width = 899,
                Height = 237
            };

            var f4 = new Frame
            {
                X = 55,
                Y = 1247,
                Width = 1032,
                Height = 873
            };

            /* var lstFrames = new List<Frame> {f1, f2, f3, f4}; */
            var lstFrames = new List<Frame> { f3 };
            #endregion

            pPath = @"E:\10001003\liubei_gray.jpg";

            var bmp = new Bitmap(Image.FromFile(pPath));

            /*
            var result = ImageLib.ToGray(bmp);
            result.Save(pSavedPath + "\\liubei_gray.jpg", ImageFormat.Jpeg);
            */

            var result = ImageHelper.ConvertTo1Bpp2(bmp);
            result.Save(pSavedPath + "\\liubei_gray_1bpp2.jpg", ImageFormat.Jpeg);

            /*
            foreach (var frame in lstFrames)
            {
                var rectangleImg = ImageLib.DrawRectangleInPicture(bmp, frame.X, frame.Y, frame.Width, frame.Height);
                rectangleImg.Save(pSavedPath + "\\rectangle.jpg", ImageFormat.Jpeg);

                var roundImg = ImageLib.DrawRoundInPicture(bmp, frame.X, frame.Y, frame.Width, frame.Height);
                roundImg.Save(pSavedPath + "\\round.jpg", ImageFormat.Jpeg);

                var textImg = ImageLib.KiSetText(bmp, "设置文字", frame.X, frame.Y);
                textImg.Save(pSavedPath + "\\text.jpg", ImageFormat.Jpeg);

                var cutImg = ImageLib.KiCut(bmp, frame.X, frame.Y, frame.Width, frame.Height);
                cutImg.Save(pSavedPath + "\\cut.jpg", ImageFormat.Jpeg);
            }
            */




            Console.Write("End...");

            Console.ReadLine();
        }

        public void TestMethod2()
        {
            const string pPath = @"E:\10001003\1d.jpg";
            const string pSavedPath = @"E:\10001003\";

            var bmp = new Bitmap(Image.FromFile(pPath));

            // 图像旋转
            var result = ImageHelper.RotateImg(bmp, 90);
            var guid = Guid.NewGuid().ToString("N").ToUpper();

            result.Save(pSavedPath + "\\1d_rotate_" + guid + ".jpg", ImageFormat.Jpeg);

            Console.Write("Processing completed");

            Console.ReadLine();

        }
    }
}
