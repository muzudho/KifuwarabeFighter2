using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace StellaQL
{
    /// <summary>
    /// 解説: 「UnityEditorを使って2D格闘(2D Fighting game)作るときのモーション遷移図作成の半自動化に挑戦しよう＜その４＞」 http://qiita.com/muzudho1/items/baf4b06cdcda96ca9a11
    /// </summary>
    public abstract class Util_StellaQL
    {
        /// <summary>
        /// 列挙型の扱い方：「文字列を列挙体に変換する」（DOBON.NET） http://dobon.net/vb/dotnet/programing/enumparse.html
        /// </summary>
        /// <param name="expression">例えば "( [ ( Alpha Cee ) ( Beta Dee ) ] { Eee } )" といった式。</param>
        /// <returns></returns>
        public static List<int> Execute(string expression, Type enumration)
        {
            List<int> result = new List<int>();
            List<string> tokens = new List<string>();

            // 字句解析
            {
                Util_StellaQL.String_to_tokens(expression, tokens);
                //StringBuilder sb = new StringBuilder();
                //foreach (string token in tokens)
                //{
                //    sb.Append("token: ");
                //    sb.AppendLine(token);
                //}
                //Debug.Log( sb.ToString() );
            }

            Stack<char> stack = new Stack<char>();

            // スキャン（XMLのSAX風）
            StellaQLScanner scanner = new StellaQLScanner(enumration);
            scanner.Scan(tokens);
            {
            }

            object enumElement = Enum.Parse(enumration, "Num"); // 変換できなかったら例外を投げる
            Debug.Log("enumElement = " + enumElement.ToString() + " typeof = " + enumElement.GetType());

            return result;
        }

        private static void String_to_tokens(string expression, List<string> tokens)
        {
            StringBuilder word = new StringBuilder();

            for (int iCaret = 0; iCaret < expression.Length; iCaret++)
            {
                char ch = expression[iCaret];
                switch (ch)
                {
                    case ' ':
                        if (0<word.Length)
                        {
                            tokens.Add(word.ToString());
                            word.Length = 0;
                        }
                        break;
                    case '(':
                    case '[':
                    case '{':
                    case ')':
                    case ']':
                    case '}':
                        tokens.Add(ch.ToString());
                        break;
                    default: word.Append(ch); break;
                }
            }

            if (0 < word.Length)//構文エラー
            {
                tokens.Add(word.ToString());
                word.Length = 0;
            }
        }
    }

    public abstract class AbstractStellaQLScanner
    {
        public void Scan(List<string> tokens)
        {
            string openParen = ""; // 閉じ括弧に対応する、「開きカッコ」
            int iCursor = 0;
            int lockerIndex = 0; // 部室のロッカー番号。スタートは 0 番から。
            while (iCursor < tokens.Count)
            {
                string token = tokens[iCursor];
                if ("" == openParen)
                {
                    this.OnGo(iCursor, token);
                    switch (token)
                    {
                        case ")": openParen = "("; tokens[iCursor] = ""; break;
                        case "]": openParen = "["; tokens[iCursor] = ""; break;
                        case "}": openParen = "{"; tokens[iCursor] = ""; break;
                        default: break; // 無視して進む
                    }
                }
                else // 後ろに進む☆
                {
                    this.OnBack(iCursor, token);
                    switch (token)
                    {
                        case "(":
                        case "[":
                        case "{": if (openParen == token) { openParen = ""; this.OnOpenParen(iCursor, token); lockerIndex++; } break;
                        default: this.OnKeywordGet(iCursor, token, lockerIndex); break;
                    }
                    tokens[iCursor] = ""; // 後ろに進んだ先では、その文字を削除する
                }

                if ("" == openParen) { iCursor++; }
                else { iCursor--; }
            }
        }

        abstract public void OnGo(int iCursor, string token);
        abstract public void OnBack(int iCursor, string token);
        abstract public void OnKeywordGet(int iCursor, string token, int locker);
        abstract public void OnOpenParen(int iCursor, string token);
    }

    public class StellaQLScanner : AbstractStellaQLScanner
    {
        /// <summary>
        /// ほんとは列挙型の要素として持っておきたいが、型指定できないので int 型として持っておく。
        /// </summary>
        private List<int> bufferKeywordEnums;
        private Type enumration;
        /// <summary>
        /// ほんとは列挙型の要素として持っておきたいが、型指定できないので int 型として持っておく。
        /// ２重のリストになっており、[ロッカー番号][要素番号] となる。
        /// </summary>
        private List<List<int>> lockerAttr;

        public StellaQLScanner(Type enumration)
        {
            this.enumration = enumration;
            this.bufferKeywordEnums = new List<int>();
            this.lockerAttr = new List<List<int>>();
        }

        public override void OnGo(int iCursor, string token)
        {
            StringBuilder sb1 = new StringBuilder();
            sb1.Append("go["); sb1.Append(iCursor); sb1.Append("]: "); sb1.Append(token); sb1.AppendLine();
            Debug.Log(sb1.ToString());
        }

        public override void OnBack(int iCursor, string token)
        {
            StringBuilder sb2 = new StringBuilder();
            sb2.Append("back["); sb2.Append(iCursor); sb2.Append("]: "); sb2.Append(token); sb2.AppendLine();
            Debug.Log(sb2.ToString());
        }

        public override void OnKeywordGet(int iCursor, string token, int locker)
        {
            object enumElement = Enum.Parse(enumration, token); // 変換できなかったら例外を投げる
            this.bufferKeywordEnums.Add((int)enumElement);// 列挙型だが、int 型に変換。
        }

        public override void OnOpenParen(int iCursor, string token)
        {
            switch (token)
            {
                case "(":
                    this.lockerAttr.Add(
                        Keyword_to_locker(this.bufferKeywordEnums, enumration));
                    break;
                case "[":
                    this.lockerAttr.Add(
                        KeywordList_to_locker(this.bufferKeywordEnums, enumration));
                    break;
                case "{":
                    this.lockerAttr.Add(
                        NGKeywordList_to_locker(this.bufferKeywordEnums, enumration));
                    break;
            }
            this.bufferKeywordEnums.Clear();
        }

        public static Dictionary<int, AstateRecordable> Filtering_AndAttributes(List<int> attrs, Dictionary<int, AstateRecordable> universe)
        {
            Dictionary<int, AstateRecordable> hitRecords = new Dictionary<int, AstateRecordable>(universe);
            foreach (int attr in attrs)
            {
                Dictionary<int, AstateRecordable> records_empty = new Dictionary<int, AstateRecordable>();
                foreach (KeyValuePair<int, AstateRecordable> pair in hitRecords)
                {
                    if (pair.Value.HasFlag_attr(attr)) { records_empty.Add(pair.Key, pair.Value); }// 該当したもの
                }
                hitRecords = records_empty;
            }
            return hitRecords;
        }

        public static Dictionary<int, AstateRecordable> Filtering_StateFullNameRegex(string pattern, Dictionary<int, AstateRecordable> universe)
        {
            Dictionary<int, AstateRecordable> hitRecords = new Dictionary<int, AstateRecordable>();

            Regex regex = new Regex(pattern);
            foreach (KeyValuePair<int, AstateRecordable> pair in universe)
            {
                if(regex.IsMatch(pair.Value.BreadCrumb + pair.Value.Name))
                {
                    hitRecords.Add(pair.Key, pair.Value);
                }
            }

            return hitRecords;
        }

        public static Dictionary<int, AstateRecordable> Filtering_OrAttributes(List<int> attrs, Dictionary<int, AstateRecordable> universe)
        {
            HashSet<int> distinctAttr = new HashSet<int>();// まず属性の重複を除外
            foreach (int attr in attrs) { distinctAttr.Add(attr); }

            HashSet<int> hitRecordIndexes = new HashSet<int>();// レコード・インデックスを属性検索（重複除外）
            foreach (KeyValuePair<int, AstateRecordable> pair in universe)
            {
                foreach (int attr in distinctAttr)
                {
                    if (pair.Value.HasFlag_attr(attr)) { hitRecordIndexes.Add(pair.Key); }
                }
            }

            Dictionary<int, AstateRecordable> hitRecords = new Dictionary<int, AstateRecordable>();
            foreach (int recordIndex in hitRecordIndexes) { hitRecords.Add(recordIndex, universe[recordIndex]); }
            return hitRecords;
        }

        public static Dictionary<int, AstateRecordable> Filtering_NotAndNotAttributes(List<int> attrs, Dictionary<int, AstateRecordable> universe)
        {
            HashSet<int> distinctAttr = new HashSet<int>();// まず属性の重複を除外
            foreach (int attr in attrs) { distinctAttr.Add(attr); }

            HashSet<int> hitRecordIndexes = new HashSet<int>();// レコード・インデックスを属性検索（重複除外）
            foreach (KeyValuePair<int, AstateRecordable> pair in universe)
            {
                foreach (int attr in distinctAttr)
                {
                    if (pair.Value.HasFlag_attr(attr)) { hitRecordIndexes.Add(pair.Key); }
                }
            }

            List<int> complementRecordIndexes = new List<int>();// 補集合を取る
            {
                foreach (int recordIndex in universe.Keys) { complementRecordIndexes.Add(recordIndex); }// 列挙型の中身をリストに移動。
                for (int iComp = complementRecordIndexes.Count - 1; -1 < iComp; iComp--)// 後ろから指定の要素を削除する。
                {
                    if (hitRecordIndexes.Contains(complementRecordIndexes[iComp]))
                    {
                        // Debug.Log("Remove[" + iComp + "] (" + complementRecordIndexes[iComp] + ")");
                        complementRecordIndexes.RemoveAt(iComp);
                    }
                    // else { Debug.Log("Tick[" + iComp + "] (" + complementRecordIndexes[iComp] + ")"); }
                }
            }

            Dictionary<int, AstateRecordable> hitRecords = new Dictionary<int, AstateRecordable>();
            foreach (int recordIndex in complementRecordIndexes) { hitRecords.Add(recordIndex, universe[recordIndex]); }
            return hitRecords;
        }

        public static List<int> Keyword_to_locker(List<int> set, Type enumration)
        { // 列挙型要素を OR 結合して持つ。
            List<int> attrs = new List<int>();
            int sum = (int)Enum.GetValues(enumration).GetValue(0);//最初の要素は 0 にしておくこと。 列挙型だが、int 型に変換。
            foreach (object elem in set) { sum |= (int)elem; }// OR結合
            attrs.Add(sum); // 列挙型の要素を結合したものを int型として入れておく。
            return attrs;
        }

        public static List<int> KeywordList_to_locker(List<int> set, Type enumration)
        { // 列挙型要素を １つ１つ　ばらばらに持つ。
            List<int> attrs = new List<int>();
            foreach (int elem in set) { attrs.Add(elem); }// 列挙型の要素を１つ１つ入れていく。
            return attrs;
        }

        public static List<int> NGKeywordList_to_locker(List<int> set, Type enumration)
        {
            return Complement(set, enumration); // 補集合を返すだけ☆
        }

        /// <summary>
        /// 補集合
        /// </summary>
        public static List<int> Complement(List<int> set, Type enumration)
        {
            List<int> complement = new List<int>();
            {
                // 列挙型の中身をリストに移動。
                foreach (int elem in Enum.GetValues(enumration)) { complement.Add(elem); }
                // 後ろから指定の要素を削除する。
                for (int iComp = complement.Count - 1; -1 < iComp; iComp--)
                {
                    if (set.Contains(complement[iComp]))
                    {
                        Debug.Log("Remove[" + iComp + "] ("+ complement[iComp]+")");
                        complement.RemoveAt(iComp);
                    }
                    else
                    {
                        Debug.Log("Tick[" + iComp + "] (" + complement[iComp] + ")");
                    }
                }
            }
            return complement;
        }
    }

}
