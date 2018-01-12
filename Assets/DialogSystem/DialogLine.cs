using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

[Serializable]
public class DialogLine
{

    public static Dictionary<string,DialogLine> DialogByLineID = new Dictionary<string, DialogLine>();

    
    public string LineID; //ID of this line

    public string DialogString;

    public string SpeakerID;
    public string TargetID;

    #region Responses Only
    
    //What the npc will say next 
    public string PassTarget;           
    public string FailTarget;
    public string CritFailTarget;

    public MoodTypes SpeakerMood;
    public MoodTypes TargetMood;
    
    //Requirement Fail        // if <= this condition, but > CritFail, failure.
    //Requirement CritFail   //If <= this condition, critical failure.
    //Requirement Available //Will this show up as an available response?
    #endregion Responses Only
    
    
    #region NPC Only
    public List<string> PossibleResponses = new List<string>();
    #endregion
    
    public string GetDialog()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(DialogString);
        try
        {
            sb.Replace("[Speaker]", Character.CharactersByID[SpeakerID].CharacterName);
        }
        catch
        {
            Debug.LogError("Invalid Speaker ID: " + SpeakerID);
        }
        try
        {
            sb.Replace("[SpeakerShort]", Character.CharactersByID[SpeakerID].CharacterNameShort);
        }
        catch
        {
            Debug.LogError("Invalid Speaker ID: " + SpeakerID);
        }
        if (!String.IsNullOrEmpty(TargetID))
        {
            try
            {
                sb.Replace("[Target]", Character.CharactersByID[TargetID].CharacterName);
            }
            catch
            {
                Debug.LogError("Invalid Target ID: " + TargetID);
            }
            try
            {
                sb.Replace("[TargetShort]", Character.CharactersByID[TargetID].CharacterNameShort);
            }
            catch
            {
                Debug.LogError("Invalid Target ID: " + TargetID);
            }
        }
        try
        {
            sb.Replace("[Player]", Character.CharactersByID["Player"].CharacterName);
        }
        catch
        {
            Debug.LogError("Invalid Player ID: " + "Player");
        }
        return sb.ToString();
    }

    public void SetDialog(string s)
    {
        DialogString = s;
    }
    public bool IsAvailable()
    {
        //TODO Evaluate AvailableReq
        return true;
    }


    //public DialogLine(string thisID)
    //{
    //    LineID = thisID;

    //    try
    //    {
    //        DialogByLineID.Add(LineID,this);
    //    }
    //    catch
    //    {
    //        Debug.LogError("Duplicate LineID: " + LineID);
    //    }
    //}

    /// <summary>
    /// Really just a debug format
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        StringBuilder responses = new StringBuilder();
        for (int i = 0; i < PossibleResponses.Count; i++)
        {
            responses.AppendLine(PossibleResponses[i]);
        }
        return string.Format("LineID: {0}\n{1}:\tHello{2}, {3}.\nHere are your possible responses:\n{4}", LineID, SpeakerID, TargetID, DialogString, responses.ToString());
    }//end of ToString override
}
