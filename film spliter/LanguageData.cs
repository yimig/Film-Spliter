using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace film_spliter
{
    class LanguageData
    {
        private int fileId, trackId;
        private string english, chinese;

        public int FileId
        {
            get => fileId;
            set => fileId = value;
        }

        public int TrackId
        {
            get => trackId;
            set => trackId = value;
        }

        public string English
        {
            get => english;
            set => english = value;
        }

        public string Chinese
        {
            get => chinese;
            set => chinese = value;
        }

        public LanguageData(Captions captions, int fileId)
        {
            FileId = fileId;
            TrackId = captions.Id;
            English = captions.English;
            Chinese = captions.Chinese;
        }

        public override string ToString()
        {
            return FileId + "|" + trackId + "|" + english + "|" + chinese + "\n";
        }

        public static string ConvertLanDB(List<LanguageData> lanDB)
        {
            string result=null;
            foreach (var lan in lanDB)
            {
                result += lan;
            }

            return result;
        }
    }
}
