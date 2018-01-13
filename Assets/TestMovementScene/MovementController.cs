using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;


public enum Facing
{
	LEFT,
	RIGHT
}

public class MovementController : MonoBehaviour
{

	public CharacterController Controller;
	public Facing facing = Facing.RIGHT;
	
	
	public Sprite Right;
	public Sprite Left;

	Dictionary<Facing,Sprite> Sprites = new Dictionary<Facing, Sprite>();
	
	public SpriteRenderer Renderer;
	
	private static float BaseSpeed = 5f;

	public static float GetSpeed()
	{
		return BaseSpeed;
	}
	
	void Awake()
	{
		Controller = GetComponent<CharacterController>();
		Renderer = GetComponentInChildren<SpriteRenderer>();
		Sprites.Add(Facing.RIGHT, Right);
		Sprites.Add(Facing.LEFT, Left);
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		InputProcess();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<IInteractable>() != null)
		{
			other.GetComponent<IInteractable>().IEnableInteraction(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.GetComponent<IInteractable>() != null)
		{
			other.GetComponent<IInteractable>().IEnableInteraction(false);
		}
		
	}

	void InputProcess()
	{
		Vector2 Movement = new Vector2();
		if (Input.GetKey(KeyCode.W))
			Movement.y += BaseSpeed;
		if(Input.GetKey(KeyCode.A))
			Movement.x -= BaseSpeed;
		if (Input.GetKey(KeyCode.S))
			Movement.y -= BaseSpeed;
		if(Input.GetKey(KeyCode.D))
			Movement.x += BaseSpeed;
		Move(Movement);
	}
	

	void Move(Vector2 M)
	{
		if (M.x < 0)
			facing = Facing.LEFT;
		else if (M.x > 0)
			facing = Facing.RIGHT;
		Controller.SimpleMove(new Vector3(M.x, 0f, M.y));
		Renderer.sprite = Sprites[facing];
	}
}
