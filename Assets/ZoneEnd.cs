using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class ZoneEnd : MonoBehaviour, IInteractable
{

	public GameObject SpawnRight;
	public GameObject SpawnLeft;
	public string RightName;
	public string LeftName;

	public Text LabelLeft;
	public Text LabelRight;
	
	
	public bool Passable;
	public float TimeToPass;
	
	public PositionRefresher Cam;

	public GameObject UIPopupEnabledLeft;
	public GameObject UIPopupEnabledRight;

	public GameObject UIPopupDisabled;
	public Animator DoorAnimator;
	public Animation DoorAnimation;
	public GameObject GatePrefab;
	public Vector3 GatePosition;
	
	
//	public WorldController World;
	public GameObject Player;
	
	// Use this for initialization
	void Start ()
	{
		Player = GameObject.FindGameObjectWithTag("Player");
		Cam = Camera.main.GetComponent<PositionRefresher>();
		UIPopupDisabled.SetActive(false);
		UIPopupEnabledLeft.SetActive(false);
		UIPopupEnabledRight.SetActive(false);
		GatePosition = DoorAnimator.transform.position;
		DoorAnimator.enabled = false;
		
		LabelRight.text = LeftName;
		LabelLeft.text = RightName;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void IEnableInteraction(bool b)
	{
		if (b)
		{
			if (Passable)
				if (Player.transform.position.x < transform.position.x)
				{
					UIPopupEnabledLeft.SetActive(true);
				}
				else
				{
					UIPopupEnabledRight.SetActive(true);
				}		
			else
				UIPopupDisabled.SetActive(true);
		}
		else
		{
				UIPopupEnabledLeft.SetActive(false);
				UIPopupEnabledRight.SetActive(false);

				UIPopupDisabled.SetActive(false);
		}
	}

	public void IInteract()
	{
		Cam.FadeFromBlack(0f);

		TransportPlayer();
	}

	void TransportPlayer()
	{
		StopAllCoroutines();
		StartCoroutine(UseDoor());

	}

	IEnumerator UseDoor()
	{
		DoorAnimator.enabled = true;

		float f = .9f;
		while (f > 0f)
		{
			f -= Time.deltaTime;
			yield return null;
		}
		
		Cam.FadeToBlack();


		//	World.Refresh();
		float t = 1f;
		while (t > 0f)
		{
			t -= Time.deltaTime;
			yield return null;
		}
		Destroy(DoorAnimator.gameObject);
		GameObject Gate = Instantiate(GatePrefab, transform);
		DoorAnimator = Gate.GetComponent<Animator>();
		Gate.transform.position = GatePosition;
		DoorAnimator.enabled = false;
		IEnableInteraction(false);
		if (Player.transform.position.x < transform.position.x)
		{
			Player.transform.position = SpawnRight.transform.position;
		}
		else
		{
			Player.transform.position = SpawnLeft.transform.position;
		}
		IEnableInteraction(true);


		Cam.SnapToPlayer();
		Cam.FadeFromBlack();

	}
}
