using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Interprets a requirement and determines whether it has been satisfied or not.
    //Quickest to check for breaches to the conditional and immediately return false if one is found.
public static class RequirementHandler {

    public static bool Evaluate(Requirement R)
    {
        Character _C;

        foreach (var f in R.CheckFlags)
        {
            bool? b = FlagHandler.GetFlag(f.ID);
            if (b == null || b != f.ToSet) ;
        }
        
        //Switch over types of requirements and solve them independently.
        switch (R._Type)
        {
            case RequirementTypes.IsAlive:
                foreach (var c in R.Targets)
                {
                    if (R.Affirmative)
                    {//Characters are all Alive
                        if (!Character.GetCharacter(c).Alive)
                            return false;
                    } 
                    else
                    {//Characters are All Dead
                        if (Character.GetCharacter(c).Alive)
                            return false;
                    }
                }
                break;
            case RequirementTypes.HasItem:
                _C =  Character.GetCharacter(R.Targets[0]);
                if (R.Affirmative)
                { //Character Has All Items
                    for (int i = 0; i < R.Targets.Count; i++)
                    {
                        if (i > 0)
                        {
                            Item L = Item.GetItem(R.Targets[i]);
                            if (!_C.Items.Contains(L))
                                return false;
                        }
                    }
                }
                else
                { //Character Does Not Have any Items
                    for (int i = 0; i < R.Targets.Count; i++)
                    {
                        if (i > 0)
                        {
                            Item L = Item.GetItem(R.Targets[i]);
                            if (_C.Items.Contains(L))
                                return false;
                        }
                    }
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        return true;
    }
}

//just a placeholder for testing requireements
public class Item 
{
    public string ID;
    
    public static Dictionary<string,Item>ItemsByID = new Dictionary<string, Item>();
    
    public static Item GetItem(string id)
    {
        try
        {
            return ItemsByID[id];
        }
        catch
        {
            Debug.LogError("Invalid Item ID: " + id);
            return null;
        }
    }

    public Item(string id)
    {
        ID = id;
       ItemsByID.Add(id,this);
    }
}