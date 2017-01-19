using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hitbox2DMaker
{
    public abstract class Utility_Hitbox2DClassFormat
    {
        public static string ToText(string className, List<List<RectangleOnSlice>> image_to_slice_to_rectangleList)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public abstract class ");
            sb.AppendLine(className);
            sb.AppendLine("{");
            sb.AppendLine("    public static float[,] imageAndSlice_To_OffsetX = new float[,]");
            sb.AppendLine("    {");
            foreach (List<RectangleOnSlice> slice_to_rectangleList in image_to_slice_to_rectangleList)
            {
                // コメント行
                {
                    sb.Append("        //");
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
                sb.Append("        { ");
                foreach (RectangleOnSlice rect in sliceList)
                {
                    sb.Append(string.Format("{0,11:F6}f, ", rect.GetOffsetX()));
                }
                sb.AppendLine(" },");
            }
            sb.AppendLine("    };");
            sb.AppendLine("    public static float[,] imageAndSlice_To_OffsetY = new float[,]");
            sb.AppendLine("    {");
            foreach (List<RectangleOnSlice> sliceList in image_to_slice_to_rectangleList)
            {
                // コメント行
                {
                    sb.Append("        //");
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
                sb.Append("        { ");
                foreach (RectangleOnSlice rect in sliceList)
                {
                    sb.Append(string.Format("{0,11:F6}f, ", rect.GetOffsetY()));
                }
                sb.AppendLine(" },");
            }
            sb.AppendLine("    };");
            sb.AppendLine("    public static float[,] imageAndSlice_To_ScaleX = new float[,]");
            sb.AppendLine("    {");
            foreach (List<RectangleOnSlice> sliceList in image_to_slice_to_rectangleList)
            {
                // コメント行
                {
                    sb.Append("        //");
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
                sb.Append("        { ");
                foreach (RectangleOnSlice rect in sliceList)
                {
                    sb.Append(string.Format("{0,11:F6}f, ", rect.GetScaleX()));
                }
                sb.AppendLine(" },");
            }
            sb.AppendLine("    };");
            sb.AppendLine("    public static float[,] imageAndSlice_To_ScaleY = new float[,]");
            sb.AppendLine("    {");
            foreach (List<RectangleOnSlice> sliceList in image_to_slice_to_rectangleList)
            {
                // コメント行
                {
                    sb.Append("        //");
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
                sb.Append("        { ");
                foreach (RectangleOnSlice rect in sliceList)
                {
                    sb.Append(string.Format("{0,11:F6}f, ", rect.GetScaleY()));
                }
                sb.AppendLine(" },");
            }
            sb.AppendLine("    };");
            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}
