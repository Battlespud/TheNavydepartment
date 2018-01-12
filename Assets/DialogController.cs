using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{

	public static DialogController Controller;


	public GameObject DialogParent;
	public GameObject ResponseButtonPrefab;

	public Image LeftSprite;
	public Image RightSprite;

	public Text SpeakerText;
	public Text DialogText;


	public RectTransform ResponseButtonStartingTransform;
	public List<GameObject> ResponseButtons;

	private void Awake()
	{
		if(Controller != null)
			Debug.LogError("Multiple Dialogcontrollers Found");
		Controller = this;
	}

	// Use this for initialization
	void Start () {
		DialogLine f = new DialogLine("test01");
		f.SetDialog("Testing Testing 123 Testing! It's me, [Player]!");
		f.PossibleResponses.Add("test02");
		f.SpeakerID = "Player";
		f.SpeakerMood = MoodTypes.Angry;
		
		DialogLine g = new DialogLine("test02");
		g.SpeakerID = "Player";
		g.SetDialog("Wow it worked!");
		g.SpeakerMood = MoodTypes.Happy;
		
		LoadLine(GetDialog("test01"));
	}

	DialogLine GetDialog(string s)
	{
		return DialogLine.DialogByID[s];
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void LoadLine(DialogLine Line)
	{
		ResponseButtons.ForEach(x =>
		{
			Destroy(x);
		});
		LeftSprite.sprite = null;
		Character speaker = Character.GetCharacter(Line.SpeakerID);
		Sprite speakerSprite = speaker.GetSprite((Line.SpeakerMood));
		if (speakerSprite)
			LeftSprite.sprite = speakerSprite;
		SpeakerText.text = Character.CharactersByID[Line.SpeakerID].CharacterName;
		DialogText.text = Line.GetDialog();
		int counter = 0;
		float offset = 1.1f;
		Line.PossibleResponses.ForEach(x =>
		{
			DialogLine D = null;
			try{D = DialogLine.DialogByID[x];}catch{
				Debug.LogError("Invalid LineID: " + x);
			}
			if (D.IsAvailable())
			{
				GameObject g = Instantiate(ResponseButtonPrefab, DialogParent.transform);
				RectTransform r = g.GetComponent<RectTransform>();
				r.anchoredPosition = new Vector2(ResponseButtonStartingTransform.anchoredPosition.x, ResponseButtonStartingTransform.anchoredPosition.y-r.sizeDelta.y*offset);
				ResponseButtonController c = g.GetComponent<ResponseButtonController>();
				c.Assign(D);
			}
		});
	}

	public void Respond(DialogLine response)
	{
		LoadLine(response);
	}


	public void Hide(bool b)
	{
		DialogParent.SetActive(!b);
	}
}
