using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor.Animations;
using System.IO;

/// <summary>
/// 解説: 「UnityEditorを使って2D格闘(2D Fighting game)作るときのモーション遷移図作成の半自動化に挑戦しよう＜その４＞」 http://qiita.com/muzudho1/items/baf4b06cdcda96ca9a11
/// </summary>
namespace StellaQL
{
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
            Where_FullnameRegex = "";
            Where_Attr = "";
        }

        public const string TRANSITION = "TRANSITION";
        public const string STATE = "STATE";
        public const string INSERT = "INSERT";
        public const string UPDATE = "UPDATE";
        public const string DELETE = "DELETE";
        public const string SELECT = "SELECT";
        public const string SET = "SET";
        public const string FROM = "FROM";
        public const string TO = "TO";
        public const string WHERE = "WHERE";
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
        public Dictionary<string, string> Set { get; set; }
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
        /// <summary>
        /// ステート・フルネーム が入る。
        /// </summary>
        public string Where_FullnameRegex { get; set; }
        /// <summary>
        /// 括弧を使った式 が入る。
        /// </summary>
        public string Where_Attr { get; set; }

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
                if (LexcalP.VarSpaces(expression, ref iCaret))
                {
                    if (0 < bufferWord.Length) { tokens.Add(bufferWord.ToString()); bufferWord.Length = 0; } // 空白を読み飛ばす。
                    // Debug.Log("iCaret=[" + iCaret + "] 空白読み飛ばし word.ToString()=[" + bufferWord.ToString() + "]");
                }
                else {
                    char ch = expression[iCaret];
                    switch (ch)
                    {
                        case '(':
                        case '[':
                        case '{':
                        case ')':
                        case ']':
                        case '}': // Debug.Log("iCaret=[" + iCaret + "] tokens.Add(ch.ToString()) ch=[" + ch + "]");
                            if (0 < bufferWord.Length) { tokens.Add(bufferWord.ToString()); bufferWord.Length = 0; }
                            tokens.Add(ch.ToString()); break;
                        default: // Debug.Log("iCaret=["+ iCaret + "] word.Append(ch) ch=[" + ch + "]");
                            bufferWord.Append(ch); break;
                    }
                    iCaret++;
                }
            }
            if (0 < bufferWord.Length) { tokens.Add(bufferWord.ToString()); bufferWord.Length = 0; } //構文エラー
        }
    }

    public abstract class QueryTokensUtility
    {
        public static HashSet<int> RecordHashes_From(QueryTokens qt, Type enumration, Dictionary<int, StateExRecordable> universe)
        {
            if ("" != qt.From_FullnameRegex) { return ElementSet.RecordHashes_FilteringStateFullNameRegex(qt.From_FullnameRegex, universe); }
            else {
                List<string> tokens; QueryTokens.String_to_tokens(qt.From_Attr, out tokens);

                List<List<string>> tokenLockers;
                List<string> tokenLockersOperation;
                Querier.Tokens_to_lockers(tokens, out tokenLockers, out tokenLockersOperation);

                List<HashSet<int>> recordHashesLockers;
                Fetcher.TokenLockers_to_recordHashesLockers(tokenLockers, tokenLockersOperation, enumration, universe, out recordHashesLockers);
                return recordHashesLockers[recordHashesLockers.Count - 1];
            }
        }

        public static HashSet<int> RecordHashes_To(QueryTokens qt, Type enumration, Dictionary<int, StateExRecordable> universe)
        {
            if ("" != qt.To_FullnameRegex) { return ElementSet.RecordHashes_FilteringStateFullNameRegex(qt.To_FullnameRegex, universe); }
            else {
                List<string> tokens; QueryTokens.String_to_tokens(qt.To_Attr, out tokens);

                List<List<string>> tokenLockers;
                List<string> tokenLockersOperation;
                Querier.Tokens_to_lockers(tokens, out tokenLockers, out tokenLockersOperation);

                List<HashSet<int>> recordHashesLockers;
                Fetcher.TokenLockers_to_recordHashesLockers(tokenLockers, tokenLockersOperation, enumration, universe, out recordHashesLockers);
                return recordHashesLockers[recordHashesLockers.Count - 1];
            }
        }

        public static HashSet<int> RecordHashes_Where(QueryTokens qt, Type enumration, Dictionary<int, StateExRecordable> universe)
        {
            if ("" != qt.Where_FullnameRegex) { return ElementSet.RecordHashes_FilteringStateFullNameRegex(qt.Where_FullnameRegex, universe); }
            else {
                List<string> tokens; QueryTokens.String_to_tokens(qt.Where_Attr, out tokens);

                List<List<string>> tokenLockers;
                List<string> tokenLockersOperation;
                Querier.Tokens_to_lockers(tokens, out tokenLockers, out tokenLockersOperation);

                List<HashSet<int>> recordHashesLockers;
                Fetcher.TokenLockers_to_recordHashesLockers(tokenLockers, tokenLockersOperation, enumration, universe, out recordHashesLockers);
                return recordHashesLockers[recordHashesLockers.Count - 1];
            }
        }
    }

    /// <summary>
    /// Query (文字列を与えて、レコード・インデックスを取ってくる)
    /// </summary>
    public abstract class Querier
    {
        public static bool Execute(AnimatorController ac, string query, Type enumration, Dictionary<int, StateExRecordable> universe, out StringBuilder message)
        {
            LexcalP.DeleteLineCommentAndBlankLine(ref query);

            QueryTokens sq;
            message = new StringBuilder();

            if (SyntaxP.ParseStatement_StateUpdate(query, out sq))
            {
                HashSet<int> recordHashes = QueryTokensUtility.RecordHashes_Where(sq, enumration, universe);
                foreach (KeyValuePair<string, string> pair in sq.Set)
                {
                    message.AppendLine(pair.Key + "=" + pair.Value);
                }
                AniconOpe_State.UpdateProperty(ac, sq.Set, Fetcher.FetchAll(ac, recordHashes, universe), message);
                int i = 0;
                foreach (int recordHash in recordHashes)
                {
                    message.AppendLine(i + ": record[" + recordHash + "]"); i++;
                }
                return true;
            }
            else if (SyntaxP.ParseStatement_StateSelect(query, out sq))
            {
                HashSet<int> recordHashes = QueryTokensUtility.RecordHashes_Where(sq, enumration, universe);
                HashSet<StateRecord> recordSet;
                AniconOpe_State.Select(ac, Fetcher.FetchAll(ac, recordHashes, universe), out recordSet, message);
                StringBuilder contents = new StringBuilder();
                AniconTables.CreateCsvTable_State(recordSet, contents);
                StellaQLWriter.Write(StellaQLWriter.Filepath_LogStateSelect(ac.name), contents, message);
                return true;
            }
            else if (SyntaxP.ParseStatement_TransitionInsert(query, out sq))
            {
                HashSet<int> recordHashesFrom = QueryTokensUtility.RecordHashes_From(sq, enumration, universe);
                HashSet<int> recordHashesTo = QueryTokensUtility.RecordHashes_To(sq, enumration, universe);
                AniconOpe_Transition.AddAll(ac,
                    Fetcher.FetchAll(ac, recordHashesFrom, universe),
                    Fetcher.FetchAll(ac, recordHashesTo, universe),
                    message);
                return true;
            }
            else if (SyntaxP.ParseStatement_TransitionUpdate(query, out sq))
            {
                foreach (KeyValuePair<string,string> pair in sq.Set)
                {
                    message.AppendLine(pair.Key+"="+pair.Value);
                }
                HashSet<int> recordHashesFrom = QueryTokensUtility.RecordHashes_From(sq, enumration, universe);
                HashSet<int> recordHashesTo = QueryTokensUtility.RecordHashes_To(sq, enumration, universe);
                AniconOpe_Transition.UpdateProperty(ac, sq.Set,
                    Fetcher.FetchAll(ac, recordHashesFrom, universe),
                    Fetcher.FetchAll(ac, recordHashesTo, universe),
                    message);
                return true;
            }
            else if (SyntaxP.ParseStatement_TransitionDelete(query, out sq))
            {
                HashSet<int> recordHashesFrom = QueryTokensUtility.RecordHashes_From(sq, enumration, universe);
                HashSet<int> recordHashesTo = QueryTokensUtility.RecordHashes_To(sq, enumration, universe);
                AniconOpe_Transition.RemoveAll(ac,
                    Fetcher.FetchAll(ac, recordHashesFrom, universe),
                    Fetcher.FetchAll(ac, recordHashesTo, universe),
                    message);
                return true;
            }
            else if (SyntaxP.ParseStatement_TransitionSelect(query, out sq)) {
                HashSet<int> recordHashesFrom = QueryTokensUtility.RecordHashes_From(sq, enumration, universe);
                HashSet<int> recordHashesTo = QueryTokensUtility.RecordHashes_To(sq, enumration, universe);
                HashSet<TransitionRecord> recordSet;
                AniconOpe_Transition.Select(ac,
                    Fetcher.FetchAll(ac, recordHashesFrom, universe),
                    Fetcher.FetchAll(ac, recordHashesTo, universe),
                    out recordSet,
                    message);
                StringBuilder contents = new StringBuilder();
                AniconTables.CreateCsvTable_Transition(recordSet, contents);
                StellaQLWriter.Write(StellaQLWriter.Filepath_LogTransitionSelect(ac.name), contents, message);
                return true;
            }

            message.AppendLine( "構文該当無し");
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query">例えば 「STATE SELECT WHERE ATTR ([(Alpha Cee)(Beta)]{Eee})」 といった式。</param>
        /// <returns></returns>
        public static bool ExecuteStateSelect(string query, Type enumration, Dictionary<int, StateExRecordable> universe, out HashSet<int> recordHashes)
        {
            LexcalP.DeleteLineCommentAndBlankLine(ref query);

            recordHashes = null;
            QueryTokens sq;
            if (!SyntaxP.ParseStatement_StateSelect(query, out sq)) { return false; }

            recordHashes = QueryTokensUtility.RecordHashes_Where(sq, enumration, universe);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query">例えば 「TRANSITION SELECT FROM "Base Layer.Zebra" TO ATTR ([(Alpha Cee)(Beta)]{Eee})」 といった式。</param>
        /// <returns></returns>
        public static bool ExecuteTransitionSelect(string query, Type enumration, Dictionary<int, StateExRecordable> universe, out HashSet<int> recordHashesSrc, out HashSet<int> recordHashesDst)
        {
            LexcalP.DeleteLineCommentAndBlankLine(ref query);

            recordHashesSrc = null;
            recordHashesDst = null;
            QueryTokens sq;
            if (!SyntaxP.ParseStatement_TransitionSelect(query, out sq)) { return false; }

            recordHashesSrc = QueryTokensUtility.RecordHashes_From(sq, enumration, universe);// FROM
            recordHashesDst = QueryTokensUtility.RecordHashes_To(sq, enumration, universe);// TO
            return true;
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
            while (iCursor < tokens.Count)
            {
                string token = tokens[iCursor];
                if ("" == openParen)
                { //StringBuilder sb1 = new StringBuilder(); sb1.Append("go["); sb1.Append(iCursor); sb1.Append("]: "); sb1.Append(token); sb1.AppendLine(); Debug.Log(sb1.ToString());
                    switch (token)
                    {
                        case ")": openParen = "("; tokens[iCursor] = ""; break;
                        case "]": openParen = "["; tokens[iCursor] = ""; break;
                        case "}": openParen = "{"; tokens[iCursor] = ""; break;
                        default: break; // 無視して進む
                    }
                }
                else // 後ろに進む☆　括弧内のメンバーの文字を削除し、開きカッコをロッカー番号に置き換える。
                { //StringBuilder sb2 = new StringBuilder(); sb2.Append("back["); sb2.Append(iCursor); sb2.Append("]: "); sb2.Append(token); sb2.AppendLine(); Debug.Log(sb2.ToString());
                    switch (token)
                    {
                        case "": break; // 無視
                        case "(":
                        case "[":
                        case "{":
                            if (openParen == token)
                            {
                                tokens[iCursor] = lockers.Count.ToString(); // ロッカー番号に置換
                                openParen = ""; lockersOperation.Add(token); lockers.Add(bufferTokens); bufferTokens = new List<string>();
                            }
                            else { throw new UnityException("Tokens_to_lockers パース・エラー？"); }
                            break;
                        default: bufferTokens.Add(token); tokens[iCursor] = ""; break;
                    }
                }
                if ("" == openParen) { iCursor++; } else { iCursor--; }
            }
        }
    }

    /// <summary>
    /// Fetch (レコード・インデックスを取ってくる)
    /// </summary>
    public abstract class Fetcher
    {
        /// <summary>
        /// トークン・ロッカーを元に、ロッカー別の検索結果を返す。
        /// </summary>
        /// <param name="tokens"></param>
        public static void TokenLockers_to_recordHashesLockers(List<List<string>> lockerNumber_to_tokens, List<string> lockerNumber_to_operation, Type attrEnumration,
            Dictionary<int, StateExRecordable> universe, out List<HashSet<int>> lockerNumber_to_recordHashes)
        {
            lockerNumber_to_recordHashes = new List<HashSet<int>>();

            for (int iLockerNumber = 0; iLockerNumber < lockerNumber_to_tokens.Count; iLockerNumber++)// 部室のロッカー番号。スタートは 0 番から。
            {
                List<string> index_to_token = lockerNumber_to_tokens[iLockerNumber];
                string operation = lockerNumber_to_operation[iLockerNumber];// 「(」「[」「{」 がある。

                int firstItem_temp;
                if (int.TryParse(index_to_token[0], out firstItem_temp))
                { // 数字だったら、ロッカー番号だ。
                    HashSet<int> lockerNumbers = AttrSet.Tokens_to_numbers(index_to_token, attrEnumration);
                    switch (operation) // ロッカー同士を演算して、まとめた答えを出す
                    {
                        case "(": lockerNumber_to_recordHashes.Add(ElementSet.RecordHashes_FilteringElementsAnd(lockerNumbers, lockerNumber_to_recordHashes)); break;
                        case "[": lockerNumber_to_recordHashes.Add(ElementSet.RecordHashes_FilteringElementsOr(lockerNumbers, lockerNumber_to_recordHashes)); break;
                        case "{":
                            lockerNumber_to_recordHashes.Add(ElementSet.RecordHashes_FilteringElementsNotAndNot(
                      lockerNumbers, lockerNumber_to_recordHashes, universe)); break;
                        default: throw new UnityException("未対応1のtokenOperation=[" + operation + "]");
                    }
                }
                else { // 数字じゃなかったら、属性名のリストだ
                    HashSet<int> attrEnumSet_src = AttrSet.Names_to_enums(new HashSet<string>(index_to_token), attrEnumration);
                    HashSet<int> attrEnumSet_calc;
                    switch (operation) // 属性結合（演算）を解消する
                    {
                        case "(":
                            attrEnumSet_calc = AttrSet.KeywordSet_to_attrLocker(attrEnumSet_src);
                            lockerNumber_to_recordHashes.Add(ElementSet.RecordHashes_FilteringAttributesAnd(attrEnumSet_calc, universe)); break;
                        case "[":
                            attrEnumSet_calc = AttrSet.KeywordlistSet_to_attrLocker(attrEnumSet_src);
                            lockerNumber_to_recordHashes.Add(ElementSet.RecordHashes_FilteringAttributesOr(attrEnumSet_calc, universe)); break;
                        case "{":
                            //attrEnumSet_calc = StellaQLAggregater.NGKeywordSet_to_attrLocker(attrEnumSet_src, attrEnumration);
                            attrEnumSet_calc = AttrSet.KeywordlistSet_to_attrLocker(attrEnumSet_src); // NOT キーワードは NOT結合ではなく OR結合 で取る。
                            lockerNumber_to_recordHashes.Add(ElementSet.RecordHashes_FilteringAttributesNotAndNot(attrEnumSet_calc, universe)); break;
                        default: throw new UnityException("未対応2のtokenOperation=[" + operation + "]");
                    }
                }
            }
        }

        public static HashSet<AnimatorState> FetchAll(AnimatorController ac, HashSet<int> recordHashes, Dictionary<int, StateExRecordable> universe)
        {
            HashSet<AnimatorState> states = new HashSet<AnimatorState>();
            foreach (int recordHash in recordHashes)
            {
                states.Add(AniconOpe_State.Lookup(ac, universe[recordHash].Fullpath));//.Name
            }
            return states;
        }
    }

    /// <summary>
    /// Element set (属性集合)
    /// </summary>
    public abstract class ElementSet
    {
        public static HashSet<int> RecordHashes_FilteringStateFullNameRegex(string pattern, Dictionary<int, StateExRecordable> universe)
        {
            HashSet<int> hitRecordHashes = new HashSet<int>();

            Regex regex = new Regex(pattern);
            foreach (KeyValuePair<int, StateExRecordable> pair in universe)
            {
                if (regex.IsMatch(pair.Value.Fullpath))
                {
                    hitRecordHashes.Add(pair.Key);
                }
            }

            return hitRecordHashes;
        }

        public static HashSet<int> RecordHashes_FilteringElementsAnd(HashSet<int> lockerNumbers, List<HashSet<int>> recordHasheslockers)
        {
            List<int> recordHashes = new List<int>();// レコード・インデックスを入れたり、削除したりする
            int iLocker = 0;
            foreach (int lockerNumber in lockerNumbers)
            {
                HashSet<int> locker = recordHasheslockers[lockerNumber];
                if (0 == iLocker) // 最初のロッカーは丸ごと入れる。
                {
                    foreach (int recordHash in locker) { recordHashes.Add(recordHash); }
                }
                else // ２つ目以降のロッカーは、全てのロッカーに共通する要素のみ残るようにする。
                {
                    for (int iElem = recordHashes.Count - 1; -1 < iElem; iElem--)// 後ろから指定の要素を削除する。
                    {
                        if (!locker.Contains(recordHashes[iElem])) { recordHashes.RemoveAt(iElem); }
                    }
                }
                iLocker++;
            }

            HashSet<int> distinctRecordHashes = new HashSet<int>();// 一応、重複を消しておく
            foreach (int recordHash in recordHashes) { distinctRecordHashes.Add(recordHash); }

            return distinctRecordHashes;
        }

        public static HashSet<int> RecordHashes_FilteringElementsOr(HashSet<int> lockerNumbers, List<HashSet<int>> recordHasheslockers)
        {
            HashSet<int> hitRecordHashes = new HashSet<int>();// どんどんレコード・インデックスを追加していく
            foreach (int lockerNumber in lockerNumbers)
            {
                HashSet<int> locker = recordHasheslockers[lockerNumber];
                if (0 == locker.Count) { throw new UnityException("#RecordHashes_FilteringElementsOr: lockerNumber=[" + lockerNumber + "]のメンバーが空っぽ☆"); }
                foreach (int recordHash in locker)
                {
                    hitRecordHashes.Add(recordHash);
                }
            }

            if (0 == hitRecordHashes.Count) { throw new UnityException("#RecordHashes_FilteringElementsOr: 結果が空っぽ☆"); }
            return hitRecordHashes;
        }

        public static HashSet<int> RecordHashes_FilteringElementsNotAndNot(HashSet<int> lockerNumbers, List<HashSet<int>> recordHasheslockers, Dictionary<int, StateExRecordable> universe)
        {
            HashSet<int> recordHashesSet = new HashSet<int>();// どんどんレコード・インデックスを追加していく
            foreach (int lockerNumber in lockerNumbers)
            {
                HashSet<int> locker = recordHasheslockers[lockerNumber];
                foreach (int recordHash in locker)
                {
                    recordHashesSet.Add(recordHash);
                }
            }

            List<int> complementRecordHashes = new List<int>(universe.Keys); // 補集合を取る（全集合から要素を除外していく）
            {
                for (int iComp = complementRecordHashes.Count - 1; -1 < iComp; iComp--)// 後ろから指定の要素を削除する。
                {
                    if (recordHashesSet.Contains(complementRecordHashes[iComp])) // 集合にある要素を削除
                    {
                        complementRecordHashes.RemoveAt(iComp);
                    }
                }
            }

            return new HashSet<int>(complementRecordHashes);
        }

        public static HashSet<int> RecordHashes_FilteringAttributesAnd(HashSet<int> attrs, Dictionary<int, StateExRecordable> universe)
        {
            HashSet<int> hitRecordHashes = new HashSet<int>(universe.Keys);
            foreach (int attr in attrs)
            {
                HashSet<int> records_empty = new HashSet<int>();
                foreach (int recordHash in hitRecordHashes)
                {
                    if (universe[recordHash].HasFlag_attr(attr)) { records_empty.Add(recordHash); }// 該当したもの
                }
                hitRecordHashes = records_empty;
            }
            return hitRecordHashes;
        }

        public static HashSet<int> RecordHashes_FilteringAttributesOr(HashSet<int> attrs, Dictionary<int, StateExRecordable> universe)
        {
            HashSet<int> distinctAttr = new HashSet<int>();// まず属性の重複を除外
            foreach (int attr in attrs) { distinctAttr.Add(attr); }

            HashSet<int> hitRecordHashes = new HashSet<int>();// レコード・インデックスを属性検索（重複除外）
            foreach (KeyValuePair<int, StateExRecordable> pair in universe)
            {
                foreach (int attr in distinctAttr)
                {
                    if (pair.Value.HasFlag_attr(attr)) { hitRecordHashes.Add(pair.Key); }
                }
            }

            return hitRecordHashes;
        }

        public static HashSet<int> RecordHashes_FilteringAttributesNotAndNot(HashSet<int> attrs, Dictionary<int, StateExRecordable> universe)
        {
            HashSet<int> distinctAttr = new HashSet<int>();// まず属性の重複を除外
            foreach (int attr in attrs) { distinctAttr.Add(attr); }

            HashSet<int> hitRecordHashes = new HashSet<int>();// レコード・インデックスを属性検索（重複除外）
            foreach (KeyValuePair<int, StateExRecordable> pair in universe)
            {
                foreach (int attr in distinctAttr)
                {
                    if (pair.Value.HasFlag_attr(attr)) { hitRecordHashes.Add(pair.Key); }
                }
            }

            List<int> complementRecordHashes = new List<int>();// 補集合を取る
            {
                foreach (int recordHash in universe.Keys) { complementRecordHashes.Add(recordHash); }// 列挙型の中身をリストに移動。
                for (int iComp = complementRecordHashes.Count - 1; -1 < iComp; iComp--)// 後ろから指定の要素を削除する。
                {
                    if (hitRecordHashes.Contains(complementRecordHashes[iComp]))
                    {
                        complementRecordHashes.RemoveAt(iComp);
                    }
                }
            }

            return new HashSet<int>(complementRecordHashes);
        }
    }

    /// <summary>
    /// Attribute set (属性集合)
    /// </summary>
    public abstract class AttrSet
    {
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
            return new HashSet<int>(complement);
        }

        public static HashSet<int> Tokens_to_numbers(List<string> tokens, Type enumration)
        {
            HashSet<int> numberSet = new HashSet<int>();
            foreach (string numberString in tokens) { numberSet.Add(int.Parse(numberString)); }// 変換できなかったら例外を投げる
            return numberSet;
        }

        /// <summary>
        /// 列挙型の扱い方：「文字列を列挙体に変換する」（DOBON.NET） http://dobon.net/vb/dotnet/programing/enumparse.html
        /// </summary>
        /// <param name="nameSet"></param>
        /// <param name="enumration"></param>
        /// <returns></returns>
        public static HashSet<int> Names_to_enums(HashSet<string> nameSet, Type enumration)
        {
            HashSet<int> enumSet = new HashSet<int>();
            foreach (string name in nameSet) { enumSet.Add((int)Enum.Parse(enumration, name)); }// 変換できなかったら例外を投げる
            return enumSet;
        }
    }

    /// <summary>
    /// Syntax parser (構文パーサー)
    /// 正規表現の参考：http://smdn.jp/programming/netfx/regex/2_expressions/
    /// </summary>
    public abstract class SyntaxP
    {
        /// <summary>
        /// STATE SELECT
        /// WHERE ATTR ([(Alpha Cee)(Beta)]{Eee})
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static bool ParseStatement_StateSelect(string query, out QueryTokens qTokens)
        {
            qTokens = new QueryTokens();
            int caret = 0;
            string stringWithoutDoubleQuotation;
            string parenthesis;
            LexcalP.VarSpaces(query, ref caret);

            if (!LexcalP.FixedWord(QueryTokens.STATE, query, ref caret)) { return false; }
            qTokens.Target = QueryTokens.STATE;

            if (!LexcalP.FixedWord(QueryTokens.SELECT, query, ref caret)) { return false; }
            qTokens.Manipulation = QueryTokens.SELECT;

            if (!LexcalP.FixedWord(QueryTokens.WHERE, query, ref caret)) { return false; }

            // 「"文字列"」か、「ATTR ～」のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qTokens.Where_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (LexcalP.FixedWord(QueryTokens.ATTR, query, ref caret))
            {
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return false; }
                qTokens.Where_Attr = parenthesis;
            }
            else { return false; }

            return true;
        }

        /// <summary>
        /// SET句だけ。
        /// Example（代表例）: UPDATE文のSET句。
        /// </summary>
        /// <returns></returns>
        public static bool ParsePhrase_AfterSet(string query, ref int caret, string delimiterWord, Dictionary<string,string> ref_properties)
        {
            string propertyName;
            string propertyValue;

            while (caret < query.Length && !LexcalP.FixedWord(delimiterWord, query, ref caret))
            {   // 名前
                if (!LexcalP.VarWord(query, ref caret, out propertyName)) { return false; }
                // 値
                if (LexcalP.VarStringliteral(query, ref caret, out propertyValue)) { } // 一致しなければelse～ifへ
                else if (!LexcalP.VarValue(query, ref caret, out propertyValue)) { return false; }
                ref_properties.Add(propertyName, propertyValue);
            }
            return true;
        }

        /// <summary>
        /// 例
        /// STATE UPDATE
        /// SET ExitTime 0.75 Duration 0.25
        /// WHERE “Base Layer.Cat”
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static bool ParseStatement_StateUpdate(string query, out QueryTokens qTokens)
        {
            qTokens = new QueryTokens();
            int caret = 0;
            string stringWithoutDoubleQuotation;
            string parenthesis;
            LexcalP.VarSpaces(query, ref caret);

            if (!LexcalP.FixedWord(QueryTokens.STATE, query, ref caret)) { return false; }
            qTokens.Target = QueryTokens.STATE;

            if (!LexcalP.FixedWord(QueryTokens.UPDATE, query, ref caret)) { return false; }
            qTokens.Manipulation = QueryTokens.UPDATE;

            if (LexcalP.FixedWord(QueryTokens.SET, query, ref caret)) {
                // 「項目名、スペース、値、スペース」の繰り返し。項目名が WHERE だった場合終わり。
                if(!SyntaxP.ParsePhrase_AfterSet(query, ref caret, QueryTokens.WHERE, qTokens.Set)) { return false; }
            } else { if (!LexcalP.FixedWord(QueryTokens.WHERE, query, ref caret)) { return false; } }

            // 「"文字列"」か、「ATTR ～」のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation)) {
                qTokens.Where_FullnameRegex = stringWithoutDoubleQuotation;
            } else if (LexcalP.FixedWord(QueryTokens.ATTR, query, ref caret)) {
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return false; }
                qTokens.Where_Attr = parenthesis;
            }
            else { return false; }
            return true;
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
        public static bool ParseStatement_TransitionInsert(string query, out QueryTokens qTokens)
        {
            qTokens = new QueryTokens();
            int caret = 0;
            string propertyName;
            string propertyValue;
            string stringWithoutDoubleQuotation;
            string parenthesis;
            LexcalP.VarSpaces(query, ref caret);

            if (!LexcalP.FixedWord(QueryTokens.TRANSITION, query, ref caret)) { return false; }
            qTokens.Target = QueryTokens.TRANSITION;

            if (!LexcalP.FixedWord(QueryTokens.INSERT, query, ref caret)) { return false; }
            qTokens.Manipulation = QueryTokens.INSERT;

            if (LexcalP.FixedWord(QueryTokens.SET, query, ref caret))
            {
                // 「項目名、スペース、値、スペース」の繰り返し。項目名が FROM だった場合終わり。
                while (caret < query.Length && !LexcalP.FixedWord(QueryTokens.FROM, query, ref caret))
                {
                    if (!LexcalP.VarWord(query, ref caret, out propertyName)) { return false; }
                    if (!LexcalP.VarValue(query, ref caret, out propertyValue)) { return false; }
                    qTokens.Set.Add(propertyName, propertyValue);
                }
            }
            else {
                if (!LexcalP.FixedWord(QueryTokens.FROM, query, ref caret)) { return false; }
            }

            // 「"文字列"」か、「ATTR ～」のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qTokens.From_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (LexcalP.FixedWord(QueryTokens.ATTR, query, ref caret))
            {
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return false; }
                qTokens.From_Attr = parenthesis;
            }
            else { return false; }

            if (!LexcalP.FixedWord(QueryTokens.TO, query, ref caret)) { return false; }

            // 「"文字列"」か、「ATTR ～」のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qTokens.To_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (LexcalP.FixedWord(QueryTokens.ATTR, query, ref caret))
            {
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return false; }
                qTokens.To_Attr = parenthesis;
            }
            else { return false; }

            return true;
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
        public static bool ParseStatement_TransitionUpdate(string query, out QueryTokens qTokens)
        {
            qTokens = new QueryTokens();
            int caret = 0;
            string propertyName;
            string propertyValue;
            string stringWithoutDoubleQuotation;
            string parenthesis;
            LexcalP.VarSpaces(query, ref caret);

            if (!LexcalP.FixedWord(QueryTokens.TRANSITION, query, ref caret)) { return false; }
            qTokens.Target = QueryTokens.TRANSITION;

            if (!LexcalP.FixedWord(QueryTokens.UPDATE, query, ref caret)) { return false; }
            qTokens.Manipulation = QueryTokens.UPDATE;

            if (LexcalP.FixedWord(QueryTokens.SET, query, ref caret))
            {
                // 「項目名、スペース、値、スペース」の繰り返し。項目名が FROM だった場合終わり。
                while (caret < query.Length && !LexcalP.FixedWord(QueryTokens.FROM, query, ref caret))
                {
                    if (!LexcalP.VarWord(query, ref caret, out propertyName)) { return false; }
                    if (!LexcalP.VarValue(query, ref caret, out propertyValue)) { return false; }
                    qTokens.Set.Add(propertyName, propertyValue);
                }
            }
            else {
                if (!LexcalP.FixedWord(QueryTokens.FROM, query, ref caret)) { return false; }
            }

            // 「"文字列"」か、「ATTR ～」のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qTokens.From_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (LexcalP.FixedWord(QueryTokens.ATTR, query, ref caret))
            {
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return false; }
                qTokens.From_Attr = parenthesis;
            }
            else {
                return false;
            }

            if (!LexcalP.FixedWord(QueryTokens.TO, query, ref caret)) { return false; }

            // 「"文字列"」か、「ATTR ～」のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qTokens.To_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (LexcalP.FixedWord(QueryTokens.ATTR, query, ref caret))
            {
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return false; }
                qTokens.To_Attr = parenthesis;
            }
            else { return false; }

            return true;
        }

        /// <summary>
        /// 例
        /// TRANSITION DELETE
        /// FROM “Base Layer.SMove”
        /// TO ATTR (BusyX Block)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static bool ParseStatement_TransitionDelete(string query, out QueryTokens qTokens)
        {
            qTokens = new QueryTokens();
            int caret = 0;
            string stringWithoutDoubleQuotation;
            string parenthesis;
            LexcalP.VarSpaces(query, ref caret);

            if (!LexcalP.FixedWord(QueryTokens.TRANSITION, query, ref caret)) { return false; }
            qTokens.Target = QueryTokens.TRANSITION;

            if (!LexcalP.FixedWord(QueryTokens.DELETE, query, ref caret)) { return false; }
            qTokens.Manipulation = QueryTokens.DELETE;

            if (!LexcalP.FixedWord(QueryTokens.FROM, query, ref caret)) { return false; }

            // 「"文字列"」か、「ATTR ～」のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qTokens.From_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (LexcalP.FixedWord(QueryTokens.ATTR, query, ref caret))
            {
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return false; }
                qTokens.From_Attr = parenthesis;
            }
            else { return false; }

            if (!LexcalP.FixedWord(QueryTokens.TO, query, ref caret)) { return false; }

            // 「"文字列"」か、「ATTR ～」のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qTokens.To_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (LexcalP.FixedWord(QueryTokens.ATTR, query, ref caret))
            {
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return false; }
                qTokens.To_Attr = parenthesis;
            }
            else { return false; }

            return true;
        }

        /// <summary>
        /// TRANSITION SELECT
        /// FROM “Base Layer.SMove”
        /// TO ATTR (BusyX Block)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static bool ParseStatement_TransitionSelect(string query, out QueryTokens qTokens)
        {
            qTokens = new QueryTokens();
            int caret = 0;
            string stringWithoutDoubleQuotation;
            string parenthesis;
            LexcalP.VarSpaces(query, ref caret);

            if (!LexcalP.FixedWord(QueryTokens.TRANSITION, query, ref caret)) { return false; }
            qTokens.Target = QueryTokens.TRANSITION;

            if (!LexcalP.FixedWord(QueryTokens.SELECT, query, ref caret)) { return false; }
            qTokens.Manipulation = QueryTokens.SELECT;

            if (!LexcalP.FixedWord(QueryTokens.FROM, query, ref caret)) { return false; }

            // 「"文字列"」か、「ATTR ～」のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qTokens.From_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (LexcalP.FixedWord(QueryTokens.ATTR, query, ref caret))
            {
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return false; }
                qTokens.From_Attr = parenthesis;
            }
            else { return false; }

            if (!LexcalP.FixedWord(QueryTokens.TO, query, ref caret)) { return false; }

            // 「"文字列"」か、「ATTR ～」のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qTokens.To_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (LexcalP.FixedWord(QueryTokens.ATTR, query, ref caret))
            {
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return false; }
                qTokens.To_Attr = parenthesis;
            }
            else { return false; }

            return true;
        }
    }

    /// <summary>
    /// Lexcal parser (字句解析パーサー)
    /// </summary>
    public abstract class LexcalP
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
            if (match.Success) {
                stringWithoutDoubleQuotation = match.Groups[1].Value;
                // ダブルクォーテーションの２文字分を足す
                caret += stringWithoutDoubleQuotation.Length + 2 + match.Groups[2].Value.Length; return true;
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
                        closeParen.Pop(); caret++; if (0 == closeParen.Count) { goto gt_Finish; }
                        break;
                    default: if (!VarWord(query, ref caret, out word)) { goto gt_Failure; } break;
                }
            }

            gt_Finish:
            if (caret == query.Length) { parentesis = query.Substring(oldCaret); }
            else { parentesis = query.Substring(oldCaret, caret - oldCaret); }
            VarSpaces(query, ref caret); return true;
            gt_Failure:
            parentesis = ""; return false;
        }

        /// <summary>
        /// 「#」で始まる行はコメントだぜ☆（＾▽＾）
        /// コメント行と、空行は削除するぜ☆（＾～＾）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static void DeleteLineCommentAndBlankLine(ref string query)
        {
            //Debug.Log("削除前 query="+ query);
            string[] lines = query.Split(new [] { Environment.NewLine }, StringSplitOptions.None);
            int caret;
            for (int iLine = 0; iLine < lines.Length; iLine++) {
                caret = 0;
                VarSpaces(lines[iLine], ref caret);
                if (FixedWord("#", lines[iLine], ref caret)) { lines[iLine] = ""; } // コメント行だ。 空行にしておけばいいだろう。
            }
            //Debug.Log("コメント削除後 join=" + string.Join(Environment.NewLine, lines));

            // 空行を消し飛ばして困ることはあるだろうか？
            StringBuilder sb = new StringBuilder();
            foreach (string line in lines) { if ("" != line.Trim()) { sb.AppendLine(line); } } // 空行以外を残す
            query = sb.ToString();
            //Debug.Log("空行削除後 query=" + query);
        }
    }
}
