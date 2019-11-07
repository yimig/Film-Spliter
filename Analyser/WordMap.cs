using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Analyser
{
    public class WordMap
    {
        private Dictionary<string,List<MapNode>> dictNode;
        private List<string> dbPaths, dbNames;
        public WordMap()
        {
            dictNode=new Dictionary<string, List<MapNode>>();
            dbPaths=new List<string>();
            dbNames=new List<string>();
        }

        public void Add(LanDB db)
        {
            dbPaths.Add(db.DbPath);
            dbNames.Add(db.DbName);
            StartConvert(db);
        }

        private void StartConvert(LanDB db)
        {
            foreach (var item in db.Items)
            {
                List<string> wordSplit = BurstWord(item);
                List<string> updateList = wordSplit.Intersect(DictNode.Keys).ToList();
                List<string> addList = wordSplit.Except(DictNode.Keys).ToList();
                if(updateList.Count!=0)UpdateList(updateList,db.DbPath,item.FileId);
                if(addList.Count!=0)AddList(addList,db.DbPath,item.FileId);
            }
        }

        private void UpdateList(List<string> updateList, string dbPath,string fileId)
        {
            foreach (var key in updateList)
            {
                DictNode[key].Add(new MapNode(key, dbPath, fileId));
            }
        }

        private void AddList(List<string> addList, string dbPath, string fileId)
        {
            foreach (var key in addList)
            {
                List<MapNode> tempNode = new List<MapNode>();
                tempNode.Add(new MapNode(key, dbPath, fileId));
                DictNode.Add(key, tempNode);
            }
        }

        private List<string> BurstWord(LanDBItem item)
        {
            string temp = item.English;
            List<string> resList = Regex.Split(temp, "[^\\p{L}]", RegexOptions.Singleline).ToList();
            CleanList(resList);
            return resList;
        }

        private void CleanList(List<string> dirtyList)
        {
            var exeList=dirtyList.Where(i => i == "").ToList();
            foreach (var delItem in exeList)
            {
                dirtyList.Remove(delItem);
            }
        }

        public Dictionary<string, List<MapNode>> DictNode
        {
            get => dictNode;
            set => dictNode = value;
        }

        public List<string> DbPaths
        {
            get => dbPaths;
            set => dbPaths = value;
        }

        public List<string> DbNames
        {
            get => dbNames;
            set => dbNames = value;
        }

        public override string ToString()
        {
            string result = "map words="+this.DictNode.Count+"\n";
            for (int i = 0; i < DictNode.Count; i++)
            {
                result += "\n\"word\":" + (DictNode.Keys.ToList())[i];
                List<MapNode> nodes = (DictNode.Values.ToList())[i];
                for (int j = 0; j < nodes.Count; j++)
                {
                    result += nodes[j];
                }
            }

            return result;
        }
    }

    public class MapNode
    {
        private string word, dbPath, fileId;

        public MapNode(string word,string dbPath,string fileId)
        {
            Word = word;
            DbPath = dbPath;
            FileId = fileId;
        }

        public string Word
        {
            get => word;
            set => word = value;
        }

        public string DbPath
        {
            get => dbPath;
            set => dbPath = value;
        }

        public string FileId
        {
            get => fileId;
            set => fileId = value;
        }

        public LanDB GetDB()
        {
            return new LanDB(DbPath);
        }

        public LanDBItem GetDBItem()
        {
            LanDB db=new LanDB(DbPath);
            return (db.Items.Where(i => i.FileId == FileId).ToList())[0];
        }

        public override string ToString()
        {
            return $"\n\"Data Base Path\":{this.dbPath}\n\"File ID\":{this.FileId}";
        }
    }
}
