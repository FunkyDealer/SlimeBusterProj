using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogLine 
{
    public string Line { get;  private set; }
    public PortraitSystem.Character Character { get; private set; }
    public string Emotion;

    public DialogLine(string line, PortraitSystem.Character character, string emotion)
    {
        this.Line = line;
        this.Character = character;
        this.Emotion = emotion;
    }

    public DialogLine(DialogLine line)
    {
        this.Line = line.Line;
        this.Character = line.Character;
        this.Emotion = line.Emotion;
    }

}
