using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRefresher : MonoBehaviour
{
	const float Z = -4.665f;

	public bool RecieveInput = false;
	private CharacterController Controller;
	public Facing OverMoveDir;
	public GameObject Player;
	[SerializeField] private float VisualAreaWidth;
	
	// Use this for initialization
	void Start ()
	{
		Player = GameObject.FindWithTag("Player");
		Controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
	//	transform.position = new Vector3(transform.position.x,transform.position.y,Z);
		VisualAreaWidth = Mathf.Abs(Mathf.Tan(Camera.main.fieldOfView) *Mathf.Abs(Player.transform.position.z - transform.position.z));

		if (RecieveInput)
		{
			InputProcess();
		}
	}
	
	void InputProcess()
	{
		Vector2 Movement = new Vector2();

		if(Input.GetKey(KeyCode.A))
			Movement.x -= MovementController.GetSpeed()*1.05f;
		if(Input.GetKey(KeyCode.D))
			Movement.x += MovementController.GetSpeed()*1.05f;
		Move(Movement*Time.deltaTime);
	}
	

	void Move(Vector2 M)
	{
		float XDist = Mathf.Abs(transform.position.x - Player.transform.position.x);
		
		Facing MovementFacing;
		if (M.x > 0)
			MovementFacing = Facing.RIGHT;
		else
			MovementFacing = Facing.LEFT;


		
		if (MovementFacing != OverMoveDir)
		{
			Controller.Move(new Vector3(M.x*(XDist/(VisualAreaWidth*.4f)), 0f, M.y));
		}
	}
}
