using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class ClockUI : MonoBehaviour
{

	public Text ClockText;

	void Awake()
	{
		ClockText = GetComponent<Text>();
		Clock.TimeChange.AddListener(UpdateTime);
	}


	void UpdateTime(int newTime)
	{
		ClockText.text = Clock.GetTimeString();
	}
	
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
