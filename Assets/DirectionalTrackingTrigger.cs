using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalTrackingTrigger : MonoBehaviour
{

	public PositionRefresher Pos;
	public Facing facing;
	
	// Use this for initialization
	void Start ()
	{
		Pos = Camera.main.gameObject.GetComponent<PositionRefresher>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
			Pos.OverMoveDir = facing;
	}
}
