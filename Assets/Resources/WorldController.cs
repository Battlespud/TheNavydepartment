using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class WorldController : MonoBehaviour {

	//Time is treated as a 4 digit int, military time is used.
	//ex: 1420 is 2:20pm.  

	public int Increment = 1;
	
	
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Clock.IncrementMinutes(Increment);
		}
	}
}
