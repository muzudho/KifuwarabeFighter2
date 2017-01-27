using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using StellaQL;
using System.Collections.Generic;
using System;
using System.Text;

public class NewEditorTest {

    static NewEditorTest()
    {
        NewEditorTest.InstanceDatabase = new AstateDatabase();
    }

    /// <summary>
    /// Animator の State に一対一対応☆
    /// </summary>
    public enum AstateIndex
    {
        Alpaca,
        Bear,
        Cat,
        Dog,
        Elephant,
        Fox,
        Giraffe,
        Horse,
        Iguana,
        Jellyfish,
        Kangaroo,
        Lion,
        Monkey,
        Nutria,
        Ox,
        Pig,
        Quetzal,
        Rabbit,
        Sheep,
        Tiger,
        Unicorn,
        Vixen,
        Wolf,
        Xenopus,
        Yak,
        Zebra,
        Num,
    }

    /// <summary>
    /// アニメーターのステート
    /// </summary>
    public class AstateRecord : AbstractAstateRecord
    {
        //public AstateDatabase.Attr attribute;

        public AstateRecord(string breadCrumb, string name, AstateDatabase.Attr attribute)
        {
            this.BreadCrumb = breadCrumb;
            this.Name = name;
            this.AttributeEnum = (int)attribute;
        }

        public override bool HasFlag_attr(int enumration)
        {
            return ((AstateDatabase.Attr)this.AttributeEnum).HasFlag((AstateDatabase.Attr)enumration);
        }
    }

    static AstateDatabase InstanceDatabase;
    public class AstateDatabase : AbstractAstateDatabase
    {
        [Flags]
        public enum Attr
        {
            Zero = 0, // (0) 最初の要素は 0 であることが必要。あとで計算に使う。
            Alpha = 1, // (1)
            Beta = 1 << 1, // (2)
            Cee = 1 << 2, // (4)
            Dee = 1 << 3, // (8)
            Eee = 1 << 4, // (16)
        }

        public AstateDatabase()
        {
            index_to_record = new Dictionary<int, AstateRecordable>()//AstateIndex
            {
                {(int)AstateIndex.Alpaca, new AstateRecord(  "Base Layer.", "Alpaca", Attr.Alpha | Attr.Cee)},// ([(Alpha Cee)(Beta)]{Eee})(1)
                {(int)AstateIndex.Bear, new AstateRecord(  "Base Layer.", "Bear", Attr.Alpha | Attr.Beta | Attr.Eee)},//AE(1) AE,B,E(1)
                {(int)AstateIndex.Cat, new AstateRecord(  "Base Layer.", "Cat", Attr.Alpha | Attr.Cee)},// ([(Alpha Cee)(Beta)]{Eee})(2)
                {(int)AstateIndex.Dog, new AstateRecord(  "Base Layer.", "Dog", Attr.Dee)},
                {(int)AstateIndex.Elephant, new AstateRecord(  "Base Layer.", "Elephant", Attr.Alpha | Attr.Eee)},//AE(2) AE,B,E(2) Nn(1)
                {(int)AstateIndex.Fox, new AstateRecord(  "Base Layer.", "Fox", Attr.Zero)},
                {(int)AstateIndex.Giraffe, new AstateRecord(  "Base Layer.", "Giraffe", Attr.Alpha | Attr.Eee)},//AE(3) AE,B,E(3)
                {(int)AstateIndex.Horse, new AstateRecord(  "Base Layer.", "Horse", Attr.Eee)},// AE,B,E(4)
                {(int)AstateIndex.Iguana, new AstateRecord(  "Base Layer.", "Iguana", Attr.Alpha)},// Nn(2)
                {(int)AstateIndex.Jellyfish, new AstateRecord(  "Base Layer.", "Jellyfish", Attr.Eee)},// AE,B,E(5)
                {(int)AstateIndex.Kangaroo, new AstateRecord(  "Base Layer.", "Kangaroo", Attr.Alpha)},// Nn(3)
                {(int)AstateIndex.Lion, new AstateRecord(  "Base Layer.", "Lion", Attr.Zero)},// Nn(4)
                {(int)AstateIndex.Monkey, new AstateRecord(  "Base Layer.", "Monkey", Attr.Eee)},// AE,B,E(6) Nn(5)
                {(int)AstateIndex.Nutria, new AstateRecord(  "Base Layer.", "Nutria", Attr.Alpha)},// Nn(6)
                {(int)AstateIndex.Ox, new AstateRecord(  "Base Layer.", "Ox", Attr.Zero)},
                {(int)AstateIndex.Pig, new AstateRecord(  "Base Layer.", "Pig", Attr.Zero)},
                {(int)AstateIndex.Quetzal, new AstateRecord(  "Base Layer.", "Quetzal", Attr.Alpha | Attr.Eee)},//AE(4) AE,B,E(7)
                {(int)AstateIndex.Rabbit, new AstateRecord(  "Base Layer.", "Rabbit", Attr.Alpha | Attr.Beta)},// ([(Alpha Cee)(Beta)]{Eee})(3)  AE,B,E(8)
                {(int)AstateIndex.Sheep, new AstateRecord(  "Base Layer.", "Sheep", Attr.Eee)},// AE,B,E(9)
                {(int)AstateIndex.Tiger, new AstateRecord(  "Base Layer.", "Tiger", Attr.Eee)},// AE,B,E(10)
                {(int)AstateIndex.Unicorn, new AstateRecord(  "Base Layer.", "Unicorn", Attr.Cee)},// Nn(7)
                {(int)AstateIndex.Vixen, new AstateRecord(  "Base Layer.", "Vixen", Attr.Eee)},// AE,B,E(11) Nn(8)
                {(int)AstateIndex.Wolf, new AstateRecord(  "Base Layer.", "Wolf", Attr.Zero)},
                {(int)AstateIndex.Xenopus, new AstateRecord(  "Base Layer.", "Xenopus", Attr.Eee)},// AE,B,E(12) Nn(9)
                {(int)AstateIndex.Yak, new AstateRecord(  "Base Layer.", "Yak", Attr.Alpha)},
                {(int)AstateIndex.Zebra, new AstateRecord(  "Base Layer.", "Zebra", Attr.Alpha | Attr.Beta | Attr.Eee)},//AE(5) AE,B,E(13)
            };
        }
    }


    [Test]
	public void EditorTest() {

        string expression = "( [ ( Alpha Cee ) ( Beta ) ] { Eee } )";

        List<int> result = Util_StellaQL.Execute(expression, typeof(AstateIndex));

		Assert.AreEqual(result.Count, 3);
        if (3==result.Count)
        {
            Assert.AreEqual(result[0], (int)AstateIndex.Alpaca);
            Assert.AreEqual(result[1], (int)AstateIndex.Cat);
            Assert.AreEqual(result[2], (int)AstateIndex.Rabbit);
        }
    }

    /// <summary>
    /// 属性検索部を解析
    /// </summary>
    [Test]
    public void Parse_AttrParentesis_TokensToLockers()
    {
        List<string> tokens = new List<string>() {
            "(","[","(","Alpaca","Bear",")","(","Cat","Dog",")","]","{","Elephant","}",")",
        };
        List<List<string>> lockers;
        AttrParenthesisParser.Tokens_to_lockers(tokens, out lockers);

        Assert.AreEqual(5, lockers.Count);
        { int i = 0; foreach (string token in lockers[0]) { Debug.Log("[0][" + i + "]=" + token); i++; } }
        Assert.AreEqual(2, lockers[0].Count);
        { int i = 0; foreach (string token in lockers[1]) { Debug.Log("[1][" + i + "]=" + token); i++; } }
        Assert.AreEqual(2, lockers[1].Count);
        { int i = 0; foreach (string token in lockers[2]) { Debug.Log("[2][" + i + "]=" + token); i++; } }
        Assert.AreEqual(2, lockers[2].Count);
        { int i = 0; foreach (string token in lockers[3]) { Debug.Log("[3][" + i + "]=" + token); i++; } }
        Assert.AreEqual(1, lockers[3].Count);
        { int i = 0; foreach (string token in lockers[4]) { Debug.Log("[4][" + i + "]=" + token); i++; } }
        Assert.AreEqual(2, lockers[4].Count);
        Assert.AreEqual("Bear", lockers[0][0]);
        Assert.AreEqual("Alpaca", lockers[0][1]);
        Assert.AreEqual("Dog", lockers[1][0]);
        Assert.AreEqual("Cat", lockers[1][1]);
        Assert.AreEqual("1", lockers[2][0]);
        Assert.AreEqual("0", lockers[2][1]);
        Assert.AreEqual("Elephant", lockers[3][0]);
        Assert.AreEqual("3", lockers[4][0]);
        Assert.AreEqual("2", lockers[4][1]);
    }

    /// <summary>
    /// 属性検索部を解析
    /// </summary>
    [Test]
    public void Parse_AttrParentesis_StringToTokens()
    {
        string attrParentesis = "([(Alpaca Bear)(Cat Dog)]{Elephant})";
        List<string> tokens;
        AttrParenthesisParser.String_to_tokens(attrParentesis, out tokens);

        //{ int i = 0; foreach (string token in tokens) { Debug.Log("Token[" + i + "]: " + token); i++; } }

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
    /// 構文解析 Insert 文
    /// </summary>
    [Test]
    public void Parse_InsertStatement()
    {
        string query = @"TRANSITION INSERT
                        SET Duration 0 ExitTime 1
                        FROM ""Base Layer.SMove""
                        TO ATTR (BusyX Block)";
        QueryTokens sq = StellaQLScanner.Parser_InsertStatement(query);

        Assert.AreEqual(QueryTokens.TRANSITION, sq.Target);
        Assert.AreEqual(QueryTokens.INSERT, sq.Manipulation);
        Assert.AreEqual(2, sq.Set.Count);
        Assert.IsTrue(sq.Set.ContainsKey("Duration"));
        Assert.AreEqual("0", sq.Set["Duration"]);
        Assert.IsTrue(sq.Set.ContainsKey("ExitTime"));
        Assert.AreEqual("1", sq.Set["ExitTime"]);
        Assert.AreEqual("Base Layer.SMove", sq.From_FullnameRegex);
        Assert.AreEqual("", sq.From_Attr);
        Assert.AreEqual("", sq.To_FullnameRegex);
        Assert.AreEqual("(BusyX Block)", sq.To_Attr);
    }

    /// <summary>
    /// 構文解析 Update 文
    /// </summary>
    [Test]
    public void Parse_UpdateStatement()
    {
        string query = @"TRANSITION UPDATE
                        SET Duration 0.25 ExitTime 0.75
                        FROM ""Base Layer.SMove""
                        TO ATTR (BusyX Block)";
        QueryTokens sq = StellaQLScanner.Parser_UpdateStatement(query);

        Assert.AreEqual(QueryTokens.TRANSITION, sq.Target);
        Assert.AreEqual(QueryTokens.UPDATE, sq.Manipulation);
        Assert.AreEqual(2, sq.Set.Count);
        Assert.IsTrue(sq.Set.ContainsKey("Duration"));
        Assert.AreEqual("0.25", sq.Set["Duration"]);
        Assert.IsTrue(sq.Set.ContainsKey("ExitTime"));
        Assert.AreEqual("0.75", sq.Set["ExitTime"]);
        Assert.AreEqual("Base Layer.SMove", sq.From_FullnameRegex);
        Assert.AreEqual("", sq.From_Attr);
        Assert.AreEqual("", sq.To_FullnameRegex);
        Assert.AreEqual("(BusyX Block)", sq.To_Attr);
    }

    /// <summary>
    /// 構文解析 Delete 文
    /// </summary>
    [Test]
    public void Parse_DeleteStatement()
    {
        string query = @"TRANSITION DELETE
                        FROM ""Base Layer.SMove""
                        TO ATTR (BusyX Block)";
        QueryTokens sq = StellaQLScanner.Parser_DeleteStatement(query);

        Assert.AreEqual(QueryTokens.TRANSITION, sq.Target);
        Assert.AreEqual(QueryTokens.DELETE, sq.Manipulation);
        Assert.AreEqual(0, sq.Set.Count);
        Assert.AreEqual("Base Layer.SMove", sq.From_FullnameRegex);
        Assert.AreEqual("", sq.From_Attr);
        Assert.AreEqual("", sq.To_FullnameRegex);
        Assert.AreEqual("(BusyX Block)", sq.To_Attr);
    }

    /// <summary>
    /// 構文解析 Select 文
    /// </summary>
    [Test]
    public void Parse_SelectStatement()
    {
        string query = @"TRANSITION SELECT
                        FROM ""Base Layer.SMove""
                        TO ATTR (BusyX Block)";
        QueryTokens sq = StellaQLScanner.Parser_SelectStatement(query);

        Assert.AreEqual(QueryTokens.TRANSITION, sq.Target);
        Assert.AreEqual(QueryTokens.SELECT, sq.Manipulation);
        Assert.AreEqual(0, sq.Set.Count);
        Assert.AreEqual("Base Layer.SMove", sq.From_FullnameRegex);
        Assert.AreEqual("", sq.From_Attr);
        Assert.AreEqual("", sq.To_FullnameRegex);
        Assert.AreEqual("(BusyX Block)", sq.To_Attr);
    }

    /// <summary>
    /// Parser 改行テスト
    /// </summary>
    [Test]
    public void Parse_Newline()
    {
        int caret = 0;
        bool hit;

        caret = 0;
        hit = StellaQLScanner.VarSpaces(@"
a", ref caret);
        Assert.IsTrue(hit);
        Assert.AreEqual(2, caret); // 改行は 2 ？ 改行の文字数は環境依存か☆？
    }

    /// <summary>
    /// Parse関連のサブ関数。
    /// </summary>
    [Test]
    public void Parse_SubFunctions()
    {
        int caret = 0;
        //StellaQLScanner.SkipSpace("  a",ref caret);
        //Assert.AreEqual(2, caret);

        bool hit;
        string word;
        string stringWithoutDoubleQuotation;
        string parenthesis;

        caret = 0;
        hit = StellaQLScanner.VarSpaces("  a", ref caret);
        Assert.IsTrue(hit);
        Assert.AreEqual(2, caret);

        caret = 1;
        hit = StellaQLScanner.VarSpaces("  a", ref caret);
        Assert.IsTrue(hit);
        Assert.AreEqual(2, caret);

        caret = 0;
        hit = StellaQLScanner.VarSpaces("a  ", ref caret);
        Assert.IsFalse(hit);
        Assert.AreEqual(0, caret);

        caret = 0;
        hit = StellaQLScanner.FixedWord("alpaca", "alpaca bear cat ", ref caret);
        Assert.IsTrue(hit);
        Assert.AreEqual(6+1, caret);
        hit = StellaQLScanner.FixedWord("bear", "alpaca bear cat ", ref caret);
        Assert.IsTrue(hit, "caret="+caret);
        Assert.AreEqual(11 + 1, caret);

        caret = 0;
        hit = StellaQLScanner.FixedWord("alpaca", "alpaca  ", ref caret);
        Assert.IsTrue(hit);
        Assert.AreEqual(6 + 2, caret);

        caret = 0;
        hit = StellaQLScanner.FixedWord("alpaca", "alpaca", ref caret);
        Assert.IsTrue(hit);
        Assert.AreEqual(6, caret);

        caret = 0;
        hit = StellaQLScanner.FixedWord("alpaca", "alpxxx ", ref caret);
        Assert.IsFalse(hit);
        Assert.AreEqual(0, caret);

        caret = 0;
        hit = StellaQLScanner.VarWord("alpaca ", ref caret, out word);
        Assert.IsTrue(hit);
        Assert.AreEqual(6+1, caret);
        Assert.AreEqual("alpaca", word);

        caret = 0;
        hit = StellaQLScanner.VarWord(" alpaca", ref caret, out word);
        Assert.IsFalse(hit);
        Assert.AreEqual(0, caret);
        Assert.AreEqual("", word);

        caret = 0;
        hit = StellaQLScanner.VarStringliteral(@"""alpaca"" ", ref caret, out stringWithoutDoubleQuotation);
        Assert.IsTrue(hit);
        Assert.AreEqual(8+1, caret);
        Assert.AreEqual("alpaca", stringWithoutDoubleQuotation);

        caret = 0;
        hit = StellaQLScanner.VarStringliteral(@"""alpaca""", ref caret, out stringWithoutDoubleQuotation);
        Assert.IsTrue(hit);
        Assert.AreEqual(8, caret);
        Assert.AreEqual("alpaca", stringWithoutDoubleQuotation);

        caret = 0;
        hit = StellaQLScanner.VarWord(@" alpaca ", ref caret, out stringWithoutDoubleQuotation);
        Assert.IsFalse(hit);
        Assert.AreEqual(0, caret);
        Assert.AreEqual("", stringWithoutDoubleQuotation);

        caret = 0;
        hit = StellaQLScanner.VarParentesis(@"( alpaca ) ", ref caret, out parenthesis);
        Assert.IsTrue(hit);
        Assert.AreEqual(10+1, caret);
        Assert.AreEqual("( alpaca )", parenthesis);

        caret = 0;
        hit = StellaQLScanner.VarWord(@"( alpaca ", ref caret, out parenthesis);
        Assert.IsFalse(hit);
        Assert.AreEqual(0, caret);
        Assert.AreEqual("", parenthesis);

        caret = 0;
        hit = StellaQLScanner.VarParentesis(@"(alpaca) ", ref caret, out parenthesis);
        Assert.IsTrue(hit);
        Assert.AreEqual(8 + 1, caret);
        Assert.AreEqual("(alpaca)", parenthesis);

        caret = 0;
        hit = StellaQLScanner.VarParentesis(@"(alpaca bear) ", ref caret, out parenthesis);
        Assert.IsTrue(hit);
        Assert.AreEqual(13 + 1, caret);
        Assert.AreEqual("(alpaca bear)", parenthesis);

        caret = 0;
        hit = StellaQLScanner.VarParentesis(@"(alpaca bear)", ref caret, out parenthesis);
        Assert.IsTrue(hit);
        Assert.AreEqual(13, caret);
        Assert.AreEqual("(alpaca bear)", parenthesis);
    }

    /// <summary>
    /// （　）要素フィルター
    /// </summary>
    [Test]
    public void Filtering_ElementsAnd()
    {
        // 条件は　「 Bear, Elephant 」 AND 「 Bear, Giraffe 」
        List<List<int>> lockers = new List<List<int>>()
        {
            new List<int>() { (int)AstateIndex.Bear, (int)AstateIndex.Elephant },
            new List<int>() { (int)AstateIndex.Bear, (int)AstateIndex.Giraffe },
        };
        Dictionary<int, AstateRecordable> recordIndexes = StellaQLAggregater.Filtering_ElementsAnd(lockers, InstanceDatabase.index_to_record);

        // 結果は　Bear
        Assert.AreEqual(1, recordIndexes.Count);
        if (1 == recordIndexes.Count)
        {
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Bear));
        }
    }

    /// <summary>
    /// [ ] 要素フィルター
    /// </summary>
    [Test]
    public void Filtering_ElementsOr()
    {
        // 条件は　「 Bear, Elephant 」 OR 「 Bear, Giraffe 」
        List<List<int>> lockers = new List<List<int>>()
        {
            new List<int>() { (int)AstateIndex.Bear, (int)AstateIndex.Elephant },
            new List<int>() { (int)AstateIndex.Bear, (int)AstateIndex.Giraffe },
        };
        Dictionary<int, AstateRecordable> recordIndexes = StellaQLAggregater.Filtering_OrElements(lockers, InstanceDatabase.index_to_record);

        // 結果は　Bear, Elephant, Giraffe
        Assert.AreEqual(3, recordIndexes.Count);
        if (3 == recordIndexes.Count)
        {
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Bear));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Elephant));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Giraffe));
        }
    }

    /// <summary>
    /// { } 要素フィルター
    /// </summary>
    [Test]
    public void Filtering_ElementsNotAndNot()
    {
        // 条件は　NOT「 Bear, Elephant 」 AND NOT「 Bear, Giraffe 」
        List<List<int>> lockers = new List<List<int>>()
        {
            new List<int>() { (int)AstateIndex.Bear, (int)AstateIndex.Elephant },
            new List<int>() { (int)AstateIndex.Bear, (int)AstateIndex.Giraffe },
        };
        Dictionary<int, AstateRecordable> recordIndexes = StellaQLAggregater.Filtering_ElementsNotAndNot(lockers, InstanceDatabase.index_to_record);

        // 結果は　Alpaca,Cat,Dog,Fox,Horse,Iguana,Jellyfish,Kangaroo,Lion,Monkey,Nutria,Ox,Pig,Quetzal,Rabbit,Sheep,Tiger,Unicorn,Vixen,Wolf,Xenopus,Yak,Zebra
        Assert.AreEqual(23, recordIndexes.Count);
        if (23 == recordIndexes.Count)
        {
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Alpaca));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Cat));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Dog));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Fox));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Horse));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Iguana));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Jellyfish));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Kangaroo));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Lion));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Monkey));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Nutria));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Ox));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Pig));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Quetzal));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Rabbit));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Sheep));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Tiger));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Unicorn));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Vixen));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Wolf));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Xenopus));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Yak));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Zebra));
        }
    }

    /// <summary>
    /// ステート名正規表現フィルター
    /// </summary>
    [Test]
    public void Filtering_StateFullNameRegex()
    {
        // 条件は、「Base Layer.」の下に、n または N が含まれるもの
        string pattern = @"Base Layer\.\w*[Nn]\w*";
        Dictionary<int, AstateRecordable> recordIndexes = StellaQLAggregater.Filtering_StateFullNameRegex(pattern, InstanceDatabase.index_to_record);

        // 結果は　Elephant、Iguana、Kangaroo、Lion、Monkey、Nutria、Unicorn、Vixen、Xenopus
        Assert.AreEqual(9, recordIndexes.Count);
        if (9 == recordIndexes.Count)
        {
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Elephant));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Iguana));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Kangaroo));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Lion));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Monkey));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Nutria));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Unicorn));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Vixen));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Xenopus));
        }
    }

    /// <summary>
    /// （　）属性フィルター
    /// </summary>
    [Test]
    public void Filtering_AttributesAnd()
    {
        // 条件は　Alpha | Eee
        List<int> attrs = new List<int>() { (int)AstateDatabase.Attr.Alpha | (int)AstateDatabase.Attr.Eee };
        Dictionary<int, AstateRecordable> recordIndexes = StellaQLAggregater.Filtering_AndAttributes(attrs, InstanceDatabase.index_to_record);

        // 結果は　Bear、Elephant、Giraffe、Quetzal、Zebra
        Assert.AreEqual(5, recordIndexes.Count);
        if (5 == recordIndexes.Count)
        {
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Bear));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Elephant));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Giraffe));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Quetzal));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Zebra));
        }
    }

    /// <summary>
    /// [　] 属性フィルター
    /// </summary>
    [Test]
    public void Filtering_AttributesOr()
    {
        // 条件は　（Alpha | Eee）、Beta、Eee
        List<int> attrs = new List<int>() { (int)AstateDatabase.Attr.Alpha | (int)AstateDatabase.Attr.Eee, (int)AstateDatabase.Attr.Beta, (int)AstateDatabase.Attr.Eee };
        Dictionary<int, AstateRecordable> recordIndexes = StellaQLAggregater.Filtering_OrAttributes(attrs, InstanceDatabase.index_to_record);

        // 結果は　Bear、Elephant、Giraffe、Horse、Jellyfish、Monkey、Quetzal、Rabbit、Sheep、Tiger、Vixen、Xenopus、Zebra
        Assert.AreEqual(13, recordIndexes.Count);
        if (13 == recordIndexes.Count)
        {
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Bear));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Elephant));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Giraffe));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Horse));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Jellyfish));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Monkey));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Quetzal));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Rabbit));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Sheep));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Tiger));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Vixen));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Xenopus));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Zebra));
        }
    }

    /// <summary>
    /// ｛　｝ 属性フィルター
    /// </summary>
    [Test]
    public void Filtering_AttributesNotAndNot()
    {
        // 条件は　（Alpha | Eee）、Beta、Eee
        List<int> attrs = new List<int>() { (int)AstateDatabase.Attr.Alpha | (int)AstateDatabase.Attr.Eee, (int)AstateDatabase.Attr.Beta, (int)AstateDatabase.Attr.Eee };
        Dictionary<int, AstateRecordable> recordIndexes = StellaQLAggregater.Filtering_NotAndNotAttributes(attrs, InstanceDatabase.index_to_record);

        // 結果は　Alpaca、Cat、Dog、Fox、Iguana、Kangaroo、Lion、Nutria、Ox、Pig、Unicorn、Wolf、Yak
        Assert.AreEqual(13, recordIndexes.Count);
        if (13 == recordIndexes.Count)
        {
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Alpaca));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Cat));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Dog));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Fox));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Iguana));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Kangaroo));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Lion));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Nutria));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Ox));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Pig));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Unicorn));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Wolf));
            Assert.IsTrue(recordIndexes.ContainsKey((int)AstateIndex.Yak));
        }
    }

    /// <summary>
    /// (　) 演算をテスト
    /// </summary>
    [Test]
    public void OperationKeyword()
    {
        List<int> set = new List<int>() { (int)AstateDatabase.Attr.Beta, (int)AstateDatabase.Attr.Dee };
        List<int> result = StellaQLAggregater.Keyword_to_locker(set, typeof(AstateDatabase.Attr));

        int i = 0;
        foreach (int attr in result) { Debug.Log("Attr[" + i + "]: " + (AstateDatabase.Attr)attr + " (" + attr + ")"); i++; }

        Assert.AreEqual(1, result.Count);
        if (1 == result.Count)
        {
            Assert.AreEqual((int)AstateDatabase.Attr.Beta | (int)AstateDatabase.Attr.Dee, result[0]);
        }
    }

    /// <summary>
    /// [　] 演算をテスト
    /// </summary>
    [Test]
    public void OperationKeywordList()
    {
        List<int> set = new List<int>() { (int)AstateDatabase.Attr.Beta, (int)AstateDatabase.Attr.Dee };
        List<int> result = StellaQLAggregater.KeywordList_to_locker(set, typeof(AstateDatabase.Attr));

        int i = 0;
        foreach (int attr in result) { Debug.Log("Attr[" + i + "]: " + (AstateDatabase.Attr)attr + " (" + attr + ")"); i++; }

        Assert.AreEqual(2, result.Count);
        if (2 == result.Count)
        {
            Assert.AreEqual((int)AstateDatabase.Attr.Beta, result[0]);
            Assert.AreEqual((int)AstateDatabase.Attr.Dee, result[1]);
        }
    }

    /// <summary>
    /// {　} 演算をテスト
    /// </summary>
    [Test]
    public void OperationNGKeywordList()
    {
        List<int> set = new List<int>() { (int)AstateDatabase.Attr.Beta, (int)AstateDatabase.Attr.Dee };
        List<int> result = StellaQLAggregater.NGKeywordList_to_locker(set, typeof(AstateDatabase.Attr));

        int i = 0;
        foreach (int attr in result) { Debug.Log("Attr[" + i + "]: " + (AstateDatabase.Attr)attr + " (" + attr + ")"); i++; }

        Assert.AreEqual(4, result.Count);
        if (4 == result.Count)
        {
            Assert.AreEqual((int)AstateDatabase.Attr.Zero, result[0]);
            Assert.AreEqual((int)AstateDatabase.Attr.Alpha, result[1]);
            Assert.AreEqual((int)AstateDatabase.Attr.Cee, result[2]);
            Assert.AreEqual((int)AstateDatabase.Attr.Eee, result[3]);
        }
    }

    /// <summary>
    /// 補集合を取れるかテスト
    /// </summary>
    [Test]
    public void GetComplement()
    {
        List<int> set = new List<int>() { (int)AstateDatabase.Attr.Beta, (int)AstateDatabase.Attr.Dee }; // int型にして持つ
        List<int> complement = StellaQLAggregater.Complement(set, typeof(AstateDatabase.Attr));

        int i = 0;
        foreach (int attr in complement)
        {
            Debug.Log("Attr[" + i + "]: " + (AstateDatabase.Attr)attr + " (" + attr + ")");
            i++;
        }

        Assert.AreEqual(4, complement.Count);
        if (4 == complement.Count)
        {
            Assert.AreEqual((int)AstateDatabase.Attr.Zero, complement[0]);
            Assert.AreEqual((int)AstateDatabase.Attr.Alpha, complement[1]);
            Assert.AreEqual((int)AstateDatabase.Attr.Cee, complement[2]);
            Assert.AreEqual((int)AstateDatabase.Attr.Eee, complement[3]);
        }
    }
}
