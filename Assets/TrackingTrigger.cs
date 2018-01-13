using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingTrigger : MonoBehaviour
{

	public Facing facing;

	public Camera Cam;
	public Rigidbody CRb;
	
	public GameObject Player;
	
	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag("Player");
		Cam = Camera.main;
		CRb = Cam.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
		//	Cam.transform.parent = Player.transform;
			Vector3 Force = new Vector3(MovementController.GetSpeed()*45f ,0f,0f);
			if (facing == Facing.LEFT)
			{
				CRb.AddForce(Force*-1f);
			}
			else
			{
				CRb.AddForce(Force*1f);

			}
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
		//	Cam.transform.parent = null;
		}	
	}


}
