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

        public override bool HasFlag(int enumration)
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
                {(int)AstateIndex.Bear, new AstateRecord(  "Base Layer.", "Bear", Attr.Alpha | Attr.Beta | Attr.Eee)},//AE(1)
                {(int)AstateIndex.Cat, new AstateRecord(  "Base Layer.", "Cat", Attr.Alpha | Attr.Cee)},// ([(Alpha Cee)(Beta)]{Eee})(2)
                {(int)AstateIndex.Dog, new AstateRecord(  "Base Layer.", "Dog", Attr.Dee)},
                {(int)AstateIndex.Elephant, new AstateRecord(  "Base Layer.", "Elephant", Attr.Alpha | Attr.Eee)},//AE(2)
                {(int)AstateIndex.Fox, new AstateRecord(  "Base Layer.", "Fox", Attr.Zero)},
                {(int)AstateIndex.Giraffe, new AstateRecord(  "Base Layer.", "Giraffe", Attr.Alpha | Attr.Eee)},//AE(3)
                {(int)AstateIndex.Horse, new AstateRecord(  "Base Layer.", "Horse", Attr.Eee)},
                {(int)AstateIndex.Iguana, new AstateRecord(  "Base Layer.", "Iguana", Attr.Alpha)},
                {(int)AstateIndex.Jellyfish, new AstateRecord(  "Base Layer.", "Jellyfish", Attr.Eee)},
                {(int)AstateIndex.Kangaroo, new AstateRecord(  "Base Layer.", "Kangaroo", Attr.Alpha)},
                {(int)AstateIndex.Lion, new AstateRecord(  "Base Layer.", "Lion", Attr.Zero)},
                {(int)AstateIndex.Monkey, new AstateRecord(  "Base Layer.", "Monkey", Attr.Eee)},
                {(int)AstateIndex.Nutria, new AstateRecord(  "Base Layer.", "Nutria", Attr.Alpha)},
                {(int)AstateIndex.Ox, new AstateRecord(  "Base Layer.", "Ox", Attr.Zero)},
                {(int)AstateIndex.Pig, new AstateRecord(  "Base Layer.", "Pig", Attr.Zero)},
                {(int)AstateIndex.Quetzal, new AstateRecord(  "Base Layer.", "Quetzal", Attr.Alpha | Attr.Eee)},//AE(4)
                {(int)AstateIndex.Rabbit, new AstateRecord(  "Base Layer.", "Rabbit", Attr.Alpha | Attr.Beta)},// ([(Alpha Cee)(Beta)]{Eee})(3)
                {(int)AstateIndex.Sheep, new AstateRecord(  "Base Layer.", "Sheep", Attr.Eee)},
                {(int)AstateIndex.Tiger, new AstateRecord(  "Base Layer.", "Tiger", Attr.Eee)},
                {(int)AstateIndex.Unicorn, new AstateRecord(  "Base Layer.", "Unicorn", Attr.Cee)},
                {(int)AstateIndex.Vixen, new AstateRecord(  "Base Layer.", "Vixen", Attr.Eee)},
                {(int)AstateIndex.Wolf, new AstateRecord(  "Base Layer.", "Wolf", Attr.Zero)},
                {(int)AstateIndex.Xenopus, new AstateRecord(  "Base Layer.", "Xenopus", Attr.Eee)},
                {(int)AstateIndex.Yak, new AstateRecord(  "Base Layer.", "Yak", Attr.Alpha)},
                {(int)AstateIndex.Zebra, new AstateRecord(  "Base Layer.", "Zebra", Attr.Alpha | Attr.Beta | Attr.Eee)},//AE(5)
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
    [Test]
    public void LookupKeyword()
    {
        Dictionary<int, AstateRecordable> table = InstanceDatabase.index_to_record;
        List<int> attrList = new List<int>() { (int)AstateDatabase.Attr.Alpha | (int)AstateDatabase.Attr.Eee };
        Dictionary<int, AstateRecordable> currentResult = new Dictionary<int, AstateRecordable>(table);

        foreach (int attr in attrList)
        {
            Dictionary<int, AstateRecordable> nextResult = new Dictionary<int, AstateRecordable>();
            foreach (KeyValuePair<int, AstateRecordable> record in currentResult)
            {
                if (record.Value.HasFlag(attr))
                {
                    // 該当したもの
                    nextResult.Add(record.Key, record.Value);
                }
            }
            currentResult = nextResult;
        }

        Assert.AreEqual(5, currentResult.Count);
        if (5 == currentResult.Count)
        {
            Assert.IsTrue(currentResult.ContainsKey((int)AstateIndex.Bear));
            Assert.IsTrue(currentResult.ContainsKey((int)AstateIndex.Elephant));
            Assert.IsTrue(currentResult.ContainsKey((int)AstateIndex.Giraffe));
            Assert.IsTrue(currentResult.ContainsKey((int)AstateIndex.Quetzal));
            Assert.IsTrue(currentResult.ContainsKey((int)AstateIndex.Zebra));
        }
    }

    /// <summary>
    /// [ ] 演算をテスト
    /// </summary>
    [Test]
    public void LookupKeywordList(Dictionary<int, AstateRecordable> table)
    {

    }

    /// <summary>
    /// { } 演算をテスト
    /// </summary>
    [Test]
    public void LookupNGKeyword(Dictionary<int, AstateRecordable> table)
    {

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
