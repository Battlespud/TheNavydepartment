using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Flag
{
	public string ID;
	public bool ToSet;

	public Flag(string i, bool b)
	{
		ID = i;
		ToSet = b;
	}

	public void Set()
	{
        FlagHandler.SetFlag(this);
	}
}

public static class FlagHandler
{

	private const bool Verbose = false;
	public static Dictionary<string,bool> Flags = new Dictionary<string, bool>();

	public static void SetFlag(Flag f)
	{
		if (Flags.ContainsKey(f.ID))
		{
			Flags[f.ID] = f.ToSet;
			if (Verbose)
				Debug.Log("Overwriting flag " + f.ID);
		}
		else
		{
			Flags.Add(f.ID, f.ToSet);
			if(Verbose)
				Debug.Log("Adding flag " + f.ID);
		}
	}

	public static bool? GetFlag(string id)
	{
		bool result;
		Flags.TryGetValue(id, out result);
		return result;
	}

	public static List<string> GetAllFlagIDs()
	{
		return new List<string> (Flags.Keys);
	}
	
}
