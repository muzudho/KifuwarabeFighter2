using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.IO;
using System;

namespace AtarihanteiMaker
{
    public class RectangleEx
    {
        public RectangleEx()
        {
            this.Collider = Rectangle.Empty;
            this.Slice = Rectangle.Empty;
        }
        public RectangleEx(int sliceX, int sliceY, int sliceWidth, int sliceHeight, int x, int y, int width, int height)
        {
            this.Collider = new Rectangle(x, y, width, height);
            this.Slice = new Rectangle(sliceX, sliceY, sliceWidth, sliceHeight);
        }

        public int SliceNumber { get; set; }
        /// <summary>
        /// 当たり判定の矩形
        /// </summary>
        public Rectangle Collider { get; set; }
        /// <summary>
        /// スライス領域の矩形
        /// </summary>
        public Rectangle Slice { get; set; }

        /// <summary>
        /// 中心座標に変換するぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public float GetOffsetX()
        {
            return ((this.Collider.X - this.Slice.X) - this.Slice.Width / 2.0f + this.Collider.Width / 2.0f) / 100.0f;
        }
        public float GetOffsetY()
        {
            return -1 * ((this.Collider.Y - this.Slice.Y) - this.Slice.Height / 2.0f + this.Collider.Height / 2.0f) / 100.0f;
        }
        /// <summary>
        /// 当たり判定画像も、１スライスのサイズと同じとするぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public float GetScaleX()
        {
            return this.Collider.Width / (float)Program.SLICE_WIDTH;
        }
        public float GetScaleY()
        {
            return this.Collider.Height / (float)Program.SLICE_HEIGHT;
        }
    }

    /// <summary>
    /// ・当たり判定は、１スライスにつき、１個所とする。
    /// ・当たり判定は、矩形のみとする。
    /// ・当たり判定は、グリッドに沿っているものとする。
    /// ・当たり判定は、重なっていないものとする。
    /// </summary>
    class Program
    {
        private const int GRID_WIDTH = 8;
        private const int GRID_HEIGHT = 8;
        public const int SLICE_WIDTH = 128;
        public const int SLICE_HEIGHT = 128;

        static void Main(string[] args)
        {
            string[] files = Directory.GetFiles("./", "*.png");

            // 大文字・小文字を無視してソート☆
            Array.Sort(files, StringComparer.OrdinalIgnoreCase);

            List<List<RectangleEx>> imageList = new List<List<RectangleEx>>();
            foreach (string file in files)
            {
                Program.Execute_1Image(imageList, file);
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("public abstract class AttackCollider2DScript");
            sb.AppendLine("{");
            sb.AppendLine("    public static float[,] imageAndSlice_To_OffsetX = new float[,]");
            sb.AppendLine("    {");
            foreach (List<RectangleEx> sliceList in imageList)
            {
                // コメント行
                {
                    sb.Append("        //");
                    int slice = 0;
                    foreach (RectangleEx rect in sliceList)
                    {
                        sb.Append(string.Format("{0,12}, ", slice));
                        slice++;
                    }
                    sb.AppendLine();
                }
                break;
            }
            foreach (List<RectangleEx> sliceList in imageList)
            {
                sb.Append("        { ");
                foreach (RectangleEx rect in sliceList)
                {
                    sb.Append(string.Format("{0,11:F6}f, ", rect.GetOffsetX()));
                }
                sb.AppendLine(" },");
            }
            sb.AppendLine("    };");
            sb.AppendLine("    public static float[,] imageAndSlice_To_OffsetY = new float[,]");
            sb.AppendLine("    {");
            foreach (List<RectangleEx> sliceList in imageList)
            {
                // コメント行
                {
                    sb.Append("        //");
                    int slice = 0;
                    foreach (RectangleEx rect in sliceList)
                    {
                        sb.Append(string.Format("{0,12}, ", slice));
                        slice++;
                    }
                    sb.AppendLine();
                }
                break;
            }
            foreach (List<RectangleEx> sliceList in imageList)
            {
                sb.Append("        { ");
                foreach (RectangleEx rect in sliceList)
                {
                    sb.Append(string.Format("{0,11:F6}f, ", rect.GetOffsetY()));
                }
                sb.AppendLine(" },");
            }
            sb.AppendLine("    };");
            sb.AppendLine("    public static float[,] imageAndSlice_To_ScaleX = new float[,]");
            sb.AppendLine("    {");
            foreach (List<RectangleEx> sliceList in imageList)
            {
                // コメント行
                {
                    sb.Append("        //");
                    int slice = 0;
                    foreach (RectangleEx rect in sliceList)
                    {
                        sb.Append(string.Format("{0,12}, ", slice));
                        slice++;
                    }
                    sb.AppendLine();
                }
                break;
            }
            foreach (List<RectangleEx> sliceList in imageList)
            {
                sb.Append("        { ");
                foreach (RectangleEx rect in sliceList)
                {
                    sb.Append(string.Format("{0,11:F6}f, ", rect.GetScaleX()));
                }
                sb.AppendLine(" },");
            }
            sb.AppendLine("    };");
            sb.AppendLine("    public static float[,] imageAndSlice_To_ScaleY = new float[,]");
            sb.AppendLine("    {");
            foreach (List<RectangleEx> sliceList in imageList)
            {
                // コメント行
                {
                    sb.Append("        //");
                    int slice = 0;
                    foreach (RectangleEx rect in sliceList)
                    {
                        sb.Append(string.Format("{0,12}, ", slice));
                        slice++;
                    }
                    sb.AppendLine();
                }
                break;
            }
            foreach (List<RectangleEx> sliceList in imageList)
            {
                sb.Append("        { ");
                foreach (RectangleEx rect in sliceList)
                {
                    sb.Append(string.Format("{0,11:F6}f, ", rect.GetScaleY()));
                }
                sb.AppendLine(" },");
            }
            sb.AppendLine("    };");
            sb.AppendLine("}");

            System.Console.WriteLine(sb.ToString());

            File.WriteAllText("AttackCollider2DScript.cs", sb.ToString());

            // おわり☆
            System.Console.WriteLine("Please, push any key.");
            System.Console.ReadKey();
        }

        private static bool ContainsLocation(int x, int y, List<RectangleEx> rectList)
        {
            foreach (RectangleEx rect in rectList)
            {
                if (rect.Collider.Contains(x, y))
                {
                    return true;
                }
            }
            return false;
        }

        private static void Execute_1Image(List<List<RectangleEx>> imageList, string file)
        {
            List<RectangleEx> sliceList = new List<RectangleEx>();
            Bitmap img = (Bitmap)Image.FromFile(file);

            int sliceNumber = 0;
            for (int y1 = 0; y1 + SLICE_HEIGHT <= img.Height; y1 += SLICE_HEIGHT)
            {
                //System.Console.WriteLine("@sliceNumber = " + sliceNumber + " y1 = " + y1 + " (y1 + SLICE_HEIGHT) = " + (y1 + SLICE_HEIGHT) + " img.Height = " + img.Height);

                for (int x1 = 0; x1 + SLICE_WIDTH <= img.Width; x1 += SLICE_WIDTH)
                {
                    //System.Console.WriteLine("@sliceNumber = " + sliceNumber + " x1 = " + x1 + " (x1 + SLICE_WIDTH) = " + (x1 + SLICE_WIDTH) + " img.Width = " + img.Width);

                    sliceList.Add(Program.Execute_1Slice(sliceList, x1, y1, sliceNumber, img, file));

                    sliceNumber++;
                }
            }
            imageList.Add(sliceList);
        }

        /// <summary>
        /// 1スライスにつき、当たり判定は１個所という前提☆
        /// </summary>
        /// <param name="rectList">判定済み判定用</param>
        /// <param name="sliceX1"></param>
        /// <param name="sliceY1"></param>
        /// <param name="sliceNumber"></param>
        /// <param name="img"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        private static RectangleEx Execute_1Slice(List<RectangleEx> rectList, int sliceX1, int sliceY1, int sliceNumber, Bitmap img, string file)
        {
            //System.Console.WriteLine("sliceNumber = " + sliceNumber);

            int LAST_X = sliceX1 + SLICE_WIDTH;
            int LAST_Y = sliceY1 + SLICE_HEIGHT;

            // 10x10 pixel ごとの方眼紙になっているものとし、
            // その左上１セルが赤色(255,0,0)だった場合、その 10x10 pixel は当たり判定があるものとする。
            for (int gridY1 = sliceY1; gridY1 + GRID_HEIGHT <= LAST_Y; gridY1 += GRID_HEIGHT)
            {
                for (int gridX1 = sliceX1; gridX1 + GRID_WIDTH <= LAST_X; gridX1 += GRID_WIDTH)
                {
                    //System.Console.WriteLine("1Slice y1 = " + y1 + " x1 = " + x1);

                    Color color = img.GetPixel(gridX1, gridY1);

                    if (color.R == 255 && color.G == 0 && color.B == 0)
                    {
                        // 既に判定済みならスキップ
                        if (ContainsLocation(gridX1, gridY1, rectList))
                        {
                            //System.Console.WriteLine("skipped " + x1 + ", " + y1);
                        }
                        else
                        {
                            // さらに横方向に調査
                            int gridX2 = gridX1 + GRID_WIDTH;
                            for (; gridX2 < LAST_X; gridX2 += GRID_WIDTH)
                            {
                                color = img.GetPixel(gridX2, gridY1); // 上辺を左から右にスキャン
                                if (color.R != 255 || color.G != 0 || color.B != 0)
                                {
                                    break;
                                }
                            }

                            // さらに縦方向に調査
                            int gridY2 = gridY1 + GRID_HEIGHT;
                            for (; gridY2 < LAST_Y; gridY2 += GRID_HEIGHT)
                            {
                                color = img.GetPixel(gridX1, gridY2); // 左辺を降りていく
                                if (color.R != 255 || color.G != 0 || color.B != 0)
                                {
                                    break;
                                }
                            }

                            return new RectangleEx(sliceX1, sliceY1, SLICE_WIDTH, SLICE_HEIGHT, gridX1, gridY1, gridX2 - gridX1, gridY2 - gridY1);
                        }
                    }
                }
            }//画像スキャン終わり

            return new RectangleEx();
        }
    }
}
