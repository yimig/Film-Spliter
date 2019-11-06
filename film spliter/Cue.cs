using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace film_spliter
{
    class Cue
    {
        private string title, performer;
        private int trackId;
        private Tick startTick, endTick;

        public string Title
        {
            get => title;
            set => title = value;
        }

        public string Performer
        {
            get => performer;
            set => performer = value;
        }

        public int TrackId
        {
            get => trackId;
            set => trackId = value;
        }

        public Tick StartTick
        {
            get => startTick;
            set => startTick = value;
        }

        public Tick EndTick
        {
            get => endTick;
            set => endTick = value;
        }

        public Cue(Captions captions,CueInfo cueInfo)
        {
            Title = cueInfo.Title;
            Performer = cueInfo.Performer;
            TrackId = captions.Id;
            StartTick = captions.StartTick;
            EndTick = captions.EndTick;
        }

        public override string ToString()
        {
            return
                $"   TRACK {this.TrackId} AUDIO\r\n    TITLE \"{this.Title}\"\r\n    PERFORMER \"{this.Performer}\"\r\n    ";
        }

        public static string ConvertCues(List<Cue> cues,CueInfo cueInfo,string fileName)
        {
            string result = "PERFORMER \"" + cueInfo.Performer + "\"\r\nTITLE \"" + cueInfo.Title + "\"\r\nFILE \"" +
                            fileName + "\" WAVE\r\n";
            result += cues[0].ToString() + "INDEX 01 00:00:00\r\n";
            for (int i = 1; i < cues.Count; i++)
            {
                result += cues[i] + "INDEX 00 " + cues[i - 1].EndTick + "\r\n    " + "INDEX 01 " +
                          cues[i].StartTick + "\r\n";
            }
            return result;
        }
    }

    class CueInfo
    {
        private string title, performer;

        public string Title
        {
            get => title;
            set => title = value;
        }

        public string Performer
        {
            get => performer;
            set => performer = value;
        }

        public CueInfo(string title,string performer)
        {
            Title = title;
            Performer = performer;
        }
    }
}
