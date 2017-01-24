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
        Stay,
        Move,
        Ready,
        Timeover,
        Entry,
        Num,
    }

    /// <summary>
    /// アニメーターのステート
    /// </summary>
    public class AstateRecord : AbstractAstateRecord
    {
        public AstateDatabase.Attr attribute;

        public AstateRecord(string breadCrumb, string name, AstateDatabase.Attr attribute)
        {
            this.BreadCrumb = breadCrumb;
            this.Name = name;
            this.attribute = attribute;
        }

        public override bool HasFlag(int enumration)
        {
            return this.attribute.HasFlag((AstateDatabase.Attr)enumration);
        }
    }

    static AstateDatabase InstanceDatabase;
    public class AstateDatabase : AbstractAstateDatabase
    {
        [Flags]
        public enum Attr
        {
            Alpha = 1,
            Beta = 1 << 1,
            Cee = 1 << 2,
            Dee = 1 << 3,
            Eee = 1 << 4,
        }

        public AstateDatabase()
        {
            index_to_record = new Dictionary<int, AstateRecordable>()//AstateIndex
            {
                {(int)AstateIndex.Stay, new AstateRecord(  "Base Layer.", "Stay", Attr.Alpha)},
                {(int)AstateIndex.Move, new AstateRecord(  "Base Layer.", "Move", Attr.Beta)},
                {(int)AstateIndex.Ready, new AstateRecord(  "Base Layer.", "Ready", Attr.Alpha | Attr.Cee)},
                {(int)AstateIndex.Timeover, new AstateRecord(  "Base Layer.", "Timeover", Attr.Beta | Attr.Dee)},
                {(int)AstateIndex.Entry, new AstateRecord(  "Base Layer.", "Entry", Attr.Alpha | Attr.Cee | Attr.Eee)},
            };
        }
    }

	[Test]
	public void EditorTest() {

        string expression = "( [ ( Alpha Cee ) ( Beta Dee ) ] { Eee } )";

        List<int> result = Util_StellaQL.Execute(expression);

		//Assert
		Assert.AreEqual(result.Count, 2);

        if (2==result.Count)
        {
            Assert.AreEqual(result[0], (int)AstateIndex.Ready);//2
            Assert.AreEqual(result[1], (int)AstateIndex.Timeover);//3
        }
    }
}
