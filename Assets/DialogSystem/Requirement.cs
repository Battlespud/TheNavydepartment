using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum RequirementTypes
{
    IsAlive, //Any number of characterIDs are alive
    HasItem //1 Character([0]) has any number of items
}

public class Requirement
{

    public RequirementTypes _Type;
    public List<string> Targets;
    public bool Affirmative;
    
    public Requirement(RequirementTypes r, List<string> objs, bool RequirementMetIfTrue = true)
    {
        _Type = r;
        Targets = objs;
        Affirmative = RequirementMetIfTrue;
    }
}



