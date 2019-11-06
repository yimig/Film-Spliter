using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace film_spliter
{
    class Tick
    {
        private int hour, min, sec, minsec;

        public int Hour
        {
            get => hour;
            set => hour = value;
        }

        public int Min
        {
            get => min;
            set => min = value;
        }

        public int Minsec
        {
            get => minsec;
            set => minsec = value;
        }

        public int Sec
        {
            get => sec;
            set => sec = value;
        }

        public Tick(int hour, int min, int sec, int minsec)
        {
            Hour = hour;
            Min = min;
            Sec = sec;
            Minsec = minsec;
        }

        public Tick(string tickString)
        {
            string[] tickFragment = tickString.Split(new char[] {':'});
            string[] secFragment = tickFragment[2].Split(new char[] {','});
            Hour = Convert.ToInt32(tickFragment[0]);
            Min = Convert.ToInt32(tickFragment[1]);
            Sec = Convert.ToInt32(secFragment[0]);
            Minsec = Convert.ToInt32(secFragment[1]);
        }

        public override string ToString()
        {
            return HyperTime() + ":" + AddDigital(Sec) + ":" + SubDigital(Minsec);
        }

        private string AddDigital(int num)
        {
            return num < 10 ? "0" + num : num.ToString();
        }

        private string SubDigital(int num)
        {
            return num > 99 ? (num / 10).ToString() : num.ToString();
        }

        private string HyperTime()
        {
            return AddDigital(Hour * 60 + Min);
        }
    }
}
