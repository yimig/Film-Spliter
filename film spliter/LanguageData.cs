using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace film_spliter
{
    class LanguageData
    {
        private string english, chinese,fileId;

        public string FileId
        {
            get => fileId;
            set => fileId = value;
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

        public LanguageData(Captions captions, string fileId)
        {
            FileId = fileId;
            English = captions.English;
            Chinese = captions.Chinese;
        }

        public override string ToString()
        {
            return FileId + "|" + english + "|" + chinese + "\n";
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
