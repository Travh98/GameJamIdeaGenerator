using Godot;
using System;
using System.Collections.Generic;

public class StringDictionary : Node
{
    // Dictionary of column titles to csv column
    public Dictionary<ColumnName, List<String>> Dict;
    RandomNumberGenerator _randGen;

    public enum ColumnName : int
    {
        Characters = 0,
        Places,
        GameplayLoopItems,
        Perspective,
        Presentation,
        ArtStyle,
        numColumns
    }

    public override void _Ready()
    {
        Dict = new Dictionary<ColumnName, List<string>>();

        for(int i = 0 ; i < (int)ColumnName.numColumns; ++i)
        {
            Dict.Add((ColumnName)i, new List<string>());
        }

        _randGen = new RandomNumberGenerator();
        _randGen.Randomize();
    }

    public void PrintDebug()
    {
        for(int i = 0 ; i < (int)ColumnName.numColumns; ++i)
        {
            foreach(String text in Dict[(ColumnName)i])
            {
                GD.Print(text + "\n");
            }
        }
    }

    public void AddToColumn(ColumnName column, String text)
    {
        Dict[column].Add(text);
    }

    public String GetRandomText(ColumnName column)
    {
        if(Dict[column] == null)
        {
            return "NA";
        }
        if(Dict[column].Count < 1)
        {
            return "NA";
        }
        int index = _randGen.RandiRange(1, Dict[column].Count - 1);
        return Dict[column][index];
    }
}