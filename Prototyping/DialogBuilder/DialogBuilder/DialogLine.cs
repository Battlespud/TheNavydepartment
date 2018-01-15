using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogBuilder
{
    /// <summary>
    /// Reps a containers for all relevant data for each line a character says
    /// </summary>
    public class DialogLine
    {
        /// <summary>
        /// Unique identifier for a dialogline
        /// </summary>
        public string LineID;
        /// <summary>
        /// The actual text being said
        /// </summary>
        public string DialogString;
        /// <summary>
        /// Line ID of the line signifying a pass
        /// </summary>
        public string PassID;
        /// <summary>
        /// Line ID of the line signifying a fail
        /// </summary>
        public string RegFailID;
        /// <summary>
        /// Line ID of the line signifying a critical fail
        /// </summary>
        public string CritFailID;
        /// <summary>
        /// One of the preset character names from the CharacterNames enum
        /// </summary>
        public CharacterNames SpeakerID;
        /// <summary>
        /// The current mood type for the speaker of this dialogline
        /// </summary>
        public MoodTypes SpeakerMood;
        /// <summary>
        /// pairs of characters being spoken to and their moods for this dialogline
        /// </summary>
        public Dictionary<CharacterNames, MoodTypes> TargetMoodPairs;
        /// <summary>
        /// A list of Line IDs that can serve as possble responses
        /// </summary>
        public List<string> Responses;
        //Requirement Fail        // if <= this condition, but > CritFail, failure.
        //Requirement CritFail   //If <= this condition, critical failure.
        //Requirement Available //Will this show up as an available response?

        public DialogLine()
        {
            TargetMoodPairs = new Dictionary<CharacterNames, MoodTypes>();
            Responses = new List<string>();
        }//end of DialogLine ctor
    }//end of DialogLine
}//end of namespace