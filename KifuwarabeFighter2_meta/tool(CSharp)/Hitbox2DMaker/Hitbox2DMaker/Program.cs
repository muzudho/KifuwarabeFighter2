using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Hitbox2DMaker
{

    /// <summary>
    /// ・当たり判定は、１スライスにつき、１個所とする。
    /// ・当たり判定は、矩形のみとする。
    /// ・当たり判定は、グリッドに沿っているものとする。
    /// ・当たり判定は、重なっていないものとする。
    /// </summary>
    public class Program
    {
        public const string FILE_NAME = "Hitbox2DScript.cs";
        /// <summary>
        /// 2の倍数であること。
        /// </summary>
        public const int GRID_WIDTH = 8;
        /// <summary>
        /// 2の倍数であること。
        /// </summary>
        public const int GRID_HEIGHT = 8;
        public const int SLICE_WIDTH = 128;
        public const int SLICE_HEIGHT = 128;

        static void Main(string[] args)
        {
            string[] files = Directory.GetFiles("./", "*.png");

            // 大文字・小文字を無視してソート☆
            Array.Sort(files, StringComparer.OrdinalIgnoreCase);

            // まずは赤い箱から。
            StringBuilder csFileText = new StringBuilder();
            ColorBoxCondition expectedBox = Utility_BoxScanner.m_redBox;
            {
                List<List<RectangleOnSlice>> image_to_slice_to_rectangleList = new List<List<RectangleOnSlice>>();
                foreach (string file in files)
                {
                    Utility_BoxScanner.Execute_1Image(expectedBox, image_to_slice_to_rectangleList, file);
                }

                string text = Utility_Hitbox2DClassFormat.ToText(expectedBox.m_outputClassName, image_to_slice_to_rectangleList);
                System.Console.WriteLine(text);
                csFileText.Append(text);
            }
            File.WriteAllText(Program.FILE_NAME, csFileText.ToString());


            // おわり☆
            System.Console.WriteLine("Please, push any key.");
            System.Console.ReadKey();
        }

    }
}
