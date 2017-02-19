//-----------------------------------------------------------------------------
// State Machine Query StellaQL
// (C) 2017 Dojin Circle Grayscale
// Version 1
//-----------------------------------------------------------------------------
State Machine Query StellaQL は、ステートマシンでの膨大なトランジションの追加、
および　膨大なデータの中の不具合探しに対して、作業の高速化の可能性を提示した
ツールです。ゲーム開発の生産性向上を助けるものです。

以下のサイトではオープンソースとして無料配布しています。
https://github.com/muzudho/StellaQL

しかし、有料版を購入していただくことで開発の助けになります。


[クエリー＆正規表現]
線引きを助けます。例えば以下のクエリーを実行した場合、

  # Fooから名前にNを含むものへ
  TRANSITION INSERT
  FROM "Base Layer\.Foo"
  TO   "Base Layer\..*[Nn].*"

Fooステートから名前にNを含む複数のステートに対してトランジションが引かれます。

[クエリー＆タグ]
線引きをさらに助けます。名前の他に、StellaQL独自の方法で、Unityとは異なるタグ
（これは単に文字列）を複数個、ステートに持たせることができます。例えば
下準備が必要ですが以下のクエリーを実行した場合、

  # タグ Exs、Uai、Zi を持つステートへ
  TRANSITION INSERT
  FROM "Base Layer\.Foo"
  TO TAG [ Eks Uai Zi ]

Fooステートから独自のタグ Eks、Uai、Zi のいずれか１つでも持つ複数のステートに
対して一斉にトランジションが引かれます。これにより、意味的に線引きの指示が
できることを期待できます。

[スプレッドシート]
間違えている数字の設定を目視確認して探すことを助けます。
無料のツール LibreOffice Calc を用いたマクロ・アプリケーションを用意しました。
StellaQL を使って CSVを書き出し、スプレッドシートのマクロを使って CSVを元に
シートを作成します。（これをビューと呼ぶとします）

ビューではいくつかのプロパティーが編集可能になっており、Old列では現在の値が
確認でき、New列には編集したい値を入れます。
マクロを実行するボタンを押すことで　インポート用のCSVを作成します。
これをStellaQLが読込むことでプロパティーを更新できます。

■使用方法

StellaQL/StellaQL_Document フォルダーにいくつか開発中版のPNG画像、GIF動画を
添付しています。

(1)StellaQL/Assets/StellaQL フォルダーを、あなたの Assetsフォルダーの直下に
置いてください。

このとき、Assets/StellaQL/StellaQLUserSettings.cs ファイルを
StellaQLフォルダーの外（例えばAssetsフォルダーの直下）に移動させて
おいてください。

(2)Unityのメニューから[Window] - [State Machine Query (StellaQL)]を選び、
StateMachineQueryStellaQLウィンドウをドッキングさせてください。

(3)Assets/StellaQL/MayBeDeletedThisFolder/AnimatorControllers/Demo_Zoo
ファイルを開いてください。
（C#スクリプトではなく、アニメーター・コントローラーの方です）

(4)また、Demo_Zooアニメーター・コントローラーを、
StateMachineQueryStellaQLウィンドウの[Animator Controller Drag & Drop]と
書かれているエリアにドラッグ＆ドロップしてください。
このドラッグ＆ドロップはUnityを再起動するたびに必要です。

StellaQLは操作完了のたびにゲームの再生ボタンを押しますが、これは画面再描画の
ためです。StellaQLが再生ボタンを押すたび、再生ボタンを押し戻してください。

(5)[Generate C# (Fullpath of states)]ボタンを押してください。

Assets/StellaQL/MayBeDeletedThisFolder/AnimatorControllers/Demo_Zoo_Abstract.cs
ファイルが自動生成されます。

このボタンを押すタイミングは、ステートを追加、削除した時と
覚えておいてください。次のようなコードが自動生成されます。

    public const string
        BASELAYER_ = "Base Layer.",
        BASELAYER_ALPACA = "Base Layer.Alpaca",
        BASELAYER_BEAR = "Base Layer.Bear",
        BASELAYER_CAT = "Base Layer.Cat",
        // 以下略

この自動生成された抽象クラスを継承して作ったのが同じフォルダーにある
Demo_Zoo.cs ファイルです。このファイルは上書きされませんので、あなたの設定を
書き込むのに使います。中には

    public const string
        TAG_EI      = "Ei",     // A(ei)
        TAG_BI      = "Bi",     // B(bi)
        TAG_SI      = "Si",     // C(si)
        // 以下略

といったように StellaQL で独自に利用するタグの定義と、

    SetTag(BASELAYER_ALPACA ,new[] { TAG_EI ,TAG_SI ,TAG_EL ,TAG_PI });
    SetTag(BASELAYER_BEAR   ,new[] { TAG_EI ,TAG_BI ,TAG_I  ,TAG_AR });
    SetTag(BASELAYER_CAT    ,new[] { TAG_EI ,TAG_SI ,TAG_TI         });
    // 以下略

といったような、タグ付けが書かれています。

Demo_Zooだけではなく、あなたのアニメーター・コントローラーをStellaQLで
使う場合も同じように 自動生成された抽象クラスを継承してクラスを作ってください。

(6)さきほどの(2)で移動させた StellaQLUserSettings.cs ファイルを開いてください。

  { "Assets/StellaQL/MayBeDeletedThisFolder/AnimatorControllers/Demo_Zoo.controller",
        StellaQL.Acons.Demo_Zoo.AControl.Instance },

といった行があります。これは　アニメーター・コントローラーと、(6)で用意した
継承クラスのインスタンスを紐づけるものです。
あなたのアニメーター・コントローラーと、継承クラスもここに追記してください。

これで準備完了です。

(7)Assets/StellaQL/Document/チュートリアル.txt にはチュートリアルを用意しました。
クエリーの実行方法はそちらを参照してください。

Assets/StellaQL/Document/コマンドリファレンス.txt には利用できるクエリーの機能を
一覧しました。

また、StellaQL/StellaQL/Work/StellaQL_MacroApplication.ods ファイルは
LibreOffice Calc で開くことができます。

    LibreOffice : https://www.libreoffice.org/

CSVの読取、ビューの作成の手順は StellaQL_MacroApplication.ods の中に
記載しています。

■バージョン履歴

1
- 初期バージョン


ありがとうございました。