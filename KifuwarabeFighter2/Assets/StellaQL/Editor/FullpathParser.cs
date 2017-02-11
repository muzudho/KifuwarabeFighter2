using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace StellaQL
{
    public class FullpathTokens
    {
        public FullpathTokens():this("")
        {

        }
        public FullpathTokens(string matchedSyntaxName)
        {
            Clear(matchedSyntaxName);
        }
        public void Clear(string matchedSyntaxName)
        {
            MatchedSyntaxName = matchedSyntaxName;
            LayerNameEndsWithoutDot = "";
            StatemachineNamesEndsWithoutDot = new List<string>();
        }

        /// <summary>
        /// "Base Layer." にヒットしても、末尾のドットを除いて "Base Layer" を保持する。
        /// </summary>
        public string LayerNameEndsWithoutDot { get; set; }
        public List<string> StatemachineNamesEndsWithoutDot { get; set; }

        /// <summary>
        /// 構文該当なしのとき、どの構文に一番多くの文字数が　該当したかを調べるための名前。
        /// </summary>
        public string MatchedSyntaxName { get; set; }
        /// <summary>
        /// 構文該当なしのとき、どの構文の何文字目まで　該当したかを調べるための数字。
        /// </summary>
        public int MatchedSyntaxCaret { get; set; }
    }

    /// <summary>
    /// フルパス・シンタックス・パーサー
    /// </summary>
    public abstract class FullpathSyntaxP
    {
        public static bool NotMatched(FullpathTokens current, int caret, ref FullpathTokens max)
        {
            if (max.MatchedSyntaxCaret < caret) { current.MatchedSyntaxCaret = caret; max = current; }
            return false;
        }

        /// <summary>
        /// "Base Layer." や、"Base Layer.Alpaca" や、"Base Layer.Alpaca.Bear" などがＯＫ。ステートマシン名は無いこともある。
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ref_caret"></param>
        /// <param name="maxFt"></param>
        /// <returns></returns>
        public static bool Fixed_LayerName_And_StatemachineNames(string query, ref int ref_caret, ref FullpathTokens maxFt)
        {
            FullpathTokens ft = new FullpathTokens("LayerName_And_StatemachineNames");
            int caret = ref_caret;
            string layerNameEndsWithoutDot;
            List<string> statemachineNamesEndsWithoutDot;

            if (!FullpathLexcalP.VarLayerName(query, ref caret, out layerNameEndsWithoutDot)) { return NotMatched(maxFt, caret, ref maxFt); }
            ft.LayerNameEndsWithoutDot = layerNameEndsWithoutDot;

            // ステートマシン名はオプション。
            if (FullpathLexcalP.VarStatemachineNames(query, ref caret, out statemachineNamesEndsWithoutDot)) { ft.StatemachineNamesEndsWithoutDot = statemachineNamesEndsWithoutDot; }

            maxFt = ft; ref_caret = caret; return true;
        }

        /// <summary>
        /// ドット(.) または文末までがレイヤー名。（FIXME: 実際はレイヤー名にはドットを含むことができるが、運用で避けるものとする）
        /// </summary>
        /// <param name="query"></param>
        /// <param name="caret"></param>
        /// <returns></returns>
        public static bool Fixed_LayerName(string query, ref int ref_caret, ref FullpathTokens maxFt)
        {
            FullpathTokens ft = new FullpathTokens("LayerName");
            int caret = ref_caret;
            string layerNameEndsWithoutDot;

            if (!FullpathLexcalP.VarLayerName(query, ref caret, out layerNameEndsWithoutDot)) { return NotMatched(maxFt, caret, ref maxFt); }
            ft.LayerNameEndsWithoutDot = layerNameEndsWithoutDot;

            maxFt = ft; ref_caret = caret; return true;
        }
    }

    /// <summary>
    /// フルパス・レキシカル・パーサー
    /// </summary>
    public abstract class FullpathLexcalP
    {
        public static bool VarStatemachineNames(string query, ref int ref_caret, out List<string> statemachineNamesEndsWithoutDot)
        {
            const int LEAF = 1;
            int caret = ref_caret;

            string[] nodes = query.Substring(caret).Split('.'); // レイヤー名を省いたパス。葉以外はステートマシン名か。途中のステートマシン名は無いこともある。

            statemachineNamesEndsWithoutDot = new List<string>();
            if (1 < nodes.Length) // ステートマシンが途中にある場合、最後のステートマシンまで降りていく。
            {
                for (int i = 0; i < nodes.Length - LEAF; i++)//葉を除く
                {
                    statemachineNamesEndsWithoutDot.Add(nodes[i]);
                    caret += nodes[i].Length + ".".Length;
                }
                ref_caret = caret; return true;
            }
            else { return false; }// 該当なし
        }

        /// <summary>
        /// "Base Layer." または "Base Layer" にヒットする。
        /// 返す文字列は 末尾のドットを除いた "Base Layer" の方。
        /// </summary>
        private static Regex regexLayerNameEndsWithDot = new Regex(@"^([\w\s]+)(\.)", RegexOptions.IgnoreCase);
        public static bool VarLayerName(string query, ref int caret, out string layerNameEndsWithoutDot)
        {
            Match match = regexLayerNameEndsWithDot.Match(query.Substring(caret));
            if (match.Success) {
                layerNameEndsWithoutDot = match.Groups[1].Value; caret += layerNameEndsWithoutDot.Length;
                if ("."==match.Groups[2].Value) { caret += match.Groups[2].Value.Length; }
                return true;
            }
            layerNameEndsWithoutDot = ""; return false;
        }
    }
}
