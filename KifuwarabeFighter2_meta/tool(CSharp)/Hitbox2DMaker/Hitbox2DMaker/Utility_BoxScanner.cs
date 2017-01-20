using System.Collections.Generic;
using System.Drawing;

namespace Hitbox2DMaker
{
    public class ColorBoxCondition
    {
        public ColorBoxCondition( string outputClassName, int expectedR, int expectedG, int expectedB, int lineBorder)
        {
            m_outputClassName = outputClassName;
            m_expectedR = expectedR;
            m_expectedG = expectedG;
            m_expectedB = expectedB;
            m_lineBold = lineBorder;
        }
        public string m_outputClassName;
        public int m_expectedR;
        public int m_expectedG;
        public int m_expectedB;
        /// <summary>
        /// 線の太さ。2の倍数であること。
        /// </summary>
        public int m_lineBold;

        /// <summary>
        /// 図解： http://qiita.com/muzudho1/items/7de6e450e1762b993a63
        /// </summary>
        /// <returns></returns>
        public int GetWinnerStairsDistance()
        {
            return this.m_lineBold / 2 - 1;
        }
    }

    public abstract class Utility_BoxScanner
    {
        /// <summary>
        /// 青箱　太さ２
        /// </summary>
        public static ColorBoxCondition m_blueBox = new ColorBoxCondition("Hitbox2DScript_Weakbox", 0, 0, 255, 2);
        /// <summary>
        /// 黄箱　太さ４
        /// </summary>
        public static ColorBoxCondition m_yellowBox = new ColorBoxCondition("Hitbox2DScript_Strongbox", 255, 255, 0, 4);
        /// <summary>
        /// 赤箱　太さ６
        /// </summary>
        public static ColorBoxCondition m_redBox = new ColorBoxCondition("Hitbox2DScript_Hitbox", 255,0,0,6);
        /// <summary>
        /// 図解： http://qiita.com/muzudho1/items/7de6e450e1762b993a63
        /// </summary>
        public const int NO_EXISTS_SPACE = -1;

        public static void Execute_1Image(ColorBoxCondition expectedBox, List<List<RectangleOnSlice>> imageList, string file)
        {
            List<RectangleOnSlice> sliceList = new List<RectangleOnSlice>();
            Bitmap img = (Bitmap)Image.FromFile(file);

            int sliceNumber = 0;
            for (int y1 = 0; y1 + Program.SLICE_HEIGHT <= img.Height; y1 += Program.SLICE_HEIGHT)
            {
                //System.Console.WriteLine("@sliceNumber = " + sliceNumber + " y1 = " + y1 + " (y1 + SLICE_HEIGHT) = " + (y1 + SLICE_HEIGHT) + " img.Height = " + img.Height);

                for (int x1 = 0; x1 + Program.SLICE_WIDTH <= img.Width; x1 += Program.SLICE_WIDTH)
                {
                    //System.Console.WriteLine("@sliceNumber = " + sliceNumber + " x1 = " + x1 + " (x1 + SLICE_WIDTH) = " + (x1 + SLICE_WIDTH) + " img.Width = " + img.Width);

                    sliceList.Add(Utility_BoxScanner.Execute_1Slice(expectedBox, sliceList, x1, y1, sliceNumber, img, file));

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
        public static RectangleOnSlice Execute_1Slice(ColorBoxCondition expectedBox, List<RectangleOnSlice> rectList, int sliceX1, int sliceY1, int sliceNumber, Bitmap img, string file)
        {
            //System.Console.WriteLine("sliceNumber = " + sliceNumber);

            int LAST_X = sliceX1 + Program.SLICE_WIDTH;
            int LAST_Y = sliceY1 + Program.SLICE_HEIGHT;

            // 指定サイズのグリッドが引かれた方眼紙になっているものとし、
            // その左上角付近の１セルが　指定色だった場合、そのグリッド１セル分 は当たり判定があるものとする。
            for (int gridY1 = sliceY1; gridY1 + Program.GRID_HEIGHT <= LAST_Y; gridY1 += Program.GRID_HEIGHT)
            {
                for (int gridX1 = sliceX1; gridX1 + Program.GRID_WIDTH <= LAST_X; gridX1 += Program.GRID_WIDTH)
                {
                    //System.Console.WriteLine("1Slice y1 = " + y1 + " x1 = " + x1);

                    // 開始形か　判定
                    if (IsStarting_ByWinningStairs(gridX1, gridY1, expectedBox, img))
                    {
                        // 既に判定済みならスキップ
                        if (ContainsLocation(gridX1, gridY1, rectList))
                        {
                            //System.Console.WriteLine("skipped " + x1 + ", " + y1);
                        }
                        else
                        {
                            // さらに横方向に調査
                            int gridX2 = gridX1 + Program.GRID_WIDTH;// 上辺を左から右にスキャン
                            for (; gridX2 < LAST_X; gridX2 += Program.GRID_WIDTH)
                            {
                                if (!HasColor_Horizontal_ByWinningStairs(gridX2, gridY1,expectedBox,img))
                                {
                                    break; // 横の終わり
                                }
                            }

                            // さらに縦方向に調査
                            int gridY2 = gridY1 + Program.GRID_HEIGHT;// 左辺を降りていく
                            for (; gridY2 < LAST_Y; gridY2 += Program.GRID_HEIGHT)
                            {
                                if (!HasColor_Vertical_WinningStairs(gridX1, gridY2, expectedBox, img))
                                //if (!HasColor_Vertical_WinningStairs(gridX2, gridY1, expectedBox, img))
                                {
                                    break; // 縦の終わり
                                }
                            }

                            return new RectangleOnSlice(sliceX1, sliceY1, Program.SLICE_WIDTH, Program.SLICE_HEIGHT, gridX1, gridY1, gridX2 - gridX1, gridY2 - gridY1);
                        }
                    }
                }
            }//画像スキャン終わり

            return new RectangleOnSlice();
        }

        private static bool ContainsLocation(int x, int y, List<RectangleOnSlice> rectList)
        {
            foreach (RectangleOnSlice rect in rectList)
            {
                if (rect.Collider.Contains(x, y))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 図解： http://qiita.com/muzudho1/items/7de6e450e1762b993a63
        /// </summary>
        /// <returns></returns>
        public static bool IsStarting_ByWinningStairs(int x, int y, ColorBoxCondition expectedBox, Bitmap img)
        {
            Color color = img.GetPixel(x + expectedBox.GetWinnerStairsDistance(), y + expectedBox.GetWinnerStairsDistance());
            return expectedBox.m_expectedR == color.R && expectedBox.m_expectedG == color.G && expectedBox.m_expectedB == color.B;
        }

        /// <summary>
        /// 図解： http://qiita.com/muzudho1/items/7de6e450e1762b993a63
        /// </summary>
        /// <returns></returns>
        public static bool HasColor_Horizontal_ByWinningStairs(int x, int y, ColorBoxCondition expectedBox, Bitmap img)
        {
            // 画像の外か判定
            if (img.Width <= x || img.Height <= y)
            {
                return true;
            }

            // 切断形か判定
            Color color = img.GetPixel(x + NO_EXISTS_SPACE + expectedBox.GetWinnerStairsDistance() + Program.GRID_WIDTH / 2, y + expectedBox.GetWinnerStairsDistance());
            return expectedBox.m_expectedR == color.R && expectedBox.m_expectedG == color.G && expectedBox.m_expectedB == color.B;
        }

        /// <summary>
        /// 図解： http://qiita.com/muzudho1/items/7de6e450e1762b993a63
        /// </summary>
        /// <returns></returns>
        public static bool HasColor_Vertical_WinningStairs(int x, int y, ColorBoxCondition expectedBox, Bitmap img)
        {
            // 画像の外か判定
            if (img.Width <= x || img.Height <= y)
            {
                return true;
            }

            // 切断形か判定
            Color color = img.GetPixel(x + expectedBox.GetWinnerStairsDistance() , y + NO_EXISTS_SPACE + expectedBox.GetWinnerStairsDistance() + Program.GRID_HEIGHT / 2);
            return expectedBox.m_expectedR == color.R && expectedBox.m_expectedG == color.G && expectedBox.m_expectedB == color.B;
        }
    }
}
