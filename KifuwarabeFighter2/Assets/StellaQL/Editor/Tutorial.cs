using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace StellaQL
{
    public abstract class Tutorial
    {
        public static void ToContents(StringBuilder contents)
        {
            contents.Append(@"---------------------------------------------------------------
StellaQL チュートリアル
Tutorial of StellaQL
---------------------------------------------------------------
    作者  : 高橋 智史 (ハンドルネーム: むずでょ)
    Author: TAKAHASHI Satoshi (Handle. Muzudho)
---------------------------------------------------------------


手順1
Step 1

    あなたのUnityプロジェクトのバックアップは取っておいてください。
    Please keep a backup of your data.

    エラー等で中断した際、データの復元機能は付いていません。
    There is no rollback at error suspension.


手順2
Step 2

    既にやっていると思いますが、メニューから[Window] - [State Machine Command Line (StellaQL)] をクリックしてください。
    I think you are doing it already. [Window] - [State Machine Command Line (StellaQL)] is here.

");
            contents.AppendLine(@"");
            contents.AppendLine(@"手順3");
            contents.AppendLine(@"Step 3");
            contents.AppendLine(@"");
            contents.Append(@"    """); contents.Append(FileUtility_Engine.PATH_ANIMATOR_CONTROLLER_FOR_DEMO_TEST); contents.AppendLine(@"""ファイルを開いてださい。");
            contents.Append(@"    Please, open """); contents.Append(FileUtility_Engine.PATH_ANIMATOR_CONTROLLER_FOR_DEMO_TEST); contents.AppendLine(@""".");
            contents.AppendLine(@"");
            contents.Append(@"    また、その "); contents.Append(Path.GetFileName(FileUtility_Engine.PATH_ANIMATOR_CONTROLLER_FOR_DEMO_TEST)); contents.AppendLine(" ファイルを [Animation Controller Drag & Drop] 枠にドラッグ＆ドロップしてください。");
            contents.Append(@"    And Drag&Drop the "); contents.Append(Path.GetFileName(FileUtility_Engine.PATH_ANIMATOR_CONTROLLER_FOR_DEMO_TEST)); contents.AppendLine(" file to [Animation Controller Drag & Drop] box.");
            contents.AppendLine(@"");
            contents.Append(@"
手順4 (任意)
Step 4 (Option)

    始める前に、開発者が夜中に StellaQL を壊していないか、テストすることができます。
    Before you begin, developers can test whether they break StellaQL at midnight.

        [Window] - [Editor Tests Runner] ... [StellaQL] - [StellaQLTest].
        [Window] - [Editor Tests Runner] ... [StellaQL] - [StellaQLTest].

    そして [Run All]を押してください。チェックが全て緑色ならＯＫです。
    And [Run All]. Please, click is all greens.

    ただし、既に Demo_Zoo を編集していた場合は結果が異なります。
    However, if you have already edited Demo_Zoo, the result will be different.


手順5
Step 5

    次の文をクエリー・テキストボックスに入力してください。
    Please enter the following statement in the Query text box.

    # Step 5
    TRANSITION INSERT
    FROM ""Base Layer\.Cat""
    TO   ""Base Layer\.Dog""

    そして、Executeボタンを押してください。
    Then press the Execute button.

    Base Layer.Catステートから Base Layer.Dog ステートへ線が引かれていることを確認してください。
    Please confirm that a line is drawn from Base Layer.Cat state to Base Layer.Dog state.


手順6
Step 6

    次の文をクエリー・テキストボックスに入力してください。
    Please enter the following statement in the Query text box.

    # Step 6
    TRANSITION INSERT
    FROM ""Base Layer\.Foo""
    TO   ""Base Layer\..*[Nn].*""

    そして、Executeボタンを押してください。
    Then press the Execute button.

    Fooステートから、名前に N が含まれるステートに線が引かれました。
    From the Foo state, a line was drawn in the state whose name contained N.


手順7
Step 7

    次の文をクエリー・テキストボックスに入力してください。
    Please enter the following statement in the Query text box.

    # Step 7
    TRANSITION SELECT
    FROM
        ""Base Layer\.Foo""
    TO
        "".*""
    THE
        Zoo001

    そして、Executeボタンを押してください。
    Then press the Execute button.

    プロジェクト・フォルダの中に CSVファイルが作られています。
    A CSV file is created in the project folder.

    フーから伸びている線が一覧されています。
    The lines extending from Foo are listed.


手順8
Step 8

    次の文をクエリー・テキストボックスに入力してください。
    Please enter the following statement in the Query text box.

    # Step 8
    TRANSITION UPDATE
    SET
        exitTime 1.0
        duration 0
    FROM
        ""Base Layer\.Foo""
    TO
        ""Base Layer\..*[Nn].*""

    そして、Executeボタンを押してください。
    Then press the Execute button.

    exitTime と duration が一斉に更新されました。
    ExitTime and duration were updated all at once.

    SELECT してみるのもいいでしょう。
    You may want to SELECT it.

    # Zoo001 to Zoo002
    TRANSITION SELECT FROM ""Base Layer\.Foo"" TO "".*"" THE Zoo002

    でも、こんな線要らないですね。
    But, I do not need this line.


手順9
Step 9

    次の文をクエリー・テキストボックスに入力してください。
    Please enter the following statement in the Query text box.

    # Step 9
    TRANSITION DELETE
    FROM ""Base Layer\.Foo""
    TO   ""Base Layer\..*""

    そして、Executeボタンを押してください。
    Then press the Execute button.

    Foo ステートから伸びている線が全て消えました。
    All the lines extending from the Foo state disappeared.

    Cat ステートから Dog ステートへの線はつながったままなことを確認してください。
    Please confirm that the line from Cat state to Dog state remains connected.


手順10
Step 10

    次の文をクエリー・テキストボックスに入力してください。
    Please enter the following statement in the Query text box.

    # Step 10
    STATE UPDATE
    SET
        speedParameterActive true
        speedParameter       ""New Float""
        speed                1.23
    WHERE
        ""Base Layer\.Cat""

    そして、Executeボタンを押してください。
    Then press the Execute button.

    Inspector ウィンドウを見て猫ステートのプロパティーが更新されていることを確認してください。
    Please check the Inspector window and check that the property of the cat state is updated.


手順11
Step 11

    次の文をクエリー・テキストボックスに入力してください。
    Please enter the following statement in the Query text box.

    # Step 11
    STATE INSERT
    WORDS
        WhiteAlpaca
        ""White Bear""
        ""\""White\"" Cat""
        ""White\\Gray\\Black Dog""
    WHERE ""Base Layer""

    そして、Executeボタンを押してください。
    Then press the Execute button.

    ステートが４つ作成されました。
    Four states have been created.


手順12
Step 12

    次の文をクエリー・テキストボックスに入力してください。
    Please enter the following statement in the Query text box.

    # Step 12
    STATE DELETE
    WORDS
        "".*(White).*""
    WHERE
        ""Base Layer""

    そして、Executeボタンを押してください。
    Then press the Execute button.

    名前に White を含むステートが削除されました。
    A state including White in the name has been deleted.


手順13
Step 13

    次の文をクエリー・テキストボックスに入力してください。
    Please enter the following statement in the Query text box.

    # Step 13
    TRANSITION INSERT
    FROM TAG ( Ei I )
    TO   ""Base Layer\.Foo""

    そして、Executeボタンを押してください。
    Then press the Execute button.

    A と E を含む動物から Foo に線が引かれました。
    A line from Foo was drawn from animals including A and E.

    丸括弧は全てのタグに一致するものを探します。
    Parentheses look for matches for all tags.

手順14
Step 14

    Projectウィンドウから Assets/StellaQL/AnimatorControllers/Demo_Zoo をダブルクリックして C#スクリプト・ファイルを開いてください。
    Double click Assets/StellaQL/AnimatorControllers/Demo_Zoo from the Project window to open the C# script file.

    ステートに１つ１つタグを設定しています。エイ、ビー、シー...
    I set the tag one by one in the state.Ei, Bi, Si...

    このタグはUnityとは関係なく、ただの文字列です。
    This tag is just a character string, regardless of Unity.

    ユニティーエディターに戻ってください。
    Please return to the unity editor.


手順15
Step 15

    次の文をクエリー・テキストボックスに入力してください。
    Please enter the following statement in the Query text box.

    # Step 15
    TRANSITION INSERT
    FROM ""Base Layer\.Foo""
    TO TAG [ Eks Uai Zi ]

    そして、Executeボタンを押してください。
    Then press the Execute button.

    Foo から X と Y と Z のいずれかを含む動物に線が引かれました。
    A line was drawn from animals containing X, Y and Z from Foo.

    角括弧は１つでもタグが一致するものを探します。
    We look for something that matches at least one square bracket.

手順16
Step 16

    次の文をクエリー・テキストボックスに入力してください。
    Please enter the following statement in the Query text box.

    # Step 16
    TRANSITION DELETE
    FROM ""Base Layer\.Foo""
    TO   TAG { I Ai Uai }

    そして、Executeボタンを押してください。
    Then press the Execute button.

    Foo から E と I と Y の１つも含まない動物への線は消されました。
    The line from Foo to an animal that does not contain E, I and Y is erased.

    中括弧はタグが１つも一致しないものを探します。
    Curly braces look for matches that do not match any tags.


手順17
Step 17

    一旦 全てのトランザクションを消しましょう。
    Let's delete all transactions once.

    次の文をクエリー・テキストボックスに入力してください。
    Please enter the following statement in the Query text box.

    # Step 17
    TRANSITION DELETE
    FROM "".*""
    TO   "".*""

    そして、Executeボタンを押してください。
    Then press the Execute button.

    全てのトランザクションが削除されました。
    All transactions have been deleted.


手順18
Step 18

    次の文をクエリー・テキストボックスに入力してください。
    Please enter the following statement in the Query text box.

    # Step 18
    TRANSITION INSERT
    FROM ""Base Layer\.Foo""
    TO TAG ( [ Ei Ou ] { I } )

    そして、Executeボタンを押してください。
    Then press the Execute button.

    Fooから、A か O を含み、E は含まない動物へ線が引かれました。
    From Foo, lines were drawn from animals containing A or O, but not E.

    タグは囲ってください。  ((A B ) (C) ) はＯＫ。((A B ) C ) はダメ。
    Please surround the tag. ((A B) (C)) is OK. ((A B) C) is not good.


手順19
Step 19

    難しい場合はセミコロンで命令文を分けましょう。
    If it is difficult, let's divide the statement with a semicolon.

    もう一度、全てのトランザクションを消します。
    Turn off all transactions again.

    次の文をクエリー・テキストボックスに入力してください。
    Please enter the following statement in the Query text box.

    # Step 19
    TRANSITION DELETE FROM "".*"" TO "".*"";
    TRANSITION INSERT FROM ""Base Layer\.Foo"" TO TAG [ Ei Ou ];
    TRANSITION DELETE FROM ""Base Layer\.Foo"" TO TAG ( I ) ;

    そして、Executeボタンを押してください。
    Then press the Execute button.

    Fooから、A か O を含み、E は含まない動物へ線が引かれました。
    From Foo, lines were drawn from animals containing A or O, but not E.


手順20
Step 20

    もう一度、全てのトランザクションを消し、猫ステートを設定し直します。
    Once again, turn off all transactions and set cat state again.

    次の文をクエリー・テキストボックスに入力してください。
    Please enter the following statement in the Query text box.

    # Step 20
    TRANSITION DELETE FROM "".* "" TO "".* "" ;
    STATE UPDATE SET speedParameterActive false speedParameter """" speed 1 WHERE ""Base Layer\.Cat""

    そして、Executeボタンを押してください。
    Then press the Execute button.

    チュートリアルの最初の状態に戻ったことでしょう。
    You will have returned to the initial state of the tutorial.


手順21
Step 21

    次の文をクエリー・テキストボックスに入力してください。
    Please enter the following statement in the Query text box.

    # Step 21
    TRANSITION INSERT FROM ""Base Layer\.Alpaca""     TO ""Base Layer\.Alpaca""     ;
    TRANSITION INSERT FROM ""Base Layer\.Bear""       TO ""Base Layer\.Rabbit""     ;
    TRANSITION INSERT FROM ""Base Layer\.Cat""        TO ""Base Layer\.Tiger""      ;
    TRANSITION INSERT FROM ""Base Layer\.Dog""        TO ""Base Layer\.Giraffe""    ;
    TRANSITION INSERT FROM ""Base Layer\.Elephant""   TO ""Base Layer\.Tiger""      ;
    TRANSITION INSERT FROM ""Base Layer\.Fox""        TO ""Base Layer\.Xenopus""    ;
    TRANSITION INSERT FROM ""Base Layer\.Giraffe""    TO ""Base Layer\.Elephant""   ;
    TRANSITION INSERT FROM ""Base Layer\.Horse""      TO ""Base Layer\.Elephant""   ;
    TRANSITION INSERT FROM ""Base Layer\.Iguana""     TO ""Base Layer\.Alpaca""     ;
    TRANSITION INSERT FROM ""Base Layer\.Jellyfish""  TO ""Base Layer\.Horse""      ;
    TRANSITION INSERT FROM ""Base Layer\.Kangaroo""   TO ""Base Layer\.Ox""         ;
    TRANSITION INSERT FROM ""Base Layer\.Lion""       TO ""Base Layer\.Nutria""     ;
    TRANSITION INSERT FROM ""Base Layer\.Monkey""     TO ""Base Layer\.Yak""        ;
    TRANSITION INSERT FROM ""Base Layer\.Nutria""     TO ""Base Layer\.Alpaca""     ;
    TRANSITION INSERT FROM ""Base Layer\.Ox""         TO ""Base Layer\.Xenopus""    ;
    TRANSITION INSERT FROM ""Base Layer\.Pig""        TO ""Base Layer\.Giraffe""    ;
    TRANSITION INSERT FROM ""Base Layer\.Quetzal""    TO ""Base Layer\.Lion""       ;
    TRANSITION INSERT FROM ""Base Layer\.Rabbit""     TO ""Base Layer\.Tiger""      ;
    TRANSITION INSERT FROM ""Base Layer\.Sheep""      TO ""Base Layer\.Pig""        ;
    TRANSITION INSERT FROM ""Base Layer\.Tiger""      TO ""Base Layer\.Rabbit""     ;
    TRANSITION INSERT FROM ""Base Layer\.Unicorn""    TO ""Base Layer\.Nutria""     ;
    TRANSITION INSERT FROM ""Base Layer\.Vixen""      TO ""Base Layer\.Nutria""     ;
    TRANSITION INSERT FROM ""Base Layer\.Wolf""       TO ""Base Layer\.Fox""        ;
    TRANSITION INSERT FROM ""Base Layer\.Xenopus""    TO ""Base Layer\.Sheep""      ;
    TRANSITION INSERT FROM ""Base Layer\.Yak""        TO ""Base Layer\.Kangaroo""   ;
    TRANSITION INSERT FROM ""Base Layer\.Zebra""      TO ""Base Layer\.Alpaca""     ;

    そして、Executeボタンを押してください。
    Then press the Execute button.

    しりとりを作りました。
    I made a word chain.


手順22
Step 22

    [Export spread sheet]ボタンを押してください。
    Please press the[Export spread sheet] button.

   プロジェクト・フォルダーの中に いくつかのCSVファイルが出力されます。
    Several CSV files are output in the project folder.


手順23
Step 23

    プロジェクト・フォルダーに置いてある StellaQL_MacroApplication.ods ファイルを開いてください。
    Please open the StellaQL_MacroApplication.ods file in the project folder.

    マクロを有効化してください。
    Activate the macro.

    StellaQL_MacroApplication.ods ファイルに書かれている手順に沿って進んでください。
    Follow the instructions in the StellaQL_MacroApplication.ods file.

    例えば、ビュー・ドキュメントの transitionsシートを編集してみてください。
    For example, try editing the transitions sheet of the view document.

    Base Layer.Bear から Base Layer.Rabbit へとつなぐトランジションの duration 0.1 を 0 にするのもいいでしょう。
    It is a good idea to set the duration 0.1 of the transition from Base Layer.Bear to Base Layer.Rabbit to 0.

    手順を最後まで終えると、インポート用のCSVファイルが１つ作られます。
    After completing the procedure, one import CSV file will be created.


手順24
Step 24

    必ず、あなたのUnityのプロジェクトを丸ごとバックアップを取っておいてください。
    Be sure to keep a backup of your Unity project altogether.

    [Import spread sheet]ボタンを押してください。
    Please press the[Import spread sheet] button.

   例えば Base Layer.Bear から Base Layer.Rabbit へとつなぐトランジションの duration が 0 になっていることを確認してください。
    For example, please check that the duration of the transition from Base Layer.Bear to Base Layer.Rabbit is 0.


基本操作の説明は以上です。
The explanation of the basic operation is above.

その他のクエリーは、コマンド リファレンスボタンを押して調べてください。
For other queries, please check by pressing the command reference button.
");
            contents.AppendLine(@"");
            contents.AppendLine(@"サンキュー☆（＾ｑ＾）");
            contents.AppendLine(@"Thank you.");
        }
    }
}
