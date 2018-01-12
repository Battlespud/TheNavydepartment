using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public static Dictionary<string,Character>CharactersByID = new Dictionary<string, Character>();

    public string CharacterID;
    
    public string CharacterName;
    public string CharacterNameShort;

    public int Relationship;


    void Awake()
    {
        CharactersByID.Add(CharacterID,this);
    }

}
