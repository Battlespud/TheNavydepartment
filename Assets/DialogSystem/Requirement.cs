using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Different types of requirements will be interpreted differently in RequirementHandler.  Besure to note what type of inputs are required for each next to it.
//All requirementTypes should be positive statements, ie HasItem not DoesntHaveItem
public enum RequirementTypes
{
    IsAlive, //Any number of characterIDs are alive
    HasItem //1 Character([0]) has any number of items
}

[Serializable]
public class Requirement
{

    
    public RequirementTypes _Type;
    //List of IDs, order will generally matter and best practice is Characters -> Other things
    public List<string> Targets;
    //If true, the requirement will be considered met if the enum is true.  Set false to check if character IsDead for example, or DoesntHaveItem.  
        //Because of the way this is handled internally, you must make that decision here, not try to negate the result later.
    public bool Affirmative;
    
    public List<Flag> CheckFlags = new List<Flag>();
    
    public Requirement(RequirementTypes r, List<string> objs, bool RequirementMetIfTrue = true, List<Flag> flags = null)
    {
        _Type = r;
        Targets = objs;
        Affirmative = RequirementMetIfTrue;
        if (flags != null)
        {
            CheckFlags.AddRange(flags);
        }
    }
    
    public Requirement(RequirementTypes r, List<string> objs, List<Flag> flags, bool RequirementMetIfTrue = true)
    {
        _Type = r;
        Targets = objs;
        Affirmative = RequirementMetIfTrue;
        if (flags != null)
        {
            CheckFlags.AddRange(flags);
        }
    }
}




