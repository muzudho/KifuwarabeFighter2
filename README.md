# KifuwarabeFighter2
Unityで2D格闘を作ろうぜ☆（＾▽＾）  Let's challenge creating 2D fighting game like street fighter 2?

ライセンス License
==================
**MIT License**  

でも次の３つのファイルはライセンス不明だぜ☆ Except 3 files. The license of this file is unknown. Author: 2dgames_jp  
-KifuwarabeFighter2/Assets/Scripts/TakoyakiParticleScript.cs  
-KifuwarabeFighter2/Assets/Scripts/TakoyakiTokenScript.cs  
-KifuwarabeFighter2/Assets/Scripts/TakoyakiUtilScript.cs  

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
弾は飛ぶ☆ I done create bullet hit.
パンチの当たり判定がまだ☆ I was not create punch hit.

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

これは半自動当たり判定座標調べツールだぜ☆ This tool is a semi automatic collider 2D coordinate metrics.  
PNG画像からテキストデータを出力する。 Png image to text data.  
1024x1024ピクセル画像を128x128ピクセルでスライスした画像に使える。 Useable 128x128 pixels slices in 1024x1024 pixels image.  
赤い矩形(R=255,G=0,B=0)から、Offset X,Y、Scale X,Y を算出するぜ☆スケールは（x=2,y=2,z=1 を想定）。 Red(R=255,G=0,B=0) Rectangle to Offset X,Y and Scale X,Y on Unity ( Scale x=2 y=2 z=1 ).  
使い方 How to use.  
(1) .pngファイルを.exeファイルと同じフォルダーに置けだぜ☆ Put on .png file in AtarihanteiMaker.exe same folder.  
(2) AtarihanteiMaker.exeファイルをダブルクリックしろだぜ☆ Double click AtarihanteiMaker.exe file.  
(3) おわり。同じフォルダに .txt ファイルができてるだろ☆ Done. Same name .txt file created.  

**ドット絵（.edgファイル） Dot image ( .edg files )  

<pre>
KifuwarabeFighter2_meta
|
+--source
    |
    +-- *.edg
</pre>

これは EDGEのファイルだぜ☆ This is a binary source file of dot image.  
EDGEを入手してくれだぜ☆ Please, Get dot editor. http://takabosoft.com/edge  

