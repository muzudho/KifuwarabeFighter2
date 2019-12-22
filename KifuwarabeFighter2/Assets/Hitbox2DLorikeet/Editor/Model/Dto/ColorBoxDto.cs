namespace Assets.Hitbox2DLorikeet.Editor.Maker.Helper
{
    /// <summary>
    /// この色の矩形を目印にして、自動で当たり判定の座標を測ります。
    /// </summary>
    public class ColorBoxDto
    {
        public const float COLOR_LENGTH = 255;

        public static ColorBoxDto FromRGB_0to255(string outputClassName, int r, int g, int b, int lineBorder)
        {
            return new ColorBoxDto(outputClassName, r / COLOR_LENGTH, g / COLOR_LENGTH, b / COLOR_LENGTH, lineBorder);
        }

        ColorBoxDto(string outputClassName, float r, float g, float b, int lineBorder)
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
}
