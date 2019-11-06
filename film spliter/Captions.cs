using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
