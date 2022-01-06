# KifuwarabeFighter2

Unityで2D格闘を作ろうぜ☆（＾▽＾）  Let's challenge creating 2D fighting game like street fighter 2?  

# Croud funding!

This page is broken. https://enty.jp/grayscale  

# How to use

Copy **KifuwarabeFighter2/Assets** and **KifuwarabeFighter2/ProjectSettings** directory to new Unity directory. ユニティにぶちこめだぜ☆  

Configure input.

* Project explorer [Assets/Scenes/Title] Scene Double click.
* Main menu [File] - [Build Settings...] - [Build] Button click.
* Main menu [Edit] - [Project Settings...] - [Input].
* Right click `Fire1` and [Delete Array Eelement].
* Same `Fire2`, `Fire3`, `Jump`, `Mouse X`, `Mouse Y`, `Mouse ScrollWheel`, `Horizontal`, `Vertical`, `Fire1`, `Fire2`, `Fire3`, `Jump`.
* Change `Size` 5 to 21.
* Click new items and input `name` text box. Rename to `P1LightPunch`, `P1MediumPunch`, `P1HardPunch`, `P1LightKick`, `P1MediumKick`, `P1HardKick`, `P1Pause`, `P2Horizontal`, `P2Vertical`, `P2LightPunch`, `P2MediumPunch`, `P2HardPunch`, `P2LightKick`, `P2MediumKick`, `P2HardKick`, `P2Pause`,
* Configure scene.
    * Click main menu [File] - [Build Settings...].
    * Double click [Assets] - [Scenes] - [Title] in project view.
    * Click [Add Open Scenes] button.
    * Double click [Assets] - [Scenes] - [Select] in project view.
    * Click [Add Open Scenes] button.
    * Double click [Assets] - [Scenes] - [Fight] in project view.
    * Click [Add Open Scenes] button.
    * Double click [Assets] - [Scenes] - [Result] in project view.
    * Click [Add Open Scenes] button.
    * Right click `Scenes/SampleScene` from `Build Settings/Scene In Build`. and Click [Remove Selection].
* Configure layer.
    * Click project view [Scenes] - [Main].
    * Click Hierarchy view [Main] - [GroundHidden].
    * Click Inspecter [Layer] - [Add Layer ...].
    * Key typing `Ground` into `User Layer 9` text box.
    * Click Hierarchy view [Main] - [GroundHidden].
    * Click Inspecter [Layer] - [9:Ground].
* Configure input.
    * See: [Conventional Game Input](https://docs.unity3d.com/Manual/ConventionalGameInput.html)
    * `Horizontal`      Negative Button = Left, Positive Button = Right                 Alt Negative Button = h,    Alt Positive Button = i,    Gravity = 0,                                    Snap = [v], Invert = [ ],   Type = Joystick Axis,           Axis = X axis,  Joy Num = Joystick 1.
    * `Vertical`        Negative Button = Down, Positive Button = Up                    Alt Negative Button = j,    Alt Positive Button = k,    Gravity = 0,                                    Snap = [v], Invert = [v],   Type = Joystick Axis,           Axis = Y axis,  Joy Num = Joystick 1.
    * `P1LightPunch`                            Positive Button = joystick 1 button 3,                              Alt Positive Button = a,    Gravity = 0,    Dead = 0.2, Sensitivity = 1,                                Type = Key or Mouse Button,                     Joy Num = Joystick 1.
    * `P1MediumPunch`                           Positive Button = joystick 1 button 2,                              Alt Positive Button = b,    Gravity = 0,    Dead = 0.2, Sensitivity = 1,                                Type = Key or Mouse Button,                     Joy Num = Joystick 1.
    * `P1HardPunch`                             Positive Button = joystick 1 button 4,                              Alt Positive Button = c,    Gravity = 0,    Dead = 0.2, Sensitivity = 1,                                Type = Key or Mouse Button,                     Joy Num = Joystick 1.
    * `P1LightKick`                             Positive Button = joystick 1 button 1,                              Alt Positive Button = d,    Gravity = 0,    Dead = 0.2, Sensitivity = 1,                                Type = Key or Mouse Button,                     Joy Num = Joystick 1.
    * `P1MediumKick`                            Positive Button = joystick 1 button 0,                              Alt Positive Button = e,    Gravity = 0,    Dead = 0.2, Sensitivity = 1,                                Type = Key or Mouse Button,                     Joy Num = Joystick 1.
    * `P1HardKick`                              Positive Button = joystick 1 button 5,                              Alt Positive Button = f,    Gravity = 0,    Dead = 0.2, Sensitivity = 1,                                Type = Key or Mouse Button,                     Joy Num = Joystick 1.
    * `P1Pause`                                 Positive Button = joystick 1 button 7,                              Alt Positive Button = g,    Gravity = 0,    Dead = 0.2, Sensitivity = 1,                                Type = Key or Mouse Button,                     Joy Num = Joystick 1.
    * `P2Horizontal`                            Positive Button = '',                                               Alt Positive Button = '',   Gravity = 0,                                    Snap = [v], Invert = [ ],   Type = Joystick Axis,           Axis = X axis,  Joy Num = Joystick 2.
    * `P2Vertical`                              Positive Button = '',                                               Alt Positive Button = '',   Gravity = 0,                                    Snap = [v], Invert = [v],   Type = Joystick Axis,           Axis = Y axis,  Joy Num = Joystick 2.
    * `P2LightPunch`                            Positive Button = joystick 2 button 3,                              Alt Positive Button = '',   Gravity = 0,    Dead = 0.2, Sensitivity = 1,                                Type = Key or Mouse Button,                     Joy Num = Joystick 2.
    * `P2MediumPunch`                           Positive Button = joystick 2 button 2,                              Alt Positive Button = '',   Gravity = 0,    Dead = 0.2, Sensitivity = 1,                                Type = Key or Mouse Button,                     Joy Num = Joystick 2.
    * `P2HardPunch`                             Positive Button = joystick 2 button 4,                              Alt Positive Button = '',   Gravity = 0,    Dead = 0.2, Sensitivity = 1,                                Type = Key or Mouse Button,                     Joy Num = Joystick 2.
    * `P2LightKick`                             Positive Button = joystick 2 button 1,                              Alt Positive Button = '',   Gravity = 0,    Dead = 0.2, Sensitivity = 1,                                Type = Key or Mouse Button,                     Joy Num = Joystick 2.
    * `P2MediumKick`                            Positive Button = joystick 2 button 0,                              Alt Positive Button = '',   Gravity = 0,    Dead = 0.2, Sensitivity = 1,                                Type = Key or Mouse Button,                     Joy Num = Joystick 2.
    * `P2HardKick`                              Positive Button = joystick 2 button 5,                              Alt Positive Button = '',   Gravity = 0,    Dead = 0.2, Sensitivity = 1,                                Type = Key or Mouse Button,                     Joy Num = Joystick 2.
    * `P2Pause`                                 Positive Button = joystick 2 button 7,                              Alt Positive Button = '',   Gravity = 0,    Dead = 0.2, Sensitivity = 1,                                Type = Key or Mouse Button,                     Joy Num = Joystick 2.


# ドキュメント Author's Technical Documents

## Instration and Setup

Qiita 「UnityEditorを使って2D格闘(2D Fighting game)作るときのモーション遷移図作成の半自動化に挑戦しよう＜その５＞」http://qiita.com/muzudho1/items/50806e7d790034d975a6  

## Explain ([(Alpaca Bear)(Cat Dot)]{Elephant})

Qiita 「UnityEditorを使って2D格闘(2D Fighting game)作るときのモーション遷移図作成の半自動化に挑戦しよう＜その４＞」http://qiita.com/muzudho1/items/baf4b06cdcda96ca9a11  

## Enjoy Game Programing!

Qiita 「Unityでスト２を作ろうと思ったらケルナグールに似てきた話」http://qiita.com/muzudho1/items/b6a842d4e15216b95305  
Qiita 「Unityで2D格闘(2D Fighting game)や2Dアクションゲームの当たり判定を簡単に作るために一手間かける話し」 http://qiita.com/muzudho1/items/7de6e450e1762b993a63  
Qiita 「Unityで2D格闘(2D Fighting game)作ったとしても解決してくれないだろうこと」http://qiita.com/muzudho1/items/56f6d287c4f6857b2900  
Qiita 「Unityで2D格闘(2D Fighting game)作るときに相手の方向を向くには？」http://qiita.com/muzudho1/items/cfe352c864d791c3a98f  
Qiita 「Unityで２Ｄ格闘ゲームを作るときに欲しいと思われる、各フレームごとのデータの用意の仕方」 http://qiita.com/muzudho1/items/629ab6d7e3c4d7caedc8  
Qiita 「Unityでスト２を作ろうと思ったときに見えてくる碁盤の目のようなもの」 http://qiita.com/muzudho1/items/06b1faa57a8ed18543af  
Qiita 「Unityでスト２を作ろうと思ったら、すでにあったでござるよの巻」http://qiita.com/muzudho1/items/20851147a165bb9ec344  
Qiita 「Unityで２Ｄ対戦格闘ゲームのモーションの表示フレーム数を＊だいたい＊で調整する計算式」 http://qiita.com/muzudho1/items/b4a40e3c01e2f0b960b4  
Qiita 「Unityで２Ｄ対戦格闘ゲームの当たり判定を作るときの何が難しいのか？」 http://qiita.com/muzudho1/items/6fc03858fbccee0d0410  
Qiita 「Unityで2D対戦格闘を作ろうと思ったときにつまづくこと」http://qiita.com/muzudho1/items/aed45f72c9f0175376cc  

## Implementation

Qiita 「設計レベルでの見直し、リファクタリングだ！ビットフィールド比較をやめて文字列比較にしよう。」http://qiita.com/muzudho1/items/5a77b960b8002829c816  
Qiita 「Unityの上で動く、自作スクリプト言語の構文の実装の仕方」 http://qiita.com/muzudho1/items/05ffb53fb4e9d4252b28  

## Explain Unity coordinate

Qiita 「Unityで２Ｄ格闘ゲームのジャンプを作るときの何が難しいのか？」 http://qiita.com/muzudho1/items/998bf723df62cd83e988  

## Explain Animator Transition

Qiita 「Unityで２Ｄ格闘ゲームの歩行を作るときの何が難しいのか？」 http://qiita.com/muzudho1/items/4f9ed16412535af5f8b5  

## Explain Animator

Qiita 「Unityで2D格闘(2D Fighting game)作るんだがアニメーターを使えば状態遷移ラクチンだなという話し」 http://qiita.com/muzudho1/items/2736fba9bdee706d25f5  

## Explain Unity Editor

Qiita 「UnityEditorを使って2D格闘(2D Fighting game)作るときのモーション遷移図作成の半自動化に挑戦しよう＜その１＞」http://qiita.com/muzudho1/items/9c50b8b894e3cf4c90da  

## Other

Qiita 「UnityEditorを使って2D格闘(2D Fighting game)作るときのモーション遷移図作成の半自動化に挑戦しよう＜その２＞」http://qiita.com/muzudho1/items/8455c0c7a26d5788b541  
Qiita 「UnityEditorを使って2D格闘(2D Fighting game)作るときのモーション遷移図作成の半自動化に挑戦しよう＜その３＞」http://qiita.com/muzudho1/items/a1669f4d1721428790c1  

# ライセンス License

(1)プログラムは **MITライセンス** だぜ☆ ただし、３つの .cs ファイルを除く。  
(2)いくつかのキャラクター画像は二次創作だぜ☆ 画像を差し替えて使ってくれだぜ☆  
(3)音楽は 魔王魂のを使用☆  著作の表記必須。  

(1)Program is **MIT License** But, Except 3 (.cs) files.  
(2)1 character graphics is fanmade. Nessesory, Change graphics.  
(3)Music Author : Maoudamashii (Neccessory, Write copyright)  

(1')  
次の３つのプログラム・ファイルはライセンス不明だぜ☆ Except 3 program files. The license of this file is unknown. Author: 2dgames_jp  
-KifuwarabeFighter2/Assets/Scripts/TakoyakiParticleScript.cs  
-KifuwarabeFighter2/Assets/Scripts/TakoyakiTokenScript.cs  
-KifuwarabeFighter2/Assets/Scripts/TakoyakiUtilScript.cs  

(1'')  
Copyright (c) 2017 Muzudho(Dojin circle "Grayscale")  
Released under the MIT license  
http://opensource.org/licenses/mit-license.php  

(2')  
次のキャラクターは二次創作だぜ☆ 画像を差し替えてくれだぜ☆  Nessesory to change, The image of this character.  
(2-1)ろぼりん娘（帽子に入っているキャラクター）  
(2-1)Roborinko(This character is on a hat.)  

(3')  
音楽：魔王魂  
フリー音楽素材/魔王魂  
http://maoudamashii.jokersounds.com/  
Music: Maoudamashii  

## 作者のブログ Author Blog

作者むずでょブログ Author Muzudho Blog  
http://kifuwarabe.warabenture.com/2017/01/11/lets-create-game%E2%98%8614-lets-challenge-creating-2d-fighting-game-like-street-fighter-2-for-unity/  

ニコニコ生配信 Japanese private web camera live(SNS)  
http://com.nicovideo.jp/community/co3353928  

きふわらべファイター２技術ブログ Kifuwarabe Fighter Technical writing blog  
http://qiita.com/muzudho1/items/aed45f72c9f0175376cc  

# 進捗 Progress

**できているもの☆ Done.**  
-弾は飛ぶ☆ I done create bullet hit.  
-パンチの当たり判定あり☆ I done create punch hit.  

**まだなもの☆ To do.**  
-ダウンがまだ☆ I was not create down.  


# フォルダーの説明 Folder Explain

**KifuwarabeFighter2_meta** ゲームに入れない元素材ファイルやツールだぜ☆ This folder is not include release game. This is a source material or tool.  

**Hitbox2DMaker.exe**  (Old name: AtarihanteiMaker.exe)

<pre>
KifuwarabeFighter2_meta
|
+--tool(CSharp)
    |
    +--Hitbox2DMaker
         |
         +--Hitbox2DMaker
              |
              +--bin
                   |
                   +--Release
                        |
                        +--Hitbox2DMaker.exe
</pre>
Hitbox2DMaker プログラム解説(Program Explain) http://qiita.com/muzudho1/items/7de6e450e1762b993a63  

これは半自動当たり判定座標調べツールだぜ☆ PNG画像からテキストデータを出力する。 1024x1024ピクセル画像を128x128ピクセルでスライスした画像に使える。 スライス１つにつき赤い矩形(R=255,G=0,B=0)１つから、Offset X,Y、Scale X,Y を算出するぜ☆スケールは（x=2,y=2,z=1 を想定）。  
使い方
(1) .pngファイルを.exeファイルと同じフォルダーに置けだぜ☆  
(2) Hitbox2DMaker.exeファイルをダブルクリックしろだぜ☆  
(3) おわり。同じフォルダに AttackCollider2DScript.cs ファイルができてるだろ☆  

This tool is a semi automatic collider 2D coordinate metrics. Png image to text data. Useable 128x128 pixels slices in 1024x1024 pixels image. One Red(R=255,G=0,B=0) Rectangle by one slice image to Offset X,Y and Scale X,Y on Unity ( Scale x=2 y=2 z=1 ).  
How to use.  
(1) Put on .png file in Hitbox2DMaker.exe same folder.  
(2) Double click Hitbox2DMaker.exe file.  
(3) Done. Hitbox2DScript.cs file created.  (Old name: AttackCollider2DScript.cs)

**ドット絵（.edgファイル） Dot image ( .edg files )  

<pre>
KifuwarabeFighter2_meta
|
+--source
    |
    +-- *.edg
</pre>

これは EDGEのファイルだぜ☆ 下記のサイトからEDGEを入手してくれだぜ☆  
This is a binary source file of dot image. Please, Get dot editor.  
http://takabosoft.com/edge  

# StellaQL

📖 [StellaQL](https://github.com/muzudho/StellaQL)  
