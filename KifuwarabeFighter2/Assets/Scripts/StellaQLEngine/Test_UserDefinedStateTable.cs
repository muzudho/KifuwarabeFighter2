﻿using StellaQL;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main シーン
/// </summary>
namespace SceneStellaQLTest
{
    /// <summary>
    /// ステートマシン、ステートの拡張データ構造
    /// </summary>
    public class UserDefinedStateRecord : AbstractUserDefinedStateRecord
    {
        /// <summary>
        /// データ入力用
        /// </summary>
        /// <param name="fullpath">ステートマシン名、ステート名のフルパス</param>
        /// <param name="userDefinedTags">StellaQL用のユーザー定義タグ</param>
        /// <returns></returns>
        public static UserDefinedStateRecord Build(string fullpath, string[] userDefinedTags)
        {
            return new UserDefinedStateRecord(fullpath, Animator.StringToHash(fullpath), Code.Hashes(userDefinedTags));
        }

        /// <summary>
        /// データ
        /// </summary>
        /// <param name="fullpath">ステートマシン名、ステート名のフルパス</param>
        /// <param name="fullpathHash">ステートマシン名、ステート名のフルパスのハッシュ</param>
        /// <param name="userDefinedTags_hash">StellaQL用のユーザー定義タグのハッシュ</param>
        public UserDefinedStateRecord(string fullpath, int fullpathHash, HashSet<int> userDefinedTags_hash) : base(fullpath, fullpathHash, userDefinedTags_hash)
        {
        }
    }

    /// <summary>
    /// ステートマシン、ステートの拡張データ構造の集まり
    /// </summary>
    public class UserDefinedStateTable : AbstractUserDefinedStateTable
    {
        static UserDefinedStateTable() { Instance = new UserDefinedStateTable(); }
        public static UserDefinedStateTable Instance;

        #region ステートマシン、ステート　フルパス一覧
        public const string STATEMACHINE_BASELAYER = "Base Layer";
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
        #endregion

        #region StellaQL用のユーザー定義タグ一覧
        public const string TAG_ZERO = "Zero";
        public const string TAG_ALPHA = "Alpha";
        public const string TAG_BETA = "Beta";
        public const string TAG_CEE = "Cee";
        public const string TAG_DEE = "Dee";
        public const string TAG_EEE = "Eee";
        public const string TAG_HORN = "Horn";
        #endregion

        protected UserDefinedStateTable()
        {
            #region タグの有効化
            TagString_to_hash = Code.HashesDic(new []{
                TAG_ZERO,
                TAG_ALPHA,
                TAG_BETA,
                TAG_CEE,
                TAG_DEE,
                TAG_EEE,
                TAG_HORN,
            });
            #endregion

            #region ステートマシン拡張データ、ステート拡張データの有効化
            List<UserDefindStateRecordable> temp = new List<UserDefindStateRecordable>()
            {
                UserDefinedStateRecord.Build(  STATEMACHINE_BASELAYER,         new []{TAG_ZERO}),
                UserDefinedStateRecord.Build(  STATE_FOO,         new []{TAG_ZERO}),
                UserDefinedStateRecord.Build(  STATE_ALPACA,      new []{TAG_ALPHA, TAG_CEE}),
                UserDefinedStateRecord.Build(  STATE_BEAR,        new []{TAG_ALPHA, TAG_BETA, TAG_EEE}),
                UserDefinedStateRecord.Build(  STATE_CAT,         new []{TAG_ALPHA, TAG_CEE }),
                UserDefinedStateRecord.Build(  STATE_DOG,         new []{TAG_DEE }),
                UserDefinedStateRecord.Build(  STATE_ELEPHANT,    new []{TAG_ALPHA, TAG_EEE }),
                UserDefinedStateRecord.Build(  STATE_FOX,         new []{TAG_ZERO }),
                UserDefinedStateRecord.Build(  STATE_GIRAFFE,     new []{TAG_ALPHA, TAG_EEE}),
                UserDefinedStateRecord.Build(  STATE_HORSE,       new []{TAG_EEE }),
                UserDefinedStateRecord.Build(  STATE_IGUANA,      new []{TAG_ALPHA }),
                UserDefinedStateRecord.Build(  STATE_JELLYFISH,   new []{TAG_EEE }),
                UserDefinedStateRecord.Build(  STATE_KANGAROO,    new []{TAG_ALPHA }),
                UserDefinedStateRecord.Build(  STATE_LION,        new []{TAG_ZERO }),
                UserDefinedStateRecord.Build(  STATE_MONKEY,      new []{TAG_EEE }),
                UserDefinedStateRecord.Build(  STATE_NUTRIA,      new []{TAG_ALPHA }),
                UserDefinedStateRecord.Build(  STATE_OX,          new []{TAG_HORN }),
                UserDefinedStateRecord.Build(  STATE_PIG,         new []{TAG_ZERO }),
                UserDefinedStateRecord.Build(  STATE_QUETZAL,     new []{TAG_ALPHA, TAG_EEE }),
                UserDefinedStateRecord.Build(  STATE_RABBIT,      new []{TAG_ALPHA, TAG_BETA }),
                UserDefinedStateRecord.Build(  STATE_SHEEP,       new []{TAG_EEE }),
                UserDefinedStateRecord.Build(  STATE_TIGER,       new []{TAG_EEE }),
                UserDefinedStateRecord.Build(  STATE_UNICORN,     new []{TAG_CEE, TAG_HORN}),
                UserDefinedStateRecord.Build(  STATE_VIXEN,       new []{TAG_EEE }),
                UserDefinedStateRecord.Build(  STATE_WOLF,        new []{TAG_ZERO }),
                UserDefinedStateRecord.Build(  STATE_XENOPUS,     new []{TAG_EEE }),
                UserDefinedStateRecord.Build(  STATE_YAK,         new []{TAG_ALPHA, TAG_HORN }),
                UserDefinedStateRecord.Build(  STATE_ZEBRA,       new []{TAG_ALPHA, TAG_BETA, TAG_EEE }),
            };
            foreach (UserDefindStateRecordable record in temp) { StateHash_to_record.Add(record.FullPathHash, record); }
            #endregion
        }
    }
}
