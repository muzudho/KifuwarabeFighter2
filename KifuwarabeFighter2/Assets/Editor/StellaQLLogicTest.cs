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
                {(int)AstateIndex.Alpaca, new AstateRecord(  "Base Layer.", "Alpaca", Attr.Alpha | Attr.Cee)},// {E}(1) AC(1) ([(A C)(B)]{E})(1)
                {(int)AstateIndex.Bear, new AstateRecord(  "Base Layer.", "Bear", Attr.Alpha | Attr.Beta | Attr.Eee)},// B(1) AE(1) AE,B,E(1)
                {(int)AstateIndex.Cat, new AstateRecord(  "Base Layer.", "Cat", Attr.Alpha | Attr.Cee)},// {E}(2) AC(2) ([(A C)(B)]{E})(2)
                {(int)AstateIndex.Dog, new AstateRecord(  "Base Layer.", "Dog", Attr.Dee)},// {E}(3)
                {(int)AstateIndex.Elephant, new AstateRecord(  "Base Layer.", "Elephant", Attr.Alpha | Attr.Eee)},//AE(2) AE,B,E(2) Nn(1)
                {(int)AstateIndex.Fox, new AstateRecord(  "Base Layer.", "Fox", Attr.Zero)},// {E}(4)
                {(int)AstateIndex.Giraffe, new AstateRecord(  "Base Layer.", "Giraffe", Attr.Alpha | Attr.Eee)},//AE(3) AE,B,E(3)
                {(int)AstateIndex.Horse, new AstateRecord(  "Base Layer.", "Horse", Attr.Eee)},// AE,B,E(4)
                {(int)AstateIndex.Iguana, new AstateRecord(  "Base Layer.", "Iguana", Attr.Alpha)},// {E}(5) Nn(2)
                {(int)AstateIndex.Jellyfish, new AstateRecord(  "Base Layer.", "Jellyfish", Attr.Eee)},// AE,B,E(5)
                {(int)AstateIndex.Kangaroo, new AstateRecord(  "Base Layer.", "Kangaroo", Attr.Alpha)},// {E}(6) Nn(3)
                {(int)AstateIndex.Lion, new AstateRecord(  "Base Layer.", "Lion", Attr.Zero)},// {E}(7) Nn(4)
                {(int)AstateIndex.Monkey, new AstateRecord(  "Base Layer.", "Monkey", Attr.Eee)},// AE,B,E(6) Nn(5)
                {(int)AstateIndex.Nutria, new AstateRecord(  "Base Layer.", "Nutria", Attr.Alpha)},// {E}(8) Nn(6)
                {(int)AstateIndex.Ox, new AstateRecord(  "Base Layer.", "Ox", Attr.Zero)},// {E}(9)
                {(int)AstateIndex.Pig, new AstateRecord(  "Base Layer.", "Pig", Attr.Zero)},// {E}(10)
                {(int)AstateIndex.Quetzal, new AstateRecord(  "Base Layer.", "Quetzal", Attr.Alpha | Attr.Eee)},//AE(4) AE,B,E(7)
                {(int)AstateIndex.Rabbit, new AstateRecord(  "Base Layer.", "Rabbit", Attr.Alpha | Attr.Beta)},// {E}(11) B(2) ([(A C)(B)]{E})(3)  AE,B,E(8)
                {(int)AstateIndex.Sheep, new AstateRecord(  "Base Layer.", "Sheep", Attr.Eee)},// AE,B,E(9)
                {(int)AstateIndex.Tiger, new AstateRecord(  "Base Layer.", "Tiger", Attr.Eee)},// AE,B,E(10)
                {(int)AstateIndex.Unicorn, new AstateRecord(  "Base Layer.", "Unicorn", Attr.Cee)},// {E}(12) Nn(7)
                {(int)AstateIndex.Vixen, new AstateRecord(  "Base Layer.", "Vixen", Attr.Eee)},// AE,B,E(11) Nn(8)
                {(int)AstateIndex.Wolf, new AstateRecord(  "Base Layer.", "Wolf", Attr.Zero)},// {E}(13)
                {(int)AstateIndex.Xenopus, new AstateRecord(  "Base Layer.", "Xenopus", Attr.Eee)},// AE,B,E(12) Nn(9)
                {(int)AstateIndex.Yak, new AstateRecord(  "Base Layer.", "Yak", Attr.Alpha)},// {E}(14)
                {(int)AstateIndex.Zebra, new AstateRecord(  "Base Layer.", "Zebra", Attr.Alpha | Attr.Beta | Attr.Eee)},// B(3) AE(5) AE,B,E(13)
            };
        }
    }

    #region Query (文字列を与えて、レコード・インデックスを取ってくる)
    [Test]
    public void Query_StateSelect()
    {
        string query = @"STATE SELECT
                        WHERE ATTR ([(Alpha Cee)(Beta)]{Eee})";
        HashSet<int> recordIndexes;
        bool successful = Querier.ExecuteStateSelect(query, typeof(AstateDatabase.Attr), InstanceDatabase.index_to_record, out recordIndexes);

        Assert.AreEqual(3, recordIndexes.Count);
        Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Alpaca));
        Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Cat));
        Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Rabbit));
    }
    #endregion

    #region Fetch (レコード・インデックスを取ってくる)
    /// <summary>
    /// ロッカーを元に、レコード・インデックスを取得
    /// </summary>
    [Test]
    public void Fetch_ByLockers()
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
            tokenLockers, tokenLockersOperation, typeof(AstateDatabase.Attr), InstanceDatabase.index_to_record, out recordIndexesLockers);

        Assert.AreEqual(5, recordIndexesLockers.Count);
        // { Debug.Log("recordIndexesLockers[0].Count=" + recordIndexesLockers[0].Count); int i = 0; foreach (int astateIndex in recordIndexesLockers[0]) { Debug.Log("[0][" + i + "] astateIndex=[" + ((AstateIndex)astateIndex).ToString() + "]"); i++; } }
        Assert.AreEqual(2, recordIndexesLockers[0].Count);
        Assert.IsTrue(recordIndexesLockers[0].Contains((int)AstateIndex.Alpaca));
        Assert.IsTrue(recordIndexesLockers[0].Contains((int)AstateIndex.Cat));
        // { Debug.Log("recordIndexesLockers[1].Count=" + recordIndexesLockers[1].Count); int i = 0; foreach (int astateIndex in recordIndexesLockers[1]) { Debug.Log("[1][" + i + "] astateIndex=[" + ((AstateIndex)astateIndex).ToString() + "]"); i++; } }
        Assert.AreEqual(3, recordIndexesLockers[1].Count);
        Assert.IsTrue(recordIndexesLockers[1].Contains((int)AstateIndex.Bear));
        Assert.IsTrue(recordIndexesLockers[1].Contains((int)AstateIndex.Rabbit));
        Assert.IsTrue(recordIndexesLockers[1].Contains((int)AstateIndex.Zebra));
        // { Debug.Log("recordIndexesLockers[2].Count=" + recordIndexesLockers[2].Count); int i = 0; foreach (int astateIndex in recordIndexesLockers[2]) { Debug.Log("[2][" + i + "] astateIndex=[" + ((AstateIndex)astateIndex).ToString() + "]"); i++; } }
        Assert.AreEqual(5, recordIndexesLockers[2].Count);
        Assert.IsTrue(recordIndexesLockers[2].Contains((int)AstateIndex.Alpaca));
        Assert.IsTrue(recordIndexesLockers[2].Contains((int)AstateIndex.Cat));
        Assert.IsTrue(recordIndexesLockers[2].Contains((int)AstateIndex.Bear));
        Assert.IsTrue(recordIndexesLockers[2].Contains((int)AstateIndex.Rabbit));
        Assert.IsTrue(recordIndexesLockers[2].Contains((int)AstateIndex.Zebra));
        // { Debug.Log("recordIndexesLockers[3].Count=" + recordIndexesLockers[3].Count); int i = 0; foreach (int astateIndex in recordIndexesLockers[3]) { Debug.Log("[3][" + i + "] astateIndex=[" + ((AstateIndex)astateIndex).ToString() + "]"); i++; } }
        Assert.AreEqual(14, recordIndexesLockers[3].Count);
        Assert.IsTrue(recordIndexesLockers[3].Contains((int)AstateIndex.Alpaca));
        Assert.IsTrue(recordIndexesLockers[3].Contains((int)AstateIndex.Cat));
        Assert.IsTrue(recordIndexesLockers[3].Contains((int)AstateIndex.Dog));
        Assert.IsTrue(recordIndexesLockers[3].Contains((int)AstateIndex.Fox));
        Assert.IsTrue(recordIndexesLockers[3].Contains((int)AstateIndex.Iguana));
        Assert.IsTrue(recordIndexesLockers[3].Contains((int)AstateIndex.Kangaroo));
        Assert.IsTrue(recordIndexesLockers[3].Contains((int)AstateIndex.Lion));
        Assert.IsTrue(recordIndexesLockers[3].Contains((int)AstateIndex.Nutria));
        Assert.IsTrue(recordIndexesLockers[3].Contains((int)AstateIndex.Ox));
        Assert.IsTrue(recordIndexesLockers[3].Contains((int)AstateIndex.Pig));
        Assert.IsTrue(recordIndexesLockers[3].Contains((int)AstateIndex.Rabbit));
        Assert.IsTrue(recordIndexesLockers[3].Contains((int)AstateIndex.Unicorn));
        Assert.IsTrue(recordIndexesLockers[3].Contains((int)AstateIndex.Wolf));
        Assert.IsTrue(recordIndexesLockers[3].Contains((int)AstateIndex.Yak));
        // { Debug.Log("recordIndexesLockers[4].Count=" + recordIndexesLockers[4].Count); int i = 0; foreach (int astateIndex in recordIndexesLockers[4]) { Debug.Log("[4][" + i + "] astateIndex=[" + ((AstateIndex)astateIndex).ToString() + "]"); i++; } }
        Assert.AreEqual(3, recordIndexesLockers[4].Count);
        Assert.IsTrue(recordIndexesLockers[4].Contains((int)AstateIndex.Alpaca));
        Assert.IsTrue(recordIndexesLockers[4].Contains((int)AstateIndex.Cat));
        Assert.IsTrue(recordIndexesLockers[4].Contains((int)AstateIndex.Rabbit));
    }
    #endregion

    #region element set (要素集合)
    /// <summary>
    /// （　）要素フィルター
    /// </summary>
    [Test]
    public void RecordIndexes_FilteringElementsAnd()
    {
        // 条件は　「 Bear, Elephant 」 AND 「 Bear, Giraffe 」
        HashSet<int> lockerNumbers = new HashSet<int>() { 0, 1 };
        List<HashSet<int>> reordIndexLockers = new List<HashSet<int>>()
        {
            new HashSet<int>() { (int)AstateIndex.Bear, (int)AstateIndex.Elephant },
            new HashSet<int>() { (int)AstateIndex.Bear, (int)AstateIndex.Giraffe },
        };
        HashSet<int> recordIndexes = ElementSet.RecordIndexes_FilteringElementsAnd(lockerNumbers, reordIndexLockers);

        // 結果は　Bear
        Assert.AreEqual(1, recordIndexes.Count);
        if (1 == recordIndexes.Count)
        {
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Bear));
        }
    }

    /// <summary>
    /// [ ] 要素フィルター
    /// </summary>
    [Test]
    public void RecordIndexes_FilteringElementsOr()
    {
        // 条件は　「 Bear, Elephant 」 OR 「 Bear, Giraffe 」
        HashSet<int> lockerNumbers = new HashSet<int>() { 0, 1 };
        List<HashSet<int>> recordIndexeslockers = new List<HashSet<int>>()
        {
            new HashSet<int>() { (int)AstateIndex.Bear, (int)AstateIndex.Elephant },
            new HashSet<int>() { (int)AstateIndex.Bear, (int)AstateIndex.Giraffe },
        };
        HashSet<int> recordIndexes = ElementSet.RecordIndexes_FilteringElementsOr(lockerNumbers, recordIndexeslockers);

        // 結果は　Bear, Elephant, Giraffe
        Assert.AreEqual(3, recordIndexes.Count);
        if (3 == recordIndexes.Count)
        {
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Bear));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Elephant));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Giraffe));
        }
    }

    /// <summary>
    /// { } 要素フィルター
    /// </summary>
    [Test]
    public void RecordIndexes_FilteringElementsNotAndNot()
    {
        // 条件は　NOT「 Bear, Elephant 」 AND NOT「 Bear, Giraffe 」
        HashSet<int> lockerNumbers = new HashSet<int>() { 0, 1 };
        List<HashSet<int>> recordIndexesLockers = new List<HashSet<int>>()
        {
            new HashSet<int>() { (int)AstateIndex.Bear, (int)AstateIndex.Elephant },
            new HashSet<int>() { (int)AstateIndex.Bear, (int)AstateIndex.Giraffe },
        };
        HashSet<int> recordIndexes = ElementSet.RecordIndexes_FilteringElementsNotAndNot(lockerNumbers, recordIndexesLockers, InstanceDatabase.index_to_record);

        // 結果は　Alpaca,Cat,Dog,Fox,Horse,Iguana,Jellyfish,Kangaroo,Lion,Monkey,Nutria,Ox,Pig,Quetzal,Rabbit,Sheep,Tiger,Unicorn,Vixen,Wolf,Xenopus,Yak,Zebra
        Assert.AreEqual(23, recordIndexes.Count);
        if (23 == recordIndexes.Count)
        {
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Alpaca));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Cat));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Dog));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Fox));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Horse));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Iguana));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Jellyfish));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Kangaroo));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Lion));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Monkey));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Nutria));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Ox));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Pig));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Quetzal));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Rabbit));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Sheep));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Tiger));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Unicorn));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Vixen));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Wolf));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Xenopus));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Yak));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Zebra));
        }
    }

    /// <summary>
    /// ステート名正規表現フィルター
    /// </summary>
    [Test]
    public void RecordIndexes_FilteringStateFullNameRegex()
    {
        // 条件は、「Base Layer.」の下に、n または N が含まれるもの
        string pattern = @"Base Layer\.\w*[Nn]\w*";
        List<int> recordIndexes = ElementSet.RecordIndexes_FilteringStateFullNameRegex(pattern, InstanceDatabase.index_to_record);

        // 結果は　Elephant、Iguana、Kangaroo、Lion、Monkey、Nutria、Unicorn、Vixen、Xenopus
        Assert.AreEqual(9, recordIndexes.Count);
        if (9 == recordIndexes.Count)
        {
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Elephant));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Iguana));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Kangaroo));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Lion));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Monkey));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Nutria));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Unicorn));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Vixen));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Xenopus));
        }
    }

    /// <summary>
    /// （　）属性フィルター
    /// </summary>
    [Test]
    public void RecordIndexes_FilteringAttributesAnd()
    {
        // 条件は　Alpha | Eee
        HashSet<int> attrs = new HashSet<int>() { (int)AstateDatabase.Attr.Alpha | (int)AstateDatabase.Attr.Eee };
        HashSet<int> recordIndexes = ElementSet.RecordIndexes_FilteringAttributesAnd(attrs, InstanceDatabase.index_to_record);

        // 結果は　Bear、Elephant、Giraffe、Quetzal、Zebra
        Assert.AreEqual(5, recordIndexes.Count);
        if (5 == recordIndexes.Count)
        {
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Bear));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Elephant));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Giraffe));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Quetzal));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Zebra));
        }
    }

    /// <summary>
    /// [　] 属性フィルター
    /// </summary>
    [Test]
    public void RecordIndexes_FilteringAttributesOr()
    {
        // 条件は　（Alpha | Eee）、Beta、Eee
        HashSet<int> attrs = new HashSet<int>() { (int)AstateDatabase.Attr.Alpha | (int)AstateDatabase.Attr.Eee, (int)AstateDatabase.Attr.Beta, (int)AstateDatabase.Attr.Eee };
        HashSet<int> recordIndexes = ElementSet.RecordIndexes_FilteringAttributesOr(attrs, InstanceDatabase.index_to_record);

        // 結果は　Bear、Elephant、Giraffe、Horse、Jellyfish、Monkey、Quetzal、Rabbit、Sheep、Tiger、Vixen、Xenopus、Zebra
        Assert.AreEqual(13, recordIndexes.Count);
        if (13 == recordIndexes.Count)
        {
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Bear));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Elephant));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Giraffe));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Horse));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Jellyfish));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Monkey));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Quetzal));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Rabbit));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Sheep));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Tiger));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Vixen));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Xenopus));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Zebra));
        }
    }

    /// <summary>
    /// ｛　｝ 属性フィルター
    /// </summary>
    [Test]
    public void RecordIndexes_FilteringAttributesNotAndNot()
    {
        // 条件は　（Alpha | Eee）、Beta、Eee
        HashSet<int> attrs = new HashSet<int>() { (int)AstateDatabase.Attr.Alpha | (int)AstateDatabase.Attr.Eee, (int)AstateDatabase.Attr.Beta, (int)AstateDatabase.Attr.Eee };
        HashSet<int> recordIndexes = ElementSet.RecordIndexes_FilteringAttributesNotAndNot(attrs, InstanceDatabase.index_to_record);

        // 結果は　Alpaca、Cat、Dog、Fox、Iguana、Kangaroo、Lion、Nutria、Ox、Pig、Unicorn、Wolf、Yak
        Assert.AreEqual(13, recordIndexes.Count);
        if (13 == recordIndexes.Count)
        {
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Alpaca));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Cat));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Dog));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Fox));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Iguana));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Kangaroo));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Lion));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Nutria));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Ox));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Pig));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Unicorn));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Wolf));
            Assert.IsTrue(recordIndexes.Contains((int)AstateIndex.Yak));
        }
    }
    #endregion

    #region attribute set (属性集合)
    /// <summary>
    /// (　) で属性集合
    /// </summary>
    [Test]
    public void ToAttrLocker_FromKeywordSet()
    {
        HashSet<int> set = new HashSet<int>() { (int)AstateDatabase.Attr.Beta, (int)AstateDatabase.Attr.Dee };
        HashSet<int> attrLocker = AttrSet.KeywordSet_to_attrLocker(set);//, typeof(AstateDatabase.Attr)

        //{ int i = 0; foreach (int attr in attrLocker) { Debug.Log("Attr[" + i + "]: " + (AstateDatabase.Attr)attr + " (" + attr + ")"); i++; } }

        Assert.AreEqual(1, attrLocker.Count);
        if (1 == attrLocker.Count)
        {
            Assert.IsTrue( attrLocker.Contains((int)AstateDatabase.Attr.Beta | (int)AstateDatabase.Attr.Dee));
        }
    }

    /// <summary>
    /// [　] で属性集合
    /// </summary>
    [Test]
    public void ToAttrLocker_FromKeywordlistSet()
    {
        HashSet<int> set = new HashSet<int>() { (int)AstateDatabase.Attr.Beta, (int)AstateDatabase.Attr.Dee };
        HashSet<int> attrLocker = AttrSet.KeywordlistSet_to_attrLocker(set);

        // { int i = 0; foreach (int attr in attrLocker) { Debug.Log("Attr[" + i + "]: " + (AstateDatabase.Attr)attr + " (" + attr + ")"); i++; } }

        Assert.AreEqual(2, attrLocker.Count);
        if (2 == attrLocker.Count)
        {
            Assert.IsTrue( attrLocker.Contains((int)AstateDatabase.Attr.Beta));
            Assert.IsTrue( attrLocker.Contains((int)AstateDatabase.Attr.Dee));
        }
    }

    /// <summary>
    /// {　} で属性集合
    /// </summary>
    [Test]
    public void ToAttrLocker_FromNGKeywordSet()
    {
        HashSet<int> set = new HashSet<int>() { (int)AstateDatabase.Attr.Beta, (int)AstateDatabase.Attr.Dee };
        HashSet<int> attrLocker = AttrSet.NGKeywordSet_to_attrLocker(set, typeof(AstateDatabase.Attr));

        // { int i = 0; foreach (int attr in attrLocker) { Debug.Log("Attr[" + i + "]: " + (AstateDatabase.Attr)attr + " (" + attr + ")"); i++; } }

        Assert.AreEqual(4, attrLocker.Count);
        if (4 == attrLocker.Count)
        {
            Assert.IsTrue( attrLocker.Contains((int)AstateDatabase.Attr.Zero));
            Assert.IsTrue( attrLocker.Contains((int)AstateDatabase.Attr.Alpha));
            Assert.IsTrue( attrLocker.Contains((int)AstateDatabase.Attr.Cee));
            Assert.IsTrue( attrLocker.Contains((int)AstateDatabase.Attr.Eee));
        }
    }

    /// <summary>
    /// 補集合を取れるかテスト
    /// </summary>
    [Test]
    public void ToAttrLocker_GetComplement()
    {
        HashSet<int> set = new HashSet<int>() { (int)AstateDatabase.Attr.Beta, (int)AstateDatabase.Attr.Dee }; // int型にして持つ
        HashSet<int> attrLocker = AttrSet.Complement(set, typeof(AstateDatabase.Attr));

        // { int i = 0; foreach (int attr in attrLocker) { Debug.Log("Attr[" + i + "]: " + (AstateDatabase.Attr)attr + " (" + attr + ")"); i++; } }

        Assert.AreEqual(4, attrLocker.Count);
        if (4 == attrLocker.Count)
        {
            Assert.IsTrue( attrLocker.Contains((int)AstateDatabase.Attr.Zero));
            Assert.IsTrue( attrLocker.Contains((int)AstateDatabase.Attr.Alpha));
            Assert.IsTrue( attrLocker.Contains((int)AstateDatabase.Attr.Cee));
            Assert.IsTrue( attrLocker.Contains((int)AstateDatabase.Attr.Eee));
        }
    }
    #endregion

    #region syntax parser (構文パーサー)
    /// <summary>
    /// 構文解析 STATE SELECT 文
    /// </summary>
    [Test]
    public void ParseStatement_StateSelect()
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
    public void ParseStatement_TransitionInsert()
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
    public void ParseStatement_TransitionUpdate()
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
    public void ParseStatement_TransitionDelete()
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
    public void ParseStatement_TransitionSelect()
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

    #region lexical parser (字句パーサー)
    /// <summary>
    /// ATTR部を解析（２）
    /// </summary>
    [Test]
    public void Parse_AttrParentesis_TokensToLockers()
    {
        List<string> tokens = new List<string>() { "(", "[", "(", "Alpaca", "Bear", ")", "(", "Cat", "Dog", ")", "]", "{", "Elephant", "}", ")", };
        List<List<string>> tokenLockers;
        List<string> tokenLockersOperation;
        Util_AttrParenthesisParser.Tokens_to_lockers(tokens, out tokenLockers, out tokenLockersOperation);

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

    /// <summary>
    /// ATTR部を解析（１）
    /// </summary>
    [Test]
    public void Parse_AttrParentesis_StringToTokens()
    {
        string attrParentesis = "([(Alpaca Bear)(Cat Dog)]{Elephant})";
        List<string> tokens;
        Util_AttrParenthesisParser.String_to_tokens(attrParentesis, out tokens);

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
    public void Parse_Newline()
    {
        int caret = 0;
        bool hit;

        caret = 0;
        hit = LexcalP.VarSpaces(@"
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
