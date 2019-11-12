using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//struct for portrait data, keeps track of what type of portrait to show, and for how long
public struct Portrait
{
    public string type;
    public float time;
    public Portrait(string typ, float tim)
    {
        type = typ;
        time = tim;
    }
}

/// <summary>
/// dialouge class, stores short text , longText, speaker, line ID, default options in case of timers, options or a list of other lines to look at next given choice, portrait transitions in case want more than one  
/// basic setup, modify as necessary for your game and what information you want to be stored for every dialouge scene, could add things like time for timer etc
    /// </summary>

public class Dialogue
{
    public string
        shortText,//text that displays when this dialogue option is listed as a choice
        longText,//the resulting full text that results from clicking the shortText as a button
        speaker;//who is speaking "Gavin" for gav, anything else for the perp
    public int ID,
        defaultOption;//the unique identifier for this diaglogue object
    public float maxTimer, maxStressTimePenalty;//max time and max time lost due to stress, respectively
    public List<int> options;//IDs of other Dialogue entries that are options in response to this dialogue.
    public List<Portrait> portraitTransitions;//list of portrait types as well as times to set the character talking to, visually

    public Dialogue(int id, Dictionary<string, object> input)
    {
        shortText = (string)input["shortText"];
        longText = (string)input["longText"];
        speaker = (string)input["speaker"];
        ID = id + 2;//the spreadsheet is offset by 2 from the true value so we store it as its marked
        defaultOption = (int)input["default"];
        //building options list
        int n = 0;
        options = new List<int>();
        //options are formatted with "a#,#" with pound being the number to look for in the script for the option text, a being a delimeter, inside a string parenthesis 
        string s = (string)input["options"];
        foreach(string st in s.Replace("a", "").Split(','))
        {
            ++n;
            options.Add(int.Parse(st.Trim()));
        }
        
        //building portrait list using n from last list
        // formatting for portraittransitions:  (nameOfPortrait,timeToChange)
        //if do not want portrait to change timeToChange = 0
        s = (string)input["portraitTransitions"];
        portraitTransitions = new List<Portrait>();
       string[] toGrabFrom = s.Replace("(", "").Replace(")", "").Replace(" ", "").Split(',');
        for (int i = 0; i < toGrabFrom.Length; i += 2)
        {
            portraitTransitions.Add(new Portrait(toGrabFrom[i], float.Parse(toGrabFrom[i + 1])));
        }
    }
}
