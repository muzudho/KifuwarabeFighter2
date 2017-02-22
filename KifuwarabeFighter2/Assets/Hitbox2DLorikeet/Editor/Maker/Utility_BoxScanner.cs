using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace DojinCircleGrayscale.Hitbox2DLorikeetMaker
{
    public class ColorBoxCondition
    {
        public const float COLOR_LENGTH = 255;

        public static ColorBoxCondition FromRGB_0to255(string outputClassName, int r, int g, int b, int lineBorder)
        {
            return new ColorBoxCondition(outputClassName, r/COLOR_LENGTH, g/COLOR_LENGTH, b/COLOR_LENGTH, lineBorder);
        }
        ColorBoxCondition( string outputClassName, float r, float g, float b, int lineBorder)
        {
            m_outputClassName = outputClassName;
            expectedR = r;
            expectedG = g;
            expectedB = b;
            m_lineBold = lineBorder;
        }
        public string m_outputClassName;
        /// <summary>
        /// 赤の値。0～255 ではなく、 0.0～1.0で表すこと。
        /// </summary>
        public float expectedR { get; private set; }
        public float expectedG { get; private set; }
        public float expectedB { get; private set; }
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
        public static ColorBoxCondition m_blueBox = ColorBoxCondition.FromRGB_0to255("WeakboxData", 0, 0, 255, 2);
        /// <summary>
        /// 黄箱　太さ４
        /// </summary>
        public static ColorBoxCondition m_yellowBox = ColorBoxCondition.FromRGB_0to255("StrongboxData", 255, 255, 0, 4);
        /// <summary>
        /// 赤箱　太さ６
        /// </summary>
        public static ColorBoxCondition m_redBox = ColorBoxCondition.FromRGB_0to255("HitboxData", 255,0,0,6);
        /// <summary>
        /// １ピクセルずらしたい。
        /// 図解： http://qiita.com/muzudho1/items/7de6e450e1762b993a63
        /// </summary>
        public const int DELETE_SPACE = 1;

        public static void Execute_1Image(ColorBoxCondition boxSettings, List<List<RectangleOnSlice>> imageList, string filepath, StringBuilder info_message)
        {
            List<RectangleOnSlice> sliceList = new List<RectangleOnSlice>();

            // 座標系は左下隅スタート
            Texture2D texture2D = PngReader.FromPngFile(filepath);
            //info_message.AppendLine("DataGenerator.SLICE_WIDTH = " + DataGenerator.SLICE_WIDTH + " DataGenerator.SLICE_HEIGHT = " + DataGenerator.SLICE_HEIGHT);
            //info_message.AppendLine("texture2D.width = " + texture2D.width + " texture2D.height = " + texture2D.height);
            //info_message.AppendLine("overX = " + (texture2D.width - DataGenerator.SLICE_WIDTH + 1) + " overY = " + (DataGenerator.SLICE_HEIGHT - 1));

            int sliceNumber = 0;
            // スライス・サイズの画像が四方形のタイル状に敷き詰められているとする。
            // 座標は 0 ～ (サイズ-1) とし、Y は下げていく。
            // tickY、tickXは スライスされた区画の左上隅を指しているものとする。
            // width は overX と読み替え、last の１つ次と考えると分かりやすい。 last < over。
            int thisY_tilefile = texture2D.height - 1;
            int nextY_tilefile = -1;
            for (int tickY = thisY_tilefile; nextY_tilefile < tickY; tickY -= DataGenerator.SLICE_HEIGHT)
            {
                int thisX_tilefile = 0;
                int nextX_tilefile = texture2D.width;
                for (int tickX = thisX_tilefile; tickX < nextX_tilefile; tickX += DataGenerator.SLICE_WIDTH)
                {
                    if (info_message.Length < 3000)
                    {
                        info_message.AppendLine("@Add sliceNumber=[" + sliceNumber + "] tickY=[" + tickY + "] tickX=[" + tickX + "]" );
                    }

                    sliceList.Add(Execute_1Slice(boxSettings, sliceList, tickX, tickY, sliceNumber, texture2D, filepath, info_message));

                    sliceNumber++;
                }
            }
            imageList.Add(sliceList);
        }

        /// <summary>
        /// 1スライスにつき、当たり判定は１個所という前提☆
        /// </summary>
        /// <param name="rectList">判定済み判定用</param>
        /// <param name="thisX_slice"></param>
        /// <param name="thisY_slice"></param>
        /// <param name="sliceNumber"></param>
        /// <param name="texture2D"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static RectangleOnSlice Execute_1Slice(ColorBoxCondition boxSettings, List<RectangleOnSlice> rectList, int thisX_slice, int thisY_slice, int sliceNumber, Texture2D texture2D, string file, StringBuilder info_message)
        {

            int nextX_slice = thisX_slice + DataGenerator.SLICE_WIDTH;
            // Y は下げていく
            int nextY_slice = thisY_slice - DataGenerator.SLICE_HEIGHT;

            if (info_message.Length < 10000)
            {
                info_message.AppendLine("Execute_1Slice(A): sliceNumber = " + sliceNumber + " nextX_slice=[" + nextX_slice + "] nextY_slice=[" + nextY_slice + "] sliceX1=["+ thisX_slice + "] sliceY1=[" + thisY_slice + "]");
            }

            // 当たり判定サイズ単位のグリッドが引かれた方眼紙になっているものとし、
            // その左上角(0+offset,0+offset)の１セルが　指定色だった場合、そのグリッド１つ分 は当たり判定があるものとする。

            // Y は下げていく
            for (int tickY1 = thisY_slice; nextY_slice < tickY1; tickY1 -= DataGenerator.GRID_HEIGHT)
            {
                if (info_message.Length < 10000)
                {
                    info_message.AppendLine("Execute_1Slice(B): tickY1 = " + tickY1 + " thisX_slice=[" + thisX_slice + "] nextX_slice=[" + nextX_slice + "]");
                }

                for (int tickX1 = thisX_slice; tickX1 < nextX_slice; tickX1 += DataGenerator.GRID_WIDTH)
                {
                    // 開始形か　判定
                    if (IsStarting_ByWinningStairs(tickX1, tickY1, boxSettings, texture2D, info_message))
                    {
                        // 既に判定済みならスキップ
                        if (ContainsLocation(tickX1, tickY1, rectList))
                        {
                            if (info_message.Length < 3000)
                            {
                                info_message.AppendLine("Execute_1Slice(D): skipped " + tickX1 + ", " + tickY1);
                            }
                        }
                        else
                        {
                            if (info_message.Length < 10000)
                            {
                                info_message.AppendLine("Execute_1Slice(E): tickY1 = " + tickY1 + " tickX1 = " + tickX1);
                            }

                            // さらに横方向に調査
                            int tickX2_onGrid = tickX1 + DataGenerator.GRID_WIDTH;// 上辺を左から右にスキャン
                            for (; tickX2_onGrid < nextX_slice; tickX2_onGrid += DataGenerator.GRID_WIDTH)
                            {
                                if (!HasColor_Horizontal_ByWinningStairs(tickX2_onGrid, tickY1,boxSettings,texture2D))
                                {
                                    break; // 横の終わり
                                }
                            }

                            // さらに縦方向に調査。左辺を降りていく
                            // Y は下に向かって減っていく
                            int tickY2_onGrid = tickY1 - DataGenerator.GRID_HEIGHT;
                            for (; nextY_slice < tickY2_onGrid; tickY2_onGrid -= DataGenerator.GRID_HEIGHT)
                            {
                                if (!HasColor_Vertical_WinningStairs(tickX1, tickY2_onGrid, boxSettings, texture2D))
                                {
                                    break; // 縦の終わり
                                }
                            }

                            return new RectangleOnSlice(thisX_slice, thisY_slice, DataGenerator.SLICE_WIDTH, DataGenerator.SLICE_HEIGHT, tickX1, tickY1, tickX2_onGrid - tickX1, tickY1 - tickY2_onGrid);
                        }
                    }
                    else
                    {
                        if (info_message.Length < 10000)
                        {
                            info_message.AppendLine("Execute_1Slice(C)開始形じゃない: tickY1 = " + tickY1 + " tickX1 = " + tickX1);
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
                if (rect.Collider.Contains(new Vector2(x, y)))
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
        public static bool IsStarting_ByWinningStairs(int x, int y, ColorBoxCondition boxSettings, Texture2D texture2D, StringBuilder info_message)
        {
            Color color = texture2D.GetPixel(
                x + boxSettings.GetWinnerStairsDistance()
                , y - boxSettings.GetWinnerStairsDistance()
                );
            if (info_message.Length < 10000)
            {
                info_message.AppendLine("IsStarting_ByWinningStairs: x=[" + (x + boxSettings.GetWinnerStairsDistance()) + "] y=[" + (y - boxSettings.GetWinnerStairsDistance()) + "] color.r,g,b=["+ color.r + "]["+ color.g + "]["+ color.b + "]");
            }
            return boxSettings.expectedR == color.r && boxSettings.expectedG == color.g && boxSettings.expectedB == color.b;
        }

        /// <summary>
        /// 図解： http://qiita.com/muzudho1/items/7de6e450e1762b993a63
        /// </summary>
        /// <returns></returns>
        public static bool HasColor_Horizontal_ByWinningStairs(int x, int y, ColorBoxCondition boxSettings, Texture2D texture2D)
        {
            // 画像の外か判定
            if (texture2D.width <= x || y < 0)
            {
                return true;
            }

            // 切断形か判定
            Color color = texture2D.GetPixel(x - DELETE_SPACE + boxSettings.GetWinnerStairsDistance() + DataGenerator.GRID_WIDTH / 2, y - boxSettings.GetWinnerStairsDistance());
            return boxSettings.expectedR == color.r && boxSettings.expectedG == color.g && boxSettings.expectedB == color.b;
        }

        /// <summary>
        /// 図解： http://qiita.com/muzudho1/items/7de6e450e1762b993a63
        /// </summary>
        /// <returns></returns>
        public static bool HasColor_Vertical_WinningStairs(int x, int y, ColorBoxCondition boxSettings, Texture2D texture2D)
        {
            // 画像の外か判定
            if (texture2D.width <= x || y < 0)
            {
                return true;
            }

            // 切断形か判定
            Color color = texture2D.GetPixel(
                x + boxSettings.GetWinnerStairsDistance()
                , y + DELETE_SPACE - boxSettings.GetWinnerStairsDistance() - DataGenerator.GRID_HEIGHT / 2
                );
            return boxSettings.expectedR == color.r && boxSettings.expectedG == color.g && boxSettings.expectedB == color.b;
        }
    }
}
