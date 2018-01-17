using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Linq;

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
        //ALL NPCS MUST HAVE A SCHEDULE SET THAT INCLUDES A POSITION AT 0000 OR EXCEPTIONS WILL OCCUR
        //Schedule
        [SerializeField]
        public List<int>ScheduleTimes = new List<int>();
        [SerializeField]
        public List<Vector3>Positions = new List<Vector3>();

        [SerializeField]
        public Dictionary<int,Vector3> Schedule = new Dictionary<int, Vector3>();
        
        //Recieves the TimeChange event from Clock.cs and determines where that time would be (its index) if it were inserted into the ScheduleTimes list using a binary search algorithm.  
            //Then, it subtracts 1 from that index to get the closest, earlier event (We assume that npcs do not move in between scheduled moves. IE if an npc is home at 4PM and at the store at 6 pm, they are still home at 5:55 PM
            //If the nearest event would have a negative index, like an event at 1am if the npc had no earlier event that morning, then itdefaults to the last event in the schedule (which in most cases should be going to bed) 
            //because we assume they should still be there from last night since they had no scheduled move
        //The schedule is entirely dependent on all lists being both consistent and sorted chronologically.  This will be handled automatically on game start, but adding events manually without calling BuildSchedule() will result in inaccuracies.
        void CheckSchedule(int newTime)
        {
            Vector3 TargetPosition = new Vector3();
            int index = ScheduleTimes.BinarySearch(newTime);
            if (0 <= index)
            {
                Debug.Log(string.Format("Returning time {0} with index of {1}",newTime, index));

                TargetPosition = Schedule[newTime];
            }
            else
            {
                index = ~index;
                index--;
                Debug.Log(string.Format("Returning time {0} with index of {1}",newTime,index));
                if(index >= 0)
                TargetPosition = Schedule[ScheduleTimes[index]];
                else  //Should handle not having an entry at 0
                TargetPosition = Schedule[ScheduleTimes[ScheduleTimes.Count-1]];
            }
            
            Debug.Log(("Closest position is " + TargetPosition));
            Agent.SetDestination(TargetPosition);
        }

        //Sorts and rebuilds the Schedule Times and Positions lists and builds the Schedule dictionary.  Overkill but necessary for keeping the schedule safe while allowing for setting it up in Inspector.
        void BuildSchedule()
        {
           List<KeyValuePair<int,Vector3>> Sorted = new List<KeyValuePair<int, Vector3>>();
            for (int i = 0; i < ScheduleTimes.Count; i++)
            {
                Sorted.Add(new KeyValuePair<int, Vector3>(ScheduleTimes[i], Positions[i]));
            }
            Sorted = Sorted.OrderBy(x => x.Key).ToList();
            Positions.Clear();
            ScheduleTimes.Clear();
            foreach (var v in Sorted)
            {
                ScheduleTimes.Add(v.Key);
                Positions.Add(v.Value);
            }
            for (int i = 0; i < ScheduleTimes.Count; i++)
            {
                Schedule.Add(ScheduleTimes[i], Positions[i]);
            }

        }
        
        //How long speech bubbles will stay open (This only affects speech bubbles, not full dialogues.)
        private const float TimeForSpeechToAppear = 3f;

        //What type of npc is this, this class is intended primarily for repetetive generic npcs.
        public NPCTypes NPCType;
        //If name is blank, the name will just be NPCType.tostring
        public string NPCName;
        
        //Used to determine which sprite to show.  NPCs use onl a single sprite and are symmetrical, so theyre easier to manage with less code.
        public Facing facing;
        public SpriteRenderer Renderer;
        
        //Worldspace UI that appears when InteractButton is pressed
        public Text SpeechTextbox;
        public GameObject SpeechBubble;

        //The blue and red button respectively. Shows up when player is near.
        public GameObject InteractButton;
        public GameObject EndConvoButton;

        //Handles movement
        public NavMeshAgent Agent;
        
        //Hardcoded default lines.  Will iterate through in order and loop infinitely.
        [SerializeField]
        public List<string> LinesOfSpeech = new List<string>(){"Hi Tanya", "Lovely Meteor Shower last weekend huh?", "Real shame how Detroit got nuked"};
        //handles aforementioned looping.
        private int SpeechCounter = 0;

        
        void Awake()
        {
            LinesOfSpeech = new List<string>(){"Hey Tanya!", "Lovely Meteor Shower last weekend huh?", "Shame how Detroit got hit though."};
            SpeechBubble.SetActive(false);
            InteractButton.SetActive(false);
            EndConvoButton.SetActive(false);
            SpeechTextbox.text = "";
            BuildSchedule();
            Renderer = GetComponentInChildren<SpriteRenderer>();
            Agent = GetComponentInParent<NavMeshAgent>();
            Clock.TimeChange.AddListener(CheckSchedule);
            if (NPCName == "")
                NPCName = NPCType.ToString();
        }
        
        //Called whenever player enters or leaves range.
        public void IEnableInteraction(bool b)
        {
                InteractButton.SetActive(b);
            if (!b) //Hides the speech bubble when player walks away too far.
                EndConvo();
        }

        //Called when interact button is pressed by player, makes the speechbubble show up.
        public void IInteract()
        {
            StopAllCoroutines();
            StartCoroutine(ShowSpeech());
        }

        public void EndConvo()
        {
            StopAllCoroutines();
            EndConvoButton.SetActive(false);
            SpeechBubble.SetActive(false);
            EndConvoButton.SetActive(false);
        }

        //Makes the speech bubble show, determines which line to display, waits a little then hides it again.
        IEnumerator ShowSpeech()
        {
            EndConvoButton.SetActive(true);
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
            EndConvoButton.SetActive(false);
        }

        void Update()
        {

            Graphics();    
           
            if (Agent.isOnOffMeshLink)
                    Warp();
        }

        //Teleports the npc to its destination when using doors.
        void Warp()
        {
            Vector3 buffer = Agent.destination;
            Agent.Warp(Agent.currentOffMeshLinkData.endPos);
            Agent.SetDestination(buffer);
        }
    
        //Reads the NavMeshAgent to determine appropriate sprite orientation.
        void Graphics()
            {
                if (Agent.desiredVelocity.x < 0)
                    facing = Facing.LEFT;
                else if (Agent.desiredVelocity.x > 0)
                        facing = Facing.RIGHT;
                
                if(facing == Facing.RIGHT)
                Renderer.flipX = false;
                else
                Renderer.flipX = true;
            }
    }
    
    