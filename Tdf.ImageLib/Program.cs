using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.ImageLib
{
    class Program
    {
        static void Main(string[] args)
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
            var lstFrames = new List<Frame> {f3};
            #endregion

            var bmp = new Bitmap(Image.FromFile(pPath));


            foreach (var frame in lstFrames)
            {
                /*
                var rectangleImg = ImageLib.DrawRectangleInPicture(bmp, frame.X, frame.Y, frame.Width, frame.Height);
                rectangleImg.Save(pSavedPath + "\\rectangle.jpg", ImageFormat.Jpeg);
                */

                /*
                var roundImg = ImageLib.DrawRoundInPicture(bmp, frame.X, frame.Y, frame.Width, frame.Height);
                roundImg.Save(pSavedPath + "\\round.jpg", ImageFormat.Jpeg);
                */

                /*
                var textImg = ImageLib.KiSetText(bmp, "设置文字", frame.X, frame.Y);
                textImg.Save(pSavedPath + "\\text.jpg", ImageFormat.Jpeg);
                */

                var cutImg = ImageLib.KiCut(bmp, frame.X, frame.Y, frame.Width, frame.Height);
                cutImg.Save(pSavedPath + "\\cut.jpg", ImageFormat.Jpeg);
            }

            Console.Write("End...");

            Console.ReadLine();
        }
    }
}
