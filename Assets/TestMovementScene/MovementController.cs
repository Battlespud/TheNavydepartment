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

	public static bool Locked = false;
	
	public CharacterController Controller;
	public Facing facing = Facing.RIGHT;
	
	//We use multiple sprites for the player so they can be a bit more customized, npc just flip a single sprite.
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
		if(!Locked)
		InputProcess();
	}

	//Enables or disables worldspace uis for interacting with objects.  See the Boundary End Zone for a sample
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

	//Catch player input.
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
	
	//Executes previously captured movement and chooses sprite based on last movement.
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
