using StellaQL;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main シーン
/// </summary>
namespace SceneStellaQLTest
{
    /// <summary>
    /// アニメーターのステート
    /// </summary>
    public class StateExRecord : AbstractStateExRecord
    {
        public static StateExRecord Build(string fullpath, StateExTable.Attr_Test attribute)
        {
            return new StateExRecord(fullpath, Animator.StringToHash(fullpath), attribute);
        }
        public StateExRecord(string fullpath, int fullpathHash, StateExTable.Attr_Test attribute) : base(fullpath, fullpathHash, (int)attribute)
        {
        }

        public override bool HasFlag_attr(int enumration)
        {
            return ((StateExTable.Attr_Test)this.AttributeEnum).HasFlag((StateExTable.Attr_Test)enumration);
        }
    }

    public class StateExTable : AbstractStateExTable
    {
        static StateExTable()
        {
            StateExTable.Instance = new StateExTable();
        }
        public static StateExTable Instance;

        /// <summary>
        /// 列挙型は拡張できないし、どうしたものか。
        /// </summary>
        [Flags]
        public enum Attr_Test
        {
            Zero = 0,
            Alpha = 1,
            Beta = Alpha << 1,
            Cee = Beta << 1,
            Dee = Cee << 1,
            Eee = Dee << 1,
        }

        public const string FULLPATH_ALPACA = "Base Layer.StellaQL Practice.Alpaca";
        public const string FULLPATH_BEAR = "Base Layer.StellaQL Practice.Bear";
        public const string FULLPATH_CAT = "Base Layer.StellaQL Practice.Cat";
        public const string FULLPATH_DOG = "Base Layer.StellaQL Practice.Dog";
        public const string FULLPATH_ELEPHANT = "Base Layer.StellaQL Practice.Elephant";
        public const string FULLPATH_FOX = "Base Layer.StellaQL Practice.Fox";
        public const string FULLPATH_GIRAFFE = "Base Layer.StellaQL Practice.Giraffe";
        public const string FULLPATH_HORSE = "Base Layer.StellaQL Practice.Horse";
        public const string FULLPATH_IGUANA = "Base Layer.StellaQL Practice.Iguana";
        public const string FULLPATH_JELLYFISH = "Base Layer.StellaQL Practice.Jellyfish";
        public const string FULLPATH_KANGAROO = "Base Layer.StellaQL Practice.Kangaroo";
        public const string FULLPATH_LION = "Base Layer.StellaQL Practice.Lion";
        public const string FULLPATH_MONKEY = "Base Layer.StellaQL Practice.Monkey";
        public const string FULLPATH_NUTRIA = "Base Layer.StellaQL Practice.Nutria";
        public const string FULLPATH_OX = "Base Layer.StellaQL Practice.Ox";
        public const string FULLPATH_PIG = "Base Layer.StellaQL Practice.Pig";
        public const string FULLPATH_QUETZAL = "Base Layer.StellaQL Practice.Quetzal";
        public const string FULLPATH_RABBIT = "Base Layer.StellaQL Practice.Rabbit";
        public const string FULLPATH_SHEEP = "Base Layer.StellaQL Practice.Sheep";
        public const string FULLPATH_TIGER = "Base Layer.StellaQL Practice.Tiger";
        public const string FULLPATH_UNICORN = "Base Layer.StellaQL Practice.Unicorn";
        public const string FULLPATH_VIXEN = "Base Layer.StellaQL Practice.Vixen";
        public const string FULLPATH_WOLF = "Base Layer.StellaQL Practice.Wolf";
        public const string FULLPATH_XENOPUS = "Base Layer.StellaQL Practice.Xenopus";
        public const string FULLPATH_YAK = "Base Layer.StellaQL Practice.Yak";
        public const string FULLPATH_ZEBRA = "Base Layer.StellaQL Practice.Zebra";

        protected StateExTable()
        {
            List<StateExRecordable> temp = new List<StateExRecordable>()
            {
                StateExRecord.Build(  FULLPATH_ALPACA, Attr_Test.Alpha | Attr_Test.Cee),// {E}(1) AC(1) ([(A C)(B)]{E})(1)
                StateExRecord.Build(  FULLPATH_BEAR, Attr_Test.Alpha | Attr_Test.Beta | Attr_Test.Eee),// B(1) AE(1) AE,B,E(1)
                StateExRecord.Build(  FULLPATH_CAT, Attr_Test.Alpha | Attr_Test.Cee),// {E}(2) AC(2) ([(A C)(B)]{E})(2)
                StateExRecord.Build(  FULLPATH_DOG, Attr_Test.Dee),// {E}(3)
                StateExRecord.Build(  FULLPATH_ELEPHANT, Attr_Test.Alpha | Attr_Test.Eee),//AE(2) AE,B,E(2) Nn(1)
                StateExRecord.Build(  FULLPATH_FOX, Attr_Test.Zero),// {E}(4)
                StateExRecord.Build(  FULLPATH_GIRAFFE, Attr_Test.Alpha | Attr_Test.Eee),//AE(3) AE,B,E(3)
                StateExRecord.Build(  FULLPATH_HORSE, Attr_Test.Eee),// AE,B,E(4)
                StateExRecord.Build(  FULLPATH_IGUANA, Attr_Test.Alpha),// {E}(5) Nn(2)
                StateExRecord.Build(  FULLPATH_JELLYFISH, Attr_Test.Eee),// AE,B,E(5)
                StateExRecord.Build(  FULLPATH_KANGAROO, Attr_Test.Alpha),// {E}(6) Nn(3)
                StateExRecord.Build(  FULLPATH_LION, Attr_Test.Zero),// {E}(7) Nn(4)
                StateExRecord.Build(  FULLPATH_MONKEY, Attr_Test.Eee),// AE,B,E(6) Nn(5)
                StateExRecord.Build(  FULLPATH_NUTRIA, Attr_Test.Alpha),// {E}(8) Nn(6)
                StateExRecord.Build(  FULLPATH_OX, Attr_Test.Zero),// {E}(9)
                StateExRecord.Build(  FULLPATH_PIG, Attr_Test.Zero),// {E}(10)
                StateExRecord.Build(  FULLPATH_QUETZAL, Attr_Test.Alpha | Attr_Test.Eee),//AE(4) AE,B,E(7)
                StateExRecord.Build(  FULLPATH_RABBIT, Attr_Test.Alpha | Attr_Test.Beta),// {E}(11) B(2) ([(A C)(B)]{E})(3)  AE,B,E(8)
                StateExRecord.Build(  FULLPATH_SHEEP, Attr_Test.Eee),// AE,B,E(9)
                StateExRecord.Build(  FULLPATH_TIGER, Attr_Test.Eee),// AE,B,E(10)
                StateExRecord.Build(  FULLPATH_UNICORN, Attr_Test.Cee),// {E}(12) Nn(7)
                StateExRecord.Build(  FULLPATH_VIXEN, Attr_Test.Eee),// AE,B,E(11) Nn(8)
                StateExRecord.Build(  FULLPATH_WOLF, Attr_Test.Zero),// {E}(13)
                StateExRecord.Build(  FULLPATH_XENOPUS, Attr_Test.Eee),// AE,B,E(12) Nn(9)
                StateExRecord.Build(  FULLPATH_YAK, Attr_Test.Alpha),// {E}(14)
                StateExRecord.Build(  FULLPATH_ZEBRA, Attr_Test.Alpha | Attr_Test.Beta | Attr_Test.Eee),// B(3) AE(5) AE,B,E(13)
            };
            foreach (StateExRecordable record in temp) { hash_to_exRecord.Add(record.FullPathHash, record); }
        }
    }
}
