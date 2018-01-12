using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class DialogLine
{

    public static Dictionary<string,DialogLine> DialogByID = new Dictionary<string, DialogLine>();

    public string LineID; //ID of this line

    string Dialog;

    public string SpeakerID;
    public string TargetID;

    #region Responses Only
    
    //What the npc will say next 
    public string PassTarget;           
    public string FailTarget;
    public string CritFailTarget;

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
        sb.Append(Dialog);
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
        try
        {
            sb.Replace("[Player]", Character.CharactersByID["Player"].CharacterName);
        }
        catch
        {
            Debug.LogError("Invalid Target ID: " + "Player");
        }
        return sb.ToString();
    }



    public DialogLine(string thisID)
    {
        LineID = thisID;

        try
        {
            DialogByID.Add(LineID,this);
        }
        catch
        {
            Debug.LogError("Duplicate LineID: " + LineID);
        }
    }

}
