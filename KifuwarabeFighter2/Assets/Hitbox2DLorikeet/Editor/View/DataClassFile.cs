namespace DojinCircleGrayscale.Hitbox2DLorikeetMaker
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using Assets.Hitbox2DLorikeet.Editor.Maker.Helper;

    /// <summary>
    /// ・当たり判定は、１スライスにつき、１個所とする。
    /// ・当たり判定は、矩形のみとする。
    /// ・当たり判定は、グリッドに沿っているものとする。
    /// ・当たり判定は、重なっていないものとする。
    /// </summary>
    public class DataClassFile
    {
        public const string FILE_PATH = "./Assets/Hitbox2DLorikeet/Data/Hitbox2DLorikeet_Data.cs";
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
        /// <summary>
        /// ユニティのグリッドは1つ100ピクセル？
        /// </summary>
        public const float UNITY_GRID_UNIT = 100.0f;

        public static void Generate(StringBuilder info_message)
        {
            #region Get Filepaths
            // 青、赤、黄と３箱読み取るので、読取ファイル数はファイルの数の３倍になります。
            string[] filepaths = Directory.GetFiles("./Assets/Hitbox2DLorikeet/Editor/Work", "*.png");
            info_message.AppendLine("filepaths.Length = " + filepaths.Length);
            #endregion

            // 大文字・小文字を無視してソート☆
            Array.Sort(filepaths, StringComparer.OrdinalIgnoreCase);

            StringBuilder script = new StringBuilder();
            script.AppendLine(@"namespace DojinCircleGrayscale.Hitbox2DLorikeet
{

    /// <summary>
    /// This file was automatically generated.
    /// It was created by [Generate C# (Data)] button.
    /// </summary>
    public class LorikeetData
    {
        #region Singleton
        static LorikeetData()
        {
            Instance = new LorikeetData();
        }
        /// <summary>
        /// シングルトン・デザインパターンとして作っています。
        /// I am making this class as a singleton design pattern.
        /// </summary>
        public static LorikeetData Instance { get; private set; }
        #endregion

        LorikeetData()
        {
            LorikeetBoxes = new LorikeetBox[]
            {
                HitboxData.Instance,
                WeakboxData.Instance,
                StrongboxData.Instance,
            };
        }
        LorikeetBox[] LorikeetBoxes { get; }
    }
");

            ColorBoxStatus[] boxesSettings = new ColorBoxStatus[]
            {
                //Utility_BoxScanner.m_blueBox,
                //Utility_BoxScanner.m_yellowBox,
                //Utility_BoxScanner.m_redBox,
                BoxScannerHelper.m_redBox,
                BoxScannerHelper.m_blueBox,
                BoxScannerHelper.m_yellowBox,
            };

            foreach(ColorBoxStatus boxSettings in boxesSettings)
            {
                List<List<SliceRectangleStatus>> image_to_slice_to_rectangleList = new List<List<SliceRectangleStatus>>();
                foreach (string filepath in filepaths)
                {
                    info_message.AppendLine("画像読込 : " + filepath);
                    BoxScannerHelper.Execute_1Image(boxSettings, image_to_slice_to_rectangleList, filepath, info_message);
                    info_message.AppendLine("list.Count : " + image_to_slice_to_rectangleList.Count);
                }

                {
                    StringBuilder sb_dammy = new StringBuilder();
                    foreach (List<SliceRectangleStatus> list in image_to_slice_to_rectangleList)
                    {
                        foreach (SliceRectangleStatus rect in list)
                        {
                            info_message.AppendLine("確認 : rect.SliceNumber=[" + rect.SliceNumber+ "] rect.Slice=[" + rect.Slice+ "] rect.Collider=[" + rect.Collider+ "] rect.GetOffsetX()=[" + rect.GetOffsetX(sb_dammy) + "] rect.GetOffsetY()=[" + rect.GetOffsetY(sb_dammy) + "] rect.GetScaleX()=[" + rect.GetScaleX()+ "] rect.GetScaleY()=[" + rect.GetScaleY()+"]" );
                        }
                    }
                }

                string text = Hitbox2DClassFile.ToText(boxSettings.m_outputClassName, image_to_slice_to_rectangleList, info_message);
                System.Console.WriteLine(text);
                script.Append(text);
            }

            script.AppendLine("}");
            File.WriteAllText(DataClassFile.FILE_PATH, script.ToString());


            // おわり☆
            System.Console.WriteLine("Please, push any key.");
            System.Console.ReadKey();
        }

    }
}
