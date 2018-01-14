﻿using System;
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
    public static bool Loaded = false;
    
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

    public bool Registered = false;

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
        Registered = true;
    }

    private void Update()
    {
  
    }
}
