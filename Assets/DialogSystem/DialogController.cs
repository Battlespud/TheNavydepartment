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

	public GameObject TextBoxes;


	public RectTransform ResponseButtonStartingTransform;
	public List<GameObject> ResponseButtons;

	private void Awake()
	{
        DialogLoader.Initialize(Application.streamingAssetsPath);
        if (Controller != null)
			Debug.LogError("Multiple Dialogcontrollers Found");
		Controller = this;
	}

	// Use this for initialization
	void Start () {
        //DialogLine f = new DialogLine("test01");
        //f.SetDialog("Testing Testing 123 Testing! It's me, [Player]!");
        //f.PossibleResponses.Add("test02");
        //f.SpeakerID = "Player";
        //f.SpeakerMood = MoodTypes.Angry;

        //DialogLine g = new DialogLine("test02");
        //g.SpeakerID = "Player";
        //g.SetDialog("Wow it worked!");
        //g.SpeakerMood = MoodTypes.Happy;
        //LoadLine(GetDialog("test01"));
        LoadLine(GetDialog("PlayerMeetsTanya"));
    }

	DialogLine GetDialog(string lineID)
	{
		return DialogLine.DialogByLineID[lineID];
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.H))
		{
			HideText(TextBoxes.active);
		}
	}


	public void LoadLine(DialogLine Line)
	{
		ResponseButtons.ForEach(x =>
		{
			Destroy(x);
		});
		LeftSprite.sprite = null;
		Character speaker = Character.GetCharacter(Line.SpeakerID);
		try
		{
			Sprite speakerSprite = speaker.GetSprite((Line.SpeakerMood));
			if (speakerSprite)
				LeftSprite.sprite = speakerSprite;
		}
		catch{}
		SpeakerText.text = Character.CharactersByID[Line.SpeakerID].CharacterName;
		DialogText.text = Line.GetDialog();
		int counter = 0;
		float offset = 1.1f;
		Line.PossibleResponses.ForEach(x =>
		{
			DialogLine D = null;
			try{D = DialogLine.DialogByLineID[x];}catch{
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
		if (Line.PossibleResponses.Count < 1 && Line.SpeakerID == "Player")
		{
			GameObject g = Instantiate(ResponseButtonPrefab, DialogParent.transform);
			RectTransform r = g.GetComponent<RectTransform>();
			r.anchoredPosition = new Vector2(ResponseButtonStartingTransform.anchoredPosition.x, ResponseButtonStartingTransform.anchoredPosition.y-r.sizeDelta.y*offset);
			ResponseButtonController c = g.GetComponent<ResponseButtonController>();
			c.AssignEnd();
		}
	}

	void Clean()
	{
		LeftSprite.sprite = null;
		RightSprite.sprite = null;
		SpeakerText.text = "";
		DialogText.text = "";
		ResponseButtons.ForEach(x =>
		{
			Destroy(x);
		});
	}
	
	public void EndConversation()
	{
		Clean();
		Hide(true);
	}

	public void Respond(DialogLine response)
	{
		LoadLine(response);
	}


	public void Hide(bool b)
	{
		DialogParent.SetActive(!b);
	}

	public void HideText(bool b)
	{
		TextBoxes.SetActive(!b);
	}
}
