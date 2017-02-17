using System.Text;

namespace StellaQL
{
    /// <summary>
    /// [Command Reference] ボタンを押したときに出るメッセージ。
    /// A message that appears when you press the "Command Reference" button.
    /// </summary>
    public abstract class Reference
    {
        public static void ToContents(StringBuilder contents)
        {
            contents.Append(@"---------------------------------------------------------------
StellaQL コマンドライン・ウィンドウ (StateCmdline window) の使い方
How to use StellaQL Command line (StateCmdline window)
---------------------------------------------------------------
作者  : 高橋 智史 (ハンドルネーム: むずでょ)
Author: TAKAHASHI Satoshi (Handle. Muzudho)
---------------------------------------------------------------

(1) コメント
(1) Comment

    行コメントだけが使えます。
    Line comment only.

    # これはコメントです。
    # This is a comment.

    FROM # ダメ！ 命令文の後ろにコメントは書けません。
    FROM # No! This is not a comment.

    # ダメ！ 複数行コメントには対応していません。
    # No! Unfortunately, Multi line comment is nothing.
    FROM TAG ( Tail /*Horn*/ Wing )

(2) 基本的な書式
(2) Basic format

    # 大文字、小文字は区別します。
    # Unfortunately, upper case, lower case is sensitive.

    # 単語は１つ以上のスペース区切りです。 次の文はどちらも同じです。
    # A word is separated by one or more spaces. Both of the following sentences are the same.

    # (1)
        STATE INSERT SET name0 ""WhiteCat"" name1 ""WhiteDog"" WHERE ""Base Layer""

    # (2)
        STATE INSERT
        SET
            name0 ""WhiteCat""
            name1 ""WhiteDog""
        WHERE
            ""Base Layer""

    # 命令文はセミコロン区切りです。
    # A statement is separated by semi colon(;).

        # Sample
        STATE SELECT WHERE "".*Dog"" THE Zoo001 ;
        STATE SELECT WHERE "".*Cat"" THE Zoo002

    # 対応しているリテラル文字列のエスケープシーケンスは \\ \"" \r \n の４つです。
    # There are four escape sequences of supported literal strings \\ \"" \r \n.

        # Sample
        LAYER INSERT WORDS NewLayer0 ""New Layer1"" ""\""New Layer2\"""" ""New\\Layer3"" ""New\rLayer4"" ""New\nLayer5"" ""New\r\nLayer6""

    # リテラル文字列は検索時は正規表現です。 \ を検索したいときは \\ としてください。
    # Literal strings are used as regular expressions when searching. If you want to search \, please set it to \\.

        # Sample
        LAYER DELETE WORDS ""New\\\\Layer3""

=================

    LAYER INSERT
    WORDS
        NewLayer1
        ""New Layer2""

    # - NewLayer1 レイヤー、New Layer2 レイヤーを新規追加します。
    # - Add a NewLayer1 layer and New Layer2 layer.

-----------------

    LAYER DELETE
    WORDS
        NewLayer1
        ""New Layer2""

    # - NewLayer1 レイヤー、New Layer2 レイヤーを削除します。
    # - Delete a NewLayer1 layer and New Layer2 layer.

=================

    STATE INSERT
    WORDS
        WhiteCat
        ""White Dog""
    WHERE
        ""Base Layer""

    # - ステートを新規追加します。
    # - Add a new state.

    # - WHERE句には、ステートマシンへのパスのみ使えます。
    # - WHERE is Statemachine path.

-----------------

    STATE UPDATE
    SET
        speed                1.23
        speedParameterActive true
        speedParameter       4.5
    WHERE
        ""Base Layer\.Cat""

    # - ステートのプロパティーをアップデートします。
    # - Some States properties update.

-----------------

    STATE DELETE
    WORDS
        WhiteCat
        ""White Dog""
    WHERE
        ""Base Layer""

    # - WHERE句には、ステートマシンへのパスのみ使えます。
    # - WHERE is Statemachine path.

-----------------

    STATE SELECT
    WHERE
        "".*Cat""

    # 該当するステートをCSV形式ファイルに一覧します。
    # List some states and write a csv file.

    STATE SELECT WHERE "".*Cat"" THE Zoo1 ;
    STATE SELECT WHERE "".*Dog"" THE Zoo2

    # 出力先ファイル名に Zoo1, Zoo2 を付加します。
    # Add Zoo1, Zoo2 to output file name.

=================

    TRANSITION INSERT
    FROM
        ""Base Layer\.Cat""
    TO
        ""Base Layer\.Dog""

    # トランジションを新規追加します。
    # Add a new transition.

-----------------

    TRANSITION ANYSTATE INSERT
    FROM
        ""Base Layer""
    TO
        ""Base Layer\.Foo""

    # [Any State] からステートへトランジションを引きます。
    # Add a transition from [Any State] to a state.

    # もし再描画されないなら、アニメーション・コントローラーの右上の[Auto Live Link]ボタンを２回押してください。
    # Please, Click [Auto Live Link] Button on right top corner, twice(toggle). If not paint line.

-----------------

    TRANSITION ENTRY INSERT
    FROM
        ""Base Layer""
    TO
        ""Base Layer\.Foo""

    # [Entry] からステートへトランジションを引きます。
    # Add a transition from [Entry] to a state.

    # もし再描画されないなら、アニメーション・コントローラーの右上の[Auto Live Link]ボタンを２回押してください。
    # Please, Click [Auto Live Link] Button on right top corner, twice(toggle). If not paint line.

-----------------

    TRANSITION EXIT INSERT
    FROM
        ""Base Layer\.Foo""

    # ステートから [Exit] へトランジションを引きます。
    # Add a transition from a state to [Exit].

-----------------

    TRANSITION UPDATE
    SET
        exitTime 1.0
        duration 0
    FROM
        ""Base Layer\.Cat""
    TO
        ""Base Layer\.Dog""

    # トランジションのプロパティーを更新します。
    # Update a transition properties.

-----------------

    TRANSITION DELETE
    FROM
        ""Base Layer\.Cat""
    TO
        ""Base Layer\.Dog""

    # トランジションを削除します。
    # Delete a transition.

-----------------

    TRANSITION SELECT
    FROM
        "".*""
    TO
        "".*""

    # 該当するトランジションをCSV形式ファイルに一覧します。
    # List some transitions and write a csv file.

    TRANSITION SELECT
    FROM
        "".*""
    TO
        "".*""
    THE
        Zoo1

    # 出力先ファイル名に Zoo1 を付加します。
    # Add Zoo1 to output file name.

=================

(3) FROM 句 と TO 句
(3) FROM or TO phrase

    # 正規表現で書いてください。
    # Regular Expression.
    FROM
        ""Base Layer\.Cat""
    TO
        ""Base Layer\.Dog""

    # タグ (""AND"" 操作)
    # Tag (""AND"" operation)
    FROM TAG ( Ei Bi )
    TO TAG ( Si Di )

    # - タグは Unityのタグではなく、StellaQLのタグです。 C#のソースに定数で書いておいてください。
    # - Tag is not unity tags. It's StellaQL tags. Please, read C# program. It's const string.

    # - スペース区切り。
    # - Space separated.

    # - ( ) はすべてのタグ一致検索。
    # - ( ) is choice everything words.

    # - 入れ子可能。
    # - Nest OK.
    #   ((A B)(C D))

    #   ただし
    #   But,

    #   ((A B)  C ) はダメ。
    #   ((A B)  C ) is NG.

    #   ((A B) (C)) はＯＫ。
    #   ((A B) (C)) is OK.

-----------------

    # タグ (""OR"" 操作)
    # Tag (""OR"" operation)
    FROM TAG [ Ei Bi ]
    TO TAG [ Si Di ]

    # [ ] は、どれか１つでも一致検索。
    # [ ] is choice anything words.

-----------------

    # タグ (""NOT"" 操作)
    # Tag (""NOT"" operation)
    FROM TAG { Ei Bi }
    TO TAG { Si Di }

    # { } は、１つも一致しない検索。
    # { } is not choice everything words.

=================

(4) SET 句 (STATE 文の場合)
(4) SET phrase (case of STATE statement)
    
    SET
        name                       ""WhiteCat""
        tag                        ""enjoy""
        speed                      1.23
        speedParameterActive       true
        speedParameter             ""Monday""
        mirror                     true
        mirrorParameterActive      true
        mirrorParameter            ""Tuesday""
        cycleOffset                4.56
        cycleOffsetParameterActive true
        cycleOffsetParameter       ""Wednesday""
        iKOnFeet                   true
        writeDefaultValues         false

-----------------

(5) SET 句 (TRANSITION 文の場合)
(5) SET phrase (case of TRANSITION statement)
    
    SET solo true
        mute true
        hasExitTime true
        exitTime 12.3
        hasFixedDuration false
        duration 4.56
        offset 7.89
        orderedInterruption false
        tag ""excellent""
        

=================

(6) その他
(6) Other

    CSHARPSCRIPT GENERATE_FULLPATH
");
            contents.Append(@"    # ["); contents.Append(StateCmdline.BUTTON_LABEL_GENERATE_FULLPATH); contents.AppendLine("]ボタンを押下するのと同じです。");
            contents.Append(@"    # Same as pressing a ["); contents.Append(StateCmdline.BUTTON_LABEL_GENERATE_FULLPATH); contents.AppendLine("] button.");
            contents.Append(@"
=================

サンキュー☆（＾▽＾）
Thank you:-)

    ステラ キューエル
    StellaQL

    (ステートマシン コマンドライン)
    (State Machine Command Line)

    著作(C) TAKAHASHI satoshi / 2017年
    (C) TAKAHASHI satoshi / 2017

    日本在住
    From    Japan

    https://github.com/muzudho/KifuwarabeFighter2");
        }
    }
}
