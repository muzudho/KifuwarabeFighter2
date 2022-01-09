namespace DojinCircleGrayscale.Hitbox2DLorikeetMaker
{
    using System.Collections.Generic;
    using System.Text;

    public abstract class Hitbox2DClassFile
    {
        public static string ToText(string className, List<List<SliceRectangleStatus>> image_to_slice_to_rectangleList, StringBuilder info_message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("    public class "); sb.Append(className); sb.Append(@" : LorikeetBox
    {
        #region Singleton
        static "); sb.Append(className); sb.Append(@"()
        {
            Instance = new "); sb.Append(className); sb.Append(@"();
        }
        /// <summary>
        /// シングルトン・デザインパターンとして作っています。
        /// I am making this class as a singleton design pattern.
        /// </summary>
        public static "); sb.Append(className); sb.Append(@" Instance { get; private set; }
        #endregion

        "); sb.Append(className); sb.Append(@"()
        {
            imageAndSlice_To_OffsetX = new float[,]
            {
");
            foreach (List<SliceRectangleStatus> slice_to_rectangleList in image_to_slice_to_rectangleList)
            {
                // コメント行
                {
                    sb.Append("                //");
                    int slice = 0;
                    foreach (SliceRectangleStatus rect in slice_to_rectangleList)
                    {
                        sb.Append(string.Format("{0,12}, ", slice));
                        slice++;
                    }
                    sb.AppendLine();
                }
                break;
            }
            foreach (List<SliceRectangleStatus> sliceList in image_to_slice_to_rectangleList)
            {
                sb.Append("                { ");
                foreach (SliceRectangleStatus rect in sliceList)
                {
                    sb.Append(string.Format("{0,11:F6}f, ", rect.GetOffsetX(info_message)));
                }
                sb.AppendLine(" },");
            }
            sb.Append(@"            };

            imageAndSlice_To_OffsetY = new float[,]
            {
");
            foreach (List<SliceRectangleStatus> sliceList in image_to_slice_to_rectangleList)
            {
                // コメント行
                {
                    sb.Append("                //");
                    int slice = 0;
                    foreach (SliceRectangleStatus rect in sliceList)
                    {
                        sb.Append(string.Format("{0,12}, ", slice));
                        slice++;
                    }
                    sb.AppendLine();
                }
                break;
            }
            foreach (List<SliceRectangleStatus> sliceList in image_to_slice_to_rectangleList)
            {
                sb.Append("                { ");
                foreach (SliceRectangleStatus rect in sliceList)
                {
                    sb.Append(string.Format("{0,11:F6}f, ", rect.GetOffsetY(info_message)));
                }
                sb.AppendLine(" },");
            }
            sb.Append(@"            };

            imageAndSlice_To_ScaleX = new float[,]
            {
");
            foreach (List<SliceRectangleStatus> sliceList in image_to_slice_to_rectangleList)
            {
                // コメント行
                {
                    sb.Append("                //");
                    int slice = 0;
                    foreach (SliceRectangleStatus rect in sliceList)
                    {
                        sb.Append(string.Format("{0,12}, ", slice));
                        slice++;
                    }
                    sb.AppendLine();
                }
                break;
            }
            foreach (List<SliceRectangleStatus> sliceList in image_to_slice_to_rectangleList)
            {
                sb.Append("                { ");
                foreach (SliceRectangleStatus rect in sliceList)
                {
                    sb.Append(string.Format("{0,11:F6}f, ", rect.GetScaleX()));
                }
                sb.AppendLine(" },");
            }
            sb.Append(@"            };

            imageAndSlice_To_ScaleY = new float[,]
            {
");
            foreach (List<SliceRectangleStatus> sliceList in image_to_slice_to_rectangleList)
            {
                // コメント行
                {
                    sb.Append("                //");
                    int slice = 0;
                    foreach (SliceRectangleStatus rect in sliceList)
                    {
                        sb.Append(string.Format("{0,12}, ", slice));
                        slice++;
                    }
                    sb.AppendLine();
                }
                break;
            }
            foreach (List<SliceRectangleStatus> sliceList in image_to_slice_to_rectangleList)
            {
                sb.Append("                { ");
                foreach (SliceRectangleStatus rect in sliceList)
                {
                    sb.Append(string.Format("{0,11:F6}f, ", rect.GetScaleY()));
                }
                sb.AppendLine(" },");
            }
            sb.AppendLine(@"            };
        }
        public float[,] imageAndSlice_To_OffsetX;
        public float[,] imageAndSlice_To_OffsetY;
        public float[,] imageAndSlice_To_ScaleX;
        public float[,] imageAndSlice_To_ScaleY;
    }");

            return sb.ToString();
        }
    }
}
