using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DialogBuilder
{
    public static class DialogLoader
    {
        /// <summary>
        /// A chunk identifier type represents the actual tagged line in the dialog text file. The enum values and values in the text file need to be exact
        /// </summary>
        enum ChunkIdentifierTypes //local enum
        {
            DialogString,
            LineID,
            SpeakerID,
            PassTarget,
            FailTarget,
            CriticalFailTarget,
            SpeakerMood,
            TargetMoods,
            Responses,
            Fail,
            CriticalFail,
            Available
        }//end of ChunkIdentifierTypes

        public static Dictionary<string, DialogLine> MasterDialogLineList;

        public static void Initialize()
        {
            MasterDialogLineList = new Dictionary<string, DialogLine>();
        }

        /// <summary>
        /// Reads each file in the Dialogs folder, later amendments will account for a dialog file prefixed by Day for more organization, though its superfluous and wont actually affect the functionality
        /// </summary>
        public static void LoadDialogLinesFromFile(string file)
        {
            DialogLine buffer = new DialogLine();
            List<string> targetMoods = new List<string>();
            using (StreamReader reader = new StreamReader(File.Open(file,FileMode.Open,FileAccess.Read,FileShare.None)))
            {
                do
                {
                    string raw = reader.ReadLine();
                    if (raw.Contains('['))//check for chunk start indicator and instantiate a new DialogLine buffer object
                    {
                        buffer = new DialogLine();
                    }
                    else if (raw.Contains(']')) //check for chunk end indicator and then check to see if the master reference contains key, add to master accordingly
                    {
                        ParseTargetIDMoodPairs(targetMoods, buffer);
                        MasterDialogLineList.Add(buffer.LineID, buffer);
                    }
                    else//in between start and end, check to see if ChunkIdentifierTypes (cit) is contained in the string currently being read, if so switch and set buffer.citValue to the string
                    {
                        foreach (ChunkIdentifierTypes cit in Enum.GetValues(typeof(ChunkIdentifierTypes)))
                        {
                            if (raw.Contains(cit.ToString()))
                            {
                                raw = raw.Substring(raw.IndexOf(':') + 1);
                                switch (cit)
                                {
                                    case ChunkIdentifierTypes.DialogString:
                                        buffer.DialogString = raw;
                                        break;
                                    case ChunkIdentifierTypes.LineID:
                                        buffer.LineID = raw;
                                        break;
                                    case ChunkIdentifierTypes.SpeakerID:
                                        buffer.SpeakerID = raw;
                                        break;
                                    case ChunkIdentifierTypes.PassTarget:
                                        buffer.PassTarget = raw;
                                        break;
                                    case ChunkIdentifierTypes.FailTarget:
                                        buffer.FailTarget = raw;
                                        break;
                                    case ChunkIdentifierTypes.CriticalFailTarget:
                                        buffer.CritFailTarget = raw;
                                        break;
                                    case ChunkIdentifierTypes.SpeakerMood:
                                        buffer.SpeakerMood = (MoodTypes)Enum.Parse(typeof(MoodTypes), raw);
                                        break;
                                    case ChunkIdentifierTypes.TargetMoods:
                                        targetMoods.Clear();
                                        do
                                        {
                                            string targetMood = reader.ReadLine();
                                            if (targetMood.Contains('{'))
                                            {
                                                continue;
                                            }
                                            else if (targetMood.Contains('}'))//end of targetblock block, break loop and read to the next line
                                            {
                                                reader.ReadLine();
                                                break;
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(targetMood))
                                                {
                                                    targetMoods.Add(targetMood); //here its just adding the raw data from the file, it gets parsed at the end of each chunk
                                                }
                                            }
                                        } while (true);
                                        break;
                                    case ChunkIdentifierTypes.Responses:
                                        do
                                        {
                                            string response = reader.ReadLine();
                                            if (response.Contains('{'))
                                            {
                                                continue;
                                            }
                                            else if (response.Contains('}'))//end of response block, break loop and read to the next line
                                            {
                                                reader.ReadLine();
                                                break;
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(response))
                                                {
                                                    buffer.PossibleResponses.Add(response);
                                                }
                                            }
                                        } while (true);
                                        break;
                                    case ChunkIdentifierTypes.Fail:
                                    case ChunkIdentifierTypes.CriticalFail:
                                    case ChunkIdentifierTypes.Available:
                                        //handle requirements later
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                } while (!reader.EndOfStream);
            }
        }//end of PopulateDialogLines

        /// <summary>
        /// Parses kvps from text file into kvp string,MoodTypes. Being as this method is called at the end of each chunk being built, it can be used in the future for additional/broader parsing
        /// </summary>
        /// <param name="targetMoods">list of raw strings from text files</param>
        /// <param name="buffer">buffer ref used to add parsed kvps to</param>
        static void ParseTargetIDMoodPairs(List<string> targetMoods, DialogLine buffer)
        {
            foreach (string targetMood in targetMoods)
            {
                int separatorIndex = targetMood.IndexOf('|');
                string target = targetMood.Substring(0, separatorIndex);
                MoodTypes mood = (MoodTypes)Enum.Parse(typeof(MoodTypes), targetMood.Substring(separatorIndex + 1));
                buffer.TargetMoods.Add(new KeyValuePair<string, MoodTypes>(target, mood));
            }
            //string targetID = line.Substring()
        }//end of ParseTargetIDMoodPairs
    }//end of class
}