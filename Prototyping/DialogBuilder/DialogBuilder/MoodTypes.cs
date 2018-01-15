using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogBuilder
{
    /// <summary>
    /// Used to distinguish mood of all game characters, including during dialogs
    /// </summary>
    public enum MoodTypes : byte
    {
        Happy,
        Annoyed,
        Angry,
        Sad,
        Neutral,
        Questioning
    }//end of MoodTypes
}//end of namespace