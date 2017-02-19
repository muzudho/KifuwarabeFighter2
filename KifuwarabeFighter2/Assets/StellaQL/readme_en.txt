//-----------------------------------------------------------------------------
// State Machine Query StellaQL
// (C) 2017 Dojin Circle Grayscale
// Version 1
//-----------------------------------------------------------------------------
State Machine Query StellaQL adds enormous transitions in the state machine,
And suggested the possibility of speeding up work for finding defects in enormous data
It is a tool. It helps to improve the productivity of game development.

On the following site, it is distributed free as open source.
Https://github.com/muzudho/StellaQL

However, purchasing a paid version will help in development.


[Query & Regular Expression]
I will help with drawing. For example, if you execute the following query,

  # From Foo to name containing N
  TRANSITION INSERT
  FROM "Base Layer\.Foo"
  TO   "Base Layer\..*[Nn].*"

Transitions are drawn for multiple states containing N from Foo state.

[Query & Tag]
I will further aid the drawing. In addition to its name, in a way unique to StellaQL, different tags from Unity
(This is simply a character string) can have multiple states in the state. For example
Preliminary preparations are necessary, but if you execute the following query,

  # To state with tags Exs, Uai, Zi
  TRANSITION INSERT
  FROM "Base Layer\.Foo"
  TO TAG [ Eks Uai Zi ]

From the Foo state to multiple states with any one of its own tags Eks, Uai, Zi
Transitions will be drawn at once. As a result, an instruction to draw a line semantically
I can expect what I can do.

[Spreadsheet]
It helps to visually check and search the wrong number setting.
We prepared a macro application using free tool LibreOffice Calc.
Export CSV using StellaQL, based on CSV using spreadsheet macros
Create a sheet. (I will call this a view)

Some properties in the view are editable, and in the Old column the current value is
You can check and enter the value you want to edit in the New column.
Press the button to execute the macro to create a CSV for import.
You can update the property by reading it by StellaQL.

■ How to use

Several PNG images and GIF movies under development are attached to the
StellaQL/StellaQL_Document folder.

(1) Place the StellaQL/Assets/StellaQL folder directly under your Assets folder
please leave it.

At this time, set the Assets/StellaQL/StellaQLUserSettings.cs file
Move it outside the StellaQL folder (eg just under the Assets folder)
Put it down please.

(2) Select [Window] - [State Machine Query (StellaQL)] from the Unity menu,
Please dock the StateMachineQueryStellaQL window.

(3) Assets/StellaQL/MayBeDeletedThisFolder/AnimatorControllers/Demo_Zoo
Please open the file.
(It is not the C# script, but the animator controller)

(4) Also, Demo_Zoo animator controller,
[Animator Controller Drag & Drop] in the StateMachineQueryStellaQL window and
Please drag and drop it to the written area.
This drag and drop is necessary every time Unity is restarted.

StellaQL pushes the play button of the game each time the operation is completed, but this is a screen redrawing
That's why. When StellaQL presses the play button, push back the play button.

(5) Press the Generate C# (Fullpath of states) button.

Assets/StellaQL/MayBeDeletedThisFolder/AnimatorControllers/Demo_Zoo_Abstract.cs
The file is automatically generated.

The timing to push this button depends on when adding and deleting states
Please remember. The following code is automatically generated.

    public const string
        BASELAYER_ = "Base Layer.",
        BASELAYER_ALPACA = "Base Layer.Alpaca",
        BASELAYER_BEAR = "Base Layer.Bear",
        BASELAYER_CAT = "Base Layer.Cat",
        // Or less

It is in the same folder that inherited this automatically generated abstract class
This is the Demo_Zoo.cs file. This file will not be overwritten, so please change your settings
It is used for writing. While

    public const string
        TAG_EI      = "Ei",     // A(ei)
        TAG_BI      = "Bi",     // B(bi)
        TAG_SI      = "Si",     // C(si)
        // Or less

The definitions of tags that StellaQL uses independently,

    SetTag(BASELAYER_ALPACA ,new[] { TAG_EI ,TAG_SI ,TAG_EL ,TAG_PI });
    SetTag(BASELAYER_BEAR   ,new[] { TAG_EI ,TAG_BI ,TAG_I  ,TAG_AR });
    SetTag(BASELAYER_CAT    ,new[] { TAG_EI ,TAG_SI ,TAG_TI         });
    // Or less

Such as, tagging is written.

Not only Demo_Zoo, but also your animator controller with StellaQL
Please also inherit the automatically generated abstract class and create a class in the same way when using it.

(6) Please open the StellaQLUserSettings.cs file you moved in (2) before.

  {"Assets/StellaQL/MayBeDeletedThisFolder/AnimatorControllers/Demo_Zoo.controller",
        StellaQL.Acons.Demo_Zoo.AControl.Instance},

There are lines such as. This was prepared with the animator controller and (6)
It binds an instance of an inherited class.
Please add your animator controller and inheritance class here as well.

You are ready.

(7) Assets/StellaQL/Document/チュートリアル.txt has prepared a tutorial.
For how to execute the query, please refer to that.

Assets/StellaQL/Document/コマンドリファレンス.txt contains the available query functions
Listed.

Also, the StellaQL/StellaQL/Work/StellaQL_MacroApplication.ods file
You can open it in LibreOffice Calc.

    LibreOffice: https://www.libreoffice.org/

Reading CSVs and creating views is described in StellaQL_MacroApplication.ods
It is stated.

Version History

1
- Initial version


Thank you very much.
