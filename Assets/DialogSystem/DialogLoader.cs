using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

public static class DialogLoader
{
    enum ChunkIdentifierTypes //local enum
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
    }//end of ChunkIdentifierTypes

    public static string streamingAssetsPath; //this will later be the StreamingAssets path
    public static string dialogsPath;//actual folder contain dialogs
    public static string[] DialogByDayPaths; //reference to each dialog text file

    public static void Initialize(string _streamingAssetsPath)
    {
        DialogLine.DialogByLineID = new Dictionary<string, DialogLine>();
        streamingAssetsPath = _streamingAssetsPath;
        dialogsPath = streamingAssetsPath + @"\Dialogs";
        DialogByDayPaths = Directory.GetFiles(dialogsPath);
        PopulateDialogLines();
    }//end of Initialize

    public static void PopulateDialogLines()
    {
        DialogLine buffer = new DialogLine();
        for (int i = 0; i < DialogByDayPaths.Length; i++)
        {
            using (StreamReader reader = new StreamReader(File.Open(DialogByDayPaths[i], FileMode.Open, FileAccess.Read, FileShare.None)))
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
                        if (!DialogLine.DialogByLineID.ContainsKey(buffer.LineID))
                        {
                            DialogLine.DialogByLineID.Add(buffer.LineID, buffer);
                        }
                        else
                        {
                            Debugger.Log(string.Format("Error- master reference already contains: {0}", buffer.LineID));
                        }
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