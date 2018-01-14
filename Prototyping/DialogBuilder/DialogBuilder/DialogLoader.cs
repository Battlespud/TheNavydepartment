using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DialogBuilder
{
    /// <summary>
    /// Handles read/write and conversion of dialog text files to and from DialogLine objects
    /// </summary>
    public static class DialogLoader
    {
        /// <summary>
        /// Enum local to DialogLoader class only; A chunk identifier represents verbatim, the tag found in a dialog text file. They must be exact
        /// </summary>
        enum ChunkIdentifierTypes : byte
        {
            /// <summary>
            /// Unique identifier for a DialogLine
            /// </summary>
            LineID,
            /// <summary>
            /// Exact text being spoken for the DialogLine
            /// </summary>
            DialogString,
            /// <summary>
            /// The current speaker of this DialogLine from the CharacterNames
            /// </summary>
            SpeakerID,
            /// <summary>
            /// The current mood of the speaker of this DialogLine from the MoodTypes
            /// </summary>
            SpeakerMood,
            /// <summary>
            /// The Line ID to the pass DialogLine
            /// </summary>
            Pass,
            /// <summary>
            /// The Line ID to the fail DialogLine
            /// </summary>
            Fail,
            /// <summary>
            /// The Line ID to the crit fail DialogLine
            /// </summary>
            CritFail,
            /// <summary>
            /// A list of possible responses 
            /// </summary>
            Responses,
            /// <summary>
            /// List of targets (listeners) for this DialogLine; Each target must have a mood
            /// </summary>
            Targets,
            /// <summary>
            /// List of moods for each target (listener) for this DialogLine; Each mood must have a target
            /// </summary>
            Moods,
            /// <summary>
            /// WIP: Reqs to fail this DialogLine
            /// </summary>
            ReqFail,
            /// <summary>
            /// WIP: reqs to crit fail this DialogLine
            /// </summary>
            ReqCritFail,
            /// <summary>
            /// WIP: reqs signifying that this DialogLine is even available
            /// </summary>
            ReqAvailable
        }//end of ChunkIdentifierTypes
        /// <summary>
        /// Holds every DialogLine object in the game. Key: LineID (string); Value: DialogLine
        /// </summary>
        public static Dictionary<string, DialogLine> MasterDialogLinesList;
        /// <summary>
        /// Used for global member initialization of DialogLoader
        /// </summary>
        public static void Initialize()
        {
            MasterDialogLinesList = new Dictionary<string, DialogLine>();
        }//end of Initialize
        /// <summary>
        /// Handles reading of one or multiple dialog text files
        /// </summary>
        /// <param name="files">Director(ies) passed by the open file dialog</param>
        public static void ReadDialogTextFiles(params string[] files)
        {
            for (int i = 0; i < files.Length; i++)
            {
                ConvertFromTextToDialogLine(File.Open(files[i], FileMode.Open, FileAccess.Read, FileShare.None));
            }
        }//end of ReadDialogTextFiles
        /// <summary>
        /// Handles writing of dialog text file. Called on an individual file
        /// </summary>
        /// <param name="files"></param>
        public static void WriteDialogTextFile(string file)
        {
            //TODO: after reading
        }//end of WriteDialogTextFiles
        /// <summary>
        /// Handles conversion of text to DialogLines (assuming its in the proper format)
        /// </summary>
        /// <param name="file">Path of file to be read and converted, passed as a Stream object</param>
        public static void ConvertFromTextToDialogLine(Stream file)
        {
            //simple reference for the DialogLine object currently being built
            DialogLine bufferDialogLine = null;
            //stores all of the target ids for the current DialogLine object being built
            List<CharacterNames> bufferTargets = null;
            //stores all of the target moods for the current DialogLine object being built
            List<MoodTypes> bufferMoods = null;
            using (StreamReader reader = new StreamReader(file))
            {
                do
                {
                    string line = reader.ReadLine();
                    if (line.Contains('['))
                    {
                        bufferDialogLine = new DialogLine();
                    }
                    else if (line.Contains(']'))
                    {
                        for (int i = 0; i < bufferTargets.Count; i++)
                        {
                            bufferDialogLine.TargetMoodPairs.Add(bufferTargets[i], bufferMoods[i]);
                        }
                        MasterDialogLinesList.Add(bufferDialogLine.LineID, bufferDialogLine);
                    }
                    else
                    {
                        ChunkIdentifierTypes chunk = (ChunkIdentifierTypes)Enum.Parse(typeof(ChunkIdentifierTypes), line.Substring(0, line.IndexOf(':')));
                        if (line[line.Length-1]!=':')
                        {
                            string value = line.Substring(line.IndexOf(':') + 1);
                            switch (chunk)
                            {
                                case ChunkIdentifierTypes.LineID:
                                    bufferDialogLine.LineID = value;
                                    break;
                                case ChunkIdentifierTypes.DialogString:
                                    bufferDialogLine.LineID = value;
                                    break;
                                case ChunkIdentifierTypes.SpeakerID:
                                    bufferDialogLine.SpeakerID = (CharacterNames)Enum.Parse(typeof(CharacterNames), value);
                                    break;
                                case ChunkIdentifierTypes.SpeakerMood:
                                    bufferDialogLine.SpeakerMood = (MoodTypes)Enum.Parse(typeof(MoodTypes), value);
                                    break;
                                case ChunkIdentifierTypes.Pass:
                                    bufferDialogLine.PassID = value;
                                    break;
                                case ChunkIdentifierTypes.Fail:
                                case ChunkIdentifierTypes.CritFail:
                                    if (!line.Contains("CritFail"))
                                    {
                                        bufferDialogLine.FailID = value;
                                    }
                                    else
                                    {
                                        bufferDialogLine.CritFailID = value;
                                    }
                                    break;
                                case ChunkIdentifierTypes.Responses:
                                case ChunkIdentifierTypes.Targets:
                                case ChunkIdentifierTypes.Moods:
                                    bufferTargets = chunk == ChunkIdentifierTypes.Targets ? new List<CharacterNames>() : bufferTargets;
                                    bufferMoods = chunk == ChunkIdentifierTypes.Moods ? new List<MoodTypes>() : bufferMoods;
                                    do
                                    {
                                        string option = reader.ReadLine();
                                        if (option.Contains('{'))
                                        {
                                            continue;
                                        }
                                        else if (option.Contains('}'))
                                        {
                                            //reader.ReadLine();
                                            break;
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(option) || !string.IsNullOrWhiteSpace(option))
                                            {
                                                switch (chunk)
                                                {
                                                    case ChunkIdentifierTypes.Responses:
                                                        bufferDialogLine.Responses.Add(option);
                                                        break;
                                                    case ChunkIdentifierTypes.Targets:
                                                        bufferTargets.Add((CharacterNames)Enum.Parse(typeof(CharacterNames), option));
                                                        break;
                                                    case ChunkIdentifierTypes.Moods:
                                                        bufferMoods.Add((MoodTypes)Enum.Parse(typeof(MoodTypes), option));
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }
                                        }
                                    } while (true);
                                    break;
                                case ChunkIdentifierTypes.ReqFail:
                                    break;
                                case ChunkIdentifierTypes.ReqCritFail:
                                    break;
                                case ChunkIdentifierTypes.ReqAvailable:
                                    break;
                                default:
                                    break;
                            }
                        }
                        //foreach (ChunkIdentifierTypes chunkIDType in Enum.GetValues(typeof(ChunkIdentifierTypes)))
                        //{
                        //    if (line.Contains(chunkIDType.ToString()))
                        //    {
                        //        if (line[line.Length - 1] != ':') //checks if the string isnt just ':'
                        //        {
                        //            string value = line.Substring(line.IndexOf(':') + 1);
                        //            switch (chunkIDType)
                        //            {
                        //                case ChunkIdentifierTypes.LineID:
                        //                    bufferDialogLine.LineID = value;
                        //                    break;
                        //                case ChunkIdentifierTypes.DialogString:
                        //                    bufferDialogLine.LineID = value;
                        //                    break;
                        //                case ChunkIdentifierTypes.SpeakerID:
                        //                    bufferDialogLine.SpeakerID = (CharacterNames)Enum.Parse(typeof(CharacterNames), value);
                        //                    break;
                        //                case ChunkIdentifierTypes.SpeakerMood:
                        //                    bufferDialogLine.SpeakerMood = (MoodTypes)Enum.Parse(typeof(MoodTypes), value);
                        //                    break;
                        //                case ChunkIdentifierTypes.Pass:
                        //                    bufferDialogLine.PassID = value;
                        //                    break;
                        //                case ChunkIdentifierTypes.Fail:
                        //                case ChunkIdentifierTypes.CritFail:
                        //                    if (!line.Contains("CritFail"))
                        //                    {
                        //                        bufferDialogLine.FailID = value;
                        //                    }
                        //                    else
                        //                    {
                        //                        bufferDialogLine.CritFailID = value;
                        //                    }
                        //                    break;
                        //                case ChunkIdentifierTypes.Responses:
                        //                case ChunkIdentifierTypes.Targets:
                        //                case ChunkIdentifierTypes.Moods:
                        //                    bufferTargets = chunkIDType == ChunkIdentifierTypes.Targets ? new List<CharacterNames>() : null;
                        //                    bufferMoods = chunkIDType == ChunkIdentifierTypes.Moods ? new List<MoodTypes>() : null;
                        //                    do
                        //                    {
                        //                        string option = reader.ReadLine();
                        //                        if (option.Contains('{'))
                        //                        {
                        //                            continue;
                        //                        }
                        //                        else if (option.Contains('}'))
                        //                        {
                        //                            reader.ReadLine();
                        //                            break;
                        //                        }
                        //                        else
                        //                        {
                        //                            if (!string.IsNullOrEmpty(option) || !string.IsNullOrWhiteSpace(option))
                        //                            {
                        //                                switch (chunkIDType)
                        //                                {
                        //                                    case ChunkIdentifierTypes.Responses:
                        //                                        bufferDialogLine.Responses.Add(option);
                        //                                        break;
                        //                                    case ChunkIdentifierTypes.Targets:
                        //                                        bufferTargets.Add((CharacterNames)Enum.Parse(typeof(CharacterNames), option));
                        //                                        break;
                        //                                    case ChunkIdentifierTypes.Moods:
                        //                                        bufferMoods.Add((MoodTypes)Enum.Parse(typeof(MoodTypes), option));
                        //                                        break;
                        //                                    default:
                        //                                        break;
                        //                                }
                        //                            }
                        //                        }
                        //                    } while (true);
                        //                    break;
                        //                case ChunkIdentifierTypes.ReqFail:
                        //                    break;
                        //                case ChunkIdentifierTypes.ReqCritFail:
                        //                    break;
                        //                case ChunkIdentifierTypes.ReqAvailable:
                        //                    break;
                        //                default:
                        //                    break;
                        //            }
                        //        }
                        //        break;
                        //    }
                        //}
                    }
                } while (!reader.EndOfStream);
            }
        }//end of ConvertFromTextToDialogLine
        /// <summary>
        /// Handles conversion of DialogLines to text
        /// </summary>
        /// <param name="dialogLine"></param>
        public static void ConvertFromDialogLineToText(DialogLine dialogLine)
        {
            //TODO: after convert from text to DialogLine
        }//end of ConvertFromDialogLineToText
    }//end of DialogLoader
}//end of namespace