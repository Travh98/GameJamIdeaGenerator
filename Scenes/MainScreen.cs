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

    [Signal]
    delegate void Signal_LineEditGenerated(String title, String text);

    public override void _Ready()
    {
        _storyColumn = GetNode<GeneratorColumn>("MarginContainer/VBoxContainer/MainHbox/StoryElements");
        _storyLineEditTitles = new List<String>();
        _storyLineEditTitles.Add("Protagonist");
        _storyLineEditTitles.Add("Antagonist");
        _storyLineEditTitles.Add("Setting");
        _storyColumn.CreateLineEdits(_storyLineEditTitles);

        _storyColumn.Connect("Signal_Regenerate", this, nameof(Slot_RegenerateStory));
        _storyColumn.Connect("Signal_Reroll", this, nameof(Slot_RerollLineEdit));
        Connect(nameof(Signal_LineEditGenerated), _storyColumn, "Slot_NewLineEditValue");

        _gameplayColumn = GetNode<GeneratorColumn>("MarginContainer/VBoxContainer/MainHbox/GameplayLoops");
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

}
