//
// Required. This file new line is \r\n.
//
// Visual Studio 2015 [File] - [Advanced Save Options ...] - Line endings: [Windows (CR LF)]
//
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using StellaQL.Acons.Demo_Zoo;

namespace StellaQL
{
    public class TestStellaQLDemo
    {
        static TestStellaQLDemo()
        {
            m_ac = AssetDatabase.LoadAssetAtPath<AnimatorController>(FileUtility_Engine.PATH_ANIMATOR_CONTROLLER_FOR_DEMO_TEST);// Get an animator controller.
        }
        static AnimatorController m_ac;


        #region N30 Execute Query (Give a string and fetch the record index)
        [Test]
        public void N30_Query_StateSelect()
        {
            string query = @"STATE SELECT
                        WHERE TAG ([(Ei Si)(Bi)]{I})";
            HashSet<int> recordHashes;
            StringBuilder message = new StringBuilder();
            bool successful = Querier.ExecuteStateSelect(query, AControl.Instance.StateHash_to_record, out recordHashes, message);

            Assert.IsTrue(successful);
            Assert.AreEqual(3, recordHashes.Count);
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_ALPACA)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_CAT)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_RABBIT)));
        }

        /// <summary>
        /// Testing comments and blank lines
        /// </summary>
        [Test]
        public void N30_Query_Comment1_WindowsCrLf()
        {
            string query = @"# comment A

                        STATE SELECT

                        # comment B

                        WHERE TAG ([(Ei Si)(Bi)]{I})

                        # comment C

                        ";

            LexcalP.DeleteLineCommentAndBlankLine(ref query);

            Assert.AreEqual(@"                        STATE SELECT
                        WHERE TAG ([(Ei Si)(Bi)]{I})
", query, "Maybe, No problem.");
        }
        /// <summary>
        /// Testing comments and blank lines
        /// </summary>
        [Test]
        public void N30_Query_Comment2()
        {
            string query = @"# comment A

                        STATE SELECT

                        # comment B

                        WHERE TAG ([(Ei Si)(Bi)]{I})

                        # comment C

                        ";

            // OK if the STATE SELECT statement moves
            int caret = 0;
            QueryTokens qt = new QueryTokens("Query syntax Not applicable");

            LexcalP.DeleteLineCommentAndBlankLine(ref query);// Delete all comments and blank lines.
            LexcalP.VarSpaces(query, ref caret); // Remove the first blank.
            SyntaxP.Pattern syntaxPattern = SyntaxP.FixedQuery(query, ref caret, ref qt);

            StringBuilder message = new StringBuilder();
            bool successful = Querier.Execute(m_ac, qt, syntaxPattern, AControl.Instance, message);

            Assert.IsTrue(successful);
        }

        [Test]
        public void N30_Query_TransitionSelect()
        {
            string query = @"TRANSITION SELECT
                        FROM ""Base Layer\.Zebra""
                        TO TAG ([(Ei Si)(Bi)]{I})";
            HashSet<int> recordHashesSrc;
            HashSet<int> recordHashesDst;
            StringBuilder message = new StringBuilder();
            bool successful = Querier.ExecuteTransitionSelect(query, AControl.Instance.StateHash_to_record, out recordHashesSrc, out recordHashesDst, message);

            Assert.AreEqual(1, recordHashesSrc.Count);
            Assert.IsTrue(recordHashesSrc.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_ZEBRA)));
            Assert.AreEqual(3, recordHashesDst.Count);
            Assert.IsTrue(recordHashesDst.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_ALPACA)));
            Assert.IsTrue(recordHashesDst.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_CAT)));
            Assert.IsTrue(recordHashesDst.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_RABBIT)));
        }

        [Test]
        public void N30_Query_TransitionAnysateInsert()
        {
            string query = @"TRANSITION ANYSTATE INSERT
                            FROM ""Base Layer""
                            TO ""Base Layer\.Foo""";
            StringBuilder message = new StringBuilder();
            int caret = 0;
            QueryTokens qt = new QueryTokens("Query syntax Not applicable");
            SyntaxP.Pattern syntaxPattern = SyntaxP.FixedQuery(query, ref caret, ref qt);

            bool successful = Querier.Execute(m_ac, qt, syntaxPattern, AControl.Instance, message);

            //ebug.Log("N30_Query_TransitionAnysateInsert: " + message.ToString());
            Assert.IsTrue(successful);
        }
        [Test]
        public void N30_Query_TransitionEntryInsert()
        {
            string query = @"TRANSITION ENTRY INSERT
                            FROM ""Base Layer""
                            TO ""Base Layer\.Foo""";
            StringBuilder message = new StringBuilder();
            int caret = 0;
            QueryTokens qt = new QueryTokens("Query syntax Not applicable");
            SyntaxP.Pattern syntaxPattern = SyntaxP.FixedQuery(query, ref caret, ref qt);

            bool successful = Querier.Execute(m_ac, qt, syntaxPattern, AControl.Instance, message);

            //ebug.Log("N30_Query_TransitionEntryInsert: " + message.ToString());
            Assert.IsTrue(successful);
        }
        [Test]
        public void N30_Query_TransitionExitInsert()
        {
            string query = @"TRANSITION EXIT INSERT
                            FROM ""Base Layer\.Foo""";
            StringBuilder message = new StringBuilder();
            int caret = 0;
            QueryTokens qt = new QueryTokens("Query syntax Not applicable");
            SyntaxP.Pattern syntaxPattern = SyntaxP.FixedQuery(query, ref caret, ref qt);

            bool successful = Querier.Execute(m_ac, qt, syntaxPattern, AControl.Instance, message);

            //ebug.Log("N30_Query_TransitionExitInsert: " + message.ToString());
            Assert.IsTrue(successful);
        }
        #endregion

        #region N40 Fetch (Fetch records and indexes)
        /// <summary>
        /// Acquire record index based on locker
        /// </summary>
        [Test]
        public void N40_Fetch_ByLockers()
        {
            List<List<string>> tokenLockers = new List<List<string>>(){ // "([(Ei Si)(Bi)]{I})"
            new List<string>() { "Si", "Ei", },
            new List<string>() { "Bi", },
            new List<string>() { "1","0",},
            new List<string>() { "I", },
            new List<string>() { "3","2",},
        };
            List<string> tokenLockersOperation = new List<string>() { "(", "(", "[", "{", "(", };

            List<HashSet<int>> recordHashesLockers;
            RecordsFilter.TokenLockers_to_recordHashesLockers(
                tokenLockers, tokenLockersOperation, AControl.Instance.StateHash_to_record, out recordHashesLockers);

            Assert.AreEqual(5, recordHashesLockers.Count);
            Assert.AreEqual(2, recordHashesLockers[0].Count);
            Assert.IsTrue(recordHashesLockers[0].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_ALPACA)));
            Assert.IsTrue(recordHashesLockers[0].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_CAT)));
            Assert.AreEqual(3, recordHashesLockers[1].Count);
            Assert.IsTrue(recordHashesLockers[1].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_BEAR)));
            Assert.IsTrue(recordHashesLockers[1].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_RABBIT)));
            Assert.IsTrue(recordHashesLockers[1].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_ZEBRA)));
            Assert.AreEqual(5, recordHashesLockers[2].Count);
            Assert.IsTrue(recordHashesLockers[2].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_ALPACA)));
            Assert.IsTrue(recordHashesLockers[2].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_CAT)));
            Assert.IsTrue(recordHashesLockers[2].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_BEAR)));
            Assert.IsTrue(recordHashesLockers[2].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_RABBIT)));
            Assert.IsTrue(recordHashesLockers[2].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_ZEBRA)));
            Assert.AreEqual(26, recordHashesLockers[3].Count);
            Assert.IsTrue(recordHashesLockers[3].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_)));
            Assert.IsTrue(recordHashesLockers[3].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_ANYSTATE)));
            Assert.IsTrue(recordHashesLockers[3].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_ENTRY)));
            Assert.IsTrue(recordHashesLockers[3].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_EXIT)));
            Assert.IsTrue(recordHashesLockers[3].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_FOO)));
            Assert.IsTrue(recordHashesLockers[3].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_ALPACA)));
            Assert.IsTrue(recordHashesLockers[3].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_CAT)));
            Assert.IsTrue(recordHashesLockers[3].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_DOG)));
            Assert.IsTrue(recordHashesLockers[3].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_FOX)));
            Assert.IsTrue(recordHashesLockers[3].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_IGUANA)));
            Assert.IsTrue(recordHashesLockers[3].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_KANGAROO)));
            Assert.IsTrue(recordHashesLockers[3].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_LION)));
            Assert.IsTrue(recordHashesLockers[3].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_NUTRIA)));
            Assert.IsTrue(recordHashesLockers[3].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_OX)));
            Assert.IsTrue(recordHashesLockers[3].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_PIG)));
            Assert.IsTrue(recordHashesLockers[3].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_RABBIT)));
            Assert.IsTrue(recordHashesLockers[3].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_UNICORN)));
            Assert.IsTrue(recordHashesLockers[3].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_WOLF)));
            Assert.IsTrue(recordHashesLockers[3].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_YAK)));
            Assert.IsTrue(recordHashesLockers[3].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_JAPAN_)));
            Assert.IsTrue(recordHashesLockers[3].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_JAPAN_SAITAMA_)));
            Assert.IsTrue(recordHashesLockers[3].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_JAPAN_SAITAMA_NIIZA)));
            Assert.IsTrue(recordHashesLockers[3].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_JAPAN_TOKYO_)));
            Assert.IsTrue(recordHashesLockers[3].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_JAPAN_TOKYO_ARIAKE)));
            Assert.IsTrue(recordHashesLockers[3].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_JAPAN_CHIBA_)));
            Assert.IsTrue(recordHashesLockers[3].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_JAPAN_CHIBA_MAKUHARI)));
            Assert.AreEqual(3, recordHashesLockers[4].Count);
            Assert.IsTrue(recordHashesLockers[4].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_ALPACA)));
            Assert.IsTrue(recordHashesLockers[4].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_CAT)));
            Assert.IsTrue(recordHashesLockers[4].Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_RABBIT)));
        }
        #endregion

        #region N50 RecordsFilter (Record set)
        /// <summary>
        /// ( ) Element filter
        /// </summary>
        [Test]
        public void N50_RecordsFilter_RecordsAnd()
        {
            // 「 Bear, Elephant 」 AND 「 Bear, Giraffe 」
            HashSet<int> lockerNumbers = new HashSet<int>() { 0, 1 };
            List<HashSet<int>> reordIndexLockers = new List<HashSet<int>>()
        {
            new HashSet<int>() { Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_BEAR),Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_ELEPHANT) },
            new HashSet<int>() { Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_BEAR), Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_GIRAFFE) },
        };
            HashSet<int> recordHashes = RecordsFilter.Records_And(lockerNumbers, reordIndexLockers);

            // The result is Bear
            Assert.AreEqual(1, recordHashes.Count);
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_BEAR)));
        }

        /// <summary>
        /// [ ] Element filter
        /// </summary>
        [Test]
        public void N50_RecordsFilter_RecordsOr()
        {
            // 「 Bear, Elephant 」 OR 「 Bear, Giraffe 」
            HashSet<int> lockerNumbers = new HashSet<int>() { 0, 1 };
            List<HashSet<int>> recordHasheslockers = new List<HashSet<int>>()
        {
            new HashSet<int>() { Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_BEAR),Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_ELEPHANT) },
            new HashSet<int>() { Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_BEAR),Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_GIRAFFE) },
        };
            HashSet<int> recordHashes = RecordsFilter.Records_Or(lockerNumbers, recordHasheslockers);

            Assert.AreEqual(3, recordHashes.Count);
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_BEAR)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_ELEPHANT)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_GIRAFFE)));
        }

        /// <summary>
        /// { } Element filter
        /// </summary>
        [Test]
        public void N50_RecordsFilter_RecordsNotAndNot()
        {
            // NOT「 Bear, Elephant 」 AND NOT「 Bear, Giraffe 」
            HashSet<int> lockerNumbers = new HashSet<int>() { 0, 1 };
            List<HashSet<int>> recordHashesLockers = new List<HashSet<int>>()
            {
                new HashSet<int>() { Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_BEAR),Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_ELEPHANT) },
                new HashSet<int>() { Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_BEAR),Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_GIRAFFE) },
            };
            HashSet<int> recordHashes = RecordsFilter.Records_NotAndNot(lockerNumbers, recordHashesLockers, AControl.Instance.StateHash_to_record);

            Assert.AreEqual(35, recordHashes.Count);
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_ANYSTATE)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_ENTRY)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_EXIT)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_FOO)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_ALPACA)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_CAT)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_DOG)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_FOX)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_HORSE)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_IGUANA)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_JELLYFISH)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_KANGAROO)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_LION)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_MONKEY)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_NUTRIA)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_OX)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_PIG)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_QUETZAL)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_RABBIT)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_SHEEP)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_TIGER)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_UNICORN)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_VIXEN)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_WOLF)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_XENOPUS)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_YAK)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_ZEBRA)));

            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_JAPAN_)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_JAPAN_SAITAMA_)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_JAPAN_SAITAMA_NIIZA)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_JAPAN_TOKYO_)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_JAPAN_TOKYO_ARIAKE)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_JAPAN_CHIBA_)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_JAPAN_CHIBA_MAKUHARI)));
        }

        /// <summary>
        /// State name regular expression filter
        /// </summary>
        [Test]
        public void N50_RecordsFilter_StateFullNameRegex()
        {
            // Conditions are those containing "n" or "N" under "Base Layer."
            string pattern = @"Base Layer\.\w*[Nn]\w*";
            StringBuilder message = new StringBuilder();
            HashSet<int> recordHashes = RecordsFilter.String_StateFullNameRegex(pattern, AControl.Instance.StateHash_to_record, message);

            Assert.AreEqual(18, recordHashes.Count);
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_ANYSTATE)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_ENTRY)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_ELEPHANT)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_IGUANA)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_KANGAROO)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_LION)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_MONKEY)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_NUTRIA)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_UNICORN)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_VIXEN)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_XENOPUS)));

            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_JAPAN_)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_JAPAN_SAITAMA_)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_JAPAN_SAITAMA_NIIZA)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_JAPAN_TOKYO_)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_JAPAN_TOKYO_ARIAKE)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_JAPAN_CHIBA_)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_JAPAN_CHIBA_MAKUHARI)));
        }

        /// <summary>
        /// ( ) Tag filter.
        /// </summary>
        [Test]
        public void N50_RecordsFilter_TagsAnd()
        {
            // Ei | I
            HashSet<int> attrs = Code.Hashes( new string[]{ AControl.TAG_EI, AControl.TAG_I });
            HashSet<int> recordHashes = RecordsFilter.Tags_And(attrs, AControl.Instance.StateHash_to_record);

            Assert.AreEqual(5, recordHashes.Count);
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_BEAR)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_ELEPHANT)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_GIRAFFE)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_QUETZAL)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_ZEBRA)));
        }

        /// <summary>
        /// [　] tag filter
        /// </summary>
        [Test]
        public void N50_RecordsFilter_TagsOr()
        {
            // TAG [ Ei Bi I ]
            HashSet<int> attrs = Code.Hashes(new string[]{AControl.TAG_EI,AControl.TAG_BI,AControl.TAG_I });
            HashSet<int> recordHashes = RecordsFilter.Tags_Or(attrs, AControl.Instance.StateHash_to_record);

            Assert.AreEqual(19, recordHashes.Count);
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_ALPACA)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_BEAR)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_CAT)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_ELEPHANT)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_GIRAFFE)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_HORSE)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_IGUANA)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_JELLYFISH)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_KANGAROO)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_MONKEY)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_NUTRIA)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_QUETZAL)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_RABBIT)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_SHEEP)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_TIGER)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_VIXEN)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_XENOPUS)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_YAK)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_ZEBRA)));
        }

        /// <summary>
        /// ｛　｝ tag filter
        /// </summary>
        [Test]
        public void N50_RecordsFilter_TagsNotAndNot()
        {
            // { Bi、I }
            HashSet<int> attrs = Code.Hashes(new string[] { AControl.TAG_BI, AControl.TAG_I });
            HashSet<int> recordHashes = RecordsFilter.Tags_NotAndNot(attrs, AControl.Instance.StateHash_to_record);

            Assert.AreEqual(25, recordHashes.Count);
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_ANYSTATE)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_ENTRY)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_EXIT)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_FOO)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_ALPACA)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_CAT)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_DOG)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_FOX)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_IGUANA)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_KANGAROO)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_LION)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_NUTRIA)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_OX)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_PIG)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_UNICORN)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_WOLF)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_YAK)));

            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_JAPAN_)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_JAPAN_SAITAMA_)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_JAPAN_SAITAMA_NIIZA)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_JAPAN_TOKYO_)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_JAPAN_TOKYO_ARIAKE)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_JAPAN_CHIBA_)));
            Assert.IsTrue(recordHashes.Contains(Animator.StringToHash(Demo_Zoo_AbstractAControl.BASELAYER_JAPAN_CHIBA_MAKUHARI)));
        }
        #endregion

        #region N60 Tag set ope (tag set)
        /// <summary>
        /// TAG (　)
        /// TAG [　]
        /// </summary>
        [Test]
        public void N60_ToAttrLocker_FromKeywordlistSet()
        {
            HashSet<int> attrLocker = Code.Hashes(new string[] { AControl.TAG_BI, AControl.TAG_DI });

            Assert.AreEqual(2, attrLocker.Count);
            Assert.IsTrue(attrLocker.Contains(Animator.StringToHash(AControl.TAG_BI)));
            Assert.IsTrue(attrLocker.Contains(Animator.StringToHash(AControl.TAG_DI )));
        }

        /// <summary>
        /// TAG {　}
        /// </summary>
        [Test]
        public void N60_ToAttrLocker_FromNGKeywordSet()
        {
            HashSet<int> set = Code.Hashes(new string[] { AControl.TAG_BI, AControl.TAG_DI });
            HashSet<int> attrLocker = TagSetOpe.Complement(set, Code.Hashes(new[] { AControl.TAG_ZERO, AControl.TAG_EI, AControl.TAG_BI, AControl.TAG_SI, AControl.TAG_DI, AControl.TAG_I, AControl.TAG_HORN, }));

            Assert.AreEqual(5, attrLocker.Count);
            Assert.IsTrue(attrLocker.Contains(Animator.StringToHash(AControl.TAG_ZERO)));
            Assert.IsTrue(attrLocker.Contains(Animator.StringToHash(AControl.TAG_EI)));
            Assert.IsTrue(attrLocker.Contains(Animator.StringToHash(AControl.TAG_SI)));
            Assert.IsTrue(attrLocker.Contains(Animator.StringToHash(AControl.TAG_I)));
            Assert.IsTrue(attrLocker.Contains(Animator.StringToHash(AControl.TAG_HORN)));
        }

        /// <summary>
        /// Test whether complement can be taken
        /// </summary>
        [Test]
        public void N60_ToAttrLocker_GetComplement()
        {
            // Hold it as an int type
            HashSet<int> set = Code.Hashes(new string[] { AControl.TAG_BI, AControl.TAG_DI });
            HashSet<int> attrLocker = TagSetOpe.Complement(set, Code.Hashes(new[] { AControl.TAG_ZERO, AControl.TAG_EI, AControl.TAG_BI, AControl.TAG_SI, AControl.TAG_DI, AControl.TAG_I, AControl.TAG_HORN, }));

            Assert.AreEqual(5, attrLocker.Count);
            Assert.IsTrue(attrLocker.Contains(Animator.StringToHash(AControl.TAG_ZERO)));
            Assert.IsTrue(attrLocker.Contains(Animator.StringToHash(AControl.TAG_EI)));
            Assert.IsTrue(attrLocker.Contains(Animator.StringToHash(AControl.TAG_SI)));
            Assert.IsTrue(attrLocker.Contains(Animator.StringToHash(AControl.TAG_I)));
            Assert.IsTrue(attrLocker.Contains(Animator.StringToHash(AControl.TAG_HORN)));
        }
        #endregion

        #region N65 data builder
        /// <summary>
        /// TAG phrase
        /// </summary>
        [Test]
        public void N65_Parse_TagParentesis_TokensToLockers()
        {
            List<string> tokens = new List<string>() { "(", "[", "(", "Alpaca", "Bear", ")", "(", "Cat", "Dog", ")", "]", "{", "Elephant", "}", ")", };
            List<List<string>> tokenLockers;
            List<string> tokenLockersOperation;
            Querier.Tokens_to_lockers(tokens, out tokenLockers, out tokenLockersOperation);

            Assert.AreEqual(5, tokenLockers.Count);
            Assert.AreEqual(5, tokenLockersOperation.Count);
            Assert.AreEqual(2, tokenLockers[0].Count);
            Assert.AreEqual(2, tokenLockers[1].Count);
            Assert.AreEqual(2, tokenLockers[2].Count);
            Assert.AreEqual(1, tokenLockers[3].Count);
            Assert.AreEqual(2, tokenLockers[4].Count);
            Assert.AreEqual("Bear", tokenLockers[0][0]);
            Assert.AreEqual("Alpaca", tokenLockers[0][1]);
            Assert.AreEqual("(", tokenLockersOperation[0]);
            Assert.AreEqual("Dog", tokenLockers[1][0]);
            Assert.AreEqual("Cat", tokenLockers[1][1]);
            Assert.AreEqual("(", tokenLockersOperation[1]);
            Assert.AreEqual("1", tokenLockers[2][0]);
            Assert.AreEqual("0", tokenLockers[2][1]);
            Assert.AreEqual("[", tokenLockersOperation[2]);
            Assert.AreEqual("Elephant", tokenLockers[3][0]);
            Assert.AreEqual("{", tokenLockersOperation[3]);
            Assert.AreEqual("3", tokenLockers[4][0]);
            Assert.AreEqual("2", tokenLockers[4][1]);
            Assert.AreEqual("(", tokenLockersOperation[4]);
        }
        #endregion

        #region N70 syntax parser statement
        /// <summary>
        /// TRANSITION ANYSTATE INSERT
        /// </summary>
        [Test]
        public void N70_Syntax_TransitionAnystateInsert()
        {
            string query = @"TRANSITION ANYSTATE INSERT
                        FROM ""Base Layer""
                        TO ""Base Layer\.Foo""";
            QueryTokens qt = new QueryTokens();
            int caret = 0;
            bool successful = SyntaxP.Fixed_TransitionAnystateInsert(query, ref caret, ref qt);

            Assert.IsTrue(successful);
            Assert.AreEqual(QueryTokens.TRANSITION, qt.Target);
            Assert.AreEqual(QueryTokens.ANYSTATE, qt.Target2);
            Assert.AreEqual(QueryTokens.INSERT, qt.Manipulation);
            Assert.AreEqual(0, qt.Words.Count);
            Assert.AreEqual(0, qt.Set.Count);
            Assert.AreEqual("Base Layer", qt.From_FullnameRegex);
            Assert.AreEqual("", qt.From_Tag);
            Assert.AreEqual(@"Base Layer\.Foo", qt.To_FullnameRegex);
            Assert.AreEqual("", qt.To_Tag);
            Assert.AreEqual("", qt.Where_FullnameRegex);
            Assert.AreEqual("", qt.Where_Tag);
            Assert.AreEqual("", qt.The);
        }

        /// <summary>
        /// TRANSITION ENTRY INSERT
        /// </summary>
        [Test]
        public void N70_Syntax_TransitionEntryInsert()
        {
            string query = @"TRANSITION ENTRY INSERT
                        FROM ""Base Layer""
                        TO ""Base Layer\.Foo""";
            QueryTokens qt = new QueryTokens();
            int caret = 0;
            bool successful = SyntaxP.Fixed_TransitionEntryInsert(query, ref caret, ref qt);

            Assert.IsTrue(successful);
            Assert.AreEqual(QueryTokens.TRANSITION, qt.Target);
            Assert.AreEqual(QueryTokens.ENTRY, qt.Target2);
            Assert.AreEqual(QueryTokens.INSERT, qt.Manipulation);
            Assert.AreEqual(0, qt.Words.Count);
            Assert.AreEqual(0, qt.Set.Count);
            Assert.AreEqual("Base Layer", qt.From_FullnameRegex);
            Assert.AreEqual("", qt.From_Tag);
            Assert.AreEqual(@"Base Layer\.Foo", qt.To_FullnameRegex);
            Assert.AreEqual("", qt.To_Tag);
            Assert.AreEqual("", qt.Where_FullnameRegex);
            Assert.AreEqual("", qt.Where_Tag);
            Assert.AreEqual("", qt.The);
        }

        /// <summary>
        /// TRANSITION EXIT INSERT
        /// </summary>
        [Test]
        public void N70_Syntax_TransitionExitInsert()
        {
            string query = @"TRANSITION EXIT INSERT
                        FROM ""Base Layer\.Foo""";
            QueryTokens qt = new QueryTokens();
            int caret = 0;
            bool successful = SyntaxP.Fixed_TransitionExitInsert(query, ref caret, ref qt);

            Assert.IsTrue(successful);
            Assert.AreEqual(QueryTokens.TRANSITION, qt.Target);
            Assert.AreEqual(QueryTokens.EXIT, qt.Target2);
            Assert.AreEqual(QueryTokens.INSERT, qt.Manipulation);
            Assert.AreEqual(0, qt.Words.Count);
            Assert.AreEqual(0, qt.Set.Count);
            Assert.AreEqual(@"Base Layer\.Foo", qt.From_FullnameRegex);
            Assert.AreEqual("", qt.From_Tag);
            Assert.AreEqual("", qt.To_FullnameRegex);
            Assert.AreEqual("", qt.To_Tag);
            Assert.AreEqual("", qt.Where_FullnameRegex);
            Assert.AreEqual("", qt.Where_Tag);
            Assert.AreEqual("", qt.The);
        }

        /// <summary>
        /// STATE INSERT
        /// </summary>
        [Test]
        public void N70_Syntax_StateInsert()
        {
            string query = @"STATE INSERT
                        WORDS WhiteCat ""White Dog""
                        WHERE ""Base Layout""";
            QueryTokens qt = new QueryTokens();
            int caret = 0;
            bool successful = SyntaxP.Fixed_StateInsert(query, ref caret, ref qt);

            Assert.IsTrue(successful);
            Assert.AreEqual(QueryTokens.STATE, qt.Target);
            Assert.AreEqual(QueryTokens.INSERT, qt.Manipulation);
            Assert.AreEqual(2, qt.Words.Count);
            Assert.AreEqual("WhiteCat", qt.Words[0]);
            Assert.AreEqual("White Dog", qt.Words[1]);
            Assert.AreEqual(0, qt.Set.Count);
            Assert.AreEqual("", qt.From_FullnameRegex);
            Assert.AreEqual("", qt.From_Tag);
            Assert.AreEqual("", qt.To_FullnameRegex);
            Assert.AreEqual("", qt.To_Tag);
            Assert.AreEqual("Base Layout", qt.Where_FullnameRegex);
            Assert.AreEqual("", qt.Where_Tag);
            Assert.AreEqual("", qt.The);
        }

        /// <summary>
        /// STATE UPDATE
        /// </summary>
        [Test]
        public void N70_Syntax_StateUpdate()
        {
            string query = @"STATE UPDATE
                        SET name ""WhiteCat"" age 7
                        WHERE TAG ([(Ei Si)(Bi)]{I})";
            QueryTokens qt = new QueryTokens();
            int caret = 0;
            bool successful = SyntaxP.Fixed_StateUpdate(query, ref caret, ref qt);

            Assert.IsTrue(successful);
            Assert.AreEqual(QueryTokens.STATE, qt.Target);
            Assert.AreEqual(QueryTokens.UPDATE, qt.Manipulation);
            Assert.AreEqual(0, qt.Words.Count);
            Assert.AreEqual(2, qt.Set.Count);
            Assert.AreEqual("WhiteCat", qt.Set["name"]);
            Assert.AreEqual("7", qt.Set["age"]);
            Assert.AreEqual("", qt.From_FullnameRegex);
            Assert.AreEqual("", qt.From_Tag);
            Assert.AreEqual("", qt.To_FullnameRegex);
            Assert.AreEqual("", qt.To_Tag);
            Assert.AreEqual("", qt.Where_FullnameRegex);
            Assert.AreEqual("([(Ei Si)(Bi)]{I})", qt.Where_Tag);
            Assert.AreEqual("", qt.The);
        }

        /// <summary>
        /// STATE DELETE
        /// </summary>
        [Test]
        public void N70_Syntax_StateDelete()
        {
            string query = @"STATE DELETE
                        WORDS WhiteCat ""White Dog""
                        WHERE ""Base Layout""";
            QueryTokens qt = new QueryTokens();
            int caret = 0;
            bool successful = SyntaxP.Fixed_StateDelete(query, ref caret, ref qt);

            Assert.IsTrue(successful);
            Assert.AreEqual(QueryTokens.STATE, qt.Target);
            Assert.AreEqual(QueryTokens.DELETE, qt.Manipulation);
            Assert.AreEqual(2, qt.Words.Count);
            Assert.AreEqual("WhiteCat", qt.Words[0]);
            Assert.AreEqual("White Dog", qt.Words[1]);
            Assert.AreEqual(0, qt.Set.Count);
            Assert.AreEqual("", qt.From_FullnameRegex);
            Assert.AreEqual("", qt.From_Tag);
            Assert.AreEqual("", qt.To_FullnameRegex);
            Assert.AreEqual("", qt.To_Tag);
            Assert.AreEqual("Base Layout", qt.Where_FullnameRegex);
            Assert.AreEqual("", qt.Where_Tag);
            Assert.AreEqual("", qt.The);
        }

        /// <summary>
        /// STATE SELECT
        /// </summary>
        [Test]
        public void N70_Syntax_StateSelect_VarWord()
        {
            string query = @"STATE SELECT
                        WHERE "".*Dog""";
            QueryTokens qt = new QueryTokens();
            int caret = 0;
            bool successful = SyntaxP.Fixed_StateSelect(query, ref caret, ref qt);

            Assert.IsTrue(successful);
            Assert.AreEqual(QueryTokens.STATE, qt.Target);
            Assert.AreEqual(QueryTokens.SELECT, qt.Manipulation);
            Assert.AreEqual(0, qt.Words.Count);
            Assert.AreEqual(0, qt.Set.Count);
            Assert.AreEqual("", qt.From_FullnameRegex);
            Assert.AreEqual("", qt.From_Tag);
            Assert.AreEqual("", qt.To_FullnameRegex);
            Assert.AreEqual("", qt.To_Tag);
            Assert.AreEqual(".*Dog", qt.Where_FullnameRegex);
            Assert.AreEqual("", qt.Where_Tag);
            Assert.AreEqual("", qt.The);
        }

        /// <summary>
        /// STATE SELECT
        /// </summary>
        [Test]
        public void N70_Syntax_StateSelect_Tag()
        {
            string query = @"STATE SELECT
                        WHERE TAG ([(Ei Si)(Bi)]{I})";
            QueryTokens qt = new QueryTokens();
            int caret = 0;
            bool successful = SyntaxP.Fixed_StateSelect(query, ref caret, ref qt);

            Assert.IsTrue(successful);
            Assert.AreEqual(QueryTokens.STATE, qt.Target);
            Assert.AreEqual(QueryTokens.SELECT, qt.Manipulation);
            Assert.AreEqual(0, qt.Words.Count);
            Assert.AreEqual(0, qt.Set.Count);
            Assert.AreEqual("", qt.From_FullnameRegex);
            Assert.AreEqual("", qt.From_Tag);
            Assert.AreEqual("", qt.To_FullnameRegex);
            Assert.AreEqual("", qt.To_Tag);
            Assert.AreEqual("", qt.Where_FullnameRegex);
            Assert.AreEqual("([(Ei Si)(Bi)]{I})", qt.Where_Tag);
            Assert.AreEqual("", qt.The);
        }

        /// <summary>
        /// Transition Insert
        /// </summary>
        [Test]
        public void N70_Syntax_TransitionInsert()
        {
            string query = @"TRANSITION INSERT
                        SET Duration 0 ExitTime 1
                        FROM ""Base Layer\.Cat""
                        TO ""Base Layer\.Dog""";
            QueryTokens qt = new QueryTokens();
            int caret = 0;
            bool successful = SyntaxP.Fixed_TransitionInsert(query, ref caret, ref qt);

            Assert.IsTrue(successful);
            Assert.AreEqual(QueryTokens.TRANSITION, qt.Target);
            Assert.AreEqual(QueryTokens.INSERT, qt.Manipulation);
            Assert.AreEqual(0, qt.Words.Count);
            Assert.AreEqual(2, qt.Set.Count);
            Assert.IsTrue(qt.Set.ContainsKey("Duration"));
            Assert.AreEqual("0", qt.Set["Duration"]);
            Assert.IsTrue(qt.Set.ContainsKey("ExitTime"));
            Assert.AreEqual("1", qt.Set["ExitTime"]);
            Assert.AreEqual(@"Base Layer\.Cat", qt.From_FullnameRegex);
            Assert.AreEqual("", qt.From_Tag);
            Assert.AreEqual(@"Base Layer\.Dog", qt.To_FullnameRegex);
            Assert.AreEqual("", qt.To_Tag);
            Assert.AreEqual("", qt.The);
        }

        /// <summary>
        /// Transition Update
        /// </summary>
        [Test]
        public void N70_Syntax_TransitionUpdate()
        {
            // First, draw a line.
            {
                string query = @"TRANSITION INSERT
                        SET Duration 0 ExitTime 1
                        FROM ""Base Layer\.Cat""
                        TO ""Base Layer\.Dog""";
                QueryTokens qt = new QueryTokens();
                int caret = 0;
                bool successful = SyntaxP.Fixed_TransitionInsert(query, ref caret, ref qt);
                Assert.IsTrue(successful);
            }

            // From here on
            {
                string query = @"TRANSITION UPDATE
                        SET Duration 0.25 ExitTime 0.75
                        FROM ""Base Layer\.Cat""
                        TO ""Base Layer\.Dog""";
                QueryTokens qt = new QueryTokens();
                int caret = 0;
                bool successful = SyntaxP.Fixed_TransitionUpdate(query, ref caret, ref qt);

                Assert.IsTrue(successful);
                Assert.AreEqual(QueryTokens.TRANSITION, qt.Target);
                Assert.AreEqual(QueryTokens.UPDATE, qt.Manipulation);
                Assert.AreEqual(0, qt.Words.Count);
                Assert.AreEqual(2, qt.Set.Count);
                Assert.IsTrue(qt.Set.ContainsKey("Duration"));
                Assert.AreEqual("0.25", qt.Set["Duration"]);
                Assert.IsTrue(qt.Set.ContainsKey("ExitTime"));
                Assert.AreEqual("0.75", qt.Set["ExitTime"]);
                Assert.AreEqual(@"Base Layer\.Cat", qt.From_FullnameRegex);
                Assert.AreEqual("", qt.From_Tag);
                Assert.AreEqual(@"Base Layer\.Dog", qt.To_FullnameRegex);
                Assert.AreEqual("", qt.To_Tag);
                Assert.AreEqual("", qt.The);
            }
        }

        /// <summary>
        /// Transition Delete
        /// </summary>
        [Test]
        public void N70_Syntax_TransitionDelete()
        {
            string query = @"TRANSITION DELETE
                        FROM ""Base Layer.SMove""
                        TO TAG (BusyX Block)";
            QueryTokens qt = new QueryTokens();
            int caret = 0;
            bool successful = SyntaxP.Fixed_TransitionDelete(query, ref caret, ref qt);

            Assert.IsTrue(successful);
            Assert.AreEqual(QueryTokens.TRANSITION, qt.Target);
            Assert.AreEqual(QueryTokens.DELETE, qt.Manipulation);
            Assert.AreEqual(0, qt.Words.Count);
            Assert.AreEqual(0, qt.Set.Count);
            Assert.AreEqual("Base Layer.SMove", qt.From_FullnameRegex);
            Assert.AreEqual("", qt.From_Tag);
            Assert.AreEqual("", qt.To_FullnameRegex);
            Assert.AreEqual("(BusyX Block)", qt.To_Tag);
            Assert.AreEqual("", qt.The);
        }

        /// <summary>
        /// Transition Select
        /// </summary>
        [Test]
        public void N70_Syntax_TransitionSelect()
        {
            string query = @"TRANSITION SELECT
                        FROM ""Base Layer.SMove""
                        TO TAG (BusyX Block)";
            QueryTokens qt = new QueryTokens();
            int caret = 0;
            bool successful = SyntaxP.Fixed_TransitionSelect(query, ref caret, ref qt);

            Assert.IsTrue(successful);
            Assert.AreEqual(QueryTokens.TRANSITION, qt.Target);
            Assert.AreEqual(QueryTokens.SELECT, qt.Manipulation);
            Assert.AreEqual(0, qt.Words.Count);
            Assert.AreEqual(0, qt.Set.Count);
            Assert.AreEqual("Base Layer.SMove", qt.From_FullnameRegex);
            Assert.AreEqual("", qt.From_Tag);
            Assert.AreEqual("", qt.To_FullnameRegex);
            Assert.AreEqual("(BusyX Block)", qt.To_Tag);
            Assert.AreEqual("", qt.The);
        }

        /// <summary>
        /// CSHARPSCRIPT GENERATE_FULLPATH
        /// </summary>
        [Test]
        public void N70_Syntax_CsharpscriptGenerateFullpath()
        {
            string query = @"CSHARPSCRIPT GENERATE_FULLPATH";
            QueryTokens qt = new QueryTokens();
            int caret = 0;
            bool successful = SyntaxP.Fixed_CsharpscriptGenerateFullpath(query, ref caret, ref qt);

            Assert.IsTrue(successful);
            Assert.AreEqual(QueryTokens.CSHARPSCRIPT, qt.Target);
            Assert.AreEqual(QueryTokens.GENERATE_FULLPATH, qt.Manipulation);
            Assert.AreEqual(0, qt.Words.Count);
            Assert.AreEqual(0, qt.Set.Count);
            Assert.AreEqual("", qt.Where_FullnameRegex);
            Assert.AreEqual("", qt.Where_Tag);
            Assert.AreEqual("", qt.From_FullnameRegex);
            Assert.AreEqual("", qt.From_Tag);
            Assert.AreEqual("", qt.To_FullnameRegex);
            Assert.AreEqual("", qt.To_Tag);
            Assert.AreEqual("", qt.The);
        }

        /// <summary>
        /// LAYER INSERT
        /// </summary>
        [Test]
        public void N70_Syntax_LayerInsert()
        {
            string query = @"LAYER INSERT
                            WORDS
                            NewLayer0
                            ""New Layer1""
                            ""\""New Layer2\""""
                            ""New\\Layer3""
                            ""New\rLayer4""
                            ""New\nLayer5""
                            ""New\r\nLayer6""
                            ";
            QueryTokens qt = new QueryTokens();
            int caret = 0;
            bool successful = SyntaxP.Fixed_LayerInsert(query, ref caret, ref qt);

            Assert.IsTrue(successful);
            Assert.AreEqual(QueryTokens.LAYER, qt.Target);
            Assert.AreEqual(QueryTokens.INSERT, qt.Manipulation);
            Assert.AreEqual(7, qt.Words.Count);
            Assert.AreEqual("NewLayer0", qt.Words[0]);
            Assert.AreEqual("New Layer1", qt.Words[1]); // The contents of the double quotes
            Assert.AreEqual(@"""New Layer2""", qt.Words[2]); // Since the contents of the double quotation is \"New Layer2\", the value is "New Layer2"
            Assert.AreEqual("New\\Layer3", qt.Words[3]); // When the escape sequence symbol is included
            Assert.AreEqual("New\rLayer4", qt.Words[4]); // When a carriage return is included
            Assert.AreEqual("New\nLayer5", qt.Words[5]); // When a new line is included
            Assert.AreEqual("New\r\nLayer6", qt.Words[6]); // When \r\n is included
            Assert.AreEqual(0, qt.Set.Count);
            Assert.AreEqual("", qt.Where_FullnameRegex);
            Assert.AreEqual("", qt.Where_Tag);
            Assert.AreEqual("", qt.From_FullnameRegex);
            Assert.AreEqual("", qt.From_Tag);
            Assert.AreEqual("", qt.To_FullnameRegex);
            Assert.AreEqual("", qt.To_Tag);
            Assert.AreEqual("", qt.The);
        }

        /// <summary>
        /// LAYER DELETE
        /// </summary>
        [Test]
        public void N70_Syntax_LayerDelete()
        {
            // Because it is a regular expression \ will be \\.
            string query = @"LAYER DELETE
                            WORDS
                            NewLayer0
                            ""New Layer1""
                            ""\""New Layer2\""""
                            ""New\\\\Layer3""
                            ""New\rLayer4""
                            ""New\nLayer5""
                            ""New\r\nLayer6""
                            ";
            QueryTokens qt = new QueryTokens();
            int caret = 0;
            bool successful = SyntaxP.Fixed_LayerDelete(query, ref caret, ref qt);

            Assert.IsTrue(successful);
            Assert.AreEqual(QueryTokens.LAYER, qt.Target);
            Assert.AreEqual(QueryTokens.DELETE, qt.Manipulation);
            Assert.AreEqual(7, qt.Words.Count);
            Assert.AreEqual("NewLayer0", qt.Words[0]);
            Assert.AreEqual("New Layer1", qt.Words[1]); // The contents of the double quotes
            Assert.AreEqual(@"""New Layer2""", qt.Words[2]); // Since the content of the double quotation is \"New Layer2\", the value is "New Layer2"
            Assert.AreEqual("New\\\\Layer3", qt.Words[3]); // Since it is a regular expression, it is set to \\. When the escape sequence symbol is included.
            Assert.AreEqual("New\rLayer4", qt.Words[4]); // When a carriage return is included
            Assert.AreEqual("New\nLayer5", qt.Words[5]); // When a new line is included
            Assert.AreEqual("New\r\nLayer6", qt.Words[6]); // If \r\n is included
            Assert.AreEqual(0, qt.Set.Count);
            Assert.AreEqual("", qt.Where_FullnameRegex);
            Assert.AreEqual("", qt.Where_Tag);
            Assert.AreEqual("", qt.From_FullnameRegex);
            Assert.AreEqual("", qt.From_Tag);
            Assert.AreEqual("", qt.To_FullnameRegex);
            Assert.AreEqual("", qt.To_Tag);
            Assert.AreEqual("", qt.The);
        }
        #endregion

        #region N75 syntax parser pharse
        /// <summary>
        /// WORDS phrase
        /// </summary>
        [Test]
        public void N70_SyntaxPhrase_AfterWords()
        {
            string phrase = @"WhiteCat ""White Dog""";
            List<string> words = new List<string>();
            int caret = 0;
            bool successful = SyntaxP.ParsePhrase_AfterWords(phrase, ref caret, QueryTokens.WHERE, words);

            Assert.IsTrue(successful);
            Assert.AreEqual(2, words.Count);
            Assert.AreEqual("WhiteCat", words[0]);
            Assert.AreEqual("White Dog", words[1]);
        }

        /// <summary>
        /// SET phrase
        /// </summary>
        [Test]
        public void N70_SyntaxPhrase_AfterSet()
        {
            string phrase = @"Duration 0 ExitTime 1";
            Dictionary<string, string> properties = new Dictionary<string, string>();
            int caret = 0;
            bool successful = SyntaxP.ParsePhrase_AfterSet(phrase, ref caret, QueryTokens.WHERE, properties);

            Assert.IsTrue(successful);
            Assert.AreEqual(2, properties.Count);
            Assert.IsTrue(properties.ContainsKey("Duration"));
            Assert.AreEqual("0", properties["Duration"]);
            Assert.IsTrue(properties.ContainsKey("ExitTime"));
            Assert.AreEqual("1", properties["ExitTime"]);
        }

        /// <summary>
        /// SET phrase
        /// </summary>
        [Test]
        public void N70_SyntaxPhrase_AfterSet_Stringliteral()
        {
            string phrase = @"tag ""hello""";
            Dictionary<string, string> properties = new Dictionary<string, string>();
            int caret = 0;
            bool successful = SyntaxP.ParsePhrase_AfterSet(phrase, ref caret, QueryTokens.WHERE, properties);

            Assert.IsTrue(successful);
            Assert.AreEqual(1, properties.Count);
            Assert.IsTrue(properties.ContainsKey("tag"));
            Assert.AreEqual("hello", properties["tag"]);
        }
        #endregion

        #region N80 lexical parser
        /// <summary>
        /// TAG部を解析
        /// </summary>
        [Test]
        public void N80_Lexical_TagParentesis_StringToTokens()
        {
            string attrParentesis = "([(Alpaca Bear)(Cat Dog)]{Elephant})";
            List<string> tokens;
            SyntaxPOther.String_to_tokens(attrParentesis, out tokens);

            Assert.AreEqual(15, tokens.Count);
            Assert.AreEqual("(", tokens[0]);
            Assert.AreEqual("[", tokens[1]);
            Assert.AreEqual("(", tokens[2]);
            Assert.AreEqual("Alpaca", tokens[3]);
            Assert.AreEqual("Bear", tokens[4]);
            Assert.AreEqual(")", tokens[5]);
            Assert.AreEqual("(", tokens[6]);
            Assert.AreEqual("Cat", tokens[7]);
            Assert.AreEqual("Dog", tokens[8]);
            Assert.AreEqual(")", tokens[9]);
            Assert.AreEqual("]", tokens[10]);
            Assert.AreEqual("{", tokens[11]);
            Assert.AreEqual("Elephant", tokens[12]);
            Assert.AreEqual("}", tokens[13]);
            Assert.AreEqual(")", tokens[14]);
        }

        /// <summary>
        /// TAG
        /// </summary>
        [Test]
        public void N80_Lexical_VarParentesis()
        {
            string query = "( ( Dash ) [ Punch Kick ] ) ";
            int caret = 0;
            string parenthesis;

            bool successful = LexcalP.VarParentesis(query, ref caret, out parenthesis);

            //ebug.Log("parenthesis="+ parenthesis);
            Assert.IsTrue(successful, parenthesis);
            Assert.AreEqual("( ( Dash ) [ Punch Kick ] )", parenthesis);
        }

        /// <summary>
        /// Parser new line test
        /// </summary>
        [Test]
        public void N80_Lexical_Newline_WindowsCrLf()
        {
            int caret = 0;
            bool hit;
            string parenthesis;

            caret = 0;
            hit = LexcalP.VarSpaces(@"
a", ref caret);
            Assert.IsTrue(hit);
            //Assert.IsTrue(0<caret && caret < 3, "caret=["+caret+"]"); // new line is 1 or 2 characters?
            Assert.AreEqual(2, caret, "Maybe, No problem. caret=[" + caret + "] (If windows, It's 2. CR LF)"); // new line is 2 characters?

            caret = 0;
            hit = LexcalP.VarParentesis(@"(alpaca bear)
", ref caret, out parenthesis);
            Assert.IsTrue(hit);
            //Assert.IsTrue(13 < caret && caret < 13 + 3, "caret=[" + caret + "]"); // new line is 1 or 2 characters?
            Assert.AreEqual(13 + 2, caret); // new line is 2 characters?
            Assert.AreEqual("(alpaca bear)", parenthesis);
        }

        [Test]
        public void N80_Lexical_VarStringliteral()
        {
            int caret = 0;
            const int DOUBLE_QUOT = 2;
            bool hit;
            string stringWithoutDoubleQuotation;

            caret = 0;
            hit = LexcalP.VarStringliteral(@"""alpaca"" ", ref caret, out stringWithoutDoubleQuotation);
            Assert.IsTrue(hit);
            Assert.AreEqual(8 + 1, caret);
            Assert.AreEqual("alpaca", stringWithoutDoubleQuotation);

            caret = 0;
            hit = LexcalP.VarStringliteral(@"""alpaca""", ref caret, out stringWithoutDoubleQuotation);
            Assert.IsTrue(hit);
            Assert.AreEqual(8, caret);
            Assert.AreEqual("alpaca", stringWithoutDoubleQuotation);

            caret = 0;
            hit = LexcalP.VarStringliteral(@"""alpaca.bear""", ref caret, out stringWithoutDoubleQuotation);
            Assert.IsTrue(hit);
            Assert.AreEqual(11+ DOUBLE_QUOT, caret);
            Assert.AreEqual("alpaca.bear", stringWithoutDoubleQuotation);

            caret = 0;
            hit = LexcalP.VarStringliteral(@"""alpaca\.bear""", ref caret, out stringWithoutDoubleQuotation);
            Assert.IsTrue(hit);
            Assert.AreEqual(12+ DOUBLE_QUOT, caret);
            Assert.AreEqual(@"alpaca\.bear", stringWithoutDoubleQuotation);

            caret = 0;
            hit = LexcalP.VarStringliteral(@"""Base Layer\.Any State""", ref caret, out stringWithoutDoubleQuotation);
            Assert.IsTrue(hit);
            Assert.AreEqual(21 + DOUBLE_QUOT, caret);
            Assert.AreEqual(@"Base Layer\.Any State", stringWithoutDoubleQuotation);
        }

        /// <summary>
        /// sub functions.
        /// </summary>
        [Test]
        public void N80_Lexical_SubFunctions()
        {
            int caret = 0;
            bool hit;
            string word;
            string parenthesis;

            caret = 0;
            hit = LexcalP.VarSpaces("  a", ref caret);
            Assert.IsTrue(hit);
            Assert.AreEqual(2, caret);

            caret = 1;
            hit = LexcalP.VarSpaces("  a", ref caret);
            Assert.IsTrue(hit);
            Assert.AreEqual(2, caret);

            caret = 0;
            hit = LexcalP.VarSpaces("a  ", ref caret);
            Assert.IsFalse(hit);
            Assert.AreEqual(0, caret);

            caret = 0;
            hit = LexcalP.FixedWord("alpaca", "alpaca bear cat ", ref caret);
            Assert.IsTrue(hit);
            Assert.AreEqual(6 + 1, caret);
            hit = LexcalP.FixedWord("bear", "alpaca bear cat ", ref caret);
            Assert.IsTrue(hit, "caret=" + caret);
            Assert.AreEqual(11 + 1, caret);

            caret = 0;
            hit = LexcalP.FixedWord("alpaca", "alpaca  ", ref caret);
            Assert.IsTrue(hit);
            Assert.AreEqual(6 + 2, caret);

            caret = 0;
            hit = LexcalP.FixedWord("alpaca", "alpaca", ref caret);
            Assert.IsTrue(hit);
            Assert.AreEqual(6, caret);

            caret = 0;
            hit = LexcalP.FixedWord("alpaca", "alpxxx ", ref caret);
            Assert.IsFalse(hit);
            Assert.AreEqual(0, caret);

            caret = 0;
            hit = LexcalP.VarWord("alpaca ", ref caret, out word);
            Assert.IsTrue(hit);
            Assert.AreEqual(6 + 1, caret);
            Assert.AreEqual("alpaca", word);

            caret = 0;
            hit = LexcalP.VarWord(" alpaca", ref caret, out word);
            Assert.IsFalse(hit);
            Assert.AreEqual(0, caret);
            Assert.AreEqual("", word);

            caret = 0;
            hit = LexcalP.VarWord(@" alpaca ", ref caret, out word);
            Assert.IsFalse(hit);
            Assert.AreEqual(0, caret);
            Assert.AreEqual("", word);

            caret = 0;
            hit = LexcalP.VarParentesis(@"( alpaca ) ", ref caret, out parenthesis);
            Assert.IsTrue(hit);
            Assert.AreEqual(10 + 1, caret);
            Assert.AreEqual("( alpaca )", parenthesis);

            caret = 0;
            hit = LexcalP.VarWord(@"( alpaca ", ref caret, out parenthesis);
            Assert.IsFalse(hit);
            Assert.AreEqual(0, caret);
            Assert.AreEqual("", parenthesis);

            caret = 0;
            hit = LexcalP.VarParentesis(@"(alpaca) ", ref caret, out parenthesis);
            Assert.IsTrue(hit);
            Assert.AreEqual(8 + 1, caret);
            Assert.AreEqual("(alpaca)", parenthesis);

            caret = 0;
            hit = LexcalP.VarParentesis(@"(alpaca bear) ", ref caret, out parenthesis);
            Assert.IsTrue(hit);
            Assert.AreEqual(13 + 1, caret);
            Assert.AreEqual("(alpaca bear)", parenthesis);

            caret = 0;
            hit = LexcalP.VarParentesis(@"(alpaca bear)", ref caret, out parenthesis);
            Assert.IsTrue(hit);
            Assert.AreEqual(13, caret);
            Assert.AreEqual("(alpaca bear)", parenthesis);
        }
        #endregion

        #region miscellaneous

        [Test]
        public void N90_Misc_Csv()
        {
            string csv = @"alpaca,""bear"",""ca""""t"",""dog"" ,elephant , fox, giraffe , ""horse"", ""Iguana"" ,""Jelly, fish"",";
            List<string> cells = CsvParser.CsvLine_to_cellList(csv);

            Assert.AreEqual(10, cells.Count);
            Assert.AreEqual("alpaca", cells[0]);
            Assert.AreEqual("bear", cells[1]);
            Assert.AreEqual(@"ca""t", cells[2]);
            Assert.AreEqual("dog", cells[3]);
            Assert.AreEqual("elephant ", cells[4]); // The space remains as it is
            Assert.AreEqual(" fox", cells[5]);
            Assert.AreEqual(" giraffe ", cells[6]);
            Assert.AreEqual("horse", cells[7]); // Spaces before and after the double quote are deleted.
            Assert.AreEqual("Iguana", cells[8]);
            Assert.AreEqual("Jelly, fish", cells[9]);
        }
        #endregion
    }
}