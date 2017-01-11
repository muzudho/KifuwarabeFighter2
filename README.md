# KifuwarabeFighter2
Unityで2D格闘を作ろうぜ☆（＾▽＾）  Let's challenge creating 2D fighting game like street fighter 2?

ライセンス License
==================
(1)プログラムは MITライセンスだぜ☆ ただし、３つの .cs ファイルを除く。
(2)いくつかのキャラクター画像は二次創作だぜ☆ 画像を差し替えて使ってくれだぜ☆

(1)Program is **MIT License** But, Except 3 (.cs) files.  
(2)1 character graphics is fanmade. Nessesory, Change graphics.  

(1')次の３つのプログラム・ファイルはライセンス不明だぜ☆ Except 3 program files. The license of this file is unknown. Author: 2dgames_jp  
-KifuwarabeFighter2/Assets/Scripts/TakoyakiParticleScript.cs  
-KifuwarabeFighter2/Assets/Scripts/TakoyakiTokenScript.cs  
-KifuwarabeFighter2/Assets/Scripts/TakoyakiUtilScript.cs  

(2')次のキャラクターは二次創作だぜ☆ 画像を差し替えてくれだぜ☆  Nessesory to change, The image of this character.  
(2-1)ろぼりん娘（帽子に入っているキャラクター）
(2-1)Roborinko(This character is on a hat.)

金くれ☆（任意） I want money (Option)
======================================
このサイトで振り込めるぜ☆ I am poor. You can suport me. Payment is here.  
https://enty.jp/posts/31990  

作者のブログ Author Blog
========================
作者 Author: むずでょ Muzudho  
http://kifuwarabe.warabenture.com/2017/01/11/lets-create-game%E2%98%8614-lets-challenge-creating-2d-fighting-game-like-street-fighter-2-for-unity/  

進捗 Progress
=============
**できているもの☆ Done.**
-弾は飛ぶ☆ I done create bullet hit.  

**まだなもの☆ To do.**
-パンチの当たり判定がまだ☆ I was not create punch hit.  

フォルダーの説明 Folder Explain
===============================
**KifuwarabeFighter2** ユニティにぶちこめだぜ☆ This folder is all files for Unity.  
**KifuwarabeFighter2_meta** ゲームに入れない元素材ファイルやツールだぜ☆ This folder is not include release game. This is a source material or tool.  

**AtarihanteiMaker.exe**  

<pre>
KifuwarabeFighter2_meta
|
+--tool(CSharp)
    |
    +--AtarihanteiMaker
         |
         +--AtarihanteiMaker
              |
              +--bin
                   |
                   +--Release
                        |
                        +--AtarihanteiMaker.exe
</pre>

これは半自動当たり判定座標調べツールだぜ☆ PNG画像からテキストデータを出力する。 1024x1024ピクセル画像を128x128ピクセルでスライスした画像に使える。 赤い矩形(R=255,G=0,B=0)から、Offset X,Y、Scale X,Y を算出するぜ☆スケールは（x=2,y=2,z=1 を想定）。  
使い方
(1) .pngファイルを.exeファイルと同じフォルダーに置けだぜ☆  
(2) AtarihanteiMaker.exeファイルをダブルクリックしろだぜ☆  
(3) おわり。同じフォルダに .txt ファイルができてるだろ☆  

This tool is a semi automatic collider 2D coordinate metrics. Png image to text data. Useable 128x128 pixels slices in 1024x1024 pixels image. Red(R=255,G=0,B=0) Rectangle to Offset X,Y and Scale X,Y on Unity ( Scale x=2 y=2 z=1 ).  
How to use.  
(1) Put on .png file in AtarihanteiMaker.exe same folder.  
(2) Double click AtarihanteiMaker.exe file.  
(3) Done. Same name .txt file created.  

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

