using System.Collections.Generic;
using System.Text;

namespace DojinCircleGrayscale.Hitbox2DMaker
{
    public abstract class Utility_Hitbox2DClassFormat
    {
        public static string ToText(string className, List<List<RectangleOnSlice>> image_to_slice_to_rectangleList)
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
            foreach (List<RectangleOnSlice> slice_to_rectangleList in image_to_slice_to_rectangleList)
            {
                // コメント行
                {
                    sb.Append("                    //");
                    int slice = 0;
                    foreach (RectangleOnSlice rect in slice_to_rectangleList)
                    {
                        sb.Append(string.Format("{0,12}, ", slice));
                        slice++;
                    }
                    sb.AppendLine();
                }
                break;
            }
            foreach (List<RectangleOnSlice> sliceList in image_to_slice_to_rectangleList)
            {
                sb.Append("                    { ");
                foreach (RectangleOnSlice rect in sliceList)
                {
                    sb.Append(string.Format("{0,11:F6}f, ", rect.GetOffsetX()));
                }
                sb.AppendLine(" },");
            }
            sb.Append(@"            };

            imageAndSlice_To_OffsetY = new float[,]
            {
");
            foreach (List<RectangleOnSlice> sliceList in image_to_slice_to_rectangleList)
            {
                // コメント行
                {
                    sb.Append("                //");
                    int slice = 0;
                    foreach (RectangleOnSlice rect in sliceList)
                    {
                        sb.Append(string.Format("{0,12}, ", slice));
                        slice++;
                    }
                    sb.AppendLine();
                }
                break;
            }
            foreach (List<RectangleOnSlice> sliceList in image_to_slice_to_rectangleList)
            {
                sb.Append("                { ");
                foreach (RectangleOnSlice rect in sliceList)
                {
                    sb.Append(string.Format("{0,11:F6}f, ", rect.GetOffsetY()));
                }
                sb.AppendLine(" },");
            }
            sb.Append(@"            };

            imageAndSlice_To_ScaleX = new float[,]
            {
");
            foreach (List<RectangleOnSlice> sliceList in image_to_slice_to_rectangleList)
            {
                // コメント行
                {
                    sb.Append("                //");
                    int slice = 0;
                    foreach (RectangleOnSlice rect in sliceList)
                    {
                        sb.Append(string.Format("{0,12}, ", slice));
                        slice++;
                    }
                    sb.AppendLine();
                }
                break;
            }
            foreach (List<RectangleOnSlice> sliceList in image_to_slice_to_rectangleList)
            {
                sb.Append("                { ");
                foreach (RectangleOnSlice rect in sliceList)
                {
                    sb.Append(string.Format("{0,11:F6}f, ", rect.GetScaleX()));
                }
                sb.AppendLine(" },");
            }
            sb.Append(@"            };

            imageAndSlice_To_ScaleY = new float[,]
            {
");
            foreach (List<RectangleOnSlice> sliceList in image_to_slice_to_rectangleList)
            {
                // コメント行
                {
                    sb.Append("                //");
                    int slice = 0;
                    foreach (RectangleOnSlice rect in sliceList)
                    {
                        sb.Append(string.Format("{0,12}, ", slice));
                        slice++;
                    }
                    sb.AppendLine();
                }
                break;
            }
            foreach (List<RectangleOnSlice> sliceList in image_to_slice_to_rectangleList)
            {
                sb.Append("                { ");
                foreach (RectangleOnSlice rect in sliceList)
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
