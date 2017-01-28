using NUnit.Framework;
using StellaQL;
using System;
using System.Collections.Generic;

public class StellaQLTest {

    static StellaQLTest()
    {
        StellaQLTest.InstanceTable = new StateExTable();
    }

    /// <summary>
    /// Animator の State に一対一対応☆
    /// </summary>
    public enum StateIndex
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
    public class StateExRecord : AbstractStateExRecord
    {
        public StateExRecord(string breadCrumb, string name, StateExTable.Attr attribute)
        {
            this.BreadCrumb = breadCrumb;
            this.Name = name;
            this.AttributeEnum = (int)attribute;
        }

        public override bool HasFlag_attr(int enumration)
        {
            return ((StateExTable.Attr)this.AttributeEnum).HasFlag((StateExTable.Attr)enumration);
        }
    }

    public static StateExTable InstanceTable;
    public class StateExTable : AbstractStateExTable
    {
        [Flags]
        public enum Attr
        {
            Zero = 0, // (0) 別に 0 は無くてもいい。
            Alpha = 1, // (1)
            Beta = 1 << 1, // (2)
            Cee = 1 << 2, // (4)
            Dee = 1 << 3, // (8)
            Eee = 1 << 4, // (16)
        }

        public StateExTable()
        {
            index_to_exRecord = new Dictionary<int, StateExRecordable>()//AstateIndex
            {
                {(int)StateIndex.Alpaca, new StateExRecord(  "Base Layer.StellaQL Practice.", "Alpaca", Attr.Alpha | Attr.Cee)},// {E}(1) AC(1) ([(A C)(B)]{E})(1)
                {(int)StateIndex.Bear, new StateExRecord(  "Base Layer.StellaQL Practice.", "Bear", Attr.Alpha | Attr.Beta | Attr.Eee)},// B(1) AE(1) AE,B,E(1)
                {(int)StateIndex.Cat, new StateExRecord(  "Base Layer.StellaQL Practice.", "Cat", Attr.Alpha | Attr.Cee)},// {E}(2) AC(2) ([(A C)(B)]{E})(2)
                {(int)StateIndex.Dog, new StateExRecord(  "Base Layer.StellaQL Practice.", "Dog", Attr.Dee)},// {E}(3)
                {(int)StateIndex.Elephant, new StateExRecord(  "Base Layer.StellaQL Practice.", "Elephant", Attr.Alpha | Attr.Eee)},//AE(2) AE,B,E(2) Nn(1)
                {(int)StateIndex.Fox, new StateExRecord(  "Base Layer.StellaQL Practice.", "Fox", Attr.Zero)},// {E}(4)
                {(int)StateIndex.Giraffe, new StateExRecord(  "Base Layer.StellaQL Practice.", "Giraffe", Attr.Alpha | Attr.Eee)},//AE(3) AE,B,E(3)
                {(int)StateIndex.Horse, new StateExRecord(  "Base Layer.StellaQL Practice.", "Horse", Attr.Eee)},// AE,B,E(4)
                {(int)StateIndex.Iguana, new StateExRecord(  "Base Layer.StellaQL Practice.", "Iguana", Attr.Alpha)},// {E}(5) Nn(2)
                {(int)StateIndex.Jellyfish, new StateExRecord(  "Base Layer.StellaQL Practice.", "Jellyfish", Attr.Eee)},// AE,B,E(5)
                {(int)StateIndex.Kangaroo, new StateExRecord(  "Base Layer.StellaQL Practice.", "Kangaroo", Attr.Alpha)},// {E}(6) Nn(3)
                {(int)StateIndex.Lion, new StateExRecord(  "Base Layer.StellaQL Practice.", "Lion", Attr.Zero)},// {E}(7) Nn(4)
                {(int)StateIndex.Monkey, new StateExRecord(  "Base Layer.StellaQL Practice.", "Monkey", Attr.Eee)},// AE,B,E(6) Nn(5)
                {(int)StateIndex.Nutria, new StateExRecord(  "Base Layer.StellaQL Practice.", "Nutria", Attr.Alpha)},// {E}(8) Nn(6)
                {(int)StateIndex.Ox, new StateExRecord(  "Base Layer.StellaQL Practice.", "Ox", Attr.Zero)},// {E}(9)
                {(int)StateIndex.Pig, new StateExRecord(  "Base Layer.StellaQL Practice.", "Pig", Attr.Zero)},// {E}(10)
                {(int)StateIndex.Quetzal, new StateExRecord(  "Base Layer.StellaQL Practice.", "Quetzal", Attr.Alpha | Attr.Eee)},//AE(4) AE,B,E(7)
                {(int)StateIndex.Rabbit, new StateExRecord(  "Base Layer.StellaQL Practice.", "Rabbit", Attr.Alpha | Attr.Beta)},// {E}(11) B(2) ([(A C)(B)]{E})(3)  AE,B,E(8)
                {(int)StateIndex.Sheep, new StateExRecord(  "Base Layer.StellaQL Practice.", "Sheep", Attr.Eee)},// AE,B,E(9)
                {(int)StateIndex.Tiger, new StateExRecord(  "Base Layer.StellaQL Practice.", "Tiger", Attr.Eee)},// AE,B,E(10)
                {(int)StateIndex.Unicorn, new StateExRecord(  "Base Layer.StellaQL Practice.", "Unicorn", Attr.Cee)},// {E}(12) Nn(7)
                {(int)StateIndex.Vixen, new StateExRecord(  "Base Layer.StellaQL Practice.", "Vixen", Attr.Eee)},// AE,B,E(11) Nn(8)
                {(int)StateIndex.Wolf, new StateExRecord(  "Base Layer.StellaQL Practice.", "Wolf", Attr.Zero)},// {E}(13)
                {(int)StateIndex.Xenopus, new StateExRecord(  "Base Layer.StellaQL Practice.", "Xenopus", Attr.Eee)},// AE,B,E(12) Nn(9)
                {(int)StateIndex.Yak, new StateExRecord(  "Base Layer.StellaQL Practice.", "Yak", Attr.Alpha)},// {E}(14)
                {(int)StateIndex.Zebra, new StateExRecord(  "Base Layer.StellaQL Practice.", "Zebra", Attr.Alpha | Attr.Beta | Attr.Eee)},// B(3) AE(5) AE,B,E(13)
            };
        }
    }

    #region N30 Query (文字列を与えて、レコード・インデックスを取ってくる)
    [Test]
    public void N30_Query_StateSelect()
    {
        string query = @"STATE SELECT
                        WHERE ATTR ([(Alpha Cee)(Beta)]{Eee})";
        HashSet<int> recordIndexes;
        bool successful = Querier.ExecuteStateSelect(query, typeof(StateExTable.Attr), InstanceTable.index_to_exRecord, out recordIndexes);

        Assert.AreEqual(3, recordIndexes.Count);
        Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Alpaca));
        Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Cat));
        Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Rabbit));
    }

    /// <summary>
    /// コメントと空行のテスト
    /// </summary>
    [Test]
    public void N30_Query_Comment()
    {
        string query = @"# コメントＡ

                        STATE SELECT

                        # コメントＢ

                        WHERE ATTR ([(Alpha Cee)(Beta)]{Eee})

                        # コメントＣ

                        ";
        // STATE SELECT文が動けば OK☆
        HashSet<int> recordIndexes;
        bool successful = Querier.ExecuteStateSelect(query, typeof(StateExTable.Attr), InstanceTable.index_to_exRecord, out recordIndexes);

        Assert.AreEqual(3, recordIndexes.Count);
        Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Alpaca));
        Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Cat));
        Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Rabbit));
    }

    [Test]
    public void N30_Query_TransitionSelect()
    {
        string query = @"TRANSITION SELECT
                        FROM ""Base Layer\.StellaQL Practice\.Zebra""
                        TO ATTR ([(Alpha Cee)(Beta)]{Eee})";
        HashSet<int> recordIndexesSrc;
        HashSet<int> recordIndexesDst;
        bool successful = Querier.ExecuteTransitionSelect(query, typeof(StateExTable.Attr), InstanceTable.index_to_exRecord, out recordIndexesSrc, out recordIndexesDst);

        Assert.AreEqual(1, recordIndexesSrc.Count);
        Assert.IsTrue(recordIndexesSrc.Contains((int)StateIndex.Zebra));
        Assert.AreEqual(3, recordIndexesDst.Count);
        Assert.IsTrue(recordIndexesDst.Contains((int)StateIndex.Alpaca));
        Assert.IsTrue(recordIndexesDst.Contains((int)StateIndex.Cat));
        Assert.IsTrue(recordIndexesDst.Contains((int)StateIndex.Rabbit));
    }
    #endregion

    #region N40 Fetch (レコード・インデックスを取ってくる)
    /// <summary>
    /// ロッカーを元に、レコード・インデックスを取得
    /// </summary>
    [Test]
    public void N40_Fetch_ByLockers()
    {
        List<List<string>> tokenLockers = new List<List<string>>(){ // 元は "([(Alpha Cee)(Beta)]{Eee})"
            new List<string>() { "Cee", "Alpha", },
            new List<string>() { "Beta", },
            new List<string>() { "1","0",},
            new List<string>() { "Eee", },
            new List<string>() { "3","2",},
        };
        List<string> tokenLockersOperation = new List<string>() { "(", "(", "[", "{", "(", };

        List<HashSet<int>> recordIndexesLockers;
        Fetcher.TokenLockers_to_recordIndexesLockers(
            tokenLockers, tokenLockersOperation, typeof(StateExTable.Attr), InstanceTable.index_to_exRecord, out recordIndexesLockers);

        Assert.AreEqual(5, recordIndexesLockers.Count);
        // { Debug.Log("recordIndexesLockers[0].Count=" + recordIndexesLockers[0].Count); int i = 0; foreach (int astateIndex in recordIndexesLockers[0]) { Debug.Log("[0][" + i + "] astateIndex=[" + ((AstateIndex)astateIndex).ToString() + "]"); i++; } }
        Assert.AreEqual(2, recordIndexesLockers[0].Count);
        Assert.IsTrue(recordIndexesLockers[0].Contains((int)StateIndex.Alpaca));
        Assert.IsTrue(recordIndexesLockers[0].Contains((int)StateIndex.Cat));
        // { Debug.Log("recordIndexesLockers[1].Count=" + recordIndexesLockers[1].Count); int i = 0; foreach (int astateIndex in recordIndexesLockers[1]) { Debug.Log("[1][" + i + "] astateIndex=[" + ((AstateIndex)astateIndex).ToString() + "]"); i++; } }
        Assert.AreEqual(3, recordIndexesLockers[1].Count);
        Assert.IsTrue(recordIndexesLockers[1].Contains((int)StateIndex.Bear));
        Assert.IsTrue(recordIndexesLockers[1].Contains((int)StateIndex.Rabbit));
        Assert.IsTrue(recordIndexesLockers[1].Contains((int)StateIndex.Zebra));
        // { Debug.Log("recordIndexesLockers[2].Count=" + recordIndexesLockers[2].Count); int i = 0; foreach (int astateIndex in recordIndexesLockers[2]) { Debug.Log("[2][" + i + "] astateIndex=[" + ((AstateIndex)astateIndex).ToString() + "]"); i++; } }
        Assert.AreEqual(5, recordIndexesLockers[2].Count);
        Assert.IsTrue(recordIndexesLockers[2].Contains((int)StateIndex.Alpaca));
        Assert.IsTrue(recordIndexesLockers[2].Contains((int)StateIndex.Cat));
        Assert.IsTrue(recordIndexesLockers[2].Contains((int)StateIndex.Bear));
        Assert.IsTrue(recordIndexesLockers[2].Contains((int)StateIndex.Rabbit));
        Assert.IsTrue(recordIndexesLockers[2].Contains((int)StateIndex.Zebra));
        // { Debug.Log("recordIndexesLockers[3].Count=" + recordIndexesLockers[3].Count); int i = 0; foreach (int astateIndex in recordIndexesLockers[3]) { Debug.Log("[3][" + i + "] astateIndex=[" + ((AstateIndex)astateIndex).ToString() + "]"); i++; } }
        Assert.AreEqual(14, recordIndexesLockers[3].Count);
        Assert.IsTrue(recordIndexesLockers[3].Contains((int)StateIndex.Alpaca));
        Assert.IsTrue(recordIndexesLockers[3].Contains((int)StateIndex.Cat));
        Assert.IsTrue(recordIndexesLockers[3].Contains((int)StateIndex.Dog));
        Assert.IsTrue(recordIndexesLockers[3].Contains((int)StateIndex.Fox));
        Assert.IsTrue(recordIndexesLockers[3].Contains((int)StateIndex.Iguana));
        Assert.IsTrue(recordIndexesLockers[3].Contains((int)StateIndex.Kangaroo));
        Assert.IsTrue(recordIndexesLockers[3].Contains((int)StateIndex.Lion));
        Assert.IsTrue(recordIndexesLockers[3].Contains((int)StateIndex.Nutria));
        Assert.IsTrue(recordIndexesLockers[3].Contains((int)StateIndex.Ox));
        Assert.IsTrue(recordIndexesLockers[3].Contains((int)StateIndex.Pig));
        Assert.IsTrue(recordIndexesLockers[3].Contains((int)StateIndex.Rabbit));
        Assert.IsTrue(recordIndexesLockers[3].Contains((int)StateIndex.Unicorn));
        Assert.IsTrue(recordIndexesLockers[3].Contains((int)StateIndex.Wolf));
        Assert.IsTrue(recordIndexesLockers[3].Contains((int)StateIndex.Yak));
        // { Debug.Log("recordIndexesLockers[4].Count=" + recordIndexesLockers[4].Count); int i = 0; foreach (int astateIndex in recordIndexesLockers[4]) { Debug.Log("[4][" + i + "] astateIndex=[" + ((AstateIndex)astateIndex).ToString() + "]"); i++; } }
        Assert.AreEqual(3, recordIndexesLockers[4].Count);
        Assert.IsTrue(recordIndexesLockers[4].Contains((int)StateIndex.Alpaca));
        Assert.IsTrue(recordIndexesLockers[4].Contains((int)StateIndex.Cat));
        Assert.IsTrue(recordIndexesLockers[4].Contains((int)StateIndex.Rabbit));
    }
    #endregion

    #region N50 element set (要素集合)
    /// <summary>
    /// （　）要素フィルター
    /// </summary>
    [Test]
    public void N50_RecordIndexes_FilteringElementsAnd()
    {
        // 条件は　「 Bear, Elephant 」 AND 「 Bear, Giraffe 」
        HashSet<int> lockerNumbers = new HashSet<int>() { 0, 1 };
        List<HashSet<int>> reordIndexLockers = new List<HashSet<int>>()
        {
            new HashSet<int>() { (int)StateIndex.Bear, (int)StateIndex.Elephant },
            new HashSet<int>() { (int)StateIndex.Bear, (int)StateIndex.Giraffe },
        };
        HashSet<int> recordIndexes = ElementSet.RecordIndexes_FilteringElementsAnd(lockerNumbers, reordIndexLockers);

        // 結果は　Bear
        Assert.AreEqual(1, recordIndexes.Count);
        if (1 == recordIndexes.Count)
        {
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Bear));
        }
    }

    /// <summary>
    /// [ ] 要素フィルター
    /// </summary>
    [Test]
    public void N50_RecordIndexes_FilteringElementsOr()
    {
        // 条件は　「 Bear, Elephant 」 OR 「 Bear, Giraffe 」
        HashSet<int> lockerNumbers = new HashSet<int>() { 0, 1 };
        List<HashSet<int>> recordIndexeslockers = new List<HashSet<int>>()
        {
            new HashSet<int>() { (int)StateIndex.Bear, (int)StateIndex.Elephant },
            new HashSet<int>() { (int)StateIndex.Bear, (int)StateIndex.Giraffe },
        };
        HashSet<int> recordIndexes = ElementSet.RecordIndexes_FilteringElementsOr(lockerNumbers, recordIndexeslockers);

        // 結果は　Bear, Elephant, Giraffe
        Assert.AreEqual(3, recordIndexes.Count);
        if (3 == recordIndexes.Count)
        {
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Bear));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Elephant));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Giraffe));
        }
    }

    /// <summary>
    /// { } 要素フィルター
    /// </summary>
    [Test]
    public void N50_RecordIndexes_FilteringElementsNotAndNot()
    {
        // 条件は　NOT「 Bear, Elephant 」 AND NOT「 Bear, Giraffe 」
        HashSet<int> lockerNumbers = new HashSet<int>() { 0, 1 };
        List<HashSet<int>> recordIndexesLockers = new List<HashSet<int>>()
        {
            new HashSet<int>() { (int)StateIndex.Bear, (int)StateIndex.Elephant },
            new HashSet<int>() { (int)StateIndex.Bear, (int)StateIndex.Giraffe },
        };
        HashSet<int> recordIndexes = ElementSet.RecordIndexes_FilteringElementsNotAndNot(lockerNumbers, recordIndexesLockers, InstanceTable.index_to_exRecord);

        // 結果は　Alpaca,Cat,Dog,Fox,Horse,Iguana,Jellyfish,Kangaroo,Lion,Monkey,Nutria,Ox,Pig,Quetzal,Rabbit,Sheep,Tiger,Unicorn,Vixen,Wolf,Xenopus,Yak,Zebra
        Assert.AreEqual(23, recordIndexes.Count);
        if (23 == recordIndexes.Count)
        {
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Alpaca));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Cat));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Dog));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Fox));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Horse));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Iguana));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Jellyfish));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Kangaroo));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Lion));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Monkey));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Nutria));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Ox));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Pig));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Quetzal));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Rabbit));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Sheep));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Tiger));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Unicorn));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Vixen));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Wolf));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Xenopus));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Yak));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Zebra));
        }
    }

    /// <summary>
    /// ステート名正規表現フィルター
    /// </summary>
    [Test]
    public void N50_RecordIndexes_FilteringStateFullNameRegex()
    {
        // 条件は、「Base Layer.」の下に、n または N が含まれるもの
        string pattern = @"Base Layer\.StellaQL Practice\.\w*[Nn]\w*";
        HashSet<int> recordIndexes = ElementSet.RecordIndexes_FilteringStateFullNameRegex(pattern, InstanceTable.index_to_exRecord);

        // 結果は　Elephant、Iguana、Kangaroo、Lion、Monkey、Nutria、Unicorn、Vixen、Xenopus
        Assert.AreEqual(9, recordIndexes.Count);
        if (9 == recordIndexes.Count)
        {
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Elephant));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Iguana));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Kangaroo));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Lion));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Monkey));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Nutria));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Unicorn));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Vixen));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Xenopus));
        }
    }

    /// <summary>
    /// （　）属性フィルター
    /// </summary>
    [Test]
    public void N50_RecordIndexes_FilteringAttributesAnd()
    {
        // 条件は　Alpha | Eee
        HashSet<int> attrs = new HashSet<int>() { (int)StateExTable.Attr.Alpha | (int)StateExTable.Attr.Eee };
        HashSet<int> recordIndexes = ElementSet.RecordIndexes_FilteringAttributesAnd(attrs, InstanceTable.index_to_exRecord);

        // 結果は　Bear、Elephant、Giraffe、Quetzal、Zebra
        Assert.AreEqual(5, recordIndexes.Count);
        if (5 == recordIndexes.Count)
        {
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Bear));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Elephant));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Giraffe));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Quetzal));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Zebra));
        }
    }

    /// <summary>
    /// [　] 属性フィルター
    /// </summary>
    [Test]
    public void N50_RecordIndexes_FilteringAttributesOr()
    {
        // 条件は　（Alpha | Eee）、Beta、Eee
        HashSet<int> attrs = new HashSet<int>() { (int)StateExTable.Attr.Alpha | (int)StateExTable.Attr.Eee, (int)StateExTable.Attr.Beta, (int)StateExTable.Attr.Eee };
        HashSet<int> recordIndexes = ElementSet.RecordIndexes_FilteringAttributesOr(attrs, InstanceTable.index_to_exRecord);

        // 結果は　Bear、Elephant、Giraffe、Horse、Jellyfish、Monkey、Quetzal、Rabbit、Sheep、Tiger、Vixen、Xenopus、Zebra
        Assert.AreEqual(13, recordIndexes.Count);
        if (13 == recordIndexes.Count)
        {
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Bear));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Elephant));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Giraffe));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Horse));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Jellyfish));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Monkey));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Quetzal));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Rabbit));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Sheep));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Tiger));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Vixen));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Xenopus));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Zebra));
        }
    }

    /// <summary>
    /// ｛　｝ 属性フィルター
    /// </summary>
    [Test]
    public void N50_RecordIndexes_FilteringAttributesNotAndNot()
    {
        // 条件は　（Alpha | Eee）、Beta、Eee
        HashSet<int> attrs = new HashSet<int>() { (int)StateExTable.Attr.Alpha | (int)StateExTable.Attr.Eee, (int)StateExTable.Attr.Beta, (int)StateExTable.Attr.Eee };
        HashSet<int> recordIndexes = ElementSet.RecordIndexes_FilteringAttributesNotAndNot(attrs, InstanceTable.index_to_exRecord);

        // 結果は　Alpaca、Cat、Dog、Fox、Iguana、Kangaroo、Lion、Nutria、Ox、Pig、Unicorn、Wolf、Yak
        Assert.AreEqual(13, recordIndexes.Count);
        if (13 == recordIndexes.Count)
        {
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Alpaca));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Cat));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Dog));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Fox));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Iguana));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Kangaroo));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Lion));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Nutria));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Ox));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Pig));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Unicorn));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Wolf));
            Assert.IsTrue(recordIndexes.Contains((int)StateIndex.Yak));
        }
    }
    #endregion

    #region N60 attribute set (属性集合)
    /// <summary>
    /// (　) で属性集合
    /// </summary>
    [Test]
    public void N60_ToAttrLocker_FromKeywordSet()
    {
        HashSet<int> set = new HashSet<int>() { (int)StateExTable.Attr.Beta, (int)StateExTable.Attr.Dee };
        HashSet<int> attrLocker = AttrSet.KeywordSet_to_attrLocker(set);//, typeof(AstateDatabase.Attr)

        //{ int i = 0; foreach (int attr in attrLocker) { Debug.Log("Attr[" + i + "]: " + (AstateDatabase.Attr)attr + " (" + attr + ")"); i++; } }

        Assert.AreEqual(1, attrLocker.Count);
        if (1 == attrLocker.Count)
        {
            Assert.IsTrue( attrLocker.Contains((int)StateExTable.Attr.Beta | (int)StateExTable.Attr.Dee));
        }
    }

    /// <summary>
    /// [　] で属性集合
    /// </summary>
    [Test]
    public void N60_ToAttrLocker_FromKeywordlistSet()
    {
        HashSet<int> set = new HashSet<int>() { (int)StateExTable.Attr.Beta, (int)StateExTable.Attr.Dee };
        HashSet<int> attrLocker = AttrSet.KeywordlistSet_to_attrLocker(set);

        // { int i = 0; foreach (int attr in attrLocker) { Debug.Log("Attr[" + i + "]: " + (AstateDatabase.Attr)attr + " (" + attr + ")"); i++; } }

        Assert.AreEqual(2, attrLocker.Count);
        if (2 == attrLocker.Count)
        {
            Assert.IsTrue( attrLocker.Contains((int)StateExTable.Attr.Beta));
            Assert.IsTrue( attrLocker.Contains((int)StateExTable.Attr.Dee));
        }
    }

    /// <summary>
    /// {　} で属性集合
    /// </summary>
    [Test]
    public void N60_ToAttrLocker_FromNGKeywordSet()
    {
        HashSet<int> set = new HashSet<int>() { (int)StateExTable.Attr.Beta, (int)StateExTable.Attr.Dee };
        HashSet<int> attrLocker = AttrSet.NGKeywordSet_to_attrLocker(set, typeof(StateExTable.Attr));

        // { int i = 0; foreach (int attr in attrLocker) { Debug.Log("Attr[" + i + "]: " + (AstateDatabase.Attr)attr + " (" + attr + ")"); i++; } }

        Assert.AreEqual(4, attrLocker.Count);
        if (4 == attrLocker.Count)
        {
            Assert.IsTrue( attrLocker.Contains((int)StateExTable.Attr.Zero));
            Assert.IsTrue( attrLocker.Contains((int)StateExTable.Attr.Alpha));
            Assert.IsTrue( attrLocker.Contains((int)StateExTable.Attr.Cee));
            Assert.IsTrue( attrLocker.Contains((int)StateExTable.Attr.Eee));
        }
    }

    /// <summary>
    /// 補集合を取れるかテスト
    /// </summary>
    [Test]
    public void N60_ToAttrLocker_GetComplement()
    {
        HashSet<int> set = new HashSet<int>() { (int)StateExTable.Attr.Beta, (int)StateExTable.Attr.Dee }; // int型にして持つ
        HashSet<int> attrLocker = AttrSet.Complement(set, typeof(StateExTable.Attr));

        // { int i = 0; foreach (int attr in attrLocker) { Debug.Log("Attr[" + i + "]: " + (AstateDatabase.Attr)attr + " (" + attr + ")"); i++; } }

        Assert.AreEqual(4, attrLocker.Count);
        if (4 == attrLocker.Count)
        {
            Assert.IsTrue( attrLocker.Contains((int)StateExTable.Attr.Zero));
            Assert.IsTrue( attrLocker.Contains((int)StateExTable.Attr.Alpha));
            Assert.IsTrue( attrLocker.Contains((int)StateExTable.Attr.Cee));
            Assert.IsTrue( attrLocker.Contains((int)StateExTable.Attr.Eee));
        }
    }
    #endregion

    #region N65 data builder (データ・ビルダー)
    /// <summary>
    /// ATTR部を解析（２）
    /// </summary>
    [Test]
    public void N65_Parse_AttrParentesis_TokensToLockers()
    {
        List<string> tokens = new List<string>() { "(", "[", "(", "Alpaca", "Bear", ")", "(", "Cat", "Dog", ")", "]", "{", "Elephant", "}", ")", };
        List<List<string>> tokenLockers;
        List<string> tokenLockersOperation;
        Querier.Tokens_to_lockers(tokens, out tokenLockers, out tokenLockersOperation);

        Assert.AreEqual(5, tokenLockers.Count);
        Assert.AreEqual(5, tokenLockersOperation.Count);
        //{ int i = 0; foreach (string token in lockers[0]) { Debug.Log("[0][" + i + "]=" + token); i++; } }
        Assert.AreEqual(2, tokenLockers[0].Count);
        //{ int i = 0; foreach (string token in lockers[1]) { Debug.Log("[1][" + i + "]=" + token); i++; } }
        Assert.AreEqual(2, tokenLockers[1].Count);
        //{ int i = 0; foreach (string token in lockers[2]) { Debug.Log("[2][" + i + "]=" + token); i++; } }
        Assert.AreEqual(2, tokenLockers[2].Count);
        //{ int i = 0; foreach (string token in lockers[3]) { Debug.Log("[3][" + i + "]=" + token); i++; } }
        Assert.AreEqual(1, tokenLockers[3].Count);
        //{ int i = 0; foreach (string token in lockers[4]) { Debug.Log("[4][" + i + "]=" + token); i++; } }
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

    #region N70 syntax parser (構文パーサー)
    /// <summary>
    /// 構文解析 SET 句
    /// </summary>
    [Test]
    public void N70_ParsePhrase_AfterSet()
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
    /// 構文解析 SET 句
    /// </summary>
    [Test]
    public void N70_ParsePhrase_AfterSet_Stringliteral()
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

    /// <summary>
    /// 構文解析 STATE SELECT 文
    /// </summary>
    [Test]
    public void N70_ParseStatement_StateSelect()
    {
        string query = @"STATE SELECT
                        WHERE ATTR ([(Alpha Cee)(Beta)]{Eee})";
        QueryTokens sq;
        bool successful = SyntaxP.ParseStatement_StateSelect(query, out sq);

        Assert.IsTrue(successful);
        Assert.AreEqual(QueryTokens.STATE, sq.Target);
        Assert.AreEqual(QueryTokens.SELECT, sq.Manipulation);
        Assert.AreEqual(0, sq.Set.Count);
        Assert.AreEqual("", sq.From_FullnameRegex);
        Assert.AreEqual("", sq.From_Attr);
        Assert.AreEqual("", sq.To_FullnameRegex);
        Assert.AreEqual("", sq.To_Attr);
        Assert.AreEqual("", sq.Where_FullnameRegex);
        Assert.AreEqual("([(Alpha Cee)(Beta)]{Eee})", sq.Where_Attr);
    }

    /// <summary>
    /// 構文解析 Transition Insert 文
    /// </summary>
    [Test]
    public void N70_ParseStatement_TransitionInsert()
    {
        string query = @"TRANSITION INSERT
                        SET Duration 0 ExitTime 1
                        FROM ""Base Layer.SMove""
                        TO ATTR (BusyX Block)";
        QueryTokens sq;
        bool successful = SyntaxP.ParseStatement_TransitionInsert(query, out sq);

        Assert.IsTrue(successful);
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
    /// 構文解析 Transition Update 文
    /// </summary>
    [Test]
    public void N70_ParseStatement_TransitionUpdate()
    {
        string query = @"TRANSITION UPDATE
                        SET Duration 0.25 ExitTime 0.75
                        FROM ""Base Layer.SMove""
                        TO ATTR (BusyX Block)";
        QueryTokens sq;
        bool successful = SyntaxP.ParseStatement_TransitionUpdate(query, out sq);

        Assert.IsTrue(successful);
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
    /// 構文解析 Transition Delete 文
    /// </summary>
    [Test]
    public void N70_ParseStatement_TransitionDelete()
    {
        string query = @"TRANSITION DELETE
                        FROM ""Base Layer.SMove""
                        TO ATTR (BusyX Block)";
        QueryTokens sq;
        bool successful = SyntaxP.ParseStatement_TransitionDelete(query, out sq);

        Assert.IsTrue(successful);
        Assert.AreEqual(QueryTokens.TRANSITION, sq.Target);
        Assert.AreEqual(QueryTokens.DELETE, sq.Manipulation);
        Assert.AreEqual(0, sq.Set.Count);
        Assert.AreEqual("Base Layer.SMove", sq.From_FullnameRegex);
        Assert.AreEqual("", sq.From_Attr);
        Assert.AreEqual("", sq.To_FullnameRegex);
        Assert.AreEqual("(BusyX Block)", sq.To_Attr);
    }

    /// <summary>
    /// 構文解析 Transition Select 文
    /// </summary>
    [Test]
    public void N70_ParseStatement_TransitionSelect()
    {
        string query = @"TRANSITION SELECT
                        FROM ""Base Layer.SMove""
                        TO ATTR (BusyX Block)";
        QueryTokens sq;
        bool successful = SyntaxP.ParseStatement_TransitionSelect(query, out sq);

        Assert.IsTrue(successful);
        Assert.AreEqual(QueryTokens.TRANSITION, sq.Target);
        Assert.AreEqual(QueryTokens.SELECT, sq.Manipulation);
        Assert.AreEqual(0, sq.Set.Count);
        Assert.AreEqual("Base Layer.SMove", sq.From_FullnameRegex);
        Assert.AreEqual("", sq.From_Attr);
        Assert.AreEqual("", sq.To_FullnameRegex);
        Assert.AreEqual("(BusyX Block)", sq.To_Attr);
    }
    #endregion

    #region N80 lexical parser (字句パーサー)

    /// <summary>
    /// ATTR部を解析（１）
    /// </summary>
    [Test]
    public void N80_Parse_AttrParentesis_StringToTokens()
    {
        string attrParentesis = "([(Alpaca Bear)(Cat Dog)]{Elephant})";
        List<string> tokens;
        QueryTokens.String_to_tokens(attrParentesis, out tokens);

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
    /// Parser 改行テスト
    /// </summary>
    [Test]
    public void N80_Parse_Newline()
    {
        int caret = 0;
        bool hit;
        string parenthesis;

        caret = 0;
        hit = LexcalP.VarSpaces(@"
a", ref caret);
        Assert.IsTrue(hit);
        Assert.AreEqual(2, caret); // 改行は 2 ？ 改行の文字数は環境依存か☆？

        caret = 0;
        hit = LexcalP.VarParentesis(@"(alpaca bear)
", ref caret, out parenthesis);
        Assert.IsTrue(hit);
        Assert.AreEqual(13 + 2, caret); // 改行は 2
        Assert.AreEqual("(alpaca bear)", parenthesis);
    }

    /// <summary>
    /// Parse関連のサブ関数。
    /// </summary>
    [Test]
    public void N80_Parse_SubFunctions()
    {
        int caret = 0;
        //StellaQLScanner.SkipSpace("  a",ref caret);
        //Assert.AreEqual(2, caret);

        bool hit;
        string word;
        string stringWithoutDoubleQuotation;
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
        hit = LexcalP.VarWord(@" alpaca ", ref caret, out stringWithoutDoubleQuotation);
        Assert.IsFalse(hit);
        Assert.AreEqual(0, caret);
        Assert.AreEqual("", stringWithoutDoubleQuotation);

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
}
