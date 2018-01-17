using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Events;

public static class Clock
{
	
	public static TimeEvent TimeChange = new TimeEvent();
	
	public static int Day = 0;
		
	private static int[] Time = new int[]{0,0};
			
	public static int GetTime()
	{
		return (Time[0] * 100  + Time[1]);
	}

	public static int[] GetTimeArray()
	{
		return Time;
	}

	public static string GetTimeString()
	{
		string suffix = "PM";
		if (Clock.GetTimeArray()[0] < 12)
			suffix = "AM";
		string hours = Clock.GetTimeArray()[0] < 12 ? Clock.GetTimeArray()[0].ToString() : (Clock.GetTimeArray()[0] - 12).ToString() ;
		if (hours == "0" && suffix == "PM")
			hours = "12";
		string minutes = Clock.GetTimeArray()[1].ToString();
		if (minutes.Length < 2)
		{
			minutes = "0" + minutes;
		}


		return hours + ":" + minutes + " " + suffix;
	}
	
	
	public static void IncrementHours(int i)
	{
		for (int x = 0; x < i; x++)
		{
			IncrementMinutes(60);
		}
	}


	
	public static void IncrementMinutes(int AddedTime)
	{   
		
		/*
		//Hours, Minutes
		int[] AddArray = new int[2]{0,0};
		if (AddedTime < 100) //x or xx
		{
			AddArray = new int[]{0, AddedTime};
		}
		/*
		else
		{
			
			float multiplier = AddedTime / 99;
			for (int i = 0; i < multiplier; i++)
			{
				IncrementTime(99);
			}
			float remainder = multiplier - Mathf.Floor(multiplier);
			IncrementTime((int)(remainder*99));
			return;
		}*/
		
		/*
		else //xxxxxxxxx
		{
			float ms = AddedTime / 100;
			int min = (int) ((ms -  Mathf.Floor(ms))*100);
			int hours = (int)Mathf.Floor(ms);
			//int h = Int32.Parse(AddedTime.ToString()[0].ToString());
			//Debug.Log("is it a byte or "+ h);
			AddArray = new int[] {hours, min};
			Debug.Log(AddedTime);
			Debug.Log(AddArray[0] + " Hours  |  Mins  " + AddArray[1]);
		}
		
		*/
		
		int Limit = 60;
		int result = Time[1] + AddedTime;
		int hours = 0;
		int days = 0;
		while (result >= Limit)
		{
			result -= Limit;
			hours++;
		}
		Time[1] = result;
		Time[0] += hours;
		while (Time[0] >= 24)
		{
			Time[0] -= 24;
			days++;
		}
		ProgressDays(days);
			if (days > 0)
				Debug.Log(days + " days have passed");

		TimeChange.Invoke(GetTime());
	}

	static void ProgressDays(int i)
	{
		//TODO event
		if (i > 0)
		{
			Day += i;

		}
	}
}
