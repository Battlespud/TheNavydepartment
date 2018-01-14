using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class ZoneEnd : MonoBehaviour, IInteractable
{

	public GameObject TargetSpawn;
	public string NameofDest;

	public bool Passable;
	public float TimeToPass;
	
	public PositionRefresher Cam;

	public GameObject UIPopupEnabled;
	public Text Label;

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
		UIPopupEnabled.SetActive(false);
		GatePosition = DoorAnimator.transform.position;
		DoorAnimator.enabled = false;
		
		Label.text = NameofDest;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void IEnableInteraction(bool b)
	{
		if (b)
		{
			if (Passable)
			{
				UIPopupEnabled.SetActive(true);
			}
			else
			{
				UIPopupDisabled.SetActive(true);
			}
		}
		else
		{
				UIPopupEnabled.SetActive(false);
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
		IEnableInteraction(false);
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
			Player.transform.position = TargetSpawn.transform.position;

		IEnableInteraction(true);


		Cam.SnapToPlayer();
		Cam.FadeFromBlack();

	}
}
