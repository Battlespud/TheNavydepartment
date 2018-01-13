using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogBuilder
{
    class DialogLine
    {
        public string DialogString;
        public string LineID; //ID of this 
        public string SpeakerID;
        public MoodTypes SpeakerMood;
        public List<KeyValuePair<string, MoodTypes>> TargetMoods;

        #region Responses Only
        //what the npc will say next
        public string PassTarget;
        public string FailTarget;
        public string CritFailTarget;
        //Requirement Fail        // if <= this condition, but > CritFail, failure.
        //Requirement CritFail   //If <= this condition, critical failure.
        //Requirement Available //Will this show up as an available response?
        #endregion Responses Only

        #region NPC Only
        public List<string> PossibleResponses = new List<string>();
        #endregion
    }
}
