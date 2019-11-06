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
        const string PERFORMER = "YYesTs";
        private const string CUE_TARGET_FILE_NAME = "Forrest.Gump.1994.REMASTERED.1080p.BluRay.X264-AMIABLE";
        private const string CUE_AND_LANDB_NAME = "Forrest.Gump.1994.REMASTERED.1080p.BluRay.X264-AMIABLE";
        private static string filePath;
        static void Main(string[] args)
        {
            Console.WriteLine("Pull a .srt file to here and type enter:");
            filePath = Console.ReadLine();
            List<Captions> captionses = GetCaptions();
            int[] fileId=new int[captionses.Count+1];
            for (int i = 0; i < captionses.Count; i++) fileId[i] = i + 1;
            List<Cue> cues = GetCues(captionses,fileId);
            List<LanguageData> lanDB = GetLanauageDB(captionses, fileId);
            WriteFile(cues);
            WriteDB(lanDB);
        }


        private static List<Captions> GetCaptions()
        {
            FileStream stream = new FileStream(filePath, FileMode.Open);
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
                }
            }
            return captionses;
        }

        private static List<Cue> GetCues(List<Captions> captionses, int[] fileId)
        {
            List<Cue> cues = new List<Cue>();
            List<CueInfo> infos = new List<CueInfo>();

            for (int i = 0; i < captionses.Count; i++)
            {
                infos.Add(new CueInfo(fileId[i].ToString(), PERFORMER));
            }

            for (int i = 0; i < captionses.Count; i++)
            {
                cues.Add(new Cue(captionses[i], infos[i]));
            }

            return cues;
        }

        private static void WriteFile(List<Cue> cues)
        {
            CueInfo cueInfo = new CueInfo("Forrest. Forrest Gump", PERFORMER);
            FileStream wfs = new FileStream(Path.GetDirectoryName(filePath) + "\\"+CUE_AND_LANDB_NAME+".cue", FileMode.Create);
            StreamWriter writer = new StreamWriter(wfs);
            writer.Write(Cue.ConvertCues(cues, cueInfo, CUE_TARGET_FILE_NAME));
            writer.Close();
            wfs.Close();
        }

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
            FileStream wfs = new FileStream(Path.GetDirectoryName(filePath) + "\\"+CUE_AND_LANDB_NAME+".db", FileMode.Create);
            StreamWriter writer = new StreamWriter(wfs);
            writer.Write(LanguageData.ConvertLanDB(lanDB));
            writer.Close();
            wfs.Close();
        }
    }
}
