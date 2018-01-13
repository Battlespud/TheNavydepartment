﻿using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine.UI;

public enum NPCTypes
{
    Soldier,
    Sailor,
    Officer,
    Man,
    Woman,
    Child
}

    public class NPC : MonoBehaviour, IInteractable
    {
        private const float TimeForSpeechToAppear = 3f;

        public NPCTypes NPCType;
        
        public Text SpeechTextbox;
        public GameObject SpeechBubble;

        public GameObject InteractButton;


        
        [SerializeField]
        public List<string> LinesOfSpeech = new List<string>(){"Hi Tanya", "Lovely Meteor Shower last weekend huh?", "Real shame how Detroit got nuked"};

        private int SpeechCounter = 0;

        
        void Awake()
        {
            LinesOfSpeech = new List<string>(){"Hi Tanya!", "Lovely Meteor Shower last weekend huh?", "Real shame how Detroit got nuked.."};
            SpeechBubble.SetActive(false);
            InteractButton.SetActive(false);
            SpeechTextbox.text = "";
        }
        
        public void IEnableInteraction(bool b)
        {
                InteractButton.SetActive(b);
        }

        public void IInteract()
        {
            StopAllCoroutines();
            StartCoroutine(ShowSpeech());
        }

        IEnumerator ShowSpeech()
        {
            if (SpeechCounter >= LinesOfSpeech.Count)
                SpeechCounter = 0;
            try
            {
                SpeechTextbox.text = LinesOfSpeech[SpeechCounter];

            }
            catch 
            {
                Debug.LogError("SpeechCounter index is invalid: " + SpeechCounter);
            }
            SpeechBubble.SetActive(true);
            SpeechCounter++;
            float time = 0f;
            while (time < TimeForSpeechToAppear)
            {
                time += Time.deltaTime;
                yield return null;
            }
            SpeechBubble.SetActive(false);
        }
    }
    
    