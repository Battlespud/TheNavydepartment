using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{

	public static DialogController Controller;
	public static StringEvent DialogEvent = new StringEvent();

	public GameObject DialogParent;
	public GameObject ResponseButtonPrefab;

	public ImageHandler LeftSprite;
	public ImageHandler RightSprite;
	public ImageHandler InnerLeftSprite;
	public ImageHandler InnerRightSprite;
	
	public Text SpeakerText;
	public Text DialogText;

	public GameObject TextBoxes;


	public RectTransform ResponseButtonStartingTransform;
	public List<GameObject> ResponseButtons;

	private void Awake()
	{

	}

    // Use this for initialization
    void Start()
    {
	    StartCoroutine(Initialize());
    }

	IEnumerator Initialize()
	{
		if (Controller != null)
			Debug.LogError("Multiple Dialogcontrollers Found");
		Controller = this;
		Clean();
		while (!Character.Loaded)
		{
			yield return null;
		}
		DialogLoader.Initialize(Application.streamingAssetsPath);
		foreach (var v in Character.CharactersByID.Values)
		{
//			Debug.Log(v.CharacterID + " " + v.CharacterName);
		}
		ChrisTestingDialog();
	}

    void ChrisTestingDialog()
    {
        DialogLine f = new DialogLine("test01");
        f.SetDialog("Testing Testing 123 Testing! It's me, [Player]!");
        f.PossibleResponses.Add("test02");
        f.SpeakerID = "Player";
        f.TargetMoods = new List<KeyValuePair<string, MoodTypes>>() { new KeyValuePair<string, MoodTypes>("Player", MoodTypes.Happy) };
        f.SpeakerMood = MoodTypes.Angry;

        DialogLine g = new DialogLine("test02");
        g.SpeakerID = "Player";
        g.SetDialog("Wow it worked!");
        g.SpeakerMood = MoodTypes.Happy;

        LoadLine(GetDialog("test01"));
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
		ResponseButtons.Clear();

		AssignSprites(Line);
		Character speaker = Character.GetCharacter(Line.SpeakerID);
		
		SpeakerText.text = Character.CharactersByID[Line.SpeakerID].CharacterName;
		DialogText.text = Line.GetDialog();
		float counter = 1.8f;
		float offset = 90;
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
				r.anchoredPosition = new Vector2(ResponseButtonStartingTransform.anchoredPosition.x, ResponseButtonStartingTransform.anchoredPosition.y-(counter*offset));
				ResponseButtonController c = g.GetComponent<ResponseButtonController>();
				c.Assign(D);
			}
			counter++;

		});
		if (Line.PossibleResponses.Count < 1)
		{
			GameObject g = Instantiate(ResponseButtonPrefab, DialogParent.transform);
			RectTransform r = g.GetComponent<RectTransform>();
			r.anchoredPosition = new Vector2(ResponseButtonStartingTransform.anchoredPosition.x, ResponseButtonStartingTransform.anchoredPosition.y-counter*offset);
			ResponseButtonController c = g.GetComponent<ResponseButtonController>();
			c.AssignEnd();
		}
		DialogEvent.Invoke(Line.LineID);
	}

	private void AssignSprites(DialogLine Line)
	{
		Character speaker = Character.GetCharacter(Line.SpeakerID);

		try
		{
			Sprite speakerSprite = speaker.GetSprite((Line.SpeakerMood));
			if (speakerSprite)
				LeftSprite.Assign(speakerSprite, speaker);
		}
		catch{Debug.LogError("Something went wrong with assigning speaker sprites.");}

		int targets = 0;
		foreach (var t in Line.TargetMoods)
		{
			Character c = Character.GetCharacter(t.Key);
			Sprite sprite = c.GetSprite((t.Value));
			if (sprite)
			{

				if (ImageHandler.CharacterIsOnscreen(c))
				{
					ImageHandler h = ImageHandler.GetCharacterImageHandler(c);
					h.Assign(sprite,c);
				}

				else
				{
					switch (targets)
					{
						case(0):
						{
							if (RightSprite.C != null)
							{
								goto case 1;
							}
							RightSprite.Assign(sprite, c);
							break;
						}
						case(1):
						{
							if (InnerRightSprite.C != null)
							{
								goto case 2;
							}
							InnerRightSprite.Assign(sprite, c);
							break;
						}
						case(2):
						{
							if (InnerLeftSprite.C != null)
							{
								goto default;
							}
							InnerLeftSprite.Assign(sprite, c);
							break;
						}
						default:
						{
							Debug.LogError(
								string.Format("Dialog Line [{0}] has too many targets and not all will be displayed", Line.LineID));
							break;
						}
					}
				}
			}
		}
	}

	void Clean()
	{
		LeftSprite.Unassign();
		RightSprite.Unassign();
		InnerLeftSprite.Unassign();
		InnerRightSprite.Unassign();
		SpeakerText.text = "";
		DialogText.text = "";
		ResponseButtons.ForEach(x =>
		{
			Destroy(x);
		});
		ResponseButtons.Clear();
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
