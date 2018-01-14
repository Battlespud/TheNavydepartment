using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System.Linq;
using CielaSpike;

public class CharacterLoader : MonoBehaviour
{

	public bool Verbose = false;
	[SerializeField]
	public static bool CharactersLoaded = false;
	public GameObject GameController; //Where we attach the character monos

	public float CompletePercent = 0f;

	private void Awake()
	{
		CompletePercent = 0f;
		CharactersLoaded = false;
		LoadCharacters();
	}

	/*
	 *Each Folder is a character, folders must contain a Character.txt as well as a Sprites folder with properly sized .png files named for MoodTypes.
	 */
	private void LoadCharacters()
	{
		int CharacterCount = 0;
			List<string> CharacterFolders = new List<string> ();
			CharacterFolders.AddRange(Directory.GetDirectories (System.IO.Path.Combine (Application.streamingAssetsPath, "Characters")));
			Log(CharacterFolders.Count + " Character folders detected.");
			int counter = 0;
			foreach (string path in CharacterFolders)
			{
				Character C = GameController.AddComponent<Character>();
				string SpritePath = System.IO.Path.Combine(path, "Sprites");
				//Load Sprites
				foreach (var f in Directory.GetFiles(SpritePath))
				{
					if(!f.Contains(".png"))
						Debug.LogWarning("Non .png images are not supported for character sprites. Transparency will not work.");
					if (!f.Contains(".meta"))
					{
						WWW laughsinchinese = new WWW(f);
						Texture2D preSprite = new Texture2D(4, 4, TextureFormat.DXT1, false);
						;
						laughsinchinese.LoadImageIntoTexture(preSprite);
						Sprite s = Sprite.Create(preSprite, new Rect(0f, 0f, preSprite.width, preSprite.height), new Vector2(.5f, .5f),
							100f);
						s.name = f.Split('\\').Last();
						C.SpriteList
							.Add(s); //Add sprite to characters list, they'll be added to the dictionary automatically during registration.
					}
				}
				//Load Text
				List<string>CharacterText = new List<string>();
				string CharTextPath = System.IO.Path.Combine(path, "Character.txt");
				Log(CharTextPath);
				CharacterText.AddRange(File.ReadAllLines(CharTextPath));
				foreach (string line in CharacterText)
				{
					if (!string.IsNullOrEmpty(line))
					{
						string x ="", id = "";
						try
						{
							 x = line.Split(':')[1].Trim();
							 id = line.Split(':')[0].Trim();
						}
						catch
						{
							Debug.LogError("Invalid Format: " + line);
						}
						switch (id)
						{
							case ("ID"):
							{
								C.CharacterID = x;
								break;
							}
							case ("Name"):
							{
								C.CharacterName = x;
								break;
							}
							case ("ShortName"):
							{
								C.CharacterNameShort = x;
								break;
							}
							default:
							{
								Debug.LogError("Character.txt is formatted incorrectly. Cannot be processed");
								break;
							}
						}
					}
				}
				
				//Finish up
				C.Register();
				counter++;
				CompletePercent = counter / CharacterFolders.Count *100f;
				Log(string.Format("{0} has been processed. Completion Percent is {1}", C.CharacterName, CompletePercent));
			}
		Character.Loaded = true;
		Log(Time.realtimeSinceStartup + " was taken for loading.");
	}

	


	//image dictionary via index, input when coroutine finishes and use initialization variable plus while loop to wait before loading;


	void Log(string s)
	{
		if(Verbose)
			Debug.LogWarning(s);
	}
	
}
