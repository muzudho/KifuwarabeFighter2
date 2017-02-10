﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor.Animations;
using UnityEngine;

/// <summary>
/// StellaQL のセットアップ手順は Assets/StellaQL/Demo_Zoo.cs の Step 1. から読んでください。
/// 
/// 解説: 「UnityEditorを使って2D格闘(2D Fighting game)作るときのモーション遷移図作成の半自動化に挑戦しよう＜その４＞」 http://qiita.com/muzudho1/items/baf4b06cdcda96ca9a11
/// 解説: 「Unityの上で動く、自作スクリプト言語の構文の実装の仕方」http://qiita.com/muzudho1/items/05ffb53fb4e9d4252b28
/// </summary>
namespace StellaQL
{
    /// <summary>
    /// セミコロンでつなげられたクエリーを先頭から順次、実行していく。
    /// </summary>
    public abstract class SequenceQuerier
    {
        public static bool Execute(AnimatorController ac, string query, AControllable userDefinedAControl, StringBuilder info_message)
        {
            LexcalP.DeleteLineCommentAndBlankLine(ref query);// コメントと空行を全削除する。

            // phase
            // 0: 次はクエリーか、セミコロンのどちらか。（読込み初期時や、セミコロンを読込んだ直後など）
            // 1: 次にセミコロンがくることが必要。（クエリーを読込んだ直後など）
            int phase = 0; 
            int caret = 0;
            QueryTokens qt = new QueryTokens("クエリー構文該当なし");
            while (caret<query.Length)
            {
                switch (phase)
                {
                    case 1:
                        if (LexcalP.FixedWord(";", query, ref caret)) { phase = 0; break; }// セミコロンが有った。次はクエリーか、セミコロンのどちらか。
                        else { throw new UnityException("セミコロンが無かったぜ☆（＾▽＾）！ 残り:" + query.Substring(caret)); }
                    default:
                        {
                            SyntaxP.Pattern syntaxPattern = SyntaxP.FixedQuery(query, ref caret, ref qt);
                            if (SyntaxP.Pattern.NotMatch == syntaxPattern) // 正常動作
                            {
                                qt.Clear("クエリー構文該当なし"); // 次に使うためにクリアーしておく。
                                if (LexcalP.FixedWord(";", query, ref caret)) { phase = 0; return true; }// セミコロンが有った。次はクエリーか、セミコロンのどちらか。
                                else { throw new UnityException("クエリーも、セミコロンも無いぜ☆（＾▽＾）！ 残り:" + query.Substring(caret)); }// クエリーもセミコロンも無いときは異常終了。
                            }
                            else
                            {
                                Querier.Execute(ac, qt, syntaxPattern, userDefinedAControl, info_message);//実行
                                qt.Clear("クエリー構文該当なし"); // 次に使うためにクリアーしておく。
                                phase = 1;
                            }// クエリーが有った。次は必ずセミコロンが必要。
                        }
                        break;
                }
            }
            return true;//正常終了
        }
    }

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
            this.Clear(matchedSyntaxName);
        }
        public void Clear(string matchedSyntaxName)
        {
            Target = "";
            Target2 = "";
            Manipulation = "";
            Words = new List<string>();
            Set = new Dictionary<string, string>();
            From_FullnameRegex = "";
            From_Tag = "";
            To_FullnameRegex = "";
            To_Tag = "";
            Where_FullnameRegex = "";
            Where_Tag = "";
            The = "";

            MatchedSyntaxName = matchedSyntaxName;
        }

        public const string STATEMACHINE = "STATEMACHINE";
        public const string STATE = "STATE";
        public const string TRANSITION = "TRANSITION";
        public const string ANYSTATE = "ANYSTATE";
        public const string ENTRY = "ENTRY";
        public const string EXIT = "EXIT";
        public const string CSHARPSCRIPT = "CSHARPSCRIPT";
        public const string INSERT = "INSERT";
        public const string UPDATE = "UPDATE";
        public const string DELETE = "DELETE";
        public const string SELECT = "SELECT";
        public const string GENERATE_FULLPATH = "GENERATE_FULLPATH";
        public const string WORDS = "WORDS";
        public const string SET = "SET";
        public const string FROM = "FROM";
        public const string TO = "TO";
        public const string WHERE = "WHERE";
        public const string TAG = "TAG";
        public const string THE = "THE";

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
        /// WORDS部。大文字小文字は区別したい。
        /// </summary>
        public List<string> Words { get; set; }
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
        public string From_Tag { get; set; }
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
        /// 出力ファイル名の重複を防ぐための文字列 が入る。
        /// </summary>
        public string The { get; set; }

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
    /// Execute Query (分解後のクエリー・トークンを与えて、レコード・ハッシュを取ってきて、フェッチャーがオブジェクトを取ってくる)
    /// FIXME: クエリー文字列ソースや、キャレットは与えない方がいい。パーサーとは切り分けたい。
    /// </summary>
    public abstract class Querier
    {
        public static bool Execute(AnimatorController ac, QueryTokens qt, SyntaxP.Pattern syntaxPattern, AControllable userDefinedAControl, StringBuilder info_message)
        {
            Dictionary<int, AcStateRecordable> universe = userDefinedAControl.StateHash_to_record;
            switch (syntaxPattern)
            {
                case SyntaxP.Pattern.TransitionAnystateInsert:
                    {
                        return Operation_Transition.AddAnyState(ac,
                            Fetcher.Statemachines(ac, RecordsFilter.Qt_From(qt, universe, info_message), universe),
                            Fetcher.States(ac, RecordsFilter.Qt_To(qt, universe, info_message), universe),
                            info_message);
                    }
                case SyntaxP.Pattern.TransitionEntryInsert:
                    {
                        return Operation_Transition.AddEntryState(ac,
                            Fetcher.Statemachines(ac, RecordsFilter.Qt_From(qt, universe, info_message), universe),
                            Fetcher.States(ac, RecordsFilter.Qt_To(qt, universe, info_message), universe),
                            info_message);
                    }
                case SyntaxP.Pattern.TransitionExitInsert:
                    {
                        return Operation_Transition.AddExitState(ac,
                            Fetcher.States(ac, RecordsFilter.Qt_From(qt, universe, info_message), universe),
                            info_message);
                    }
                case SyntaxP.Pattern.StateInsert:
                    {
                        Operation_State.AddAll(ac, Fetcher.Statemachines(ac, RecordsFilter.Qt_Where(qt, universe, info_message), universe), qt.Words, info_message);
                        return true;
                    }
                case SyntaxP.Pattern.StateUpdate:
                    {
                        Operation_State.UpdateProperty(ac, qt.Set, Fetcher.States(ac, RecordsFilter.Qt_Where(qt, universe, info_message), universe), info_message);
                        return true;
                    }
                case SyntaxP.Pattern.StateDelete:
                    {
                        Operation_State.RemoveAll(ac, Fetcher.Statemachines(ac, RecordsFilter.Qt_Where(qt, universe, info_message), universe), qt.Words, info_message);
                        return true;
                    }
                case SyntaxP.Pattern.StateSelect:
                    {
                        HashSet<StateRecord> recordSet;
                        Operation_State.Select(ac, Fetcher.States(ac, RecordsFilter.Qt_Where(qt, universe, info_message), universe), out recordSet, info_message);
                        StringBuilder contents = new StringBuilder();
                        AconDataUtility.CreateCsvTable_State(recordSet, false, contents);
                        StellaQLWriter.Write(StellaQLWriter.Filepath_LogStateSelect(ac.name, qt.The), contents, info_message);
                        return true;
                    }
                case SyntaxP.Pattern.TransitionInsert:
                    {
                        Operation_Transition.Insert(ac,
                            Fetcher.States(ac, RecordsFilter.Qt_From(qt, universe, info_message), universe),
                            Fetcher.States(ac, RecordsFilter.Qt_To(qt, universe, info_message), universe),
                            qt.Set, info_message);
                        return true;
                    }
                case SyntaxP.Pattern.TransitionUpdate:
                    {
                        foreach (KeyValuePair<string, string> pair in qt.Set) { info_message.AppendLine(pair.Key + "=" + pair.Value); }
                        Operation_Transition.Update(ac,
                            Fetcher.States(ac, RecordsFilter.Qt_From(qt, universe, info_message), universe),
                            Fetcher.States(ac, RecordsFilter.Qt_To(qt, universe, info_message), universe),
                                qt.Set, info_message);
                        return true;
                    }
                case SyntaxP.Pattern.TransitionDelete:
                    {
                        Operation_Transition.RemoveAll(ac,
                            Fetcher.States(ac, RecordsFilter.Qt_From(qt, universe, info_message), universe),
                            Fetcher.States(ac, RecordsFilter.Qt_To(qt, universe, info_message), universe),
                            info_message);
                        return true;
                    }
                case SyntaxP.Pattern.TransitionSelect:
                    {
                        HashSet<TransitionRecord> recordSet;
                        Operation_Transition.Select(ac,
                            Fetcher.States(ac, RecordsFilter.Qt_From(qt, universe, info_message), universe),
                            Fetcher.States(ac, RecordsFilter.Qt_To(qt, universe, info_message), universe),
                            out recordSet,
                            info_message);
                        StringBuilder contents = new StringBuilder();
                        AconDataUtility.CreateCsvTable_Transition(recordSet, false, contents);
                        StellaQLWriter.Write(StellaQLWriter.Filepath_LogTransitionSelect(ac.name, qt.The), contents, info_message);
                        return true;
                    }
                case SyntaxP.Pattern.CsharpscriptGenerateFullpath:
                    {
                        FullpathConstantGenerator.WriteCshapScript(ac, info_message);
                        return true;
                    }
                case SyntaxP.Pattern.NotMatch: // thru
                default:
                    {
                        throw new UnityException("未定義の指示 syntaxPattern=[" + syntaxPattern + "]");
                    }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static bool ExecuteStateSelect(string query, Dictionary<int, AcStateRecordable> universe, out HashSet<int> recordHashes, StringBuilder message)
        {
            LexcalP.DeleteLineCommentAndBlankLine(ref query);

            recordHashes = null;
            QueryTokens qt = new QueryTokens();
            int caret = 0;
            if (!SyntaxP.Fixed_StateSelect(query, ref caret, ref qt)) { return false; }

            recordHashes = RecordsFilter.Qt_Where(qt, universe, message);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static bool ExecuteTransitionSelect(string query, Dictionary<int, AcStateRecordable> universe, out HashSet<int> recordHashesSrc, out HashSet<int> recordHashesDst, StringBuilder message)
        {
            LexcalP.DeleteLineCommentAndBlankLine(ref query);

            recordHashesSrc = null;
            recordHashesDst = null;
            QueryTokens qt = new QueryTokens();
            int caret = 0;
            if (!SyntaxP.Fixed_TransitionSelect(query, ref caret, ref qt)) { return false; }

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
        public static HashSet<AnimatorStateMachine> Statemachines(AnimatorController ac, HashSet<int> recordHashes, Dictionary<int, AcStateRecordable> universe)
        {
            HashSet<AnimatorStateMachine> statemachines = new HashSet<AnimatorStateMachine>();
            foreach (int recordHash in recordHashes)
            {
                statemachines.Add(Operation_Statemachine.Lookup(ac, universe[recordHash].Fullpath));
            }
            return statemachines;
        }

        /// <summary>
        /// 検索結果に含まれるステートマシンは無視する。
        /// </summary>
        /// <param name="ac"></param>
        /// <param name="recordHashes"></param>
        /// <param name="universe"></param>
        /// <returns></returns>
        public static HashSet<AnimatorState> States(AnimatorController ac, HashSet<int> recordHashes, Dictionary<int, AcStateRecordable> universe)
        {
            HashSet<AnimatorState> states = new HashSet<AnimatorState>();
            foreach (int recordHash in recordHashes)
            {
                AnimatorState state = Operation_State.Lookup(ac, universe[recordHash].Fullpath);
                if (null== state)
                {
                    // ステートマシンかもしれない。
                    AnimatorStateMachine stateMachine = Operation_Statemachine.Lookup(ac, universe[recordHash].Fullpath);
                    if(null!= stateMachine)
                    {
                        // ステートマシンだったのなら、ヌルで合っている☆（＾～＾）
                        continue; // このレコードは飛ばして次へ☆
                    }
                    else
                    {
                        throw new UnityException("フルパス[" + universe[recordHash].Fullpath + "]に対応するステートは無いぜ☆（＞＿＜）フルパス一覧を確かめろだぜ☆！ universe.Count=[" + universe.Count + "]");
                    }
                }

                states.Add(state);
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
            Dictionary<int, AcStateRecordable> universe, out List<HashSet<int>> lockerNumber_to_recordHashes)
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

        public static HashSet<int> Qt_From(QueryTokens qt, Dictionary<int, AcStateRecordable> universe, StringBuilder message)
        {
            if ("" != qt.From_FullnameRegex) { return RecordsFilter.String_StateFullNameRegex(qt.From_FullnameRegex, universe, message); }
            else {
                List<string> tokens; SyntaxPOther.String_to_tokens(qt.From_Tag, out tokens);

                List<List<string>> tokenLockers;
                List<string> tokenLockersOperation;
                Querier.Tokens_to_lockers(tokens, out tokenLockers, out tokenLockersOperation);

                List<HashSet<int>> recordHashesLockers;
                RecordsFilter.TokenLockers_to_recordHashesLockers(tokenLockers, tokenLockersOperation, universe, out recordHashesLockers);
                return recordHashesLockers[recordHashesLockers.Count - 1];
            }
        }

        public static HashSet<int> Qt_To(QueryTokens qt, Dictionary<int, AcStateRecordable> universe, StringBuilder message)
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

        public static HashSet<int> Qt_Where(QueryTokens qt, Dictionary<int, AcStateRecordable> universe, StringBuilder message)
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

        public static HashSet<int> String_StateFullNameRegex(string pattern, Dictionary<int, AcStateRecordable> universe, StringBuilder message)
        {
            HashSet<int> hitRecordHashes = new HashSet<int>();

            Regex regex = new Regex(pattern);
            foreach (KeyValuePair<int, AcStateRecordable> pair in universe)
            {
                if (regex.IsMatch(pair.Value.Fullpath))
                {
                    hitRecordHashes.Add(pair.Key);
                }
            }

            if (hitRecordHashes.Count < 1) {
                message.AppendLine("Mension: Animation Controller Path OK?");
                message.AppendLine("Mension: ["+ StateCmdline.BUTTON_LABEL_GENERATE_FULLPATH + "] update OK?");
                message.AppendLine("Mension: parent path OK? ex.) Base Layer.Hoge");
                message.AppendLine("Mension: Spell OK?");
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

        public static HashSet<int> Records_NotAndNot(HashSet<int> lockerNumbers, List<HashSet<int>> recordHasheslockers, Dictionary<int, AcStateRecordable> universe)
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

        public static HashSet<int> Tags_And(HashSet<int> attrs, Dictionary<int, AcStateRecordable> universe)
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

        public static HashSet<int> Tags_Or(HashSet<int> orAllTags, Dictionary<int, AcStateRecordable> universe)
        {
            HashSet<int> hitRecordHashes = new HashSet<int>();// レコード・インデックスを属性検索（重複除外）
            foreach (KeyValuePair<int, AcStateRecordable> pair in universe)
            {
                foreach (int attr in orAllTags)
                {
                    if (pair.Value.HasEverythingTags(new HashSet<int>() { attr })) { hitRecordHashes.Add(pair.Key); }
                }
            }

            return hitRecordHashes;
        }

        public static HashSet<int> Tags_NotAndNot(HashSet<int> requireAllTags, Dictionary<int, AcStateRecordable> recordUniverse)
        {
            HashSet<int> hitRecordHashes = new HashSet<int>();// レコード・インデックスを属性検索（重複除外）
            foreach (KeyValuePair<int, AcStateRecordable> pair in recordUniverse)
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
        public enum Pattern
        {
            TransitionAnystateInsert,
            TransitionEntryInsert,
            TransitionExitInsert,
            StateInsert,
            StateUpdate,
            StateDelete,
            StateSelect,
            TransitionInsert,
            TransitionUpdate,
            TransitionDelete,
            TransitionSelect,
            CsharpscriptGenerateFullpath,
            NotMatch
        }

        /// <summary>
        /// キャレットを進めることと、どの構文にパターンマッチしたかと、分解されたトークンを返すことをします。
        /// </summary>
        public static Pattern FixedQuery(string query, ref int ref_caret, ref QueryTokens qt)
        {
            int caret = ref_caret;
            if (Fixed_TransitionAnystateInsert(query, ref caret, ref qt)) { ref_caret = caret; return Pattern.TransitionAnystateInsert; }
            else if (Fixed_TransitionEntryInsert(query, ref caret, ref qt)) { ref_caret = caret; return Pattern.TransitionEntryInsert; }
            else if (Fixed_TransitionExitInsert(query, ref caret, ref qt)) { ref_caret = caret; return Pattern.TransitionExitInsert; }
            else if (Fixed_StateInsert(query, ref caret, ref qt)) { ref_caret = caret; return Pattern.StateInsert; }
            else if (Fixed_StateUpdate(query, ref caret, ref qt)) { ref_caret = caret; return Pattern.StateUpdate; }
            else if (Fixed_StateDelete(query, ref caret, ref qt)) { ref_caret = caret; return Pattern.StateDelete; }
            else if (Fixed_StateSelect(query, ref caret, ref qt)) { ref_caret = caret; return Pattern.StateSelect; }
            else if (Fixed_TransitionInsert(query, ref caret, ref qt)) { ref_caret = caret; return Pattern.TransitionInsert; }
            else if (Fixed_TransitionUpdate(query, ref caret, ref qt)) { ref_caret = caret; return Pattern.TransitionUpdate; }
            else if (Fixed_TransitionDelete(query, ref caret, ref qt)) { ref_caret = caret; return Pattern.TransitionDelete; }
            else if (Fixed_TransitionSelect(query, ref caret, ref qt)) { ref_caret = caret; return Pattern.TransitionSelect; }
            else if (Fixed_CsharpscriptGenerateFullpath(query, ref caret, ref qt)) { ref_caret = caret; return Pattern.CsharpscriptGenerateFullpath; }
            return Pattern.NotMatch;// 構文にはマッチしなかった。
        }

        public static bool NotMatched(QueryTokens current, int caret, ref QueryTokens max)
        {
            if (max.MatchedSyntaxCaret < caret) { current.MatchedSyntaxCaret = caret; max = current; } return false;
        }

        /// <summary>
        /// WORDS句だけ。
        /// Example（代表例）: INSERT文のWORDS句。
        /// </summary>
        public static bool ParsePhrase_AfterWords(string query, ref int caret, string endsDelimiterWord, List<string> ref_words)
        {
            string word;
            while (caret < query.Length && !LexcalP.FixedWord(endsDelimiterWord, query, ref caret))
            {
                if (LexcalP.VarStringliteral(query, ref caret, out word)) { } // 一致しなければelse～ifへ
                else if (!LexcalP.VarValue(query, ref caret, out word)) { return false; }
                ref_words.Add(word);
            }
            return true;
        }
        /// <summary>
        /// SET句だけ。
        /// Example（代表例）: UPDATE文のSET句。
        /// </summary>
        public static bool ParsePhrase_AfterSet(string query, ref int caret, string endsDelimiterWord, Dictionary<string,string> ref_properties)
        {
            string propertyName;
            string propertyValue;

            while (caret < query.Length && !LexcalP.FixedWord(endsDelimiterWord, query, ref caret))
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
        public static bool Fixed_TransitionAnystateInsert(string query, ref int ref_caret, ref QueryTokens maxQt)
        {
            QueryTokens qt = new QueryTokens("TRANSITION ANYSTATE INSERT");
            int caret = ref_caret;
            string stringWithoutDoubleQuotation, parenthesis;

            LexcalP.VarSpaces(query, ref caret);

            if (!LexcalP.FixedWord(QueryTokens.TRANSITION, query, ref caret)) { return NotMatched(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.TRANSITION;

            if (!LexcalP.FixedWord(QueryTokens.ANYSTATE, query, ref caret)) { return NotMatched(qt, caret, ref maxQt); }
            qt.Target2 = QueryTokens.ANYSTATE;

            if (!LexcalP.FixedWord(QueryTokens.INSERT, query, ref caret)) { return NotMatched(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.INSERT;

            if (!LexcalP.FixedWord(QueryTokens.FROM, query, ref caret)) { return NotMatched(qt, caret, ref maxQt); }

            // 正規表現か、タグ検索のどちらか。（マシンステート検索でもタグを使うことがあるだろうか）
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation)){
                qt.From_FullnameRegex = stringWithoutDoubleQuotation;
            }else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret)){
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return NotMatched(qt, caret, ref maxQt); }
                qt.From_Tag = parenthesis;
            }else { return NotMatched(qt, caret, ref maxQt); }

            if (!LexcalP.FixedWord(QueryTokens.TO, query, ref caret)) { return NotMatched(qt, caret, ref maxQt); }

            // 正規表現か、タグ検索のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation)){
                qt.To_FullnameRegex = stringWithoutDoubleQuotation;
            }else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret)){
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return NotMatched(qt, caret, ref maxQt); }
                qt.To_Tag = parenthesis;
            }
            else { return NotMatched(qt, caret, ref maxQt); }
            maxQt = qt; ref_caret = caret; return true;
        }

        /// <summary>
        /// TRANSITION ENTRY INSERT
        /// </summary>
        public static bool Fixed_TransitionEntryInsert(string query, ref int ref_caret, ref QueryTokens maxQt)
        {
            QueryTokens qt = new QueryTokens("TRANSITION ENTRY INSERT");
            int caret = ref_caret;
            string stringWithoutDoubleQuotation, parenthesis;

            LexcalP.VarSpaces(query, ref caret);

            if (!LexcalP.FixedWord(QueryTokens.TRANSITION, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.TRANSITION;

            if (!LexcalP.FixedWord(QueryTokens.ENTRY, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Target2 = QueryTokens.ENTRY;

            if (!LexcalP.FixedWord(QueryTokens.INSERT, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.INSERT;

            if (!LexcalP.FixedWord(QueryTokens.FROM, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }

            // 正規表現か、タグ検索のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation)){
                qt.From_FullnameRegex = stringWithoutDoubleQuotation;
            }else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret)){
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
                qt.From_Tag = parenthesis;
            }else { return SyntaxP.NotMatched(qt, caret, ref maxQt); }

            if (!LexcalP.FixedWord(QueryTokens.TO, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }

            // 正規表現か、タグ検索のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation)){
                qt.To_FullnameRegex = stringWithoutDoubleQuotation;
            }else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret)){
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
                qt.To_Tag = parenthesis;
            }
            else { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            maxQt = qt; ref_caret = caret; return true;
        }

        /// <summary>
        /// TRANSITION EXIT INSERT
        /// </summary>
        public static bool Fixed_TransitionExitInsert(string query, ref int ref_caret, ref QueryTokens maxQt)
        {
            QueryTokens qt = new QueryTokens("TRANSITION EXIT INSERT");
            int caret = ref_caret;
            string stringWithoutDoubleQuotation, parenthesis;

            LexcalP.VarSpaces(query, ref caret);

            if (!LexcalP.FixedWord(QueryTokens.TRANSITION, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.TRANSITION;

            if (!LexcalP.FixedWord(QueryTokens.EXIT, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Target2 = QueryTokens.EXIT;

            if (!LexcalP.FixedWord(QueryTokens.INSERT, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.INSERT;

            if (!LexcalP.FixedWord(QueryTokens.FROM, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }

            // 正規表現か、タグ検索のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation)){
                qt.From_FullnameRegex = stringWithoutDoubleQuotation;
            }else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret)){
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
                qt.From_Tag = parenthesis;
            }else { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            maxQt = qt; ref_caret = caret; return true;
        }

        /// <summary>
        /// STATE INSERT
        /// </summary>
        public static bool Fixed_StateInsert(string query, ref int ref_caret, ref QueryTokens maxQt)
        {
            QueryTokens qt = new QueryTokens("STATE INSERT");
            int caret = ref_caret;
            string stringWithoutDoubleQuotation;
            LexcalP.VarSpaces(query, ref caret);

            if (!LexcalP.FixedWord(QueryTokens.STATE, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.STATE;

            if (!LexcalP.FixedWord(QueryTokens.INSERT, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.INSERT;

            if (LexcalP.FixedWord(QueryTokens.WORDS, query, ref caret))
            {
                // 「値、スペース、値、スペース」の繰り返し。項目名が WHERE だった場合終わり。
                if (!SyntaxP.ParsePhrase_AfterWords(query, ref caret, QueryTokens.WHERE, qt.Words)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            }
            else if (LexcalP.FixedWord(QueryTokens.SET, query, ref caret))
            {
                throw new UnityException("SET句は廃止したぜ☆（＾～＾） これからは WORDS句を使えだぜ☆（＾▽＾） WORDS Bear Cat Dog Elephant だぜ☆楽だろ☆（＾▽＾）");
                //// 「項目名、スペース、値、スペース」の繰り返し。項目名が WHERE だった場合終わり。
                //if (!SyntaxP.ParsePhrase_AfterSet(query, ref caret, QueryTokens.WHERE, qt.Set)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            }
            else { if (!LexcalP.FixedWord(QueryTokens.WHERE, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); } }

            // 正規表現。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qt.Where_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            maxQt = qt; ref_caret = caret; return true;
        }

        /// <summary>
        /// STATE UPDATE
        /// </summary>
        public static bool Fixed_StateUpdate(string query, ref int ref_caret, ref QueryTokens maxQt)
        {
            QueryTokens qt = new QueryTokens("STATE UPDATE");
            int caret = ref_caret;
            string stringWithoutDoubleQuotation;
            string parenthesis;
            LexcalP.VarSpaces(query, ref caret);

            if (!LexcalP.FixedWord(QueryTokens.STATE, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.STATE;

            if (!LexcalP.FixedWord(QueryTokens.UPDATE, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.UPDATE;

            if (LexcalP.FixedWord(QueryTokens.SET, query, ref caret)) {
                // 「項目名、スペース、値、スペース」の繰り返し。項目名が WHERE だった場合終わり。
                if(!SyntaxP.ParsePhrase_AfterSet(query, ref caret, QueryTokens.WHERE, qt.Set)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            } else { if (!LexcalP.FixedWord(QueryTokens.WHERE, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); } }

            // 正規表現か、タグ検索のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation)) {
                qt.Where_FullnameRegex = stringWithoutDoubleQuotation;
            } else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret)) {
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
                qt.Where_Tag = parenthesis;
            }
            else { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            maxQt = qt; ref_caret = caret; return true;
        }

        /// <summary>
        /// STATE DELETE
        /// </summary>
        public static bool Fixed_StateDelete(string query, ref int ref_caret, ref QueryTokens maxQt)
        {
            QueryTokens qt = new QueryTokens("STATE DELETE");
            int caret = ref_caret;
            string stringWithoutDoubleQuotation;
            LexcalP.VarSpaces(query, ref caret);

            if (!LexcalP.FixedWord(QueryTokens.STATE, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.STATE;

            if (!LexcalP.FixedWord(QueryTokens.DELETE, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.DELETE;

            if (LexcalP.FixedWord(QueryTokens.WORDS, query, ref caret))
            {
                // 「値、スペース、値、スペース」の繰り返し。項目名が WHERE だった場合終わり。
                if (!SyntaxP.ParsePhrase_AfterWords(query, ref caret, QueryTokens.WHERE, qt.Words)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            }
            else if (LexcalP.FixedWord(QueryTokens.SET, query, ref caret))
            {
                throw new UnityException("SET句は廃止したぜ☆（＾～＾） これからは WORDS句を使えだぜ☆（＾▽＾） WORDS Bear Cat Dog Elephant だぜ☆楽だろ☆（＾▽＾）");
                //// 「項目名、スペース、値、スペース」の繰り返し。項目名が WHERE だった場合終わり。
                //if (!SyntaxP.ParsePhrase_AfterSet(query, ref caret, QueryTokens.WHERE, qt.Set)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            }
            else { if (!LexcalP.FixedWord(QueryTokens.WHERE, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); } }

            // 正規表現。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qt.Where_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            maxQt = qt; ref_caret = caret; return true;
        }

        /// <summary>
        /// STATE SELECT
        /// </summary>
        public static bool Fixed_StateSelect(string query, ref int ref_caret, ref QueryTokens maxQt)
        {
            QueryTokens qt = new QueryTokens("STATE SELECT");
            int caret = ref_caret;
            string stringWithoutDoubleQuotation, word;
            string parenthesis;
            LexcalP.VarSpaces(query, ref caret);

            if (!LexcalP.FixedWord(QueryTokens.STATE, query, ref caret)) { return NotMatched(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.STATE;

            if (!LexcalP.FixedWord(QueryTokens.SELECT, query, ref caret)) { return NotMatched(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.SELECT;

            if (!LexcalP.FixedWord(QueryTokens.WHERE, query, ref caret)) { return NotMatched(qt, caret, ref maxQt); }

            // 正規表現か、タグ検索のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qt.Where_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret))
            {
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return NotMatched(qt, caret, ref maxQt); }
                qt.Where_Tag = parenthesis;
            }
            else { return NotMatched(qt, caret, ref maxQt); }

            if (LexcalP.FixedWord(QueryTokens.THE, query, ref caret))// オプション
            {
                if (LexcalP.VarWord(query, ref caret, out word)) { qt.The = word; }
                else if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation)) { qt.The = stringWithoutDoubleQuotation; }
                else { return NotMatched(qt, caret, ref maxQt); }
            }

            maxQt = qt; ref_caret = caret; return true;
        }

        /// <summary>
        /// TRANSITION INSERT
        /// </summary>
        public static bool Fixed_TransitionInsert(string query, ref int ref_caret, ref QueryTokens maxQt)
        {
            QueryTokens qt = new QueryTokens("TRANSITION INSERT");
            int caret = ref_caret;
            string stringWithoutDoubleQuotation;
            string parenthesis;
            LexcalP.VarSpaces(query, ref caret);

            if (!LexcalP.FixedWord(QueryTokens.TRANSITION, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.TRANSITION;

            if (!LexcalP.FixedWord(QueryTokens.INSERT, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.INSERT;

            if (LexcalP.FixedWord(QueryTokens.SET, query, ref caret))
            {
                // 「項目名、スペース、値、スペース」の繰り返し。項目名が FROM だった場合終わり。
                if (!SyntaxP.ParsePhrase_AfterSet(query, ref caret, QueryTokens.FROM, qt.Set)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            }
            else {
                if (!LexcalP.FixedWord(QueryTokens.FROM, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            }

            // 正規表現か、タグ検索のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qt.From_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret))
            {
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
                qt.From_Tag = parenthesis;
            }
            else { return SyntaxP.NotMatched(qt, caret, ref maxQt); }

            if (!LexcalP.FixedWord(QueryTokens.TO, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }

            // 正規表現か、タグ検索のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qt.To_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret))
            {
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
                qt.To_Tag = parenthesis;
            }
            else { return SyntaxP.NotMatched(qt, caret, ref maxQt); }

            maxQt = qt; ref_caret = caret; return true;
        }

        /// <summary>
        /// TRANSITION UPDATE
        /// </summary>
        public static bool Fixed_TransitionUpdate(string query, ref int ref_caret, ref QueryTokens maxQt)
        {
            QueryTokens qt = new QueryTokens("TRANSITION UPDATE");
            int caret = ref_caret;
            string stringWithoutDoubleQuotation;
            string parenthesis;
            LexcalP.VarSpaces(query, ref caret);

            if (!LexcalP.FixedWord(QueryTokens.TRANSITION, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.TRANSITION;

            if (!LexcalP.FixedWord(QueryTokens.UPDATE, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.UPDATE;

            if (LexcalP.FixedWord(QueryTokens.SET, query, ref caret))
            {
                // 「項目名、スペース、値、スペース」の繰り返し。項目名が FROM だった場合終わり。
                if (!SyntaxP.ParsePhrase_AfterSet(query, ref caret, QueryTokens.FROM, qt.Set)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            }
            else {
                if (!LexcalP.FixedWord(QueryTokens.FROM, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            }

            // 正規表現か、タグ検索のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qt.From_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret))
            {
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
                qt.From_Tag = parenthesis;
            }
            else {
                return SyntaxP.NotMatched(qt, caret, ref maxQt);
            }

            if (!LexcalP.FixedWord(QueryTokens.TO, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }

            // 正規表現か、タグ検索のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qt.To_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret))
            {
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
                qt.To_Tag = parenthesis;
            }
            else { return SyntaxP.NotMatched(qt, caret, ref maxQt); }

            maxQt = qt; ref_caret = caret; return true;
        }

        /// <summary>
        /// TRANSITION DELETE
        /// </summary>
        public static bool Fixed_TransitionDelete(string query, ref int ref_caret, ref QueryTokens maxQt)
        {
            QueryTokens qt = new QueryTokens("TRANSITION DELETE");
            int caret = ref_caret;
            string stringWithoutDoubleQuotation;
            string parenthesis;
            LexcalP.VarSpaces(query, ref caret);

            if (!LexcalP.FixedWord(QueryTokens.TRANSITION, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.TRANSITION;

            if (!LexcalP.FixedWord(QueryTokens.DELETE, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.DELETE;

            if (!LexcalP.FixedWord(QueryTokens.FROM, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }

            // 正規表現か、タグ検索のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qt.From_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret))
            {
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
                qt.From_Tag = parenthesis;
            }
            else { return SyntaxP.NotMatched(qt, caret, ref maxQt); }

            if (!LexcalP.FixedWord(QueryTokens.TO, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }

            // 正規表現か、タグ検索のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qt.To_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret))
            {
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
                qt.To_Tag = parenthesis;
            }
            else { return SyntaxP.NotMatched(qt, caret, ref maxQt); }

            maxQt = qt; ref_caret = caret; return true;
        }

        /// <summary>
        /// TRANSITION SELECT
        /// </summary>
        public static bool Fixed_TransitionSelect(string query, ref int ref_caret, ref QueryTokens maxQt)
        {
            QueryTokens qt = new QueryTokens("TRANSITION SELECT");
            int caret = ref_caret;
            string stringWithoutDoubleQuotation, word;
            string parenthesis;
            LexcalP.VarSpaces(query, ref caret);

            if (!LexcalP.FixedWord(QueryTokens.TRANSITION, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.TRANSITION;

            if (!LexcalP.FixedWord(QueryTokens.SELECT, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.SELECT;

            if (!LexcalP.FixedWord(QueryTokens.FROM, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }

            // 正規表現か、タグ検索のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qt.From_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret))
            {
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
                qt.From_Tag = parenthesis;
            }
            else { return SyntaxP.NotMatched(qt, caret, ref maxQt); }

            if (!LexcalP.FixedWord(QueryTokens.TO, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }

            // 正規表現か、タグ検索のどちらか。
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qt.To_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret))
            {
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
                qt.To_Tag = parenthesis;
            }
            else { return SyntaxP.NotMatched(qt, caret, ref maxQt); }

            if (LexcalP.FixedWord(QueryTokens.THE, query, ref caret))// オプション
            {
                if (LexcalP.VarWord(query, ref caret, out word)) { qt.The = word; }
                else if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation)) { qt.The = stringWithoutDoubleQuotation; }
                else { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            }

            maxQt = qt; ref_caret = caret; return true;
        }

        /// <summary>
        /// CSHARPSCRIPT GENERATE_FULLPATH
        /// </summary>
        public static bool Fixed_CsharpscriptGenerateFullpath(string query, ref int ref_caret, ref QueryTokens maxQt)
        {
            QueryTokens qt = new QueryTokens(QueryTokens.CSHARPSCRIPT + " " + QueryTokens.GENERATE_FULLPATH);
            int caret = ref_caret;
            LexcalP.VarSpaces(query, ref caret);

            if (!LexcalP.FixedWord(QueryTokens.CSHARPSCRIPT, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.CSHARPSCRIPT;

            if (!LexcalP.FixedWord(QueryTokens.GENERATE_FULLPATH, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.GENERATE_FULLPATH;

            maxQt = qt; ref_caret = caret; return true;
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

        /// <summary>
        /// 固定の単語がそこにあるか判定する。
        /// </summary>
        /// <param name="word"></param>
        /// <param name="query"></param>
        /// <param name="caret"></param>
        /// <returns></returns>
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
