using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace __dialogSystems
{
    public enum MoodTypes
    {
        Happy,
        Sad,
        Gay,
        ReallyFuckingGay,
    }//end of MoodTypes

    class Program
    {
        static string lower = "abcdefghijklmnopqrstuvwxyz";
        static string upper = lower.ToUpper();
        static string specials = "`1234567890-=[]\\;',./~!@#$%^&*()_+{}:\"<>?";
        static string[] sources = { lower, upper, specials };
        static void Main(string[] args)
        {
            Console.WriteLine("Test");
            //WriteCharBytePairs();
            DialogManager.Awake();
            DialogLoader.PopulateDialogLines();
            foreach (var kvp in DialogManager.DialogByLineID)
            {
                Console.WriteLine(kvp.Key);
                Console.WriteLine(kvp.Value.ToString());
            }
            Console.WriteLine("Done");
            Console.ReadLine();
        }//end of Main
        static void WriteCharBytePairs()
        {
            for (int i = 0; i < sources.Length; i++)
            {
                GenerateCharBytePairs(sources[i]);
            }
        }//end of WriteCharBytePairs
        static void GenerateCharBytePairs(string source)
        {
            for (int i = 0; i < source.Length; i++)
            {
                char c = source[i];
                Console.Write("{0}:{1}\n", c, (byte)c);
            }
        }//end of GenerateCharBytePairs
    }//end of class

    public static class DialogLoader
    {
        static Dictionary<ChunkFormatTypes, int> ChunkFormat;
        enum ChunkFormatTypes //local enum
        {
            String,
            MoodTypes,
            ListString,
            Requirements
        }//end of ChunkFormatTypes

        enum ChunkIdentifierTypes
        {
            DialogString,
            LineID,
            SpeakerID,
            TargetID,
            PassTarget,
            FailTarget,
            CriticalFailTarget,
            SpeakerMood,
            TargetMood,
            Responses,
            Fail,
            CriticalFail,
            Available
        }

        public static void PopulateDialogLines()
        {
            //not using this cause i hardcoded it sorta in the streamreader 
            //ChunkFormat = new Dictionary<ChunkFormatTypes, int>();
            //foreach (ChunkFormatTypes chunkFormatType in Enum.GetValues(typeof(ChunkFormatTypes)))
            //{
            //    int count;
            //    switch (chunkFormatType)
            //    {
            //        case ChunkFormatTypes.String: count = 7; break;
            //        case ChunkFormatTypes.MoodTypes: count = 2; break;
            //        case ChunkFormatTypes.ListString: count = 1; break;
            //        case ChunkFormatTypes.Requirements: count = 3; break;
            //        default: count = 0; break;
            //    }
            //    ChunkFormat.Add(chunkFormatType, count);
            //}

            DialogLine buffer = new DialogLine();
            for (int i = 0; i < DialogManager.dialogByDayPaths.Length; i++)
            {
                using (StreamReader reader = new StreamReader(File.Open(DialogManager.dialogByDayPaths[i], FileMode.Open, FileAccess.Read, FileShare.None)))
                {
                    do
                    {
                        string raw = reader.ReadLine();
                        if (raw.Contains('['))
                        {
                            buffer = new DialogLine();
                        }
                        else if (raw.Contains(']'))
                        {
                            DialogManager.DialogByLineID.Add(buffer.LineID, buffer);
                        }
                        else
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
                                        case ChunkIdentifierTypes.TargetID:
                                            buffer.TargetID = raw;
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
                                        case ChunkIdentifierTypes.TargetMood:
                                            buffer.TargetMood = (MoodTypes)Enum.Parse(typeof(MoodTypes), raw);
                                            break;
                                        case ChunkIdentifierTypes.Responses:
                                            bool isStartOfResponseBlock = true;
                                            do
                                            {
                                                string response = reader.ReadLine();
                                                if (response.Contains('{'))
                                                {
                                                    continue;
                                                }
                                                else if (response.Contains('}'))
                                                {
                                                    isStartOfResponseBlock = false;
                                                    reader.ReadLine();
                                                    break;
                                                }
                                                else
                                                {
                                                    buffer.PossibleResponses.Add(response);
                                                }
                                            } while (isStartOfResponseBlock);
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
            }
        }//end of PopulateDialogLines
    }//end of class

    public static class DialogManager
    {

        public static Dictionary<string, DialogLine> DialogByLineID; //key is LineID
        public static string rootDialogPath; //this will later be the StreamingAssets path
        public static string dialogsPath;//actual folder contain dialogs
        public static string[] dialogByDayPaths; //reference to each dialog text file

        public static void Awake()
        {
            DialogByLineID = new Dictionary<string, DialogLine>();
            rootDialogPath = Directory.GetCurrentDirectory();
            dialogsPath = rootDialogPath + @"\Dialogs";
            dialogByDayPaths = Directory.GetFiles(dialogsPath);
        }//end of Awake
    }
    public class DialogLine
    {
        public string DialogString;
        public string LineID; //ID of this line
        public string SpeakerID;
        public string TargetID;

        #region Responses Only
        public string PassTarget;
        public string FailTarget;
        public string CritFailTarget;
        //Requirement Fail        // if <= this condition, but > CritFail, failure.
        //Requirement CritFail   //If <= this condition, critical failure.
        //Requirement Available //Will this show up as an available response?
        #endregion Responses Only

        public MoodTypes SpeakerMood;
        public MoodTypes TargetMood;

        #region NPC Only
        public List<string> PossibleResponses = new List<string>();
        #endregion

        public override string ToString()
        {
            StringBuilder responses = new StringBuilder();
            for (int i = 0; i < PossibleResponses.Count; i++)
            {
                responses.AppendLine(PossibleResponses[i]);
            }
            return string.Format("LineID: {0}\n{1}:\tHello{2}, {3}.\nHere are your possible responses:\n{4}", LineID, SpeakerID, TargetID, DialogString, responses.ToString());
        }
    }//end of class
}//end of namespace