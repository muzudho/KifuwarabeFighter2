using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using StellaQL;
using System.Collections.Generic;
using System;

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
                {(int)AstateIndex.Elephant, new AstateRecord(  "Base Layer.", "Elephant", Attr.Alpha | Attr.Eee)},//AE(2) AE,B,E(2)
                {(int)AstateIndex.Fox, new AstateRecord(  "Base Layer.", "Fox", Attr.Zero)},
                {(int)AstateIndex.Giraffe, new AstateRecord(  "Base Layer.", "Giraffe", Attr.Alpha | Attr.Eee)},//AE(3) AE,B,E(3)
                {(int)AstateIndex.Horse, new AstateRecord(  "Base Layer.", "Horse", Attr.Eee)},// AE,B,E(4)
                {(int)AstateIndex.Iguana, new AstateRecord(  "Base Layer.", "Iguana", Attr.Alpha)},
                {(int)AstateIndex.Jellyfish, new AstateRecord(  "Base Layer.", "Jellyfish", Attr.Eee)},// AE,B,E(5)
                {(int)AstateIndex.Kangaroo, new AstateRecord(  "Base Layer.", "Kangaroo", Attr.Alpha)},
                {(int)AstateIndex.Lion, new AstateRecord(  "Base Layer.", "Lion", Attr.Zero)},
                {(int)AstateIndex.Monkey, new AstateRecord(  "Base Layer.", "Monkey", Attr.Eee)},// AE,B,E(6)
                {(int)AstateIndex.Nutria, new AstateRecord(  "Base Layer.", "Nutria", Attr.Alpha)},
                {(int)AstateIndex.Ox, new AstateRecord(  "Base Layer.", "Ox", Attr.Zero)},
                {(int)AstateIndex.Pig, new AstateRecord(  "Base Layer.", "Pig", Attr.Zero)},
                {(int)AstateIndex.Quetzal, new AstateRecord(  "Base Layer.", "Quetzal", Attr.Alpha | Attr.Eee)},//AE(4) AE,B,E(7)
                {(int)AstateIndex.Rabbit, new AstateRecord(  "Base Layer.", "Rabbit", Attr.Alpha | Attr.Beta)},// ([(Alpha Cee)(Beta)]{Eee})(3)  AE,B,E(8)
                {(int)AstateIndex.Sheep, new AstateRecord(  "Base Layer.", "Sheep", Attr.Eee)},// AE,B,E(9)
                {(int)AstateIndex.Tiger, new AstateRecord(  "Base Layer.", "Tiger", Attr.Eee)},// AE,B,E(10)
                {(int)AstateIndex.Unicorn, new AstateRecord(  "Base Layer.", "Unicorn", Attr.Cee)},
                {(int)AstateIndex.Vixen, new AstateRecord(  "Base Layer.", "Vixen", Attr.Eee)},// AE,B,E(11)
                {(int)AstateIndex.Wolf, new AstateRecord(  "Base Layer.", "Wolf", Attr.Zero)},
                {(int)AstateIndex.Xenopus, new AstateRecord(  "Base Layer.", "Xenopus", Attr.Eee)},// AE,B,E(12)
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
    /// (　) 演算をテスト
    /// </summary>
    //[Test]
    public void LookupKeyword()
    { // c は条件（condition）、 r は結果（result）
        /*
        List<List<int>> cLockers = new List<List<int>>()
        {
            new List<int>() { (int)AstateDatabase.Attr.Alpha | (int)AstateDatabase.Attr.Eee },
        };
        Dictionary<int, AstateRecordable> rRecordIndexes = new Dictionary<int, AstateRecordable>(InstanceDatabase.index_to_record);

        foreach (List<int> cLocker in cLockers)
        {
            foreach (int cAttr in cLocker)
            {
                Dictionary<int, AstateRecordable> rRecords_empty = new Dictionary<int, AstateRecordable>();
                foreach (KeyValuePair<int, AstateRecordable> rPair in rRecordIndexes)
                {
                    if (rPair.Value.HasFlag(cAttr))
                    {
                        // 該当したもの
                        rRecords_empty.Add(rPair.Key, rPair.Value);
                    }
                }
                rRecordIndexes = rRecords_empty;
            }
        }

        Assert.AreEqual(5, rRecordIndexes.Count);
        if (5 == rRecordIndexes.Count)
        {
            Assert.IsTrue(rRecordIndexes.ContainsKey((int)AstateIndex.Bear));
            Assert.IsTrue(rRecordIndexes.ContainsKey((int)AstateIndex.Elephant));
            Assert.IsTrue(rRecordIndexes.ContainsKey((int)AstateIndex.Giraffe));
            Assert.IsTrue(rRecordIndexes.ContainsKey((int)AstateIndex.Quetzal));
            Assert.IsTrue(rRecordIndexes.ContainsKey((int)AstateIndex.Zebra));
        }
        */
    }

    /// <summary>
    /// [ ] 演算をテスト
    /// </summary>
    //[Test]
    public void LookupKeywordList() // 重複の削除
    { // c は条件（condition）、 r は結果（result）
        /*        
        List<List<int>> cLockers = new List<List<int>>()
        {
            new List<int>() { (int)AstateDatabase.Attr.Alpha | (int)AstateDatabase.Attr.Cee },
            new List<int>() { (int)AstateDatabase.Attr.Alpha | (int)AstateDatabase.Attr.Dee },
            new List<int>() { (int)AstateDatabase.Attr.Alpha , (int)AstateDatabase.Attr.Eee },
        };
        HashSet<int> rRecordIndexes = new HashSet<int>();

        // 属性検索して、単純にセットに入れていけばいい。（重複は入らない）
        foreach (List<int> cAttrList in cLockers)
        {
            foreach (int cAttr in cAttrList)
            {
                rRecordIndexes.Add(cAttr);
            }
        }

        Assert.AreEqual(4, rRecordIndexes.Count);
        if (4 == rRecordIndexes.Count)
        {            
            Assert.IsTrue(rRecordIndexes.Contains((int)AstateDatabase.Attr.Alpha));
            Assert.IsTrue(rRecordIndexes.Contains((int)AstateDatabase.Attr.Cee));
            Assert.IsTrue(rRecordIndexes.Contains((int)AstateDatabase.Attr.Dee));
            Assert.IsTrue(rRecordIndexes.Contains((int)AstateDatabase.Attr.Eee));
        }
        */
    }

    /// <summary>
    /// { } 演算をテスト
    /// </summary>
    //[Test]
    public void LookupNGKeyword()
    {
        /*
        Dictionary<int, AstateRecordable> universe = InstanceDatabase.index_to_record;
        List<List<int>> lockers = new List<List<int>>()
        {
            new List<int>() { (int)AstateDatabase.Attr.Alpha , (int)AstateDatabase.Attr.Cee },
            new List<int>() { (int)AstateDatabase.Attr.Eee , (int)AstateDatabase.Attr.Cee },
        };
        HashSet<int> recordIndexes = new HashSet<int>();

        // 全集合にあり、セットないものを入れていけばいい。
        foreach (KeyValuePair<int, AstateRecordable> pair in universe)
        {
            bool ok = true;
            foreach (List<int> attrList in lockers)
            {
                foreach (int attr in attrList)
                {
                    if (pair.Value.HasFlag(attr))
                    {
                        ok = false;
                    }
                }
            }
            if (ok) { recordIndexes.Add(pair.Key); }
        }

        Assert.AreEqual(4, recordIndexes.Count);
        if (4 == recordIndexes.Count)
        {
            Assert.IsTrue(recordIndexes.Contains((int)AstateDatabase.Attr.Alpha));
            Assert.IsTrue(recordIndexes.Contains((int)AstateDatabase.Attr.Cee));
            Assert.IsTrue(recordIndexes.Contains((int)AstateDatabase.Attr.Dee));
            Assert.IsTrue(recordIndexes.Contains((int)AstateDatabase.Attr.Eee));
        }
        */
    }

    /// <summary>
    /// （　）属性フィルター
    /// </summary>
    [Test]
    public void Filtering_AndAttributes()
    {
        // 条件は　Alpha | Eee
        List<int> attrs = new List<int>() { (int)AstateDatabase.Attr.Alpha | (int)AstateDatabase.Attr.Eee };
        Dictionary<int, AstateRecordable> recordIndexes = StellaQLScanner.Filtering_AndAttributes(attrs, InstanceDatabase.index_to_record);

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
    public void Filtering_OrAttributes()
    {
        // 条件は　（Alpha | Eee）、Beta、Eee
        List<int> attrs = new List<int>() { (int)AstateDatabase.Attr.Alpha | (int)AstateDatabase.Attr.Eee, (int)AstateDatabase.Attr.Beta, (int)AstateDatabase.Attr.Eee };
        Dictionary<int, AstateRecordable> recordIndexes = StellaQLScanner.Filtering_OrAttributes(attrs, InstanceDatabase.index_to_record);

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
    public void Filtering_NotAndNotAttributes()
    {
        // 条件は　（Alpha | Eee）、Beta、Eee
        List<int> attrs = new List<int>() { (int)AstateDatabase.Attr.Alpha | (int)AstateDatabase.Attr.Eee, (int)AstateDatabase.Attr.Beta, (int)AstateDatabase.Attr.Eee };
        Dictionary<int, AstateRecordable> recordIndexes = StellaQLScanner.Filtering_NotAndNotAttributes(attrs, InstanceDatabase.index_to_record);

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
        List<int> result = StellaQLScanner.Keyword_to_locker(set, typeof(AstateDatabase.Attr));

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
        List<int> result = StellaQLScanner.KeywordList_to_locker(set, typeof(AstateDatabase.Attr));

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
        List<int> result = StellaQLScanner.NGKeywordList_to_locker(set, typeof(AstateDatabase.Attr));

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
        List<int> complement = StellaQLScanner.Complement(set, typeof(AstateDatabase.Attr));

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
