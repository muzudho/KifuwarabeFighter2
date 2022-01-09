namespace DojinCircleGrayscale.Hitbox2DLorikeetMaker
{
    using System.IO;
    using UnityEngine;

    /// <summary>
    /// 参考 : 【Unity】pngファイルを読み込み同サイズのTexture2Dを生成する http://qiita.com/r-ngtm/items/6cff25643a1a6ba82a6c
    /// 参考 : ma_comu雑記帳「UnityでPNGファイルを動的に読み込む方法」 http://macomu.sakura.ne.jp/blog/?p=55
    /// 
    /// PNG画像の幅の格納場所 : 16バイト～19バイト(長さ4バイト)
    /// PNG画像の高さの格納場所 : 20バイト～23バイト(長さ4バイト)
    /// </summary>
    public static class PngReaderHelper
    {
        public static Texture2D FromPngFile(string path)
        {
            byte[] readBinary = ReadBytes(path);

            int pos = 16; // 16～19バイト読込（画像の幅）、20～23バイト読込（画像の高さ）

            int width = 0;
            for (int i = 0; i < 4; i++)
            {
                width = width * 256 + readBinary[pos++];
            }

            int height = 0;
            for (int i = 0; i < 4; i++)
            {
                height = height * 256 + readBinary[pos++];
            }

            Texture2D texture = new Texture2D(width, height);
            texture.LoadImage(readBinary);

            return texture;
        }

        static byte[] ReadBytes(string path)
        {
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader bin = new BinaryReader(fileStream);
            byte[] values = bin.ReadBytes((int)bin.BaseStream.Length);

            bin.Close();

            return values;
        }
    }
}
