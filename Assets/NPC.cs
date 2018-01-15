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
                TargetPosition = Schedule[ScheduleTimes[index]];
            }
            
            Debug.Log(("Closest position is " + TargetPosition));
            Agent.SetDestination(TargetPosition);
        }

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
        
        private const float TimeForSpeechToAppear = 3f;

        public NPCTypes NPCType;
        
        public Text SpeechTextbox;
        public GameObject SpeechBubble;

        public GameObject InteractButton;
        public GameObject EndConvoButton;

        public NavMeshAgent Agent;
        
        [SerializeField]
        public List<string> LinesOfSpeech = new List<string>(){"Hi Tanya", "Lovely Meteor Shower last weekend huh?", "Real shame how Detroit got nuked"};

        private int SpeechCounter = 0;

        
        void Awake()
        {
            LinesOfSpeech = new List<string>(){"Hi Tanya!", "Lovely Meteor Shower last weekend huh?", "Real shame how Detroit got nuked.."};
            SpeechBubble.SetActive(false);
            InteractButton.SetActive(false);
            EndConvoButton.SetActive(false);
            SpeechTextbox.text = "";
            BuildSchedule();
            Agent = GetComponent<NavMeshAgent>();
            Clock.TimeChange.AddListener(CheckSchedule);
        }
        
        public void IEnableInteraction(bool b)
        {
                InteractButton.SetActive(b);

            if (!b)
            {
                EndConvo();
            }
        }

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
            if (Agent.isOnOffMeshLink)
            {
                Vector3 buffer = Agent.destination;
                Agent.Warp(Agent.currentOffMeshLinkData.endPos);
                Agent.SetDestination(buffer);
            }
        }
    }
    
    