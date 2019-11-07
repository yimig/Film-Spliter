using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace film_spliter
{
    class Captions
    {
        private int id;
        private Tick startTick, endTick;
        private string english, chinese;

        public int Id
        {
            get => id;
            set => id = value;
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

        public string English
        {
            get => english;
            set
            {
                if (value.Contains('\n'))
                {
                    english=value.Replace('\n', ' ');
                }
                else if(value.Contains('\r'))
                {
                    english = value.Replace('\r', ' ');
                }
                else if (value.Contains('|'))
                {
                    english = value.Replace('|', '-');
                }
                else english = value;
            }
        }

        public string Chinese
        {
            get => chinese;
            set
            {
                if (value.Contains('\n'))
                {
                    chinese = value.Replace('\n', ' ');
                }
                else if (value.Contains('\r'))
                {
                    chinese = value.Replace('\r', ' ');
                }
                else if (value.Contains('|'))
                {
                    chinese = value.Replace('|', '-');
                }
                else chinese = value;
            }
        }

        public Captions(string captionString)
        {
            string[] rowFragments = captionString.Split(new char[] {'\n'});
            string[] tickFragments =
                rowFragments[1].Split(new string[] {" --> "}, StringSplitOptions.RemoveEmptyEntries);
            Id = Convert.ToInt32(rowFragments[0]);
            StartTick=new Tick(tickFragments[0]);
            EndTick=new Tick(tickFragments[1]);
            Chinese = rowFragments[2];
            English = rowFragments[3];
        }

        public Captions() { }

        public TimeSpan GetStartTime()
        {
            return new TimeSpan(0,StartTick.Hour,StartTick.Min,StartTick.Sec,StartTick.Minsec);
        }

        public TimeSpan GetEndTime()
        {
            return new TimeSpan(0,EndTick.Hour,EndTick.Min,EndTick.Sec,EndTick.Minsec);
        }

        public static Captions operator +(Captions cap1, Captions cap2)
        {
            Captions resCap=new Captions();
            resCap.Id = cap1.Id;
            resCap.startTick = cap1.startTick;
            resCap.EndTick = cap2.EndTick;
            resCap.English = cap1.English + cap2.English;
            resCap.Chinese = cap1.Chinese + "  " + cap2.Chinese;
            return resCap;
        }

        public static List<Captions> MergeCaptions(List<Captions> captionses)
        {
            List<List<Captions>> ctrQueue=new List<List<Captions>>();
            List<Captions> subQueue=null;
            bool flag = true;
            captionses = CleanCaptions(captionses);
            for (int i = 0; i < captionses.Count; i++)
            {
                if (!(captionses[i].English.Contains('.') || captionses[i].English.Contains('?') ||
                    captionses[i].English.Contains('!')))
                {
                    if (flag)
                    {
                        subQueue=new List<Captions>();
                        subQueue.Add(captionses[i]);
                        flag = false;
                    }
                    else
                    {
                        subQueue.Add(captionses[i]);
                    }
                }
                else
                {
                    if (!flag)
                    {
                        subQueue.Add(captionses[i]);
                        ctrQueue.Add(subQueue);
                        flag = true;
                    }
                }
            }

            return StartMerge(ctrQueue, captionses);

        }

        private static List<Captions> CleanCaptions(List<Captions> captionses)
        {
            List<Captions> removeList = new List<Captions>();
            foreach (var captions in captionses)
            {
                if (captions.English == ""||Regex.IsMatch(captions.English, "[\u4e00-\u9fa5]"))removeList.Add(captions);
            }

            foreach (var removeCaptions in removeList)
            {
                captionses.Remove(removeCaptions);
            }

            return captionses;
        }

        private static List<Captions> StartMerge(List<List<Captions>> ctrQueue,List<Captions> rowCaptionses)
        {
            foreach (var subQueue in ctrQueue)
            {
                rowCaptionses.Remove(subQueue[subQueue.Count - 1]);
                for (int i = 0; i < subQueue.Count-1; i++)
                {
                    rowCaptionses.Add(subQueue[i] + subQueue[i+1]);
                    rowCaptionses.Remove(subQueue[i]);
                }
            }

            return rowCaptionses;
        }
    }
}
