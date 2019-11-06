using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace film_spliter
{
    class Program
    {
        //const string PERFORMER = "YYesTs";
        //private const string CUE_TARGET_FILE_NAME = "Forrest.Gump.1994.REMASTERED.1080p.BluRay.X264-AMIABLE";
        private const string CUE_AND_LANDB_NAME = "Forrest.Gump.1994.REMASTERED.1080p.BluRay.X264-AMIABLE";
        private static string captionFilePath;
        private static string musicFilePath;
        static void Main(string[] args)
        {
            Console.WriteLine("Pull a captions file (*.srt) to here and type enter:");
            captionFilePath = Console.ReadLine();
            List<Captions> captionses = GetCaptions();
            captionses=Captions.MergeCaptions(captionses);
            int[] fileId=new int[captionses.Count+1];
            for (int i = 0; i < captionses.Count; i++) fileId[i] = i + 1;
            //List<Cue> cues = GetCues(captionses,fileId);
            List<LanguageData> lanDB = GetLanauageDB(captionses, fileId);
            //WriteFile(cues);
            WriteDB(lanDB);
            Console.WriteLine("Database create complete,pull the music file(*.mp3) to here and type enter:");
            musicFilePath = Console.ReadLine();
            WriteWave(fileId, captionses);
        }

        private static void WriteWave(int[] fileId, List<Captions> captionses)
        {
            for (int i = 0; i < captionses.Count; i++)
            {
                Spliter.TrimMp3File(musicFilePath,Path.GetDirectoryName(musicFilePath)+"\\"+fileId[i]+".wav",captionses[i].GetStartTime(),captionses[i].GetEndTime());
            }
        }

        private static List<Captions> GetCaptions()
        {
            FileStream stream = new FileStream(captionFilePath, FileMode.Open);
            List<Captions> captionses = new List<Captions>();
            if (stream.CanRead)
            {
                StreamReader sr = new StreamReader(stream);
                string content = sr.ReadToEnd();
                sr.Close();
                stream.Close();
                string[] tempStrings = Regex.Split(content, "^(\\s*)\\r\\n", RegexOptions.Multiline);
                foreach (string s in tempStrings)
                {
                    if (s != "") captionses.Add(new Captions(s));
                    if(captionses.Count==50)break;
                }
            }
            return captionses;
        }


        
        //private static List<Cue> GetCues(List<Captions> captionses, int[] fileId)
        //{
        //    List<Cue> cues = new List<Cue>();
        //    List<CueInfo> infos = new List<CueInfo>();

        //    for (int i = 0; i < captionses.Count; i++)
        //    {
        //        infos.Add(new CueInfo(fileId[i].ToString(), PERFORMER));
        //    }

        //    for (int i = 0; i < captionses.Count; i++)
        //    {
        //        cues.Add(new Cue(captionses[i], infos[i]));
        //    }

        //    return cues;
        //}

        //private static void WriteFile(List<Cue> cues)
        //{
        //    CueInfo cueInfo = new CueInfo("Forrest. Forrest Gump", PERFORMER);
        //    FileStream wfs = new FileStream(Path.GetDirectoryName(captionFilePath) + "\\"+CUE_AND_LANDB_NAME+".cue", FileMode.Create);
        //    StreamWriter writer = new StreamWriter(wfs);
        //    writer.Write(Cue.ConvertCues(cues, cueInfo, CUE_TARGET_FILE_NAME));
        //    writer.Close();
        //    wfs.Close();
        //}

        private static List<LanguageData> GetLanauageDB(List<Captions> captionses, int[] fileId)
        {
            List<LanguageData> landb = new List<LanguageData>();
            for (int i = 0; i < captionses.Count; i++)
            {
                landb.Add(new LanguageData(captionses[i], fileId[i]));
            }

            return landb;
        }

        private static void WriteDB(List<LanguageData> lanDB)
        {
            FileStream wfs = new FileStream(Path.GetDirectoryName(captionFilePath) + "\\"+CUE_AND_LANDB_NAME+".db", FileMode.Create);
            StreamWriter writer = new StreamWriter(wfs);
            writer.Write(LanguageData.ConvertLanDB(lanDB));
            writer.Close();
            wfs.Close();
        }
    }
}
