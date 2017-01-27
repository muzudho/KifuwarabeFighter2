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
            List<string> tokens;

            // 字句解析
            {
                AttrParenthesisParser.String_to_tokens(expression, out tokens);
                //StringBuilder sb = new StringBuilder();
                //foreach (string token in tokens)
                //{
                //    sb.Append("token: ");
                //    sb.AppendLine(token);
                //}
                //Debug.Log( sb.ToString() );
            }

            Stack<char> stack = new Stack<char>();

            //// スキャン（XMLのSAX風）
            //LockersScanner scanner = new LockersScanner(enumration);
            //scanner.Scan(tokens);
            //{
            //}

            //object enumElement = Enum.Parse(enumration, "Num"); // 変換できなかったら例外を投げる
            //Debug.Log("enumElement = " + enumElement.ToString() + " typeof = " + enumElement.GetType());

            return result;
        }
    }

    public class QueryTokens
    {
        public QueryTokens()
        {
            Target = "";
            Manipulation = "";
            Set = new Dictionary<string, string>();
            From_FullnameRegex = "";
            From_Attr = "";
            To_FullnameRegex = "";
            To_Attr = "";
        }

        public const string TRANSITION = "TRANSITION";
        public const string INSERT = "INSERT";
        public const string UPDATE = "UPDATE";
        public const string DELETE = "DELETE";
        public const string SELECT = "SELECT";
        public const string SET = "SET";
        public const string FROM = "FROM";
        public const string TO = "TO";
        public const string ATTR = "ATTR";

        /// <summary>
        /// Transition の１つ。大文字小文字は区別しない。
        /// </summary>
        public string Target { get; set; }
        /// <summary>
        /// Insert、Update、Delete、Selectのいずれか。大文字小文字は区別しない。
        /// </summary>
        public string Manipulation { get; set; }
        /// <summary>
        /// SET部。大文字小文字は区別したい。
        /// </summary>
        public Dictionary<string,string> Set { get; set; }
        /// <summary>
        /// ステート・フルネーム が入る。
        /// </summary>
        public string From_FullnameRegex { get; set; }
        /// <summary>
        /// 括弧を使った式 が入る。
        /// </summary>
        public string From_Attr { get; set; }
        /// <summary>
        /// ステート・フルネーム が入る。
        /// </summary>
        public string To_FullnameRegex { get; set; }
        /// <summary>
        /// 括弧を使った式 が入る。
        /// </summary>
        public string To_Attr { get; set; }
    }

    public class StellaQLAggregater
    {
        public static List<int> RecordIndexes_FilteringStateFullNameRegex(string pattern, Dictionary<int, AstateRecordable> universe)
        {
            List<int> hitRecordIndexes = new List<int>();

            Regex regex = new Regex(pattern);
            foreach (KeyValuePair<int, AstateRecordable> pair in universe)
            {
                if (regex.IsMatch(pair.Value.BreadCrumb + pair.Value.Name))
                {
                    hitRecordIndexes.Add(pair.Key);
                }
            }

            return hitRecordIndexes;
        }

        public static HashSet<int> RecordIndexes_FilteringElementsAnd(HashSet<int> lockerNumbers, List<HashSet<int>> recordIndexeslockers)
        {
            List<int> recordIndexes = new List<int>();// レコード・インデックスを入れたり、削除したりする
            int iLocker = 0;
            foreach (int lockerNumber in lockerNumbers)
            {
                HashSet<int> locker = recordIndexeslockers[lockerNumber];
                if (0 == iLocker) // 最初のロッカーは丸ごと入れる。
                {
                    foreach (int recordIndex in locker) { recordIndexes.Add(recordIndex); }
                }
                else // ２つ目以降のロッカーは、全てのロッカーに共通する要素のみ残るようにする。
                {
                    for (int iElem = recordIndexes.Count - 1; -1 < iElem; iElem--)// 後ろから指定の要素を削除する。
                    {
                        if (!locker.Contains(recordIndexes[iElem])) { recordIndexes.RemoveAt(iElem); }
                    }
                }
                iLocker++;
            }

            HashSet<int> distinctRecordIndexes = new HashSet<int>();// 一応、重複を消しておく
            foreach (int recordIndex in recordIndexes) { distinctRecordIndexes.Add(recordIndex); }

            return distinctRecordIndexes;
        }

        public static HashSet<int> RecordIndexes_FilteringElementsOr(HashSet<int> lockerNumbers, List<HashSet<int>> recordIndexeslockers)
        {
            HashSet<int> hitRecordIndexes = new HashSet<int>();// どんどんレコード・インデックスを追加していく
            foreach (int lockerNumber in lockerNumbers)
            {
                HashSet<int> locker = recordIndexeslockers[lockerNumber];
                if (0==locker.Count) { throw new UnityException("#RecordIndexes_FilteringElementsOr: lockerNumber=[" + lockerNumber + "]のメンバーが空っぽ☆"); }
                foreach (int recordIndex in locker)
                {
                    hitRecordIndexes.Add(recordIndex);
                }
            }

            if (0 == hitRecordIndexes.Count) { throw new UnityException("#RecordIndexes_FilteringElementsOr: 結果が空っぽ☆"); }
            return hitRecordIndexes;
        }

        public static HashSet<int> RecordIndexes_FilteringElementsNotAndNot(HashSet<int> lockerNumbers, List<HashSet<int>> recordIndexeslockers, Dictionary<int, AstateRecordable> universe)
        {
            HashSet<int> recordIndexesSet = new HashSet<int>();// どんどんレコード・インデックスを追加していく
            foreach (int lockerNumber in lockerNumbers)
            {
                HashSet<int> locker = recordIndexeslockers[lockerNumber];
                foreach (int recordIndex in locker)
                {
                    recordIndexesSet.Add(recordIndex);
                }
            }

            List<int> complementRecordIndexes = new List<int>(universe.Keys); // 補集合を取る（全集合から要素を除外していく）
            {
                for (int iComp = complementRecordIndexes.Count - 1; -1 < iComp; iComp--)// 後ろから指定の要素を削除する。
                {
                    if (recordIndexesSet.Contains(complementRecordIndexes[iComp])) // 集合にある要素を削除
                    {
                        complementRecordIndexes.RemoveAt(iComp);
                    }
                }
            }

            return new HashSet<int>(complementRecordIndexes);
        }

        public static HashSet<int> RecordIndexes_FilteringAttributesAnd(HashSet<int> attrs, Dictionary<int, AstateRecordable> universe)
        {
            HashSet<int> hitRecordIndexes = new HashSet<int>(universe.Keys);
            foreach (int attr in attrs)
            {
                HashSet<int> records_empty = new HashSet<int>();
                foreach (int recordIndex in hitRecordIndexes)
                {
                    if (universe[recordIndex].HasFlag_attr(attr)) { records_empty.Add(recordIndex); }// 該当したもの
                }
                hitRecordIndexes = records_empty;
            }
            return hitRecordIndexes;
        }

        public static HashSet<int> RecordIndexes_FilteringAttributesOr(HashSet<int> attrs, Dictionary<int, AstateRecordable> universe)
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

            return hitRecordIndexes;
        }

        public static HashSet<int> RecordIndexes_FilteringAttributesNotAndNot(HashSet<int> attrs, Dictionary<int, AstateRecordable> universe)
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

            return new HashSet<int>( complementRecordIndexes);
        }

        public static HashSet<int> KeywordSet_to_attrLocker(HashSet<int> set)//, Type enumration
        { // 列挙型要素を OR 結合して持つ。
            HashSet<int> attrs = new HashSet<int>();
            int sum = 0;// (int)Enum.GetValues(enumration).GetValue(0);//最初の要素は 0 にしておくこと。 列挙型だが、int 型に変換。
            foreach (object elem in set) { sum |= (int)elem; }// OR結合
            attrs.Add(sum); // 列挙型の要素を結合したものを int型として入れておく。
            return attrs;
        }

        public static HashSet<int> KeywordlistSet_to_attrLocker(HashSet<int> set)
        { // 列挙型要素を １つ１つ　ばらばらに持つ。
            return set;
        }

        public static HashSet<int> NGKeywordSet_to_attrLocker(HashSet<int> set, Type enumration)
        {
            return Complement(set, enumration); // 補集合を返すだけ☆
        }

        /// <summary>
        /// 補集合
        /// </summary>
        public static HashSet<int> Complement(HashSet<int> set, Type enumration)
        {
            List<int> complement = new List<int>();
            foreach (int elem in Enum.GetValues(enumration)) { complement.Add(elem); }// 列挙型の中身をリストに移動。
            for (int iComp = complement.Count - 1; -1 < iComp; iComp--)// 後ろから指定の要素を削除する。
            {
                if (set.Contains(complement[iComp]))
                {
                    //Debug.Log("Remove[" + iComp + "] (" + complement[iComp] + ")");
                    complement.RemoveAt(iComp);
                }
                //else
                //{
                //    Debug.Log("Tick[" + iComp + "] (" + complement[iComp] + ")");
                //}
            }
            return new HashSet<int>( complement);
        }

        public static HashSet<int> Tokens_to_numbers(List<string> tokens, Type enumration)
        {
            HashSet<int> numberSet = new HashSet<int>();
            foreach (string numberString in tokens) { numberSet.Add(int.Parse(numberString)); }// 変換できなかったら例外を投げる
            return numberSet;
        }

        public static HashSet<int> Names_to_enums(HashSet<string> nameSet, Type enumration)
        {
            HashSet<int> enumSet = new HashSet<int>();
            foreach (string name in nameSet) { enumSet.Add((int)Enum.Parse(enumration, name)); }// 変換できなかったら例外を投げる
            return enumSet;
        }
    }

    public abstract class AttrParenthesisParser
    {
        /// <summary>
        /// スキャンに渡すトークンを作るのが仕事。
        /// 
        /// ([(Alpaca Bear)(Cat Dog)]{Elephant})
        /// を、
        /// 「(」「[」「(」「Alpaca」「Bear」「)」「(」「Cat」「Dog」「)」「]」「{」「Elephant」「}」「)」
        /// に分解する。
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="tokens"></param>
        public static void String_to_tokens(string expression, out List<string> tokens)
        {
            tokens = new List<string>();
            StringBuilder bufferWord = new StringBuilder();

            for (int iCaret = 0; iCaret < expression.Length;)
            {
                if(StellaQLScanner.VarSpaces(expression, ref iCaret)) {
                    if (0 < bufferWord.Length) { tokens.Add(bufferWord.ToString()); bufferWord.Length = 0; } // 空白を読み飛ばす。
                    // Debug.Log("iCaret=[" + iCaret + "] 空白読み飛ばし word.ToString()=[" + bufferWord.ToString() + "]");
                }else{
                    char ch = expression[iCaret];
                    switch (ch) {
                        case '(':
                        case '[':
                        case '{':
                        case ')':
                        case ']':
                        case '}': // Debug.Log("iCaret=[" + iCaret + "] tokens.Add(ch.ToString()) ch=[" + ch + "]");
                            if (0 < bufferWord.Length) { tokens.Add(bufferWord.ToString()); bufferWord.Length = 0; } tokens.Add(ch.ToString()); break;
                        default: // Debug.Log("iCaret=["+ iCaret + "] word.Append(ch) ch=[" + ch + "]");
                            bufferWord.Append(ch); break;
                    }
                    iCaret++;
                }
            }
            if (0 < bufferWord.Length) { tokens.Add(bufferWord.ToString()); bufferWord.Length = 0; } //構文エラー
        }

        /// <summary>
        /// 「(」「[」「(」「Alpaca」「Bear」「)」「(」「Cat」「Dog」「)」「]」「{」「Elephant」「}」「)」
        /// ※読み取り順 ) Bear Alpaca ( ) Dog Cat ( ] [ } Elephant { ) (
        /// を、
        /// 
        /// 0: () Bear Alpaca
        /// 1: () Dog Cat
        /// 2: [] 1 0
        /// 3: {} Elephant
        /// 4: () 3 2
        /// 
        /// というロッカーに並べ替える。
        /// </summary>
        public static void Tokens_to_lockers(List<string> tokens, out List<List<string>> lockers, out List<string> lockersOperation)
        {
            string openParen = ""; // 閉じ括弧に対応する、「開きカッコ」
            int iCursor = 0;

            lockers = new List<List<string>>(); // 部室のロッカー。スタートは 0 番から。
            lockersOperation = new List<string>();
            List<string> bufferTokens = new List<string>(); // スキャン中のトークン。
            while (iCursor < tokens.Count) {
                string token = tokens[iCursor];
                if ("" == openParen) { //StringBuilder sb1 = new StringBuilder(); sb1.Append("go["); sb1.Append(iCursor); sb1.Append("]: "); sb1.Append(token); sb1.AppendLine(); Debug.Log(sb1.ToString());
                    switch (token) {
                        case ")": openParen = "("; tokens[iCursor] = ""; break;
                        case "]": openParen = "["; tokens[iCursor] = ""; break;
                        case "}": openParen = "{"; tokens[iCursor] = ""; break;
                        default: break; // 無視して進む
                    }
                } else // 後ろに進む☆　括弧内のメンバーの文字を削除し、開きカッコをロッカー番号に置き換える。
                { //StringBuilder sb2 = new StringBuilder(); sb2.Append("back["); sb2.Append(iCursor); sb2.Append("]: "); sb2.Append(token); sb2.AppendLine(); Debug.Log(sb2.ToString());
                    switch (token){
                        case "": break; // 無視
                        case "(":
                        case "[":
                        case "{": if (openParen == token) {
                                tokens[iCursor] = lockers.Count.ToString(); // ロッカー番号に置換
                                openParen = ""; lockersOperation.Add(token); lockers.Add(bufferTokens); bufferTokens = new List<string>();
                            } else { throw new UnityException("Tokens_to_lockers パース・エラー？"); } break;
                        default: bufferTokens.Add(token); tokens[iCursor] = ""; break;
                    }
                }
                if ("" == openParen) { iCursor++; } else { iCursor--; }
            }
        }
    }

    public abstract class AbstractLockersScanner
    {
        /// <summary>
        /// トークン・ロッカーを元に、レコード・インデックス・ロッカーを返す。
        /// </summary>
        /// <param name="tokens"></param>
        public static void TokenLockers_to_recordIndexesLockers(List<List<string>> tokenLockers, List<string> tokenLockersOperation, Type attrEnumration, Dictionary<int, AstateRecordable> universe, out List<HashSet<int>> recordIndexesLockers)
        {
            recordIndexesLockers = new List<HashSet<int>>();

            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("開始 tokenLockers.Count=[" + tokenLockers.Count + "]");
                Debug.Log(sb.ToString());
            }
            //*
            for (int iTokenLocker = 0; iTokenLocker<tokenLockers.Count; iTokenLocker++)// 部室のロッカー番号。スタートは 0 番から。
            {
                List<string>  tokenMembers = tokenLockers[iTokenLocker];
                string tokenOperation = tokenLockersOperation[iTokenLocker];// 「(」「[」「{」 がある。
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("ループ頭 tokenMembers.Count=[" + tokenMembers.Count + "] tokenOperation=[" + tokenOperation + "]");
                    int i = 0;
                    foreach (HashSet<int> recordIndexesSet in recordIndexesLockers)
                    {
                        sb.AppendLine("ロッカー recordIndexesLockers[" + i + "].Count=[" + recordIndexesSet.Count + "]");
                        i++;
                    }
                    Debug.Log(sb.ToString());
                }

                int memberLockerNumber_temp;
                if (int.TryParse(tokenMembers[0], out memberLockerNumber_temp))
                { // レコード・インデックス・ロッカー番号だ。
                    HashSet<int> lockerNumbers = StellaQLAggregater.Tokens_to_numbers(tokenMembers, attrEnumration);
                    { Debug.Log("レコード・インデックス・ロッカー番号だ。 tokenMembers.Count=[" + tokenMembers.Count+ "] lockerNumbers.Count = " + lockerNumbers.Count); int i = 0; foreach (int lockerNumber in lockerNumbers) { Debug.Log("["+i+"] lockerNumber=["+ lockerNumber + "]"); } i++; }
                    switch (tokenOperation)// 属性結合を解消する
                    {
                        case "(":
                            recordIndexesLockers.Add(StellaQLAggregater.RecordIndexes_FilteringElementsAnd(lockerNumbers, recordIndexesLockers));
                            break;
                        case "[":
                            recordIndexesLockers.Add(StellaQLAggregater.RecordIndexes_FilteringElementsOr(lockerNumbers, recordIndexesLockers));
                            break;
                        case "{":
                            recordIndexesLockers.Add(StellaQLAggregater.RecordIndexes_FilteringElementsNotAndNot(lockerNumbers, recordIndexesLockers, universe));
                            break;
                        default: throw new UnityException("未対応1のtokenOperation=["+ tokenOperation + "]");
                    }
                }
                else { // 属性名のリストなら
                    HashSet<int> attrEnumSet = StellaQLAggregater.Names_to_enums(new HashSet<string>(tokenMembers), attrEnumration);
                    HashSet<int> attrIndexes;
                    switch (tokenOperation)// 属性結合を解消する
                    {
                        case "(":
                            attrIndexes = StellaQLAggregater.KeywordSet_to_attrLocker(attrEnumSet);
                            recordIndexesLockers.Add(StellaQLAggregater.RecordIndexes_FilteringAttributesAnd(attrIndexes, universe));
                            break;
                        case "[":
                            attrIndexes = StellaQLAggregater.KeywordlistSet_to_attrLocker(attrEnumSet);
                            recordIndexesLockers.Add(StellaQLAggregater.RecordIndexes_FilteringAttributesOr(attrIndexes, universe));
                            break;
                        case "{":
                            //attrIndexes = StellaQLAggregater.NGKeywordSet_to_attrLocker(attrEnumSet, attrEnumration);
                            attrIndexes = StellaQLAggregater.KeywordlistSet_to_attrLocker(attrEnumSet); // NOT キーワードは NOT結合ではなく OR結合 で取る。
                            { Debug.Log("属性NOT attrIndexes.Count=[" + attrIndexes.Count + "]"); foreach (int attrIndex in attrIndexes) { Debug.Log("属性NOT attrIndex=["+ attrIndex + "]"); } }
                            HashSet<int> temp = StellaQLAggregater.RecordIndexes_FilteringAttributesNotAndNot(attrIndexes, universe);
                            recordIndexesLockers.Add(temp);
                            { Debug.Log("属性NOT temp.Count=[" + temp.Count+ "] universe.Count=[" + universe.Count+"]"); }
                            break;
                        default: throw new UnityException("未対応2のtokenOperation=[" + tokenOperation + "]");
                    }
                }
            }
            // */
        }
    }

    /// <summary>
    /// 正規表現の参考：http://smdn.jp/programming/netfx/regex/2_expressions/
    /// </summary>
    public class StellaQLScanner
    {
        private static Regex regexSpaces = new Regex(@"^(\s+)");
        public static bool VarSpaces(string query, ref int caret)
        {
            Match match = regexSpaces.Match(query.Substring(caret));
            if (match.Success) { caret += match.Groups[1].Value.Length; return true; }
            return false;
        }

        public static bool FixedWord(string word, string query, ref int caret)
        {
            int oldCaret = caret;
            if (caret == query.IndexOf(word, caret, StringComparison.OrdinalIgnoreCase))
            {
                caret += word.Length;
                if (caret == query.Length || VarSpaces(query, ref caret)) { return true; }
            }
            caret = oldCaret; return false;
        }

        /// <summary>
        /// "bear)" など後ろに半角スペースが付かないケースもあるので、スペースは 0 個も OK とする。
        /// </summary>
        private static Regex regexWordAndSpaces = new Regex(@"^(\w+)(\s*)", RegexOptions.IgnoreCase);
        public static bool VarWord(string query, ref int caret, out string word)
        {
            Match match = regexWordAndSpaces.Match(query.Substring(caret));
            if (match.Success) { word = match.Groups[1].Value; caret += word.Length + match.Groups[2].Value.Length; return true; }
            word = ""; return false;
        }

        /// <summary>
        /// 浮動小数点の「.」もOKとする。
        /// </summary>
        private static Regex regexValueAndSpaces = new Regex(@"^((?:\w|\.)+)(\s*)", RegexOptions.IgnoreCase);
        public static bool VarValue(string query, ref int caret, out string word)
        {
            Match match = regexValueAndSpaces.Match(query.Substring(caret));
            if (match.Success) { word = match.Groups[1].Value; caret += word.Length + match.Groups[2].Value.Length; return true; }
            word = ""; return false;
        }

        private static Regex regexStringliteralAndSpaces = new Regex(@"^""((?:(?:\\"")|[^""])*)""(\s*)", RegexOptions.IgnoreCase);
        public static bool VarStringliteral(string query, ref int caret, out string stringWithoutDoubleQuotation)
        {
            Match match = regexStringliteralAndSpaces.Match(query.Substring(caret));
            // ダブルクォーテーションの２文字分を足す
            if (match.Success) {
                stringWithoutDoubleQuotation = match.Groups[1].Value; caret += stringWithoutDoubleQuotation.Length + 2 + match.Groups[2].Value.Length;
                return true;
            }
            stringWithoutDoubleQuotation = ""; return false;
        }

        public static bool VarParentesis(string query, ref int caret, out string parentesis)
        {
            int oldCaret = caret;
            string word;
            Stack<char> closeParen = new Stack<char>();

            switch (query[caret])// 開始時
            {
                case '(': closeParen.Push(')'); caret++; break;
                case '[': closeParen.Push(']'); caret++; break;
                case '{': closeParen.Push('}'); caret++; break;
                default: goto gt_Failure;
            }
            VarSpaces(query, ref caret);

            while (caret < query.Length)
            {
                switch (query[caret])
                {
                    case '(': closeParen.Push(')'); caret++; break;
                    case '[': closeParen.Push(']'); caret++; break;
                    case '{': closeParen.Push('}'); caret++; break;
                    case ')':
                    case ']':
                    case '}':
                        if (query[caret] != closeParen.Peek()) { goto gt_Failure; }
                        closeParen.Pop(); caret++; if (0 == closeParen.Count) { goto gt_Finish; } break;
                    default: if (!VarWord(query, ref caret, out word)) { goto gt_Failure; } break;
                }
            }

        gt_Finish:
            if (caret == query.Length) { parentesis = query.Substring(oldCaret); }
            else { parentesis = query.Substring(oldCaret, caret); }
            VarSpaces(query, ref caret); return true;
        gt_Failure:
            parentesis = ""; return false;
        }

        /// <summary>
        /// 例
        /// TRANSITION INSERT
        /// SET Duration 0 ExitTime 1
        /// FROM “Base Layer.SMove”
        /// TO ATTR (BusyX Block)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static QueryTokens Parser_InsertStatement(string query)
        {
            QueryTokens qTokens = new QueryTokens();
            int caret = 0;
            string propertyName;
            string propertyValue;
            string stringWithoutDoubleQuotation;
            string parenthesis;
            VarSpaces(query, ref caret);

            if (!FixedWord(QueryTokens.TRANSITION, query, ref caret)) { return qTokens; }
            qTokens.Target = QueryTokens.TRANSITION;

            if (!FixedWord(QueryTokens.INSERT, query, ref caret)) { return qTokens; }
            qTokens.Manipulation = QueryTokens.INSERT;

            if (FixedWord(QueryTokens.SET, query, ref caret))
            {
                // 「項目名、スペース、値、スペース」の繰り返し。項目名が FROM だった場合終わり。
                while (caret < query.Length && !FixedWord(QueryTokens.FROM, query, ref caret))
                {
                    if (!VarWord(query, ref caret, out propertyName)) { return qTokens; }
                    if (!VarValue(query, ref caret, out propertyValue)) { return qTokens; }
                    qTokens.Set.Add(propertyName, propertyValue);
                }
            }
            else
            {
                if (!FixedWord(QueryTokens.FROM, query, ref caret)) { return qTokens; }
            }

            // 「"文字列"」か、「ATTR ～」のどちらか。
            if (VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qTokens.From_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (FixedWord(QueryTokens.ATTR, query, ref caret))
            {
                if (!VarParentesis(query, ref caret, out parenthesis)) { return qTokens; }
                qTokens.From_Attr = parenthesis;
            }
            else
            {
                return qTokens;
            }

            if (!FixedWord(QueryTokens.TO, query, ref caret)) { return qTokens; }

            // 「"文字列"」か、「ATTR ～」のどちらか。
            if (VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qTokens.To_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (FixedWord(QueryTokens.ATTR, query, ref caret))
            {
                if (!VarParentesis(query, ref caret, out parenthesis)) { return qTokens; }
                qTokens.To_Attr = parenthesis;
            }
            else
            {
                return qTokens;
            }

            return qTokens;
        }

        /// <summary>
        /// 例
        /// TRANSITION UPDATE
        /// SET Duration 0.25 ExitTime 0.75
        /// FROM “Base Layer.SMove”
        /// TO ATTR (BusyX Block)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static QueryTokens Parser_UpdateStatement(string query)
        {
            QueryTokens qTokens = new QueryTokens();
            int caret = 0;
            string propertyName;
            string propertyValue;
            string stringWithoutDoubleQuotation;
            string parenthesis;
            VarSpaces(query, ref caret);

            if (!FixedWord(QueryTokens.TRANSITION, query, ref caret)) { return qTokens; }
            qTokens.Target = QueryTokens.TRANSITION;

            if (!FixedWord(QueryTokens.UPDATE, query, ref caret)) { return qTokens; }
            qTokens.Manipulation = QueryTokens.UPDATE;

            if (FixedWord(QueryTokens.SET, query, ref caret))
            {
                // 「項目名、スペース、値、スペース」の繰り返し。項目名が FROM だった場合終わり。
                while (caret < query.Length && !FixedWord(QueryTokens.FROM, query, ref caret))
                {
                    if (!VarWord(query, ref caret, out propertyName)) { return qTokens; }
                    if (!VarValue(query, ref caret, out propertyValue)) { return qTokens; }
                    qTokens.Set.Add(propertyName, propertyValue);
                }
            }
            else
            {
                if (!FixedWord(QueryTokens.FROM, query, ref caret)) { return qTokens; }
            }

            // 「"文字列"」か、「ATTR ～」のどちらか。
            if (VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qTokens.From_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (FixedWord(QueryTokens.ATTR, query, ref caret))
            {
                if (!VarParentesis(query, ref caret, out parenthesis)) { return qTokens; }
                qTokens.From_Attr = parenthesis;
            }
            else
            {
                return qTokens;
            }

            if (!FixedWord(QueryTokens.TO, query, ref caret)) { return qTokens; }

            // 「"文字列"」か、「ATTR ～」のどちらか。
            if (VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qTokens.To_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (FixedWord(QueryTokens.ATTR, query, ref caret))
            {
                if (!VarParentesis(query, ref caret, out parenthesis)) { return qTokens; }
                qTokens.To_Attr = parenthesis;
            }
            else
            {
                return qTokens;
            }

            return qTokens;
        }

        /// <summary>
        /// 例
        /// TRANSITION DELETE
        /// FROM “Base Layer.SMove”
        /// TO ATTR (BusyX Block)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static QueryTokens Parser_DeleteStatement(string query)
        {
            QueryTokens qTokens = new QueryTokens();
            int caret = 0;
            string stringWithoutDoubleQuotation;
            string parenthesis;
            VarSpaces(query, ref caret);

            if (!FixedWord(QueryTokens.TRANSITION, query, ref caret)) { return qTokens; }
            qTokens.Target = QueryTokens.TRANSITION;

            if (!FixedWord(QueryTokens.DELETE, query, ref caret)) { return qTokens; }
            qTokens.Manipulation = QueryTokens.DELETE;

            if (!FixedWord(QueryTokens.FROM, query, ref caret)) { return qTokens; }

            // 「"文字列"」か、「ATTR ～」のどちらか。
            if (VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qTokens.From_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if(FixedWord(QueryTokens.ATTR, query, ref caret))
            {
                if (!VarParentesis(query, ref caret, out parenthesis)) { return qTokens; }
                qTokens.From_Attr = parenthesis;
            }
            else
            {
                return qTokens;
            }

            if (!FixedWord(QueryTokens.TO, query, ref caret)) { return qTokens; }

            // 「"文字列"」か、「ATTR ～」のどちらか。
            if (VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qTokens.To_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (FixedWord(QueryTokens.ATTR, query, ref caret))
            {
                if (!VarParentesis(query, ref caret, out parenthesis)) { return qTokens; }
                qTokens.To_Attr = parenthesis;
            }
            else
            {
                return qTokens;
            }

            return qTokens;
        }

        /// <summary>
        /// TRANSITION SELECT
        /// FROM “Base Layer.SMove”
        /// TO ATTR (BusyX Block)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static QueryTokens Parser_SelectStatement(string query)
        {
            QueryTokens qTokens = new QueryTokens();
            int caret = 0;
            string stringWithoutDoubleQuotation;
            string parenthesis;
            VarSpaces(query, ref caret);

            if (!FixedWord(QueryTokens.TRANSITION, query, ref caret)) { return qTokens; }
            qTokens.Target = QueryTokens.TRANSITION;

            if (!FixedWord(QueryTokens.SELECT, query, ref caret)) { return qTokens; }
            qTokens.Manipulation = QueryTokens.SELECT;

            if (!FixedWord(QueryTokens.FROM, query, ref caret)) { return qTokens; }

            // 「"文字列"」か、「ATTR ～」のどちらか。
            if (VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qTokens.From_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (FixedWord(QueryTokens.ATTR, query, ref caret))
            {
                if (!VarParentesis(query, ref caret, out parenthesis)) { return qTokens; }
                qTokens.From_Attr = parenthesis;
            }
            else
            {
                return qTokens;
            }

            if (!FixedWord(QueryTokens.TO, query, ref caret)) { return qTokens; }

            // 「"文字列"」か、「ATTR ～」のどちらか。
            if (VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qTokens.To_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (FixedWord(QueryTokens.ATTR, query, ref caret))
            {
                if (!VarParentesis(query, ref caret, out parenthesis)) { return qTokens; }
                qTokens.To_Attr = parenthesis;
            }
            else
            {
                return qTokens;
            }

            return qTokens;
        }
    }

}
