using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor.Animations;
using UnityEngine;
using StellaQL.Acons.Demo_Zoo;

/// <summary>
/// Explain : http://qiita.com/muzudho1/items/baf4b06cdcda96ca9a11
/// Explain : http://qiita.com/muzudho1/items/05ffb53fb4e9d4252b28
/// </summary>
namespace StellaQL
{
    /// <summary>
    /// The queries connected by semicolons are executed sequentially from the beginning.
    /// </summary>
    public abstract class SequenceQuerier
    {
        public static bool Execute(AnimatorController ac, string query, AControllable userDefinedAControl, StringBuilder info_message)
        {
            LexcalP.DeleteLineCommentAndBlankLine(ref query);// Delete all comments and blank lines.
            int caret = 0;
            LexcalP.VarSpaces(query, ref caret); // Remove the first blank.

            // phase
            // 0: Next is either a query or a semicolon. (For example, at the beginning of reading, immediately after reading a semicolon, etc.)
            // 1: Next, it is necessary to have a semicolon. (For example, immediately after loading the query)
            int phase = 0; 
            QueryTokens qt = new QueryTokens("Query syntax Not applicable");
            while (caret<query.Length)
            {
                switch (phase)
                {
                    case 1:
                        if (LexcalP.FixedWord(";", query, ref caret)) { phase = 0; break; }// There was a semicolon. Next is either a query or a semicolon.
                        else { throw new UnityException("There was no semicolon. Remaining:" + query.Substring(caret)); }
                    default:
                        {
                            SyntaxP.Pattern syntaxPattern = SyntaxP.FixedQuery(query, ref caret, ref qt);
                            if (SyntaxP.Pattern.NotMatch == syntaxPattern) // Normal operation
                            {
                                qt.Clear("Query syntax Not applicable"); // Clear it for next use.
                                if (LexcalP.FixedWord(";", query, ref caret)) { phase = 0; return true; }// There was a semicolon. Next is either a query or a semicolon.
                                else { throw new UnityException("There is neither a query nor a semicolon. Remaining:" + query.Substring(caret)); }// Abnormal termination when there is neither query nor semicolon.
                            }
                            else
                            {
                                Querier.Execute(ac, qt, syntaxPattern, userDefinedAControl, info_message);//Execution
                                qt.Clear("Query syntax Not applicable"); // Clear it for next use.
                                phase = 1;
                            }// There was a query. Next semicolon is necessary.
                        }
                        break;
                }
            }
            return true;//Successful completion
        }
    }

    /// <summary>
    /// Put the parsed token here.
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

        public const string
            LAYER = "LAYER",
            STATEMACHINE = "STATEMACHINE",
            STATE = "STATE",
            TRANSITION = "TRANSITION",
            ANYSTATE = "ANYSTATE",
            ENTRY = "ENTRY",
            EXIT = "EXIT",
            CSHARPSCRIPT = "CSHARPSCRIPT",
            INSERT = "INSERT",
            UPDATE = "UPDATE",
            DELETE = "DELETE",
            SELECT = "SELECT",
            GENERATE_FULLPATH = "GENERATE_FULLPATH",
            WORDS = "WORDS",
            SET = "SET",
            SEMICOLON = ";",
            FROM = "FROM",
            TO = "TO",
            WHERE = "WHERE",
            TAG = "TAG",
            THE = "THE";

        /// <summary>
        /// STATEMACHINE, STATE, TRANSITION either.
        /// </summary>
        public string Target { get; set; }
        /// <summary>
        /// ANYSTATE, ENTRY, EXIT either.
        /// </summary>
        public string Target2 { get; set; }
        /// <summary>
        /// INSERT, UPDATE, DELETE, SELECT either.
        /// </summary>
        public string Manipulation { get; set; }
        /// <summary>
        /// WORDS phrase. I want to distinguish upper case letters and lower case letters.
        /// </summary>
        public List<string> Words { get; set; }
        /// <summary>
        /// SET phrase. I want to distinguish upper case letters and lower case letters.
        /// </summary>
        public Dictionary<string, string> Set { get; set; }
        /// <summary>
        /// State full name enters.
        /// </summary>
        public string From_FullnameRegex { get; set; }
        /// <summary>
        /// An expression using parentheses is entered.
        /// </summary>
        public string From_Tag { get; set; }
        /// <summary>
        /// State full name enters.
        /// </summary>
        public string To_FullnameRegex { get; set; }
        /// <summary>
        /// An expression using parentheses is entered.
        /// </summary>
        public string To_Tag { get; set; }
        /// <summary>
        /// State full name enters.
        /// </summary>
        public string Where_FullnameRegex { get; set; }
        /// <summary>
        /// An expression using parentheses is entered.
        /// </summary>
        public string Where_Tag { get; set; }
        /// <summary>
        /// A character string for preventing duplication of output file names is entered.
        /// </summary>
        public string The { get; set; }

        /// <summary>
        /// Syntax When not applicable, a name for checking which syntax has the most number of characters.
        /// </summary>
        public string MatchedSyntaxName { get; set; }
        /// <summary>
        /// Syntax A number for checking which character of which syntax was applicable when not applicable.
        /// </summary>
        public int MatchedSyntaxCaret { get; set; }
    }

    /// <summary>
    /// Execute Query (Given the decomposed query token, fetch the record hash, and the fetcher retrieves the object)
    /// FIXME: You should not give the query string source or caret. I want to separate from the parser.
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
                        //foreach (KeyValuePair<string, string> pair in qt.Set) { info_message.AppendLine(pair.Key + "=" + pair.Value); }
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
                case SyntaxP.Pattern.LayerInsert:
                    {
                        Operation_Layer.AddAll(ac, qt.Words, info_message);
                        return true;
                    }
                case SyntaxP.Pattern.LayerDelete:
                    {
                        Operation_Layer.RemoveAll(ac, qt.Words, info_message);
                        return true;
                    }
                case SyntaxP.Pattern.NotMatch: // thru
                default:
                    {
                        throw new UnityException("Not supported. syntaxPattern=[" + syntaxPattern + "]");
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
            int caret = 0;
            LexcalP.VarSpaces(query, ref caret); // Remove the first blank.

            recordHashes = null;
            QueryTokens qt = new QueryTokens();
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
            int caret = 0;
            LexcalP.VarSpaces(query, ref caret); // Remove the first blank.

            recordHashesSrc = null;
            recordHashesDst = null;
            QueryTokens qt = new QueryTokens();
            if (!SyntaxP.Fixed_TransitionSelect(query, ref caret, ref qt)) { return false; }

            recordHashesSrc = RecordsFilter.Qt_From(qt, universe, message);// FROM
            recordHashesDst = RecordsFilter.Qt_To(qt, universe, message);// TO
            return true;
        }

        /// <summary>
        /// 「(」「[」「(」「Alpaca」「Bear」「)」「(」「Cat」「Dog」「)」「]」「{」「Elephant」「}」「)」
        /// Reading order ) Bear Alpaca ( ) Dog Cat ( ] [ } Elephant { ) (
        /// 
        /// 0: () Bear Alpaca
        /// 1: () Dog Cat
        /// 2: [] 1 0
        /// 3: {} Elephant
        /// 4: () 3 2
        /// 
        /// Sort this into lockers.
        /// </summary>
        public static void Tokens_to_lockers(List<string> tokens, out List<List<string>> lockers, out List<string> lockersOperation)
        {
            string openParen = ""; // "Open parenthesis" corresponding to closing parenthesis
            int iCursor = 0;

            lockers = new List<List<string>>(); // Locker. Start from number 0.
            lockersOperation = new List<string>();
            List<string> bufferTokens = new List<string>(); // The token being scanned.
            while (iCursor < tokens.Count)
            {
                string token = tokens[iCursor];
                if ("" == openParen)
                {
                    switch (token)
                    {
                        case ")": openParen = "("; tokens[iCursor] = ""; break;
                        case "]": openParen = "["; tokens[iCursor] = ""; break;
                        case "}": openParen = "{"; tokens[iCursor] = ""; break;
                        default: break; // Ignore and proceed
                    }
                }
                else // Go behind. Deletes the characters of the members in parentheses, and replaces the parenthesis with the locker number.
                {
                    switch (token)
                    {
                        case "": break; // ignore
                        case "(":
                        case "[":
                        case "{":
                            if (openParen == token)
                            {
                                tokens[iCursor] = lockers.Count.ToString(); // Replace with locker number
                                openParen = ""; lockersOperation.Add(token); lockers.Add(bufferTokens); bufferTokens = new List<string>();
                            }
                            else { throw new UnityException("Tokens_to_lockers parse error?"); }
                            break;
                        default: bufferTokens.Add(token); tokens[iCursor] = ""; break;
                    }
                }
                if ("" == openParen) { iCursor++; } else { iCursor--; }
            }
        }
    }

    /// <summary>
    /// Fetch (Fetch objects)
    /// </summary>
    public abstract class Fetcher
    {
        public static HashSet<AnimatorControllerLayer> Layers(AnimatorController ac, HashSet<int> targetHashes, Dictionary<int, AcStateRecordable> universe)
        {
            HashSet<AnimatorControllerLayer> layers = new HashSet<AnimatorControllerLayer>();
            foreach (int targetHash in targetHashes)
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                FullpathSyntaxP.Fixed_LayerName(universe[targetHash].Fullpath, ref caret, ref ft);

                AnimatorControllerLayer layer = AconFetcher.FetchLayer_JustLayerName(ac, ft.LayerNameEndsWithoutDot);
                layers.Add(layer);
            }
            return layers;
        }

        public static HashSet<AnimatorStateMachine> Statemachines(AnimatorController ac, HashSet<int> targetHashes, Dictionary<int, AcStateRecordable> universe)
        {
            HashSet<AnimatorStateMachine> statemachines = new HashSet<AnimatorStateMachine>();
            foreach (int targetHash in targetHashes)
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames(universe[targetHash].Fullpath, ref caret, ref ft)) { throw new UnityException("Parse failure. [" + universe[targetHash].Fullpath + "] ac=[" + ac.name + "]"); }

                AnimatorControllerLayer layer = AconFetcher.FetchLayer_JustLayerName(ac, ft.LayerNameEndsWithoutDot);
                AnimatorStateMachine statemachine = AconFetcher.FetchStatemachine(ac, ft.StatemachineNamesEndsWithoutDot, layer);
                statemachines.Add(statemachine);
            }
            return statemachines;
        }

        /// <summary>
        /// Ignore the state machine included in the search result.
        /// </summary>
        /// <param name="ac"></param>
        /// <param name="targetHashes"></param>
        /// <param name="universe"></param>
        /// <returns></returns>
        public static HashSet<AnimatorState> States(AnimatorController ac, HashSet<int> targetHashes, Dictionary<int, AcStateRecordable> universe)
        {
            HashSet<AnimatorState> states = new HashSet<AnimatorState>();
            foreach (int targetHash in targetHashes)
            {
                AnimatorState state;
                {
                    int caret = 0;
                    FullpathTokens ft = new FullpathTokens();
                    if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(universe[targetHash].Fullpath, ref caret, ref ft)) { throw new UnityException("Parse failure: [" + universe[targetHash].Fullpath + "] ac=[" + ac.name + "]"); }
                    state = AconFetcher.FetchState(ac, ft);
                }

                if (null== state)
                {
                    // It may be a state machine.
                    int caret = 0;
                    FullpathTokens ft = new FullpathTokens();
                    if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames(universe[targetHash].Fullpath, ref caret, ref ft)) { throw new UnityException("Parse failure: [" + universe[targetHash].Fullpath + "] ac=[" + ac.name + "]"); }

                    AnimatorControllerLayer layer = AconFetcher.FetchLayer_JustLayerName(ac, ft.LayerNameEndsWithoutDot);
                    AnimatorStateMachine stateMachine = AconFetcher.FetchStatemachine(ac, ft.StatemachineNamesEndsWithoutDot, layer);

                    if(null!= stateMachine)
                    {
                        // If it was a state machine, it is null matched.
                        continue; // Skip this record and go next.
                    }
                    else
                    {
                        throw new UnityException("Not supported. [" + universe[targetHash].Fullpath + "] universe.Count=[" + universe.Count + "]");
                    }
                }

                states.Add(state);
            }
            return states;
        }
    }

    /// <summary>
    /// Record Filter (Fetch record hashes)
    /// </summary>
    public abstract class RecordsFilter
    {
        /// <summary>
        /// Based on token locker, return search result by locker.
        /// </summary>
        /// <param name="tokens"></param>
        public static void TokenLockers_to_recordHashesLockers(List<List<string>> lockerNumber_to_tokens, List<string> lockerNumber_to_operation,
            Dictionary<int, AcStateRecordable> universe, out List<HashSet<int>> lockerNumber_to_recordHashes)
        {
            lockerNumber_to_recordHashes = new List<HashSet<int>>();

            for (int iLockerNumber = 0; iLockerNumber < lockerNumber_to_tokens.Count; iLockerNumber++)// Locker number. Start from number 0.
            {
                List<string> index_to_token = lockerNumber_to_tokens[iLockerNumber];
                string operation = lockerNumber_to_operation[iLockerNumber];// 「(」「[」「{」 

                int firstItem_temp;
                if (int.TryParse(index_to_token[0], out firstItem_temp))
                { // If it's a number, it's a locker number.
                    HashSet<int> lockerNumbers = TagSetOpe.NumberToken_to_int(index_to_token);
                    switch (operation) // Compute the lockers and put together the answers summarized
                    {
                        case "(": lockerNumber_to_recordHashes.Add(RecordsFilter.Records_And(lockerNumbers, lockerNumber_to_recordHashes)); break;
                        case "[": lockerNumber_to_recordHashes.Add(RecordsFilter.Records_Or(lockerNumbers, lockerNumber_to_recordHashes)); break;
                        case "{":
                            lockerNumber_to_recordHashes.Add(RecordsFilter.Records_NotAndNot(
                      lockerNumbers, lockerNumber_to_recordHashes, universe)); break;
                        default: throw new UnityException("Not supported. tokenOperation=[" + operation + "]");
                    }
                }
                else
                { // If it's not a number, it's a list of tag names
                    HashSet<int> attrEnumSet_src = TagSetOpe.Name_to_hash(new HashSet<string>(index_to_token));
                    switch (operation) // Resolving join (computation)
                    {
                        case "(":
                            lockerNumber_to_recordHashes.Add(RecordsFilter.Tags_And(attrEnumSet_src, universe)); break;
                        case "[":
                            lockerNumber_to_recordHashes.Add(RecordsFilter.Tags_Or(attrEnumSet_src, universe)); break;
                        case "{":
                            lockerNumber_to_recordHashes.Add(RecordsFilter.Tags_NotAndNot(attrEnumSet_src, universe)); break;
                        default: throw new UnityException("Not supported. tokenOperation=[" + operation + "]");
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
            List<int> recordHashes = new List<int>();// Include record index, delete
            int iLocker = 0;
            foreach (int lockerNumber in lockerNumbers)
            {
                HashSet<int> locker = recordHasheslockers[lockerNumber];
                if (0 == iLocker) // I put the whole locker whole.
                {
                    foreach (int recordHash in locker) { recordHashes.Add(recordHash); }
                }
                else // For the second and subsequent lockers, only elements common to all the lockers are left.
                {
                    for (int iElem = recordHashes.Count - 1; -1 < iElem; iElem--)// Delete the specified element from the back.
                    {
                        if (!locker.Contains(recordHashes[iElem])) { recordHashes.RemoveAt(iElem); }
                    }
                }
                iLocker++;
            }

            HashSet<int> distinctRecordHashes = new HashSet<int>();// For once, eliminate duplicates
            foreach (int recordHash in recordHashes) { distinctRecordHashes.Add(recordHash); }

            return distinctRecordHashes;
        }

        public static HashSet<int> Records_Or(HashSet<int> lockerNumbers, List<HashSet<int>> recordHasheslockers)
        {
            HashSet<int> hitRecordHashes = new HashSet<int>();// We will continue to add more records and indexes
            foreach (int lockerNumber in lockerNumbers)
            {
                HashSet<int> locker = recordHasheslockers[lockerNumber];
                if (0 == locker.Count) { throw new UnityException("#RecordHashes_FilteringElementsOr: lockerNumber=[" + lockerNumber + "] member is empty."); }
                foreach (int recordHash in locker)
                {
                    hitRecordHashes.Add(recordHash);
                }
            }

            if (0 == hitRecordHashes.Count) { throw new UnityException("#RecordHashes_FilteringElementsOr: result is empty."); }
            return hitRecordHashes;
        }

        public static HashSet<int> Records_NotAndNot(HashSet<int> lockerNumbers, List<HashSet<int>> recordHasheslockers, Dictionary<int, AcStateRecordable> universe)
        {
            HashSet<int> recordHashesSet = new HashSet<int>();// We will continue to add more records and indexes
            foreach (int lockerNumber in lockerNumbers)
            {
                HashSet<int> locker = recordHasheslockers[lockerNumber];
                foreach (int recordHash in locker)
                {
                    recordHashesSet.Add(recordHash);
                }
            }

            List<int> complementRecordHashes = new List<int>(universe.Keys); // Take a complementary set (to exclude elements from the whole set)
            {
                for (int iComp = complementRecordHashes.Count - 1; -1 < iComp; iComp--)// Delete the specified element from the back.
                {
                    if (recordHashesSet.Contains(complementRecordHashes[iComp])) // Delete element in collection
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
                    if (universe[recordHash].HasEverythingTags(new HashSet<int>() { attr })) { records_empty.Add(recordHash); }// Applicable
                }
                hitRecordHashes = records_empty;
            }
            return hitRecordHashes;
        }

        public static HashSet<int> Tags_Or(HashSet<int> orAllTags, Dictionary<int, AcStateRecordable> universe)
        {
            HashSet<int> hitRecordHashes = new HashSet<int>();// Tag search (deduplication) for record indexes
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
            HashSet<int> hitRecordHashes = new HashSet<int>();// Tag search (deduplication) for record indexes
            foreach (KeyValuePair<int, AcStateRecordable> pair in recordUniverse)
            {
                foreach (int attr in requireAllTags)
                {
                    if (pair.Value.HasEverythingTags(new HashSet<int>() { attr })) { hitRecordHashes.Add(pair.Key); }
                }
            }

            List<int> complementRecordHashes = new List<int>();// Take complementary set
            {
                foreach (int recordHash in recordUniverse.Keys) { complementRecordHashes.Add(recordHash); }// Move content of enumerated type to list.
                for (int iComp = complementRecordHashes.Count - 1; -1 < iComp; iComp--)// Delete the specified element from the back.
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
    /// Tag Set Operation (Tag hash)
    /// TODO: I will stop using enumerated types, I want to make a full score search of tags.
    /// 
    /// enumration :（DOBON.NET） http://dobon.net/vb/dotnet/programing/enumparse.html
    /// </summary>
    public abstract class TagSetOpe
    {
        /// <summary>
        /// Complementary set
        /// </summary>
        public static HashSet<int> Complement(HashSet<int> bitfieldSet, HashSet<int> tagUniverse)
        {
            List<int> complement = new List<int>();
            foreach (int elem in tagUniverse) { complement.Add(elem); }// Move content of enumerated type to list.
            for (int iComp = complement.Count - 1; -1 < iComp; iComp--)// Delete the specified element from the back.
            {
                if (bitfieldSet.Contains(complement[iComp])) { complement.RemoveAt(iComp); }
            }
            return new HashSet<int>(complement);
        }

        public static HashSet<int> NumberToken_to_int(List<string> numberTokens)
        {
            HashSet<int> intSet = new HashSet<int>();
            foreach (string numberToken in numberTokens) { intSet.Add(int.Parse(numberToken)); }// Throw an exception if it can not be converted
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
            foreach (string name in nameSet) { hashSet.Add( Animator.StringToHash(name)); }// Throw an exception if it can not be converted
            return hashSet;
        }
    }

    /// <summary>
    /// Syntax parser
    /// regex : http://smdn.jp/programming/netfx/regex/2_expressions/
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
            LayerInsert,
            LayerDelete,
            NotMatch
        }

        /// <summary>
        /// We will proceed with the caret and return the decomposed token as to which syntax matched the pattern.
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
            else if (Fixed_LayerInsert(query, ref caret, ref qt)) { ref_caret = caret; return Pattern.LayerInsert; }
            else if (Fixed_LayerDelete(query, ref caret, ref qt)) { ref_caret = caret; return Pattern.LayerDelete; }
            return Pattern.NotMatch;// It did not match the syntax.
        }

        public static bool NotMatched(QueryTokens current, int caret, ref QueryTokens max)
        {
            if (max.MatchedSyntaxCaret < caret) { current.MatchedSyntaxCaret = caret; max = current; } return false;
        }

        /// <summary>
        /// WORDS phrase only.
        /// ex: INSERT WORDS
        /// </summary>
        public static bool ParsePhrase_AfterWords(string query, ref int caret, string endsDelimiterWord, List<string> ref_words)
        {
            string word;
            while (caret < query.Length && !LexcalP.FixedWord(endsDelimiterWord, query, ref caret))
            {
                if (LexcalP.VarStringliteral(query, ref caret, out word)) { } // If they do not match, else if
                else if (!LexcalP.VarValue(query, ref caret, out word)) { return false; }
                ref_words.Add(word);
            }
            return true;
        }
        /// <summary>
        /// SET phrase only.
        /// ex: UPDATE SET
        /// </summary>
        public static bool ParsePhrase_AfterSet(string query, ref int caret, string endsDelimiterWord, Dictionary<string,string> ref_properties)
        {
            string propertyName;
            string propertyValue;

            while (caret < query.Length && !LexcalP.FixedWord(endsDelimiterWord, query, ref caret))
            {   // name
                if (!LexcalP.VarWord(query, ref caret, out propertyName)) { return false; }
                // value
                if (LexcalP.VarStringliteral(query, ref caret, out propertyValue)) { } // If they do not match, else if
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
            StringBuilder sb = new StringBuilder(); sb.Append(QueryTokens.TRANSITION); sb.Append(" "); sb.Append(QueryTokens.ANYSTATE); sb.Append(" "); sb.Append(QueryTokens.INSERT);
            QueryTokens qt = new QueryTokens(sb.ToString());
            int caret = ref_caret;
            string stringWithoutDoubleQuotation, parenthesis;

            if (!LexcalP.FixedWord(QueryTokens.TRANSITION, query, ref caret)) { return NotMatched(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.TRANSITION;

            if (!LexcalP.FixedWord(QueryTokens.ANYSTATE, query, ref caret)) { return NotMatched(qt, caret, ref maxQt); }
            qt.Target2 = QueryTokens.ANYSTATE;

            if (!LexcalP.FixedWord(QueryTokens.INSERT, query, ref caret)) { return NotMatched(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.INSERT;

            if (!LexcalP.FixedWord(QueryTokens.FROM, query, ref caret)) { return NotMatched(qt, caret, ref maxQt); }

            // Either regular expression or tag search. (Do you use tags even in machine state search?)
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation)){
                qt.From_FullnameRegex = stringWithoutDoubleQuotation;
            }else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret)){
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return NotMatched(qt, caret, ref maxQt); }
                qt.From_Tag = parenthesis;
            }else { return NotMatched(qt, caret, ref maxQt); }

            if (!LexcalP.FixedWord(QueryTokens.TO, query, ref caret)) { return NotMatched(qt, caret, ref maxQt); }

            // Either regular expression or tag search.
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
            StringBuilder sb = new StringBuilder(); sb.Append(QueryTokens.TRANSITION); sb.Append(" "); sb.Append(QueryTokens.ENTRY); sb.Append(" "); sb.Append(QueryTokens.INSERT);
            QueryTokens qt = new QueryTokens(sb.ToString());
            int caret = ref_caret;
            string stringWithoutDoubleQuotation, parenthesis;

            if (!LexcalP.FixedWord(QueryTokens.TRANSITION, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.TRANSITION;

            if (!LexcalP.FixedWord(QueryTokens.ENTRY, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Target2 = QueryTokens.ENTRY;

            if (!LexcalP.FixedWord(QueryTokens.INSERT, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.INSERT;

            if (!LexcalP.FixedWord(QueryTokens.FROM, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }

            // Either regular expression or tag search.
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation)){
                qt.From_FullnameRegex = stringWithoutDoubleQuotation;
            }else if (LexcalP.FixedWord(QueryTokens.TAG, query, ref caret)){
                if (!LexcalP.VarParentesis(query, ref caret, out parenthesis)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
                qt.From_Tag = parenthesis;
            }else { return SyntaxP.NotMatched(qt, caret, ref maxQt); }

            if (!LexcalP.FixedWord(QueryTokens.TO, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }

            // Either regular expression or tag search.
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
            StringBuilder sb = new StringBuilder(); sb.Append(QueryTokens.TRANSITION); sb.Append(" "); sb.Append(QueryTokens.EXIT); sb.Append(" "); sb.Append(QueryTokens.INSERT);
            QueryTokens qt = new QueryTokens(sb.ToString());
            int caret = ref_caret;
            string stringWithoutDoubleQuotation, parenthesis;

            if (!LexcalP.FixedWord(QueryTokens.TRANSITION, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.TRANSITION;

            if (!LexcalP.FixedWord(QueryTokens.EXIT, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Target2 = QueryTokens.EXIT;

            if (!LexcalP.FixedWord(QueryTokens.INSERT, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.INSERT;

            if (!LexcalP.FixedWord(QueryTokens.FROM, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }

            // Either regular expression or tag search.
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
            StringBuilder sb = new StringBuilder(); sb.Append(QueryTokens.STATE); sb.Append(" "); sb.Append(QueryTokens.INSERT);
            QueryTokens qt = new QueryTokens(sb.ToString());
            int caret = ref_caret;
            string stringWithoutDoubleQuotation;

            if (!LexcalP.FixedWord(QueryTokens.STATE, query, ref caret)) { return NotMatched(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.STATE;

            if (!LexcalP.FixedWord(QueryTokens.INSERT, query, ref caret)) { return NotMatched(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.INSERT;

            if (LexcalP.FixedWord(QueryTokens.WORDS, query, ref caret))
            {
                // Repeat "value, space, value, space". It ends when the item name is WHERE.
                if (!ParsePhrase_AfterWords(query, ref caret, QueryTokens.WHERE, qt.Words)) { return NotMatched(qt, caret, ref maxQt); }
            }
            else if (LexcalP.FixedWord(QueryTokens.SET, query, ref caret))
            {
                throw new UnityException("Deprecated SET phrase. Next, WORDS phrase. ex: WORDS Bear Cat Dog Elephant");
            }
            else { if (!LexcalP.FixedWord(QueryTokens.WHERE, query, ref caret)) { return NotMatched(qt, caret, ref maxQt); } }

            // regex.
            if (LexcalP.VarStringliteral(query, ref caret, out stringWithoutDoubleQuotation))
            {
                qt.Where_FullnameRegex = stringWithoutDoubleQuotation;
            }
            else { return NotMatched(qt, caret, ref maxQt); }
            maxQt = qt; ref_caret = caret; return true;
        }

        /// <summary>
        /// STATE UPDATE
        /// </summary>
        public static bool Fixed_StateUpdate(string query, ref int ref_caret, ref QueryTokens maxQt)
        {
            StringBuilder sb = new StringBuilder(); sb.Append(QueryTokens.STATE); sb.Append(" "); sb.Append(QueryTokens.UPDATE);
            QueryTokens qt = new QueryTokens(sb.ToString());
            int caret = ref_caret;
            string stringWithoutDoubleQuotation;
            string parenthesis;

            if (!LexcalP.FixedWord(QueryTokens.STATE, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.STATE;

            if (!LexcalP.FixedWord(QueryTokens.UPDATE, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.UPDATE;

            if (LexcalP.FixedWord(QueryTokens.SET, query, ref caret)) {
                // Repeat "item name, space, value, space". It ends when the item name is WHERE.
                if (!SyntaxP.ParsePhrase_AfterSet(query, ref caret, QueryTokens.WHERE, qt.Set)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            } else { if (!LexcalP.FixedWord(QueryTokens.WHERE, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); } }

            // Either regular expression or tag search.
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
            StringBuilder sb = new StringBuilder(); sb.Append(QueryTokens.STATE); sb.Append(" "); sb.Append(QueryTokens.DELETE);
            QueryTokens qt = new QueryTokens(sb.ToString());
            int caret = ref_caret;
            string stringWithoutDoubleQuotation;

            if (!LexcalP.FixedWord(QueryTokens.STATE, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.STATE;

            if (!LexcalP.FixedWord(QueryTokens.DELETE, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.DELETE;

            if (LexcalP.FixedWord(QueryTokens.WORDS, query, ref caret))
            {
                // Repeat "value, space, value, space". It ends when the item name is WHERE.
                if (!SyntaxP.ParsePhrase_AfterWords(query, ref caret, QueryTokens.WHERE, qt.Words)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            }
            else if (LexcalP.FixedWord(QueryTokens.SET, query, ref caret))
            {
                throw new UnityException("Deprecated SET phrase. Next, WORDS phrase. ex: WORDS Bear Cat Dog Elephant");
            }
            else { if (!LexcalP.FixedWord(QueryTokens.WHERE, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); } }

            // regex.
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
            StringBuilder sb = new StringBuilder(); sb.Append(QueryTokens.STATE); sb.Append(" "); sb.Append(QueryTokens.SELECT);
            QueryTokens qt = new QueryTokens(sb.ToString());
            int caret = ref_caret;
            string stringWithoutDoubleQuotation, word;
            string parenthesis;

            if (!LexcalP.FixedWord(QueryTokens.STATE, query, ref caret)) { return NotMatched(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.STATE;

            if (!LexcalP.FixedWord(QueryTokens.SELECT, query, ref caret)) { return NotMatched(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.SELECT;

            if (!LexcalP.FixedWord(QueryTokens.WHERE, query, ref caret)) { return NotMatched(qt, caret, ref maxQt); }

            // Either regular expression or tag search.
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

            if (LexcalP.FixedWord(QueryTokens.THE, query, ref caret)) // option
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
            StringBuilder sb = new StringBuilder(); sb.Append(QueryTokens.TRANSITION); sb.Append(" "); sb.Append(QueryTokens.INSERT);
            QueryTokens qt = new QueryTokens(sb.ToString());
            int caret = ref_caret;
            string stringWithoutDoubleQuotation;
            string parenthesis;

            if (!LexcalP.FixedWord(QueryTokens.TRANSITION, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.TRANSITION;

            if (!LexcalP.FixedWord(QueryTokens.INSERT, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.INSERT;

            if (LexcalP.FixedWord(QueryTokens.SET, query, ref caret))
            {
                // Repeat "item name, space, value, space". It ends when the item name is FROM.
                if (!SyntaxP.ParsePhrase_AfterSet(query, ref caret, QueryTokens.FROM, qt.Set)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            }
            else {
                if (!LexcalP.FixedWord(QueryTokens.FROM, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            }

            // Either regular expression or tag search.
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

            // Either regular expression or tag search.
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
            StringBuilder sb = new StringBuilder(); sb.Append(QueryTokens.TRANSITION); sb.Append(" "); sb.Append(QueryTokens.UPDATE);
            QueryTokens qt = new QueryTokens(sb.ToString());
            int caret = ref_caret;
            string stringWithoutDoubleQuotation;
            string parenthesis;

            if (!LexcalP.FixedWord(QueryTokens.TRANSITION, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.TRANSITION;

            if (!LexcalP.FixedWord(QueryTokens.UPDATE, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.UPDATE;

            if (LexcalP.FixedWord(QueryTokens.SET, query, ref caret))
            {
                // Repeat "item name, space, value, space". It ends when the item name is FROM.
                if (!SyntaxP.ParsePhrase_AfterSet(query, ref caret, QueryTokens.FROM, qt.Set)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            }
            else {
                if (!LexcalP.FixedWord(QueryTokens.FROM, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            }

            // Either regular expression or tag search.
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

            // Either regular expression or tag search.
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
            StringBuilder sb = new StringBuilder(); sb.Append(QueryTokens.TRANSITION); sb.Append(" "); sb.Append(QueryTokens.DELETE);
            QueryTokens qt = new QueryTokens(sb.ToString());
            int caret = ref_caret;
            string stringWithoutDoubleQuotation;
            string parenthesis;

            if (!LexcalP.FixedWord(QueryTokens.TRANSITION, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.TRANSITION;

            if (!LexcalP.FixedWord(QueryTokens.DELETE, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.DELETE;

            if (!LexcalP.FixedWord(QueryTokens.FROM, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }

            // Either regular expression or tag search.
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

            // Either regular expression or tag search.
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
            StringBuilder sb = new StringBuilder(); sb.Append(QueryTokens.TRANSITION); sb.Append(" "); sb.Append(QueryTokens.SELECT);
            QueryTokens qt = new QueryTokens("TRANSITION SELECT");
            int caret = ref_caret;
            string stringWithoutDoubleQuotation, word;
            string parenthesis;

            if (!LexcalP.FixedWord(QueryTokens.TRANSITION, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.TRANSITION;

            if (!LexcalP.FixedWord(QueryTokens.SELECT, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.SELECT;

            if (!LexcalP.FixedWord(QueryTokens.FROM, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }

            // Either regular expression or tag search.
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

            // Either regular expression or tag search.
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
            StringBuilder sb = new StringBuilder(); sb.Append(QueryTokens.CSHARPSCRIPT); sb.Append(" "); sb.Append(QueryTokens.GENERATE_FULLPATH);
            QueryTokens qt = new QueryTokens(sb.ToString());
            int caret = ref_caret;

            if (!LexcalP.FixedWord(QueryTokens.CSHARPSCRIPT, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.CSHARPSCRIPT;

            if (!LexcalP.FixedWord(QueryTokens.GENERATE_FULLPATH, query, ref caret)) { return SyntaxP.NotMatched(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.GENERATE_FULLPATH;

            maxQt = qt; ref_caret = caret; return true;
        }

        /// <summary>
        /// LAYER INSERT
        /// </summary>
        public static bool Fixed_LayerInsert(string query, ref int ref_caret, ref QueryTokens maxQt)
        {
            StringBuilder sb = new StringBuilder(); sb.Append(QueryTokens.LAYER); sb.Append(" "); sb.Append(QueryTokens.INSERT);
            QueryTokens qt = new QueryTokens(sb.ToString());
            int caret = ref_caret;

            if (!LexcalP.FixedWord(QueryTokens.LAYER, query, ref caret)) { return NotMatched(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.LAYER;

            if (!LexcalP.FixedWord(QueryTokens.INSERT, query, ref caret)) { return NotMatched(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.INSERT;

            if (LexcalP.FixedWord(QueryTokens.WORDS, query, ref caret))
            {
                // Repeat "value, space, value, space". It ends when the item name is a semicolon.
                if (!ParsePhrase_AfterWords(query, ref caret, QueryTokens.SEMICOLON, qt.Words)) { return NotMatched(qt, caret, ref maxQt); }
            }
            else { return NotMatched(qt, caret, ref maxQt); }

            maxQt = qt; ref_caret = caret; return true;
        }

        /// <summary>
        /// LAYER DELETE
        /// </summary>
        public static bool Fixed_LayerDelete(string query, ref int ref_caret, ref QueryTokens maxQt)
        {
            StringBuilder sb = new StringBuilder(); sb.Append(QueryTokens.LAYER); sb.Append(" "); sb.Append(QueryTokens.DELETE);
            QueryTokens qt = new QueryTokens(sb.ToString());
            int caret = ref_caret;

            if (!LexcalP.FixedWord(QueryTokens.LAYER, query, ref caret)) { return NotMatched(qt, caret, ref maxQt); }
            qt.Target = QueryTokens.LAYER;

            if (!LexcalP.FixedWord(QueryTokens.DELETE, query, ref caret)) { return NotMatched(qt, caret, ref maxQt); }
            qt.Manipulation = QueryTokens.DELETE;

            if (LexcalP.FixedWord(QueryTokens.WORDS, query, ref caret))
            {
                // Repeat "value, space, value, space". It ends when the item name is a semicolon.
                if (!ParsePhrase_AfterWords(query, ref caret, QueryTokens.SEMICOLON, qt.Words)) { return NotMatched(qt, caret, ref maxQt); }
            }
            else { return NotMatched(qt, caret, ref maxQt); }

            maxQt = qt; ref_caret = caret; return true;
        }
    }

    public abstract class SyntaxPOther
    {
        /// <summary>
        /// It is a job to create a token to pass to the scan.
        /// 
        /// ([(Alpaca Bear)(Cat Dog)]{Elephant})
        /// to
        /// 「(」「[」「(」「Alpaca」「Bear」「)」「(」「Cat」「Dog」「)」「]」「{」「Elephant」「}」「)」
        /// 
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
                    if (0 < bufferWord.Length) { tokens.Add(bufferWord.ToString()); bufferWord.Length = 0; } // Skip whitespace.
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
                        case '}':
                            if (0 < bufferWord.Length) { tokens.Add(bufferWord.ToString()); bufferWord.Length = 0; }
                            tokens.Add(ch.ToString()); break;
                        default:
                            bufferWord.Append(ch); break;
                    }
                    iCaret++;
                }
            }
            if (0 < bufferWord.Length) { tokens.Add(bufferWord.ToString()); bufferWord.Length = 0; } // Syntax error
        }
    }

    /// <summary>
    /// Lexcal parser
    /// </summary>
    public abstract class LexcalP
    {
        private static Regex regexSpaces = new Regex(@"^(\s+)");
        public static bool VarSpaces(string query, ref int caret) {
            Match match = regexSpaces.Match(query.Substring(caret));
            if (match.Success) { caret += match.Groups[1].Value.Length; return true; } return false;
        }

        /// <summary>
        /// Determine if a fixed word is there.
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
        /// "Bear)" and there are cases in which half-width spaces are not attached at the back, so 0 spaces are OK.
        /// </summary>
        private static Regex regexWordAndSpaces = new Regex(@"^(\w+)(\s*)", RegexOptions.IgnoreCase);
        public static bool VarWord(string query, ref int caret, out string word) {
            Match match = regexWordAndSpaces.Match(query.Substring(caret));
            if (match.Success) { word = match.Groups[1].Value; caret += word.Length + match.Groups[2].Value.Length; return true; }
            word = ""; return false;
        }

        /// <summary>
        /// Floating point "." Is also OK.
        /// </summary>
        private static Regex regexValueAndSpaces = new Regex(@"^((?:\w|\.)+)(\s*)", RegexOptions.IgnoreCase);
        public static bool VarValue(string query, ref int caret, out string word) {
            Match match = regexValueAndSpaces.Match(query.Substring(caret));
            if (match.Success) { word = match.Groups[1].Value; caret += word.Length + match.Groups[2].Value.Length; return true; }
            word = ""; return false;
        }

        /// <summary>
        /// Unescape "\"" "\n" "\r" "\\".
        /// </summary>
        /// <param name="stringWithoutDoubleQuotation"></param>
        /// <returns></returns>
        public static string UnescapeEscapesequence(string stringWithoutDoubleQuotation)
        {
            StringBuilder dst = new StringBuilder();
            int phase = 0;
            for (int srcCaret = 0; srcCaret < stringWithoutDoubleQuotation.Length; srcCaret++)
            {
                switch (phase)
                {
                    case 1: // Immediately after reading "\". 
                        switch (stringWithoutDoubleQuotation[srcCaret])
                        {
                            case '\\': // "\\" was.
                            case '\"': // "\" was.
                                dst.Append(stringWithoutDoubleQuotation[srcCaret]); break; // Read only the second letter.
                            case 'n': dst.Append('\n'); break; // It was "\r". Read line feed (LF 10).
                            case 'r': dst.Append('\r'); break; // It was "\n". Read carriage return carriage return (CHR 13).
                            default: dst.Append('\\'); dst.Append(stringWithoutDoubleQuotation[srcCaret]); break; // Read "\"" and the next character as it is.
                        }
                        phase = 0;
                        break;
                    default:
                        switch (stringWithoutDoubleQuotation[srcCaret])
                        {
                            case '\\': phase = 1; break; // "\" came out. We do not read "\" yet.
                            default: dst.Append(stringWithoutDoubleQuotation[srcCaret]); break;
                        }
                        break;
                }
            }
            return dst.ToString();
        }
        private static Regex regexStringliteralAndSpaces = new Regex(@"^""((?:(?:\\"")|[^""])*)""(\s*)", RegexOptions.IgnoreCase);
        public static bool VarStringliteral(string query, ref int caret, out string stringWithoutDoubleQuotation) {
            Match match = regexStringliteralAndSpaces.Match(query.Substring(caret));
            if (match.Success) {
                stringWithoutDoubleQuotation = match.Groups[1].Value;
                // Add two characters of double quotation
                caret += stringWithoutDoubleQuotation.Length + 2 + match.Groups[2].Value.Length;
                // Some processing such as unescape "\" to (").
                stringWithoutDoubleQuotation = UnescapeEscapesequence(stringWithoutDoubleQuotation);
                return true;
            }
            stringWithoutDoubleQuotation = ""; return false;
        }

        public static bool VarParentesis(string query, ref int caret, out string parentesis) {
            int oldCaret = caret;
            string word;
            Stack<char> closeParen = new Stack<char>();

            switch (query[caret])
            { // When starting
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
            parentesis = query.Substring(oldCaret, caret - oldCaret); // Although it is an error, I want to return it until I analyzed it.
            return false;
        }

        /// <summary>
        /// A line beginning with "#" is a comment.
        /// Delete comment line and blank line.
        /// 
        /// I reffered it.
        /// \r to \r\n : http://www.madeinclinic.jp/c/20140201/
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static void DeleteLineCommentAndBlankLine(ref string query) {
            query = query.Replace("\n", "\r\n").Replace("\r\r", "\r"); // \r to \r\n

            string[] lines = query.Split(new [] { Environment.NewLine }, StringSplitOptions.None);
            int caret;
            for (int iLine = 0; iLine < lines.Length; iLine++) {
                caret = 0;
                VarSpaces(lines[iLine], ref caret);
                if (FixedWord("#", lines[iLine], ref caret)) { lines[iLine] = ""; } // It is a comment line. You should leave it blank.
            }

            // Is there anything in trouble by erasing the blank line?
            StringBuilder sb = new StringBuilder();
            foreach (string line in lines) { if ("" != line.Trim()) { sb.AppendLine(line); } } // Leave something other than blank lines
            query = sb.ToString();
        }
    }
}
