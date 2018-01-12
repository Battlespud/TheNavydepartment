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
		DialogController.Controller.Respond(Line);
	}

}
