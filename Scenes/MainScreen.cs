using Godot;
using System;
using System.Collections.Generic;

public class MainScreen : Control
{
    GeneratorColumn _storyColumn;
    List<String> _storyLineEditTitles;

    CsvLoader _csvLoader;
    StringDictionary _stringDict;

    [Signal]
    delegate void Signal_LineEditGenerated(String title, String text);

    public override void _Ready()
    {
        _storyColumn = GetNode<GeneratorColumn>("MarginContainer/HBoxContainer/StoryElements");
        _storyLineEditTitles = new List<String>();
        _storyLineEditTitles.Add("Protagonist");
        _storyLineEditTitles.Add("Antagonist");
        _storyLineEditTitles.Add("Setting");
        _storyColumn.CreateLineEdits(_storyLineEditTitles);

        _storyColumn.Connect("Signal_Regenerate", this, nameof(Slot_RegenerateStory));
        _storyColumn.Connect("Signal_Reroll", this, nameof(Slot_RerollLineEdit));
        Connect(nameof(Signal_LineEditGenerated), _storyColumn, "Slot_NewLineEditValue");

        _stringDict = new StringDictionary();
        _stringDict._Ready();

        _csvLoader = new CsvLoader();
        _csvLoader.LoadCsv(_stringDict);

        _stringDict.PrintDebug();
    }

    public void Slot_RerollLineEdit(String title)
    {
        // Reroll this single line edit
        GenerateRandomText(title);

        // Emit signal to this line edit
        GD.Print("Rerolling Line Edit: ", title);
    }

    private void Slot_RegenerateStory()
    {
        // Regenerate for all values in story line edits list
        foreach(String title in _storyLineEditTitles)
        {
            GenerateRandomText(title);
        }

        // Call the slots in story columns
        GD.Print("Regenerating Story Elements");

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
            default:
            {
                text = "NA";
                break;
            }
        }

        GD.Print("MainScreen emitting title and text: ", title, text);
        EmitSignal(nameof(Signal_LineEditGenerated), title, text);
    }

}
