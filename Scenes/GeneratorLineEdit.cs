using Godot;
using System;

public class GeneratorLineEdit : Control
{
    Label _titleLabel;
    LineEdit _lineEdit;
    Button _rerollButton;
    CheckButton _lockCheckButton;
    String titleName;

    [Signal]
    delegate void Signal_Reroll(String lineEditTitle);

    public override void _Ready()
    {
        _titleLabel = GetNode<Label>("VBoxContainer/TitleLabel");
        _lineEdit = GetNode<LineEdit>("VBoxContainer/HBoxContainer/LineEdit");
        _rerollButton = GetNode<Button>("VBoxContainer/HBoxContainer/RerollButton");
        _lockCheckButton = GetNode<CheckButton>("VBoxContainer/HBoxContainer/LockCheckButton");

        _rerollButton.Connect("pressed", this, nameof(Slot_RerollPressed));
        SetTitle(titleName);
    }

    public void Slot_SetLineEdit(String text)
    {
        
        // if(_lockCheckButton.ToggleMode == true)
        // {
        //     return;
        // }
        _lineEdit.Text = text;
    }

    public void SetTitle(String title)
    {
        _titleLabel.Text = title;
    }

    public String GetTitle()
    {
        return _titleLabel.Text;
    }

    public void Slot_RerollPressed()
    {
        EmitSignal(nameof(Signal_Reroll), _titleLabel.Text);
    }
}
