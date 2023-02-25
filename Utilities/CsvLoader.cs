using Godot;
using System;
using System.Collections.Generic;

public class CsvLoader
{
    public void LoadCsv(StringDictionary targetDict)
    {
        File file = new File();
        file.Open("res://Resources/Data/GameJamIdeas.txt", File.ModeFlags.Read);
        if(file.IsOpen())
        {
            while (file.GetPosition() < file.GetLen())
            {
                String[] line = file.GetCsvLine();
                
                for(int i = 0; i < (int)StringDictionary.ColumnName.numColumns; ++i)
                {
                    if(i > line.Length)
                    {
                        continue;
                    }

                    // At the column in the dictionary,
                    // Add the string at the column from the csv file
                    targetDict.AddToColumn((StringDictionary.ColumnName)i, line[i]);
                }
            }
        }
        else
        {
            GD.PrintErr("Failed to open file!");
        }
        file.Close();
    }
}
