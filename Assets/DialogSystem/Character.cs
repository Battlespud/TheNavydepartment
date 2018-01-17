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

public class Character : MonoBehaviour
{
    //Game should not start until this is true, set by CharacterLoader at awake.  If this is staying false, then you probably forgot to put a CharacterLoader component somewhere in your scene.
    public static bool Loaded = false;
    //All Characters
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
    } //Convenience
 
    //How we organize and find the character
    public string CharacterID;
    
    public string CharacterName;
    public string CharacterNameShort; //Nickname

    public int Relationship;         //Relationship to the player, will probably expand into a struct of its own later.

    public bool Alive = true;        //Character is still kicking, just used for testing requirements atm.
    public List<Item> Items = new List<Item>(); //Used for testing requirements

    //Sprites must be named for the MoodType they are inteded to represent.
    public List<Sprite> SpriteList = new List<Sprite>();  
    //We move our sprites here from Spritelist on game start, this is because Dictionaries cant be edited in inspector.
    Dictionary<MoodTypes,Sprite> SpriteDictionary = new Dictionary<MoodTypes, Sprite>();

    public Sprite GetSprite(MoodTypes m)
    {
        if (SpriteDictionary.ContainsKey(m))
        {
            return SpriteDictionary[m];
        }
        return null;
    }
    
    
    public void Register()
    {
        CharactersByID.Add(CharacterID,this);
        foreach (var s in SpriteList)
        {
            string parsable = s.name;
            if (s.name.Contains("."))
            {
                parsable = s.name.Split('.')[0];
            }
            s.name = parsable;
            try
            {
               MoodTypes m = (MoodTypes)System.Enum.Parse(typeof(MoodTypes), parsable );
                SpriteDictionary.Add(m,s);
            }
            catch
            {
                Debug.LogError("Failed to parse Sprite Name to MoodTypes  "+ CharacterName + " " + parsable);
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

    private void Update()
    {
  
    }
}
