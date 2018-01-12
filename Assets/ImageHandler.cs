using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageHandler : MonoBehaviour
{
	
	private static Dictionary<Character,ImageHandler> CharacterPositionsDict = new Dictionary<Character, ImageHandler>();

	public static ImageHandler GetCharacterImageHandler(Character c)
	{
		if (CharacterIsOnscreen(c))
		{
			ImageHandler i = CharacterPositionsDict[c];
			return i;
		}
		return null;
	}

	public static bool CharacterIsOnscreen(Character c)
	{
		if (c == null)
			return false;
		return CharacterPositionsDict.ContainsKey(c);
	}

	public int PositionIndex; //0 is Leftmost
	
	public Character C;
	private Image I;
	private bool Fading = false;

	void Awake()
	{
		I = GetComponent<Image>();
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	public void Assign(Sprite s, Character c)
	{
		if (C != null)
			CharacterPositionsDict.Remove(C);
		if (c == C)
		{
			InstantAssign(s);
		}
		else
		{
			if (I.sprite == null)
			{
				I.sprite = s;
				StartCoroutine(Fade(true));
			}
			else
			{
				Debug.Log("Dif chars, so fading out then in");
				StartCoroutine(Fade(false));
				I.sprite = s;
				StartCoroutine(Fade(true));

			}
		}
		try
		{
			CharacterPositionsDict.Add(C, this);
		}
		catch
		{
			Debug.LogError("Character already on screen: ");
		}
		C = c;
	}

	public void InstantAssign(Sprite s)
	{
		I.sprite = s;
		StartCoroutine(Fade(true));
	}
	
	public void Unassign()
	{
		if(CharacterIsOnscreen(C))
			CharacterPositionsDict.Remove(C);
		C = null;
		StartCoroutine(Fade(false));
	}


	IEnumerator Fade(bool fadein)
	{
		while (Fading)
		{
			yield return null;
		}
		Fading = true;
		Color c = I.color;
		float target;
		if (fadein)
			target = 1f;
		else
			target = 0f;

		float fadeRate = 2f;
		while (Mathf.Abs(I.color.a - target) > .1)
		{
			c.a = Mathf.Lerp(c.a, target, fadeRate * Time.deltaTime);
			I.color = c;
			yield return null;
		}
		c.a = target;
		I.color = c;
		if (!fadein)
			I.sprite = null;
		Fading = false;
	}
}
