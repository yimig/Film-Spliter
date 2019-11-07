using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyser
{
    public class LanDB
    {
        private string dbName,dbPath;
        private List<LanDBItem> items;
        public LanDB(string filePath)
        {
            dbName = Path.GetFileNameWithoutExtension(filePath);
            dbPath = filePath;
            items=new List<LanDBItem>();
            string content = ReadFile(filePath);
            string[] rowItems = content.Split(new string[] {"\n"}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var rowitem in rowItems)
            {
                items.Add(new LanDBItem(rowitem));
            }
        }

        public List<LanDBItem> Items
        {
            get => items;
            set => items = value;
        }

        public string DbName
        {
            get => dbName;
            set => dbName = value;
        }

        public string DbPath
        {
            get => dbPath;
            set => dbPath = value;
        }

        private string ReadFile(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open);
            StreamReader reader = new StreamReader(fs);
            string content = reader.ReadToEnd();
            reader.Close();
            fs.Close();
            return content;
        }
    }

    public class LanDBItem
    {
        private string fileId, english, chinese;
        public LanDBItem(string rowItem)
        {
            string[] infos = rowItem.Split(new char[] {'|'});
            FileId = infos[0];
            English = infos[1].ToLower();
            Chinese = infos[2];
        }

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
    }
}
