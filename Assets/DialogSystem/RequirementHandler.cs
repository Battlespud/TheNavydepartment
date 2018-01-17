using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RequirementHandler {

    public static bool Evaluate(Requirement R)
    {
        Character _C;
        
        switch (R._Type)
        {
            case RequirementTypes.IsAlive:
                foreach (var c in R.Targets)
                {
                    if (!Character.GetCharacter(c).Alive)
                        return false;
                }
                break;
            case RequirementTypes.HasItem:
                _C =  Character.GetCharacter(R.Targets[0]);
                for (int i = 0; i < R.Targets.Count; i++)
                {
                    if (i > 0)
                    {
                        Item L = Item.GetItem(R.Targets[i]);
                        if (!_C.Items.Contains(L))
                            return false;
                    }
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        return true;
    }
}

//placeholder for testing
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