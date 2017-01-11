using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.IO;

namespace AtarihanteiMaker
{
    public class RectangleEx
    {
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
    }

    /// <summary>
    /// ・当たり判定は、矩形のみとする。
    /// ・当たり判定は、グリッドに沿っているものとする。
    /// ・当たり判定は、重なっていないものとする。
    /// </summary>
    class Program
    {
        private const int GRID_WIDTH = 8;
        private const int GRID_HEIGHT = 8;
        private const int SLICE_WIDTH = 128;
        private const int SLICE_HEIGHT = 128;

        static void Main(string[] args)
        {
            string[] files = Directory.GetFiles("./", "*.png");
            foreach (string file in files)
            {
                Program.Execute(file);
            }

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

        private static void Execute(string file)
        {
            Bitmap img = (Bitmap)Image.FromFile(file);
            StringBuilder sb = new StringBuilder();

            int sliceNumber = 0;
            for (int y1 = 0; y1 + SLICE_HEIGHT <= img.Height; y1 += SLICE_HEIGHT)
            {
                //System.Console.WriteLine("@sliceNumber = " + sliceNumber + " y1 = " + y1 + " (y1 + SLICE_HEIGHT) = " + (y1 + SLICE_HEIGHT) + " img.Height = " + img.Height);

                for (int x1 = 0; x1 + SLICE_WIDTH <= img.Width; x1 += SLICE_WIDTH)
                {
                    //System.Console.WriteLine("@sliceNumber = " + sliceNumber + " x1 = " + x1 + " (x1 + SLICE_WIDTH) = " + (x1 + SLICE_WIDTH) + " img.Width = " + img.Width);

                    Program.Execute_1Slice(sb, x1, y1, sliceNumber, img, file);

                    sliceNumber++;
                }
            }

            System.Console.WriteLine(Path.GetFileNameWithoutExtension(file) + ".txt");
            System.Console.WriteLine(sb.ToString());

            File.WriteAllText(Path.GetFileNameWithoutExtension(file) + ".txt", sb.ToString());
        }

        private static void Execute_1Slice(StringBuilder sb, int sliceX1, int sliceY1, int sliceNumber, Bitmap img, string file)
        {
            //System.Console.WriteLine("sliceNumber = " + sliceNumber);

            int LAST_X = sliceX1 + SLICE_WIDTH;
            int LAST_Y = sliceY1 + SLICE_HEIGHT;
            List<RectangleEx> rectList = new List<RectangleEx>();

            // 10x10 pixel ごとの方眼紙になっているものとし、
            // その左上１セルが赤色(255,0,0)だった場合、その 10x10 pixel は当たり判定があるものとする。
            for (int y1 = sliceY1; y1 + GRID_HEIGHT <= LAST_Y; y1 += GRID_HEIGHT)
            {
                for (int x1 = sliceX1; x1 + GRID_WIDTH <= LAST_X; x1 += GRID_WIDTH)
                {
                    //System.Console.WriteLine("1Slice y1 = " + y1 + " x1 = " + x1);

                    Color color = img.GetPixel(x1, y1);

                    if (color.R == 255 && color.G == 0 && color.B == 0)
                    {
                        // 既に判定済みならスキップ
                        if (ContainsLocation(x1, y1, rectList))
                        {
                            //System.Console.WriteLine("skipped " + x1 + ", " + y1);
                        }
                        else
                        {
                            // さらに横方向に調査
                            int x2 = x1 + GRID_WIDTH;
                            for (; x2 < LAST_X; x2 += GRID_WIDTH)
                            {
                                color = img.GetPixel(x2, y1); // 上辺を左から右にスキャン
                                if (color.R != 255 || color.G != 0 || color.B != 0)
                                {
                                    break;
                                }
                            }

                            // さらに縦方向に調査
                            int y2 = y1 + GRID_HEIGHT;
                            for (; y2 < LAST_Y; y2 += GRID_HEIGHT)
                            {
                                color = img.GetPixel(x1, y2); // 左辺を降りていく
                                if (color.R != 255 || color.G != 0 || color.B != 0)
                                {
                                    break;
                                }
                            }

                            rectList.Add(new RectangleEx(sliceX1, sliceY1, SLICE_WIDTH, SLICE_HEIGHT, x1, y1, x2 - x1, y2 - y1));
                        }
                    }
                }
            }//画像スキャン終わり

            // 中心座標に変換して、１００分の１スケールにするぜ☆（＾▽＾）
            foreach (RectangleEx rect in rectList)
            {
                sb.Append("\"");
                sb.Append(Path.GetFileNameWithoutExtension(file));
                sb.Append("_");
                sb.Append(sliceNumber);
                sb.Append("\", ");
                sb.Append(((rect.Collider.X- rect.Slice.X) - rect.Slice.Width / 2.0f + rect.Collider.Width / 2.0f) /100.0f);
                sb.Append(", ");
                sb.Append(-1 * ((rect.Collider.Y- rect.Slice.Y) - rect.Slice.Height / 2.0f + rect.Collider.Height / 2.0f) /100.0f);
                sb.Append(", ");
                sb.Append(rect.Collider.Width/100.0f);
                sb.Append(", ");
                sb.Append(rect.Collider.Height/100.0f);
                //sb.Append("  ...  x = ( ( ");
                //sb.Append(rect.Collider.X);
                //sb.Append(" - ");
                //sb.Append(rect.Slice.X);
                //sb.Append(" ) - ");
                //sb.Append(rect.Slice.Width);
                //sb.Append(" / 2.0f + ");
                //sb.Append(rect.Collider.Width);
                //sb.Append(" / 2.0f ) / 100.0f  ...  y = -1 * ( ( ");
                //sb.Append(rect.Collider.Y);
                //sb.Append(" - ");
                //sb.Append(rect.Slice.Y);
                //sb.Append(" ) - ");
                //sb.Append(rect.Slice.Height);
                //sb.Append(" / 2.0f + ");
                //sb.Append(rect.Collider.Height);
                //sb.Append(" / 2.0f ) / 100.0f");
                sb.AppendLine();
            }

        }
    }
}
