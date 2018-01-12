using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public enum MoodTypes
{
    Happy,
    Annoyed,
    Angry,
    Sad,
    Neutral,
    Questioning
    
}

public class Character : MonoBehaviour {

    public static Dictionary<string,Character>CharactersByID = new Dictionary<string, Character>();

    public static Character GetCharacter(string ID)
    {
        try
        {
            return CharactersByID[ID];
        }
        catch
        {
            Debug.LogError("Invalid Character ID: " + ID);
            return null;
        }
    }

    public string CharacterID;
    
    public string CharacterName;
    public string CharacterNameShort;

    public int Relationship;

    public List<Sprite> SpriteList = new List<Sprite>();
    
    Dictionary<MoodTypes,Sprite> SpriteDictionary = new Dictionary<MoodTypes, Sprite>();

    public Sprite GetSprite(MoodTypes m)
    {
        if (SpriteDictionary.ContainsKey(m))
        {
            return SpriteDictionary[m];
        }
        return null;
    }
    
    
    void Awake()
    {
        CharactersByID.Add(CharacterID,this);
        foreach (var s in SpriteList)
        {

            try
            {
               MoodTypes m = (MoodTypes)System.Enum.Parse(typeof(MoodTypes),s.name);
                SpriteDictionary.Add(m,s);
            }
            catch
            {
                Debug.LogError("Failed to parse  "+ CharacterName + " " + s.name);
            }
        }
        StringBuilder sb = new StringBuilder();
        sb.Append(CharacterName + " contains these Sprites: ");
        foreach (var s in SpriteDictionary.Values)
        {
            sb.Append(s.name + ", ");
        }
        Debug.Log(sb.ToString());
    }

}
