using Godot;
using System;
using System.Collections.Generic;

public class MainScreen : Control
{
    GeneratorColumn _storyColumn;
    List<String> _storyLineEditTitles;
    GeneratorColumn _gameplayColumn;
    List<String> _gameplayLineEditTitles;

    CsvLoader _csvLoader;
    StringDictionary _stringDict;
    Button _generateJamPlanButton;
    TextEdit _jamPlanTextEdit;

    [Signal]
    delegate void Signal_LineEditGenerated(String title, String text);

    public override void _Ready()
    {
        _storyColumn = GetNode<GeneratorColumn>("MarginContainer/HBoxContainer/VBoxContainer/MainHbox/StoryElements");
        _storyLineEditTitles = new List<String>();
        _storyLineEditTitles.Add("Protagonist");
        _storyLineEditTitles.Add("Antagonist");
        _storyLineEditTitles.Add("Setting");
        _storyColumn.CreateLineEdits(_storyLineEditTitles);

        _storyColumn.Connect("Signal_Regenerate", this, nameof(Slot_RegenerateStory));
        _storyColumn.Connect("Signal_Reroll", this, nameof(Slot_RerollLineEdit));
        Connect(nameof(Signal_LineEditGenerated), _storyColumn, "Slot_NewLineEditValue");

        _gameplayColumn = GetNode<GeneratorColumn>("MarginContainer/HBoxContainer/VBoxContainer/MainHbox/GameplayLoops");
        _gameplayLineEditTitles = new List<string>();
        _gameplayLineEditTitles.Add("LoopItem1");
        _gameplayLineEditTitles.Add("LoopItem2");
        _gameplayLineEditTitles.Add("LoopItem3");
        _gameplayLineEditTitles.Add("LoopItem4");
        _gameplayLineEditTitles.Add("LoopItem5");
        _gameplayLineEditTitles.Add("LoopItem6");
        _gameplayColumn.CreateLineEdits(_gameplayLineEditTitles);
        _gameplayColumn.SetLineEditTitlesVisible(false);

        _gameplayColumn.Connect("Signal_Regenerate", this, nameof(Slot_RegenerateGameplay));
        _gameplayColumn.Connect("Signal_Reroll", this, nameof(Slot_RerollLineEdit));
        Connect(nameof(Signal_LineEditGenerated), _gameplayColumn, "Slot_NewLineEditValue");

        _stringDict = new StringDictionary();
        _stringDict._Ready();

        _csvLoader = new CsvLoader();
        _csvLoader.LoadCsv(_stringDict);

        _jamPlanTextEdit = GetNode<TextEdit>("MarginContainer/HBoxContainer/VBoxContainer/TextEdit");

        _generateJamPlanButton = GetNode<Button>("MarginContainer/HBoxContainer/VBoxContainer/DocumentGen/GenerateDocumentButton");
        _generateJamPlanButton.Connect("pressed", this, nameof(Slot_GenerateGameJamPlan));
    }

    public void Slot_RerollLineEdit(String title)
    {
        // Reroll this single line edit
        GenerateRandomText(title);
    }

    private void Slot_RegenerateStory()
    {
        // Regenerate for all values in story line edits list
        foreach(String title in _storyLineEditTitles)
        {
            GenerateRandomText(title);
        }
    }

    private void Slot_RegenerateGameplay()
    {
        // Regenerate for all values
        foreach(String title in _gameplayLineEditTitles)
        {
            GenerateRandomText(title);
        }
    }

    private void GenerateRandomText(String title)
    {
        String text = "NA";
        switch(title)
        {
            case "Protagonist":
            case "Antagonist":
            {
                text = _stringDict.GetRandomText(StringDictionary.ColumnName.Characters);
                break;
            }
            case "Setting":
            {
                text = _stringDict.GetRandomText(StringDictionary.ColumnName.Places);
                break;
            }
            case "LoopItem1":
            case "LoopItem2":
            case "LoopItem3":
            case "LoopItem4":
            case "LoopItem5":
            case "LoopItem6":
            {
                text = _stringDict.GetRandomText(StringDictionary.ColumnName.GameplayLoopItems);
                break;
            }
            default:
            {
                text = "NA";
                break;
            }
        }

        EmitSignal(nameof(Signal_LineEditGenerated), title, text);
    }

    public void Slot_GenerateGameJamPlan()
    {
        String jamPlan = "Game Jam Plan" + "\n";
        jamPlan += "Theme: " + GetNode<LineEdit>("MarginContainer/HBoxContainer/VBoxContainer/MainHbox/ThemeVbox/ThemeLineEdit").Text;
        jamPlan += "\n";
        jamPlan += "Wildcards / Limitations:" + "\n";
        for(int i = 1; i <= 5; ++i)
        {
            LineEdit wildcard = GetNode<LineEdit>("MarginContainer/HBoxContainer/VBoxContainer/MainHbox/ThemeVbox/WildcardLineEdit" + i.ToString());
            if(wildcard.Text.Length > 0)
            {
                jamPlan += "\t-" + wildcard.Text + "\n";
            }
        }

        jamPlan += "\n";
        jamPlan += "Story:" + "\n";
        jamPlan += _storyColumn.GetColumnStringData();

        jamPlan += "\n";
        jamPlan += "Gameplay Loop:" + "\n";
        jamPlan += _gameplayColumn.GetColumnStringData();

        jamPlan += "\n";
        jamPlan += "(Coming soon) Game Type:" + "\n";
        jamPlan += "\tPerspective: 1st Person, 3rd Person" + "\n";
        jamPlan += "\tPresentation Style: 2D, 3D, Pokemon-style 2D," + "\n";
        jamPlan += "\tArt Style: Pixelated, PS1-style, High-res quality" + "\n";

        jamPlan += "\n";
        jamPlan += "============================\n";
        jamPlan += "(This part you edit)" + "\n";
        jamPlan += "Management Details:" + "\n";
        jamPlan += "\t" + "Target Display Resolution: 1920 x 1080 pixels" + "\n";
        jamPlan += "\t" + "Target Platform: HTML or Windows, Linux, Mac" + "\n";

        jamPlan += "\n";
        jamPlan += "Responsibilities:" + "\n";
        jamPlan += "(Consider assigning responsibilities)" + "\n";
        jamPlan += "\t" + "Project manager:" + "\n";
        jamPlan += "\t" + "Gameplay mechanics dev:" + "\n";
        jamPlan += "\t" + "Level design dev:" + "\n";
        jamPlan += "\t" + "Main artist:" + "\n";
        jamPlan += "\t" + "Music producer:" + "\n";
        jamPlan += "\t" + "Sound effects producer:" + "\n";

        jamPlan += "\n";
        jamPlan += "Team Schedule Availabilities:" + "\n";
        jamPlan += "(Consider when people are available or not to work on the project, timezones, etc)" + "\n";
        jamPlan += "\t" + "ExamplePerson1: " + "UTC-5, free after work Mon-Fri, busy on Sat" + "\n";

        jamPlan += "\n";
        jamPlan += "Schedule:" + "\n";
        jamPlan += "\t" + "-Set up repository and all tools" + "\n";
        jamPlan += "\t" + "-Brainstorm" + "\n";
        jamPlan += "\t" + "-Draw a full mock-up of what your game will look like at the end" + "\n";
        jamPlan += "\t" + "-Prototype the main gameplay mechanics" + "\n";
        jamPlan += "\t" + "-Playtest, make sure mechanics are fun to play" + "\n";
        jamPlan += "\t" + "-Art, sounds, music" + "\n";
        jamPlan += "\t" + "-Title screen, pause menu, list of controls, volume slider" + "\n";
        jamPlan += "\t" + "-Upload a test build of your game to make sure it works" + "\n";
        jamPlan += "\t" + "-Tutorial Section" + "\n";
        jamPlan += "\t" + "-Credits, end screen" + "\n";
        jamPlan += "\t" + "-Speedrun timer, screenshake, post processing" + "\n";
        jamPlan += "\t" + "-Submit game early" + "\n";
        jamPlan += "\t" + "-Stylize the itch.io game page" + "\n";
        jamPlan += "\t" + "-Play, rate, and leave feedback on other's games!" + "\n";

        _jamPlanTextEdit.Text = jamPlan;
    }
}
