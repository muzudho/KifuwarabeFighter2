金くれ☆（任意） I want money (Option)
======================================
開発資金がもらえると思ってオープンソースにしている☆（＾～＾）１月末までにくれだぜ☆ 次のサイトで振り込めるぜ☆  
I am poor. You can suport me. Payment is here.  
https://enty.jp/posts/31990  

KifuwarabeFighter2
==================
Unityで2D格闘を作ろうぜ☆（＾▽＾）  Let's challenge creating 2D fighting game like street fighter 2?  

ライセンス License
==================
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

作者のブログ Author Blog
========================
作者むずでょブログ Author Muzudho Blog  
http://kifuwarabe.warabenture.com/2017/01/11/lets-create-game%E2%98%8614-lets-challenge-creating-2d-fighting-game-like-street-fighter-2-for-unity/  

ニコニコ生配信 Japanese private web camera live(SNS)  
http://com.nicovideo.jp/community/co3353928  

きふわらべファイター２技術ブログ Kifuwarabe Fighter Technical writing blog  
http://qiita.com/muzudho1/items/aed45f72c9f0175376cc  

進捗 Progress
=============
**できているもの☆ Done.**  
-弾は飛ぶ☆ I done create bullet hit.  
-パンチの当たり判定あり☆ I done create punch hit.  

**まだなもの☆ To do.**  
-ダウンがまだ☆ I was not create down.  

フォルダーの説明 Folder Explain
===============================
**KifuwarabeFighter2** ユニティにぶちこめだぜ☆ This folder is all files for Unity.  
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

