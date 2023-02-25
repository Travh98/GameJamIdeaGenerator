using Godot;
using System;
using System.Collections.Generic;

public class GeneratorColumn : VBoxContainer
{
    PackedScene _lineEditScene = ResourceLoader.Load("res://Scenes/GeneratorLineEdit.tscn") as PackedScene;
    Button _regenerateButton;
    VSeparator _spacer;
    List<GeneratorLineEdit> _lineEdits;

    [Signal]
    delegate void Signal_Regenerate();

    [Signal]
    delegate void Signal_Reroll(String lineEditTitle);

    public override void _Ready()
    {
        _lineEdits = new List<GeneratorLineEdit>();
        _regenerateButton = GetNode<Button>("RegenerateButton");
        _regenerateButton.Connect("pressed", this, nameof(Slot_RegeneratePressed));
        _spacer = GetNode<VSeparator>("VSeparator");
    }

    public void CreateLineEdits(List<String> genLineEdits)
    {
        foreach(String title in genLineEdits)
        {
            GeneratorLineEdit lineEdit = _lineEditScene.Instance() as GeneratorLineEdit;
            AddChild(lineEdit);
            lineEdit.SetTitle(title);
            lineEdit.Connect("Signal_Reroll", this, nameof(Slot_LineEditReroll));
            _lineEdits.Add(lineEdit);
        }

        MoveChild(_regenerateButton, GetChildCount());
        MoveChild(_spacer, GetChildCount());
    }

    public void SetLineEditTitlesVisible(bool visible)
    {
        foreach(GeneratorLineEdit lineEdit in _lineEdits)
        {
            lineEdit.SetTitleVisible(visible);
        }
    }

    public void Slot_RegeneratePressed()
    {
        EmitSignal(nameof(Signal_Regenerate));
    }

    public void Slot_LineEditReroll(String lineEditTitle)
    {
        EmitSignal(nameof(Signal_Reroll), lineEditTitle);
    }

    public void Slot_NewLineEditValue(String title, String value)
    {
        // "Premature optimization is the root of all evil" - Knuth
        foreach(GeneratorLineEdit lineEdit in _lineEdits)
        {
            if(String.Compare(lineEdit.GetTitle(), title) == 0)
            {
                lineEdit.Slot_SetLineEdit(value);
            }
        }
    }

    public String GetColumnStringData()
    {
        String text = "";
        foreach(GeneratorLineEdit lineEdit in _lineEdits)
        {
            text += "\t" + lineEdit.GetTitle() + ": "
                + lineEdit.GetValue() + "\n";
        }
        return text;
    }
}
