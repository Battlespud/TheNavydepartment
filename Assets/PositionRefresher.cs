using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.UI;

public class PositionRefresher : MonoBehaviour
{
	const float Z = -9.2f;
	const float YDiff = +.31f;

	public bool RecieveInput = false;
	private CharacterController Controller;
	public Facing OverMoveDir;
	public GameObject Player;
	[SerializeField] private float VisualAreaWidth;
	public Camera Cam;
	public Image blackScreen;

	
	// Use this for initialization
	void Start ()
	{
		Cam = GetComponent<Camera>();
		Player = GameObject.FindWithTag("Player");
		Controller = GetComponent<CharacterController>();
		FadeFromBlack();
	}
	
	// Update is called once per frame
	void Update () {
	//	transform.position = new Vector3(transform.position.x,transform.position.y,Z);
		transform.position = new Vector3(transform.position.x, Player.transform.position.y + YDiff,Z);
		VisualAreaWidth = Mathf.Abs(Mathf.Tan(Camera.main.fieldOfView) *Mathf.Abs(Player.transform.position.z - transform.position.z));

		if (RecieveInput)
		{
			InputProcess();
		}
	}
	
	void InputProcess()
	{
		Vector2 Movement = new Vector2();
		if (Player.transform.position.x > transform.position.x)
			OverMoveDir = Facing.LEFT;
		else
			OverMoveDir = Facing.RIGHT;

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

	public void FadeToBlack(float time = .65f)
	{
		blackScreen.color = Color.black;
		blackScreen.canvasRenderer.SetAlpha(0.0f);
		blackScreen.CrossFadeAlpha (1.0f, time, false);
	}
     
	public void FadeFromBlack (float time = .75f)
	{
		blackScreen.color = Color.black;
		blackScreen.canvasRenderer.SetAlpha(1.0f);
		blackScreen.CrossFadeAlpha (0.0f, time, false);
	}

	public void SnapToPlayer()
	{
		transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y + YDiff,Z);
	}
}
