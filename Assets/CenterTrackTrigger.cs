using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterTrackTrigger : MonoBehaviour {

	
	public Camera Cam;
	public Rigidbody CRb;
	private PositionRefresher pos;
	
	public GameObject Player;
	
	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag("Player");
		Cam = Camera.main;
		CRb = Cam.GetComponent<Rigidbody>();
		pos = Cam.GetComponent<PositionRefresher>();
	}
	
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == Player)
			pos.RecieveInput = false;
		//		CRb.isKinematic = true;
	}
	
	
	private void OnTriggerExit(Collider other)
	{
		if(other.gameObject == Player)
			pos.RecieveInput = true;

		//	CRb.isKinematic = false;
	}
}
