using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ResponseButtonController : MonoBehaviour
{
	[SerializeField]
	public DialogLine Line;
	public Text text;

	void Awake()
	{
		DialogController.Controller.ResponseButtons.Add(gameObject);
		text = GetComponentInChildren<Text>();
	}

	public void Assign(DialogLine L = null)
	{
		Line = L;
		text.text = Line.GetDialog();
	}

	public void AssignEnd()
	{
		text.text = "<End Conversation>";
	}

	public void Respond()
	{
		if(text.text != "<End Conversation>")
			DialogController.Controller.Respond(Line);
		else
			DialogController.Controller.EndConversation();
	}

}
