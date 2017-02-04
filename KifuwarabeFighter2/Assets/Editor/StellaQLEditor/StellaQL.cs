using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor.Animations;
using UnityEngine;

/// <summary>
/// StellaQL のセットアップ手順は Assets/Scripts/StellaQLEngine/Test_UserDefinedStateTable.cs の Step 1. から読んでください。
/// 
/// 解説: 「UnityEditorを使って2D格闘(2D Fighting game)作るときのモーション遷移図作成の半自動化に挑戦しよう＜その４＞」 http://qiita.com/muzudho1/items/baf4b06cdcda96ca9a11
/// 解説: 「Unityの上で動く、自作スクリプト言語の構文の実装の仕方」http://qiita.com/muzudho1/items/05ffb53fb4e9d4252b28
/// </summary>
namespace StellaQL
{
    /// <summary>
    /// 構文解析したトークンをここに入れる。
    /// </summary>
    public class QueryTokens
    {
        public QueryTokens() : this("")
        {
        }
        public QueryTokens(string matchedSyntaxName)
        {
            Target = "";
            Target2 = "";
            Manipulation = "";
            Set = new Dictionary<string, string>();
            From_FullnameRegex = "";
            From_Attr = "";
            To_FullnameRegex = "";
            To_Tag = "";
            Where_FullnameRegex = "";
            Where_Tag = "";

            MatchedSyntaxName = matchedSyntaxName;
        }

        public const string STATEMACHINE = "STATEMACHINE";
        public const string STATE = "STATE";
        public const string TRANSITION = "TRANSITION";
        public const string ANYSTATE = "ANYSTATE";
        public const string ENTRY = "ENTRY";
        public const string EXIT = "EXIT";
        public const string INSERT = "INSERT";
        public const string UPDATE = "UPDATE";
        public const string DELETE = "DELETE";
        public const string SELECT = "SELECT";
        public const string SET = "SET";
        public const string FROM = "FROM";
        public const string TO = "TO";
        public const string WHERE = "WHERE";
        public const string TAG = "TAG";

        /// <summary>
        /// STATEMACHINE, STATE, TRANSITION のいずれか。
        /// </summary>
        public string Target { get; set; }
        /// <summary>
        /// ANYSTATE, ENTRY, EXIT のいずれか。
        /// </summary>
        public string Target2 { get; set; }
        /// <summary>
        /// INSERT, UPDATE, DELETE, SELECT のいずれか。
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
        public string To_Tag { get; set; }
        /// <summary>
        /// ステート・フルネーム が入る。
        /// </summary>
        public string Where_FullnameRegex { get; set; }
        /// <summary>
        /// 括弧を使った式 が入る。
        /// </summary>
        public string Where_Tag { get; set; }

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
    /// Execute Query (クエリー文字列を与えて、レコード・ハッシュを取ってきて、フェッチャーがオブジェクトを取ってくる)
    /// </summary>
    public abstract class Querier
    {
        public static bool Execute(AnimatorController ac, string query, UserDefinedStateTableable userDefinedStateTable, StringBuilder message)
        {
            Dictionary<int, UserDefindStateRecordable> universe = userDefinedStateTable.StateHash_to_record;
            LexcalP.DeleteLineCommentAndBlankLine(ref query);

            QueryTokens qt = new QueryTokens("構文該当なし");

            if (SyntaxP.Parse_TransitionAnystateInsert(query, ref qt))
            {
                return Operation_Transition.AddAnyState(ac,
                    Fetcher.Statemachines(ac, RecordsFilter.Qt_From(qt, universe, message), universe),
                    Fetcher.States(ac, RecordsFilter.Qt_To(qt, universe, message), universe),
                    message);
            }
            else if (SyntaxP.Parse_TransitionEntryInsert(query, ref qt))
            {
                return Operation_Transition.AddEntryState(ac,
                    Fetcher.Statemachines(ac, RecordsFilter.Qt_From(qt, universe, message), universe),
                    Fetcher.States(ac, RecordsFilter.Qt_To(qt, universe, message), universe),
                    message);
            }
            else if (SyntaxP.Parse_TransitionExitInsert(query, ref qt))
            {
                return Operation_Transition.AddExitState(ac,
                    Fetcher.States(ac, RecordsFilter.Qt_From(qt, universe, message), universe),
                    message);
            }
            else if (SyntaxP.Parse_StateInsert(query, ref qt))
            {
                Operation_State.AddAll(ac, Fetcher.Statemachines(ac, RecordsFilter.Qt_Where(qt, universe, message), universe), qt.Set, message); return true;
            }
            else if (SyntaxP.Parse_StateUpdate(query, ref qt))
            {
                Operation_State.UpdateProperty(ac, qt.Set, Fetcher.States(ac, RecordsFilter.Qt_Where(qt, universe, message), universe), message); return true;
            }
            else if (SyntaxP.Parse_StateDelete(query, ref qt))
            {
                Operation_State.RemoveAll(ac, Fetcher.Statemachines(ac, RecordsFilter.Qt_Where(qt, universe, message), universe), qt.Set, message); return true;
            }
            else if (SyntaxP.Parse_StateSelect(query, ref qt))
            {
                HashSet<StateRecord> recordSet;
                Operation_State.Select(ac, Fetcher.States(ac, RecordsFilter.Qt_Where(qt, universe, message), universe), out recordSet, message);
                StringBuilder contents = new StringBuilder();
                AniconDataUtility.CreateCsvTable_State( recordSet, false, contents);
                StellaQLWriter.Write(StellaQLWriter.Filepath_LogStateSelect(ac.name), contents, message); return true;
            }
            else if (SyntaxP.Parse_TransitionInsert(query, ref qt))
            {
                Operation_Transition.Insert(ac,
                    Fetcher.States(ac, RecordsFilter.Qt_From(qt, universe, message), universe),
                    Fetcher.States(ac, RecordsFilter.Qt_To(qt, universe, message), universe),
                    qt.Set, message); return true;
            }
            else if (SyntaxP.Parse_TransitionUpdate(query, ref qt))
            {
                foreach (KeyValuePair<string, string> pair in qt.Set) { message.AppendLine(pair.Key + "=" + pair.Value); }
                Operation_Transition.Update(ac,
                    Fetcher.States(ac, RecordsFilter.Qt_From(qt, universe, message), universe),
                    Fetcher.States(ac, RecordsFilter.Qt_To(qt, universe, message), universe),
                     qt.Set, message); return true;
            }
            else if (SyntaxP.Parse_TransitionDelete(query, ref qt))
            {
                Operation_Transition.RemoveAll(ac,
                    Fetcher.States(ac, RecordsFilter.Qt_From(qt, universe, message), universe),
                    Fetcher.States(ac, RecordsFilter.Qt_To(qt, universe, message), universe),
                    message); return true;
            } else if (SyntaxP.Parse_TransitionSelect(query, ref qt)) {
                HashSet<TransitionRecord> recordSet;
                Operation_Transition.Select(ac,
                    Fetcher.States(ac, RecordsFilter.Qt_From(qt, universe, message), universe),
                    Fetcher.States(ac, RecordsFilter.Qt_To(qt, universe, message), universe),
                    out recordSet,
                    message);
                StringBuilder contents = new StringBuilder();
                AniconDataUtility.CreateCsvTable_Transition(recordSet, false, contents);
                StellaQLWriter.Write(StellaQLWriter.Filepath_LogTransitionSelect(ac.name), contents, message); return true;
            }
            message.Append("構文エラー: "); message.Append( qt.MatchedSyntaxName); message.Append(" ");
            message.Append(qt.MatchedSyntaxCaret); message.AppendLine(" 文字目まで一致（改行は２文字とカウント）"); return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static bool ExecuteStateSelect(string query, Dictionary<int, UserDefindStateRecordable> universe, out HashSet<int> recordHashes, StringBuilder message)
        {
            LexcalP.DeleteLineCommentAndBlankLine(ref query);

            recordHashes = null;
            QueryTokens qt = new QueryTokens();
            if (!SyntaxP.Parse_StateSelect(query, ref qt)) { return false; }

            recordHashes = RecordsFilter.Qt_Where(qt, universe, message);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static bool ExecuteTransitionSelect(string query, Dictionary<int, UserDefindStateRecordable> universe, out HashSet<int> recordHashesSrc, out HashSet<int> recordHashesDst, StringBuilder message)
        {
            LexcalP.DeleteLineCommentAndBlankLine(ref query);

            recordHashesSrc = null;
            recordHashesDst = null;
            QueryTokens qt = new QueryTokens();
            if (!SyntaxP.Parse_TransitionSelect(query, ref qt)) { return false; }

            recordHashesSrc = RecordsFilter.Qt_From(qt, universe, message);// FROM
            recordHashesDst = RecordsFilter.Qt_To(qt, universe, message);// TO
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
    /// Fetch (オブジェクトを取ってくる)
    /// </summary>
    public abstract class Fetcher
    {
        public static HashSet<AnimatorStateMachine> Statemachines(AnimatorController ac, HashSet<int> recordHashes, Dictionary<int, UserDefindStateRecordable> universe)
        {
            HashSet<AnimatorStateMachine> statemachines = new HashSet<AnimatorStateMachine>();
            foreach (int recordHash in recordHashes)
            {
                statemachines.Add(Operation_Statemachine.Lookup(ac, universe[recordHash].Fullpath));
            }
            return statemachines;
        }

        public static HashSet<AnimatorState> States(AnimatorController ac, HashSet<int> recordHashes, Dictionary<int, UserDefindStateRecordable> universe)
        {
            HashSet<AnimatorState> states = new HashSet<AnimatorState>();
            foreach (int recordHash in recordHashes)
            {
                states.Add(Operation_State.Lookup(ac, universe[recordHash].Fullpath));//.Name
            }
            return states;
        }
    }

    /// <summary>
    /// Record Filter (レコード・ハッシュを取ってくる)
    /// </summary>
    public abstract class RecordsFilter
    {
        /// <summary>
        /// トークン・ロッカーを元に、ロッカー別の検索結果を返す。
        /// </summary>
        /// <param name="tokens"></param>
        public static void TokenLockers_to_recordHashesLockers(List<List<string>> lockerNumber_to_tokens, List<string> lockerNumber_to_operation,
            Dictionary<int, UserDefindStateRecordable> universe, out List<HashSet<int>> lockerNumber_to_recordHashes)
        {
            lockerNumber_to_recordHashes = new List<HashSet<int>>();

            for (int iLockerNumber = 0; iLockerNumber < lockerNumber_to_tokens.Count; iLockerNumber++)// 部室のロッカー番号。スタートは 0 番から。
            {
                List<string> index_to_token = lockerNumber_to_tokens[iLockerNumber];
                string operation = lockerNumber_to_operation[iLockerNumber];// 「(」「[」「{」 がある。

                int firstItem_temp;
                if (int.TryParse(index_to_token[0], out firstItem_temp))
                { // 数字だったら、ロッカー番号だ。
                    HashSet<int> lockerNumbers = TagSetOpe.NumberToken_to_int(index_to_token);
                    switch (operation) // ロッカー同士を演算して、まとめた答えを出す
                    {
                        case "(": lockerNumber_to_recordHashes.Add(RecordsFilter.Records_And(lockerNumbers, lockerNumber_to_recordHashes)); break;
                        case "[": lockerNumber_to_recordHashes.Add(RecordsFilter.Records_Or(lockerNumbers, lockerNumber_to_recordHashes)); break;
                        case "{":
                            lockerNumber_to_recordHashes.Add(RecordsFilter.Records_NotAndNot(
                      lockerNumbers, lockerNumber_to_recordHashes, universe)); break;
                        default: throw new UnityException("未対応1のtokenOperation=[" + operation + "]");
                    }
                }
                else { // 数字じゃなかったら、属性名のリストだ
                    HashSet<int> attrEnumSet_src = TagSetOpe.Name_to_hash(new HashSet<string>(index_to_token));
                    switch (operation) // 属性結合（演算）を解消する
                    {
                        case "(":
                            lockerNumber_to_recordHashes.Add(RecordsFilter.Tags_And(attrEnumSet_src, universe)); break;
                        case "[":
                            lockerNumber_to_recordHashes.Add(RecordsFilter.Tags_Or(attrEnumSet_src, universe)); break;
                        case "{":
                            lockerNumber_to_recordHashes.Add(RecordsFilter.Tags_NotAndNot(attrEnumSet_src, universe)); break;
                        default: throw new UnityException("未対応2のtokenOperation=[" + operation + "]");
                    }
                }
            }
        }

        public static HashSet<int> Qt_From(QueryTokens qt, Dictionary<int, UserDefindStateRecordable> universe, StringBuilder message)
        {
            if ("" != qt.From_FullnameRegex) { return RecordsFilter.String_StateFullNameRegex(qt.From_FullnameRegex, universe, message); }
            else {
                List<string> tokens; SyntaxPOther.String_to_tokens(qt.From_Attr, out tokens);

                List<List<string>> tokenLockers;
                List<string> tokenLockersOperation;
                Querier.Tokens_to_lockers(tokens, out tokenLockers, out tokenLockersOperation);

                List<HashSet<int>> recordHashesLockers;
                RecordsFilter.TokenLockers_to_recordHashesLockers(tokenLockers, tokenLockersOperation, universe, out recordHashesLockers);
                return recordHashesLockers[recordHashesLockers.Count - 1];
            }
        }

        public static HashSet<int> Qt_To(QueryTokens qt, Dictionary<int, UserDefindStateRecordable> universe, StringBuilder message)
        {
            if ("" != qt.To_FullnameRegex) { return RecordsFilter.String_StateFullNameRegex(qt.To_FullnameRegex, universe, message); }
            else {
                List<string> tokens; SyntaxPOther.String_to_tokens(qt.To_Tag, out tokens);

                List<List<string>> tokenLockers;
                List<string> tokenLockersOperation;
                Querier.Tokens_to_lockers(tokens, out tokenLockers, out tokenLockersOperation);

                List<HashSet<int>> recordHashesLockers;
                RecordsFilter.TokenLockers_to_recordHashesLockers(tokenLockers, tokenLockersOperation, universe, out recordHashesLockers);
                return recordHashesLockers[recordHashesLockers.Count - 1];
            }
        }

        public static HashSet<int> Qt_Where(QueryTokens qt, Dictionary<int, UserDefindStateRecordable> universe, StringBuilder message)
        {
            if ("" != qt.Where_FullnameRegex) { return RecordsFilter.String_StateFullNameRegex(qt.Where_FullnameRegex, universe, message); }
            else {
                List<string> tokens; SyntaxPOther.String_to_tokens(qt.Where_Tag, out tokens);

                List<List<string>> tokenLockers;
                List<string> tokenLockersOperation;
                Querier.Tokens_to_lockers(tokens, out tokenLockers, out tokenLockersOperation);

                List<HashSet<int>> recordHashesLockers;
                RecordsFilter.TokenLockers_to_recordHashesLockers(tokenLockers, tokenLockersOperation, universe, out recordHashesLockers);
                return recordHashesLockers[recordHashesLockers.Count - 1];
            }
        }

        public static HashSet<int> String_StateFullNameRegex(string pattern, Dictionary<int, UserDefindStateRecordable> universe, StringBuilder message)
        {
            HashSet<int> hitRecordHashes = new HashSet<int>();

            Regex regex = new Regex(pattern);
            foreach (KeyValuePair<int, UserDefindStateRecordable> pair in universe)
            {
                if (regex.IsMatch(pair.Value.Fullpath))
                {
                    hitRecordHashes.Add(pair.Key);
                }
            }

            if (hitRecordHashes.Count < 1) {
                message.AppendLine("Mension: Animation Controller Path OK?");
                message.AppendLine("Mension: const string STATE_xxx OK?");
            }
            return hitRecordHashes;
        }

        public static HashSet<int> Records_And(HashSet<int> lockerNumbers, List<HashSet<int>> recordHasheslockers)
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

        public static HashSet<int> Records_Or(HashSet<int> lockerNumbers, List<HashSet<int>> recordHasheslockers)
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

        public static HashSet<int> Records_NotAndNot(HashSet<int> lockerNumbers, List<HashSet<int>> recordHasheslockers, Dictionary<int, UserDefindStateRecordable> universe)
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

        public static HashSet<int> Tags_And(HashSet<int> attrs, Dictionary<int, UserDefindStateRecordable> universe)
        {
            HashSet<int> hitRecordHashes = new HashSet<int>(universe.Keys);
            foreach (int attr in attrs)
            {
                HashSet<int> records_empty = new HashSet<int>();
                foreach (int recordHash in hitRecordHashes)
                {
                    if (universe[recordHash].HasEverythingTags(new HashSet<int>() { attr })) { records_empty.Add(recordHash); }// 該当したもの
                }
                hitRecordHashes = records_empty;
            }
            return hitRecordHashes;
        }

        public static HashSet<int> Tags_Or(HashSet<int> orAllTags, Dictionary<int, UserDefindStateRecordable> universe)
        {
            HashSet<int> hitRecordHashes = new HashSet<int>();// レコード・インデックスを属性検索（重複除外）
            foreach (KeyValuePair<int, UserDefindStateRecordable> pair in universe)
            {
                foreach (int attr in orAllTags)
                {
                    if (pair.Value.HasEverythingTags(new HashSet<int>() { attr })) { hitRecordHashes.Add(pair.Key); }
                }
            }

            return hitRecordHashes;
        }

        public static HashSet<int> Tags_NotAndNot(HashSet<int> requireAllTags, Dictionary<int, UserDefindStateRecordable> recordUniverse)
        {
            HashSet<int> hitRecordHashes = new HashSet<int>();// レコード・インデックスを属性検索（重複除外）
            foreach (KeyValuePair<int, UserDefindStateRecordable> pair in recordUniverse)
            {
                foreach (int attr in requireAllTags)
                {
                    if (pair.Value.HasEverythingTags(new HashSet<int>() { attr })) { hitRecordHashes.Add(pair.Key); }
                }
            }

            List<int> complementRecordHashes = new List<int>();// 補集合を取る
            {
                foreach (int recordHash in recordUniverse.Keys) { complementRecordHashes.Add(recordHash); }// 列挙型の中身をリストに移動。
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
    /// Tag Set Operation (タグのハッシュ)
    /// TODO: 列挙型の使用は止め、タグの総当たり検索にしたい。
    /// 列挙型の扱い方：「文字列を列挙体に変換する」（DOBON.NET） http://dobon.net/vb/dotnet/programing/enumparse.html
    /// </summary>
    public abstract class TagSetOpe
    {
        /// <summary>
        /// 補集合
        /// </summary>
        public static HashSet<int> Complement(HashSet<int> bitfieldSet, HashSet<int> tagUniverse)
        {
            List<int> complement = new List<int>();
            foreach (int elem in tagUniverse) { complement.Add(elem); }// 列挙型の中身をリストに移動。
            for (int iComp = complement.Count - 1; -1 < iComp; iComp--)// 後ろから指定の要素を削除する。
            {
                if (bitfieldSet.Contains(complement[iComp])) { complement.RemoveAt(iComp); }
            }
            return new HashSet<int>(complement);
        }

        public static HashSet<int> NumberToken_to_int(List<string> numberTokens)
        {
            HashSet<int> intSet = new HashSet<int>();
            foreach (string numberToken in numberTokens) { intSet.Add(int.Parse(numberToken)); }// 変換できなかったら例外を投げる
            return intSet;
        }

        /// <summary>
        /// </summary>
        /// <param name="nameSet"></param>
        /// <param name="enumration"></param>
        /// <returns></returns>
        public static HashSet<int> Name_to_hash(HashSet<string> nameSet)
        {
            HashSet<int> hashSet = new HashSet<int>();
            foreach (string name in nameSet) { hashSet.Add( Animator.StringToHash(name)); }// 変換できなかったら例外を投げる
            return hashSet;
        }
    }

    /// <summary>
    /// Syntax parser (構文パーサー)
    /// 正規表現の参考：http://smdn.jp/programming/netfx/regex/2_expressions/
    /// </summary>
    public abstract class SyntaxP
    {
        public static bool Error(QueryTokens current, int caret, ref QueryTokens max)
        {
            if (max.MatchedSyntaxCaret < caret) { current.MatchedSyntaxCaret = caret; max = current; } return false;
        }

        /// <summary>
        /// SET句だけ。
        /// Example（代表例）: UPDATE文のSET句。
        /// </summary>
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
        /// TRANSITION ANYSTATE INSERT
        /// </summary>
        public static bool Parse_TransitionAnystateInsert(string query, ref QueryTokens maxQt)
        {
            QueryTokens qt = new QueryTokens("TRANSITION ANYSTATE INSERT");
            int caret = 0;
            string stringWithoutDoubleQuotation, parenthesis;

            LexcalP.VarSpaces(query, ref caret);

            if (!LexcalP.FixedWord(QueryTokens.TRANSITION, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.TRANSITION;

            if (!LexcalP.FixedWord(QueryTokens.ANYSTATE, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            qt.Target2 = QueryTokens.ANYSTATE;

            if (!LexcalP.FixedWord(QueryTokens.INSERT, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.INSERT;

            if (!LexcalP.FixedWord(QueryTokens.FROM, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }

            // 正規表現か、タグ検索のどちらか。（マシンステート検索でもタグを使うことがあるだろうか）
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation)){
                qt.From_FullnameRegex = stringWithoutDoubleQuotation;
            }else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret)){
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return SyntaxP.Error(qt, caret, ref maxQt); }
                qt.From_Attr = parenthesis;
            }else { return SyntaxP.Error(qt, caret, ref maxQt); }

            if (!LexcalP.FixedWord(QueryTokens.TO, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }

            // 正規表現か、タグ検索のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation)){
                qt.To_FullnameRegex = stringWithoutDoubleQuotation;
            }else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret)){
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return SyntaxP.Error(qt, caret, ref maxQt); }
                qt.To_Tag = parenthesis;
            }
            else { return SyntaxP.Error(qt, caret, ref maxQt); }
            maxQt = qt; return true;
        }

        /// <summary>
        /// TRANSITION ENTRY INSERT
        /// </summary>
        public static bool Parse_TransitionEntryInsert(string query, ref QueryTokens maxQt)
        {
            QueryTokens qt = new QueryTokens("TRANSITION ENTRY INSERT");
            int caret = 0;
            string stringWithoutDoubleQuotation, parenthesis;

            LexcalP.VarSpaces(query, ref caret);

            if (!LexcalP.FixedWord(QueryTokens.TRANSITION, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.TRANSITION;

            if (!LexcalP.FixedWord(QueryTokens.ENTRY, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            qt.Target2 = QueryTokens.ENTRY;

            if (!LexcalP.FixedWord(QueryTokens.INSERT, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.INSERT;

            if (!LexcalP.FixedWord(QueryTokens.FROM, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }

            // 正規表現か、タグ検索のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation)){
                qt.From_FullnameRegex = stringWithoutDoubleQuotation;
            }else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret)){
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return SyntaxP.Error(qt, caret, ref maxQt); }
                qt.From_Attr = parenthesis;
            }else { return SyntaxP.Error(qt, caret, ref maxQt); }

            if (!LexcalP.FixedWord(QueryTokens.TO, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }

            // 正規表現か、タグ検索のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation)){
                qt.To_FullnameRegex = stringWithoutDoubleQuotation;
            }else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret)){
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return SyntaxP.Error(qt, caret, ref maxQt); }
                qt.To_Tag = parenthesis;
            }
            else { return SyntaxP.Error(qt, caret, ref maxQt); }
            maxQt = qt; return true;
        }

        /// <summary>
        /// TRANSITION EXIT INSERT
        /// </summary>
        public static bool Parse_TransitionExitInsert(string query, ref QueryTokens maxQt)
        {
            QueryTokens qt = new QueryTokens("TRANSITION EXIT INSERT");
            int caret = 0;
            string stringWithoutDoubleQuotation, parenthesis;

            LexcalP.VarSpaces(query, ref caret);

            if (!LexcalP.FixedWord(QueryTokens.TRANSITION, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.TRANSITION;

            if (!LexcalP.FixedWord(QueryTokens.EXIT, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            qt.Target2 = QueryTokens.EXIT;

            if (!LexcalP.FixedWord(QueryTokens.INSERT, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.INSERT;

            if (!LexcalP.FixedWord(QueryTokens.FROM, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }

            // 正規表現か、タグ検索のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation)){
                qt.From_FullnameRegex = stringWithoutDoubleQuotation;
            }else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret)){
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return SyntaxP.Error(qt, caret, ref maxQt); }
                qt.From_Attr = parenthesis;
            }else { return SyntaxP.Error(qt, caret, ref maxQt); }
            maxQt = qt; return true;
        }

        /// <summary>
        /// STATE INSERT
        /// </summary>
        public static bool Parse_StateInsert(string query, ref QueryTokens maxQt)
        {
            QueryTokens qt = new QueryTokens("STATE INSERT");
            int caret = 0;
            string stringWithoutDoubleQuotation;
            LexcalP.VarSpaces(query, ref caret);

            if (!LexcalP.FixedWord(QueryTokens.STATE, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.STATE;

            if (!LexcalP.FixedWord(QueryTokens.INSERT, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.INSERT;

            if (LexcalP.FixedWord(QueryTokens.SET, query, ref caret))
            {
                // 「項目名、スペース、値、スペース」の繰り返し。項目名が WHERE だった場合終わり。
                if (!SyntaxP.ParsePhrase_AfterSet(query, ref caret, QueryTokens.WHERE, qt.Set)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            }
            else { if (!LexcalP.FixedWord(QueryTokens.WHERE, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); } }

            // 正規表現。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qt.Where_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else { return SyntaxP.Error(qt, caret, ref maxQt); }
            maxQt = qt; return true;
        }

        /// <summary>
        /// STATE UPDATE
        /// </summary>
        public static bool Parse_StateUpdate(string query, ref QueryTokens maxQt)
        {
            QueryTokens qt = new QueryTokens("STATE UPDATE");
            int caret = 0;
            string stringWithoutDoubleQuotation;
            string parenthesis;
            LexcalP.VarSpaces(query, ref caret);

            if (!LexcalP.FixedWord(QueryTokens.STATE, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.STATE;

            if (!LexcalP.FixedWord(QueryTokens.UPDATE, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.UPDATE;

            if (LexcalP.FixedWord(QueryTokens.SET, query, ref caret)) {
                // 「項目名、スペース、値、スペース」の繰り返し。項目名が WHERE だった場合終わり。
                if(!SyntaxP.ParsePhrase_AfterSet(query, ref caret, QueryTokens.WHERE, qt.Set)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            } else { if (!LexcalP.FixedWord(QueryTokens.WHERE, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); } }

            // 正規表現か、タグ検索のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation)) {
                qt.Where_FullnameRegex = stringWithoutDoubleQuotation;
            } else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret)) {
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return SyntaxP.Error(qt, caret, ref maxQt); }
                qt.Where_Tag = parenthesis;
            }
            else { return SyntaxP.Error(qt, caret, ref maxQt); }
            maxQt = qt; return true;
        }

        /// <summary>
        /// STATE DELETE
        /// </summary>
        public static bool Parse_StateDelete(string query, ref QueryTokens maxQt)
        {
            QueryTokens qt = new QueryTokens("STATE DELETE");
            int caret = 0;
            string stringWithoutDoubleQuotation;
            LexcalP.VarSpaces(query, ref caret);

            if (!LexcalP.FixedWord(QueryTokens.STATE, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.STATE;

            if (!LexcalP.FixedWord(QueryTokens.DELETE, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.DELETE;

            if (LexcalP.FixedWord(QueryTokens.SET, query, ref caret))
            {
                // 「項目名、スペース、値、スペース」の繰り返し。項目名が WHERE だった場合終わり。
                if (!SyntaxP.ParsePhrase_AfterSet(query, ref caret, QueryTokens.WHERE, qt.Set)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            }
            else { if (!LexcalP.FixedWord(QueryTokens.WHERE, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); } }

            // 正規表現。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qt.Where_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else { return SyntaxP.Error(qt, caret, ref maxQt); }
            maxQt = qt; return true;
        }

        /// <summary>
        /// STATE SELECT
        /// </summary>
        public static bool Parse_StateSelect(string query, ref QueryTokens maxQt)
        {
            QueryTokens qt = new QueryTokens("STATE SELECT");
            int caret = 0;
            string stringWithoutDoubleQuotation;
            string parenthesis;
            LexcalP.VarSpaces(query, ref caret);

            if (!LexcalP.FixedWord(QueryTokens.STATE, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.STATE;

            if (!LexcalP.FixedWord(QueryTokens.SELECT, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.SELECT;

            if (!LexcalP.FixedWord(QueryTokens.WHERE, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }

            // 正規表現か、タグ検索のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qt.Where_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret))
            {
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return SyntaxP.Error(qt, caret, ref maxQt); }
                qt.Where_Tag = parenthesis;
            }
            else { return SyntaxP.Error(qt, caret, ref maxQt); }

            maxQt = qt; return true;
        }

        /// <summary>
        /// TRANSITION INSERT
        /// </summary>
        public static bool Parse_TransitionInsert(string query, ref QueryTokens maxQt)
        {
            QueryTokens qt = new QueryTokens("TRANSITION INSERT");
            int caret = 0;
            string stringWithoutDoubleQuotation;
            string parenthesis;
            LexcalP.VarSpaces(query, ref caret);

            if (!LexcalP.FixedWord(QueryTokens.TRANSITION, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.TRANSITION;

            if (!LexcalP.FixedWord(QueryTokens.INSERT, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.INSERT;

            if (LexcalP.FixedWord(QueryTokens.SET, query, ref caret))
            {
                // 「項目名、スペース、値、スペース」の繰り返し。項目名が FROM だった場合終わり。
                if (!SyntaxP.ParsePhrase_AfterSet(query, ref caret, QueryTokens.FROM, qt.Set)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            }
            else {
                if (!LexcalP.FixedWord(QueryTokens.FROM, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            }

            // 正規表現か、タグ検索のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qt.From_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret))
            {
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return SyntaxP.Error(qt, caret, ref maxQt); }
                qt.From_Attr = parenthesis;
            }
            else { return SyntaxP.Error(qt, caret, ref maxQt); }

            if (!LexcalP.FixedWord(QueryTokens.TO, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }

            // 正規表現か、タグ検索のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qt.To_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret))
            {
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return SyntaxP.Error(qt, caret, ref maxQt); }
                qt.To_Tag = parenthesis;
            }
            else { return SyntaxP.Error(qt, caret, ref maxQt); }

            maxQt = qt; return true;
        }

        /// <summary>
        /// TRANSITION UPDATE
        /// </summary>
        public static bool Parse_TransitionUpdate(string query, ref QueryTokens maxQt)
        {
            QueryTokens qt = new QueryTokens("TRANSITION UPDATE");
            int caret = 0;
            //string propertyName;
            //string propertyValue;
            string stringWithoutDoubleQuotation;
            string parenthesis;
            LexcalP.VarSpaces(query, ref caret);

            if (!LexcalP.FixedWord(QueryTokens.TRANSITION, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.TRANSITION;

            if (!LexcalP.FixedWord(QueryTokens.UPDATE, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.UPDATE;

            if (LexcalP.FixedWord(QueryTokens.SET, query, ref caret))
            {
                // 「項目名、スペース、値、スペース」の繰り返し。項目名が FROM だった場合終わり。
                if (!SyntaxP.ParsePhrase_AfterSet(query, ref caret, QueryTokens.FROM, qt.Set)) { return SyntaxP.Error(qt, caret, ref maxQt); }
                //while (caret < query.Length && !LexcalP.FixedWord(QueryTokens.FROM, query, ref caret))
                //{
                //    if (!LexcalP.VarWord(query, ref caret, out propertyName)) { return false; }
                //    if (!LexcalP.VarValue(query, ref caret, out propertyValue)) { return false; }
                //    qTokens.Set.Add(propertyName, propertyValue);
                //}
            }
            else {
                if (!LexcalP.FixedWord(QueryTokens.FROM, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            }

            // 正規表現か、タグ検索のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qt.From_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret))
            {
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return SyntaxP.Error(qt, caret, ref maxQt); }
                qt.From_Attr = parenthesis;
            }
            else {
                return SyntaxP.Error(qt, caret, ref maxQt);
            }

            if (!LexcalP.FixedWord(QueryTokens.TO, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }

            // 正規表現か、タグ検索のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qt.To_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret))
            {
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return SyntaxP.Error(qt, caret, ref maxQt); }
                qt.To_Tag = parenthesis;
            }
            else { return SyntaxP.Error(qt, caret, ref maxQt); }

            maxQt = qt; return true;
        }

        /// <summary>
        /// TRANSITION DELETE
        /// </summary>
        public static bool Parse_TransitionDelete(string query, ref QueryTokens maxQt)
        {
            QueryTokens qt = new QueryTokens("TRANSITION DELETE");
            int caret = 0;
            string stringWithoutDoubleQuotation;
            string parenthesis;
            LexcalP.VarSpaces(query, ref caret);

            if (!LexcalP.FixedWord(QueryTokens.TRANSITION, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.TRANSITION;

            if (!LexcalP.FixedWord(QueryTokens.DELETE, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.DELETE;

            if (!LexcalP.FixedWord(QueryTokens.FROM, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }

            // 正規表現か、タグ検索のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qt.From_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret))
            {
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return SyntaxP.Error(qt, caret, ref maxQt); }
                qt.From_Attr = parenthesis;
            }
            else { return SyntaxP.Error(qt, caret, ref maxQt); }

            if (!LexcalP.FixedWord(QueryTokens.TO, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }

            // 正規表現か、タグ検索のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qt.To_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret))
            {
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return SyntaxP.Error(qt, caret, ref maxQt); }
                qt.To_Tag = parenthesis;
            }
            else { return SyntaxP.Error(qt, caret, ref maxQt); }

            maxQt = qt; return true;
        }

        /// <summary>
        /// TRANSITION SELECT
        /// </summary>
        public static bool Parse_TransitionSelect(string query, ref QueryTokens maxQt)
        {
            QueryTokens qt = new QueryTokens("TRANSITION SELECT");
            int caret = 0;
            string stringWithoutDoubleQuotation;
            string parenthesis;
            LexcalP.VarSpaces(query, ref caret);

            if (!LexcalP.FixedWord(QueryTokens.TRANSITION, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.TRANSITION;

            if (!LexcalP.FixedWord(QueryTokens.SELECT, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.SELECT;

            if (!LexcalP.FixedWord(QueryTokens.FROM, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }

            // 正規表現か、タグ検索のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qt.From_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret))
            {
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return SyntaxP.Error(qt, caret, ref maxQt); }
                qt.From_Attr = parenthesis;
            }
            else { return SyntaxP.Error(qt, caret, ref maxQt); }

            if (!LexcalP.FixedWord(QueryTokens.TO, query, ref caret)) { return SyntaxP.Error(qt, caret, ref maxQt); }

            // 正規表現か、タグ検索のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qt.To_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret))
            {
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return SyntaxP.Error(qt, caret, ref maxQt); }
                qt.To_Tag = parenthesis;
            }
            else { return SyntaxP.Error(qt, caret, ref maxQt); }

            maxQt = qt; return true;
        }
    }

    public abstract class SyntaxPOther
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

    /// <summary>
    /// Lexcal parser (字句解析パーサー)
    /// </summary>
    public abstract class LexcalP
    {
        private static Regex regexSpaces = new Regex(@"^(\s+)");
        public static bool VarSpaces(string query, ref int caret) {
            Match match = regexSpaces.Match(query.Substring(caret));
            if (match.Success) { caret += match.Groups[1].Value.Length; return true; } return false;
        }

        public static bool FixedWord(string word, string query, ref int caret) {
            int oldCaret = caret;
            if (caret == query.IndexOf(word, caret, StringComparison.OrdinalIgnoreCase)){
                caret += word.Length;
                if (caret == query.Length || VarSpaces(query, ref caret)) { return true; }
            }
            caret = oldCaret; return false;
        }

        /// <summary>
        /// "bear)" など後ろに半角スペースが付かないケースもあるので、スペースは 0 個も OK とする。
        /// </summary>
        private static Regex regexWordAndSpaces = new Regex(@"^(\w+)(\s*)", RegexOptions.IgnoreCase);
        public static bool VarWord(string query, ref int caret, out string word) {
            Match match = regexWordAndSpaces.Match(query.Substring(caret));
            if (match.Success) { word = match.Groups[1].Value; caret += word.Length + match.Groups[2].Value.Length; return true; }
            word = ""; return false;
        }

        /// <summary>
        /// 浮動小数点の「.」もOKとする。
        /// </summary>
        private static Regex regexValueAndSpaces = new Regex(@"^((?:\w|\.)+)(\s*)", RegexOptions.IgnoreCase);
        public static bool VarValue(string query, ref int caret, out string word) {
            Match match = regexValueAndSpaces.Match(query.Substring(caret));
            if (match.Success) { word = match.Groups[1].Value; caret += word.Length + match.Groups[2].Value.Length; return true; }
            word = ""; return false;
        }

        private static Regex regexStringliteralAndSpaces = new Regex(@"^""((?:(?:\\"")|[^""])*)""(\s*)", RegexOptions.IgnoreCase);
        public static bool VarStringliteral(string query, ref int caret, out string stringWithoutDoubleQuotation) {
            Match match = regexStringliteralAndSpaces.Match(query.Substring(caret));
            if (match.Success) {
                stringWithoutDoubleQuotation = match.Groups[1].Value;
                // ダブルクォーテーションの２文字分を足す
                caret += stringWithoutDoubleQuotation.Length + 2 + match.Groups[2].Value.Length; return true;
            }
            stringWithoutDoubleQuotation = ""; return false;
        }

        public static bool VarParentesis(string query, ref int caret, out string parentesis) {
            int oldCaret = caret;
            string word;
            Stack<char> closeParen = new Stack<char>();

            switch (query[caret]) { // 開始時
                case '(': closeParen.Push(')'); caret++; break;
                case '[': closeParen.Push(']'); caret++; break;
                case '{': closeParen.Push('}'); caret++; break;
                default: goto gt_Failure;
            }
            VarSpaces(query, ref caret);

            while (caret < query.Length) {
                switch (query[caret]) {
                    case '(': closeParen.Push(')'); caret++; VarSpaces(query, ref caret); break;
                    case '[': closeParen.Push(']'); caret++; VarSpaces(query, ref caret); break;
                    case '{': closeParen.Push('}'); caret++; VarSpaces(query, ref caret); break;
                    case ')':
                    case ']':
                    case '}': if (query[caret] != closeParen.Peek()) { goto gt_Failure; }
                        closeParen.Pop(); caret++; VarSpaces(query, ref caret);
                        if (0 == closeParen.Count) { goto gt_Finish; } break;
                    default: if (!VarWord(query, ref caret, out word)) { goto gt_Failure; } break;
                }
            }
            gt_Finish:
            if (caret == query.Length) { parentesis = query.Substring(oldCaret).TrimEnd(); }
            else { parentesis = query.Substring(oldCaret, caret - oldCaret).TrimEnd(); }
            VarSpaces(query, ref caret); return true;
            gt_Failure:
            parentesis = query.Substring(oldCaret, caret - oldCaret); // エラーだが、解析したところまでは返したい。
            return false;
        }

        /// <summary>
        /// 「#」で始まる行はコメントだぜ☆（＾▽＾）
        /// コメント行と、空行は削除するぜ☆（＾～＾）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static void DeleteLineCommentAndBlankLine(ref string query) {
            string[] lines = query.Split(new [] { Environment.NewLine }, StringSplitOptions.None);
            int caret;
            for (int iLine = 0; iLine < lines.Length; iLine++) {
                caret = 0;
                VarSpaces(lines[iLine], ref caret);
                if (FixedWord("#", lines[iLine], ref caret)) { lines[iLine] = ""; } // コメント行だ。 空行にしておけばいいだろう。
            }

            // 空行を消し飛ばして困ることはあるだろうか？
            StringBuilder sb = new StringBuilder();
            foreach (string line in lines) { if ("" != line.Trim()) { sb.AppendLine(line); } } // 空行以外を残す
            query = sb.ToString();
        }
    }
}
