using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DojinCircleGrayscale.StellaQL
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
            StateName = "";
        }
        // クローン
        public FullpathTokens(FullpathTokens source)
        {
            LayerNameEndsWithoutDot = source.LayerNameEndsWithoutDot;
            MatchedSyntaxCaret = source.MatchedSyntaxCaret;
            MatchedSyntaxName = source.MatchedSyntaxName;
            StatemachineNamesEndsWithoutDot = new List<string>(source.StatemachineNamesEndsWithoutDot);
            StateName = source.StateName;
        }

        /// <summary>
        /// "Base Layer." にヒットしても、末尾のドットを除いて "Base Layer" を保持する
        /// </summary>
        public string LayerNameEndsWithoutDot { get; set; }
        public List<string> StatemachineNamesEndsWithoutDot { get; set; }
        /// <summary>
        /// 例えば "Alpaca" "Bear" "Cat" を、"Alpaca.Bear.Cat" に連結して返す
        /// </summary>
        public string StatemachinePath { get { return string.Join(".", StatemachineNamesEndsWithoutDot.ToArray()); } }
        public string StateName { get; set; }

        /// <summary>
        /// 構文該当なしのとき、どの構文に一番多くの文字数が　該当したかを調べるための名前
        /// </summary>
        public string MatchedSyntaxName { get; set; }
        /// <summary>
        /// 構文該当なしのとき、どの構文の何文字目まで　該当したかを調べるための数字
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
        /// ドット(.) または文末までがレイヤー名。
        /// 
        /// (FIXME: 実際はレイヤー名にはドットを含むことができるが、運用で避けるものとする)
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

        /// <summary>
        /// "Base Layer." や、"Base Layer.Alpaca" や、"Base Layer.Alpaca.Bear" などがＯＫ。
        /// 
        /// ステートマシン名は無いこともある。
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
        /// "Base Layer.Alpaca" や、"Base Layer.Alpaca.Bear" などがＯＫ。
        /// 
        /// ステートマシン名は無いこともある。
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ref_caret"></param>
        /// <param name="maxFt"></param>
        /// <returns></returns>
        public static bool Fixed_LayerName_And_StatemachineNames_And_StateName(string query, ref int ref_caret, ref FullpathTokens maxFt)
        {
            FullpathTokens ft = new FullpathTokens("LayerName_And_StatemachineNames_And_StateName");
            int caret = ref_caret;
            string layerNameEndsWithoutDot;
            List<string> statemachineNamesEndsWithoutDot;
            string stateName;

            if (!FullpathLexcalP.VarLayerName(query, ref caret, out layerNameEndsWithoutDot)) { return NotMatched(maxFt, caret, ref maxFt); }
            ft.LayerNameEndsWithoutDot = layerNameEndsWithoutDot;

            // ステートマシン名はオプション。
            if (FullpathLexcalP.VarStatemachineNames(query, ref caret, out statemachineNamesEndsWithoutDot)) { ft.StatemachineNamesEndsWithoutDot = statemachineNamesEndsWithoutDot; }

            if (!FullpathLexcalP.VarStateName(query, ref caret, out stateName)) { return NotMatched(maxFt, caret, ref maxFt); }
            ft.StateName = stateName;

            maxFt = ft; ref_caret = caret; return true;
        }

        /// <summary>
        /// "Base Layer.Alpaca" や、"Base Layer.Alpaca.Bear" などがＯＫ。
        /// 
        /// ステートマシン名は無いこともある。
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ref_caret"></param>
        /// <param name="maxFt"></param>
        /// <returns></returns>
        public static bool Continued_Fixed_StateName(string query, ref int ref_caret, FullpathTokens baseFt, ref FullpathTokens maxFt)
        {
            // クローンし、追加していく。
            FullpathTokens ft = new FullpathTokens(baseFt);
            int caret = ref_caret;
            string stateName;

            if (!FullpathLexcalP.VarStateName(query, ref caret, out stateName)) { return NotMatched(maxFt, caret, ref maxFt); }
            ft.StateName = stateName;

            maxFt = ft; ref_caret = caret; return true;
        }
    }

    /// <summary>
    /// フルパス・レキシカル・パーサー
    /// </summary>
    public abstract class FullpathLexcalP
    {
        /// <summary>
        /// "Base Layer." または "Base Layer" にヒットする。
        /// 
        /// 返す文字列は 末尾のドットを除いた "Base Layer" の方。
        /// </summary>
        private static Regex regexLayerNameEndsWithDot = new Regex(@"^([\w\s]+)(\.)", RegexOptions.IgnoreCase);
        public static bool VarLayerName(string query, ref int caret, out string layerNameEndsWithoutDot)
        {
            Match match = regexLayerNameEndsWithDot.Match(query.Substring(caret));
            if (match.Success)
            {
                layerNameEndsWithoutDot = match.Groups[1].Value; caret += layerNameEndsWithoutDot.Length;
                if ("." == match.Groups[2].Value) { caret += match.Groups[2].Value.Length; }
                return true;
            }
            layerNameEndsWithoutDot = ""; return false;
        }

        public static bool VarStatemachineNames(string query, ref int ref_caret, out List<string> statemachineNamesEndsWithoutDot)
        {
            const int LEAF = 1;
            int caret = ref_caret;

            // レイヤー名を省いたパス。葉以外はステートマシン名か。途中のステートマシン名は無いこともある。
            string[] nodes = query.Substring(caret).Split('.');

            statemachineNamesEndsWithoutDot = new List<string>();

            // ステートマシンが途中にある場合、最後のステートマシンまで降りていく。
            if (1 < nodes.Length)
            {
                // 葉を除く
                for (int i = 0; i < nodes.Length - LEAF; i++)
                {
                    statemachineNamesEndsWithoutDot.Add(nodes[i]);
                    caret += nodes[i].Length + ".".Length;
                }
                ref_caret = caret; return true;
            }
            // 該当なし
            else { return false; }
        }

        public static bool VarStateName(string query, ref int ref_caret, out string stateName)
        {
            int caret = ref_caret;

            // FIXME: 後ろ全部　ステート名ということにしておく。
            stateName = query.Substring(caret);

            caret += stateName.Length;
            ref_caret = caret; return true;
        }
    }
}
