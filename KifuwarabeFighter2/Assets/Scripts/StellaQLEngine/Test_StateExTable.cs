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
        public static StateExRecord Build(string fullpath, string[] requiredAllTags)
        {
            return new StateExRecord(fullpath, Animator.StringToHash(fullpath), Code.Hashs(requiredAllTags));
        }
        public StateExRecord(string fullpath, int fullpathHash, HashSet<int> requiredAllTags) : base(fullpath, fullpathHash, requiredAllTags)
        {
        }

        public override bool HasFlag_attr(HashSet<int> requiredAllTags)
        {
            foreach (int tag in requiredAllTags)
            {
                if (!Tags.Contains(tag)) { return false; } // １個でも持ってないタグがあれば偽。
            }
            return true;
        }
    }

    public class StateExTable : AbstractStateExTable
    {
        static StateExTable()
        {
            Instance = new StateExTable();
        }
        public static StateExTable Instance;

        public const string TAG_ZERO = "Zero";
        public const string TAG_ALPHA = "Alpha";
        public const string TAG_BETA = "Beta";
        public const string TAG_CEE = "Cee";
        public const string TAG_DEE = "Dee";
        public const string TAG_EEE = "Eee";
        public const string TAG_HORN = "Horn";


        public const string STATE_FOO = "Base Layer.Foo";
        public const string STATE_ALPACA = "Base Layer.Alpaca";
        public const string STATE_BEAR = "Base Layer.Bear";
        public const string STATE_CAT = "Base Layer.Cat";
        public const string STATE_DOG = "Base Layer.Dog";
        public const string STATE_ELEPHANT = "Base Layer.Elephant";
        public const string STATE_FOX = "Base Layer.Fox";
        public const string STATE_GIRAFFE = "Base Layer.Giraffe";
        public const string STATE_HORSE = "Base Layer.Horse";
        public const string STATE_IGUANA = "Base Layer.Iguana";
        public const string STATE_JELLYFISH = "Base Layer.Jellyfish";
        public const string STATE_KANGAROO = "Base Layer.Kangaroo";
        public const string STATE_LION = "Base Layer.Lion";
        public const string STATE_MONKEY = "Base Layer.Monkey";
        public const string STATE_NUTRIA = "Base Layer.Nutria";
        public const string STATE_OX = "Base Layer.Ox";
        public const string STATE_PIG = "Base Layer.Pig";
        public const string STATE_QUETZAL = "Base Layer.Quetzal";
        public const string STATE_RABBIT = "Base Layer.Rabbit";
        public const string STATE_SHEEP = "Base Layer.Sheep";
        public const string STATE_TIGER = "Base Layer.Tiger";
        public const string STATE_UNICORN = "Base Layer.Unicorn";
        public const string STATE_VIXEN = "Base Layer.Vixen";
        public const string STATE_WOLF = "Base Layer.Wolf";
        public const string STATE_XENOPUS = "Base Layer.Xenopus";
        public const string STATE_YAK = "Base Layer.Yak";
        public const string STATE_ZEBRA = "Base Layer.Zebra";

        protected StateExTable()
        {
            String_to_tagHash = Code.HashsDic(new []{
                TAG_ZERO,
                TAG_ALPHA,
                TAG_BETA,
                TAG_CEE,
                TAG_DEE,
                TAG_EEE,
                TAG_HORN,
            });

            List<StateExRecordable> temp = new List<StateExRecordable>()
            {
                StateExRecord.Build(  STATE_FOO,         new []{TAG_ZERO}),
                StateExRecord.Build(  STATE_ALPACA,      new []{TAG_ALPHA, TAG_CEE}),
                StateExRecord.Build(  STATE_BEAR,        new []{TAG_ALPHA, TAG_BETA, TAG_EEE}),
                StateExRecord.Build(  STATE_CAT,         new []{TAG_ALPHA, TAG_CEE }),
                StateExRecord.Build(  STATE_DOG,         new []{TAG_DEE }),
                StateExRecord.Build(  STATE_ELEPHANT,    new []{TAG_ALPHA, TAG_EEE }),
                StateExRecord.Build(  STATE_FOX,         new []{TAG_ZERO }),
                StateExRecord.Build(  STATE_GIRAFFE,     new []{TAG_ALPHA, TAG_EEE}),
                StateExRecord.Build(  STATE_HORSE,       new []{TAG_EEE }),
                StateExRecord.Build(  STATE_IGUANA,      new []{TAG_ALPHA }),
                StateExRecord.Build(  STATE_JELLYFISH,   new []{TAG_EEE }),
                StateExRecord.Build(  STATE_KANGAROO,    new []{TAG_ALPHA }),
                StateExRecord.Build(  STATE_LION,        new []{TAG_ZERO }),
                StateExRecord.Build(  STATE_MONKEY,      new []{TAG_EEE }),
                StateExRecord.Build(  STATE_NUTRIA,      new []{TAG_ALPHA }),
                StateExRecord.Build(  STATE_OX,          new []{TAG_HORN }),
                StateExRecord.Build(  STATE_PIG,         new []{TAG_ZERO }),
                StateExRecord.Build(  STATE_QUETZAL,     new []{TAG_ALPHA, TAG_EEE }),
                StateExRecord.Build(  STATE_RABBIT,      new []{TAG_ALPHA, TAG_BETA }),
                StateExRecord.Build(  STATE_SHEEP,       new []{TAG_EEE }),
                StateExRecord.Build(  STATE_TIGER,       new []{TAG_EEE }),
                StateExRecord.Build(  STATE_UNICORN,     new []{TAG_CEE, TAG_HORN}),
                StateExRecord.Build(  STATE_VIXEN,       new []{TAG_EEE }),
                StateExRecord.Build(  STATE_WOLF,        new []{TAG_ZERO }),
                StateExRecord.Build(  STATE_XENOPUS,     new []{TAG_EEE }),
                StateExRecord.Build(  STATE_YAK,         new []{TAG_ALPHA, TAG_HORN }),
                StateExRecord.Build(  STATE_ZEBRA,       new []{TAG_ALPHA, TAG_BETA, TAG_EEE }),
            };
            foreach (StateExRecordable record in temp) { Hash_to_exRecord.Add(record.FullPathHash, record); }
        }
    }
}
