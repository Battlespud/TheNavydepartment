using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ResponseButtonController : MonoBehaviour
{
	public DialogLine Line;
	public Text text;

	void Awake()
	{
		DialogController.Controller.ResponseButtons.Add(gameObject);
		text = GetComponentInChildren<Text>();
	}

	public void Assign(DialogLine L)
	{
		Line = L;
		text.text = Line.GetDialog();
	}

	public void Respond()
	{
<<<<<<< HEAD:Assets/DialogSystem/ResponseButtonController.cs
		if(text.text != "<End Conversation>")
			DialogController.Controller.Respond(Line);
		else
			DialogController.Controller.EndConversation();
=======
		DialogController.Controller.Respond(Line);
>>>>>>> 0ad35304c5cf51ac03de8e62113869e147e1772c:Assets/ResponseButtonController.cs
	}

}
