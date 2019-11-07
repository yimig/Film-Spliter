using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyser
{
    class Program
    {
        private static string path = @"C:\Users\zhang\Downloads\workplace\";
        static void Main(string[] args)
        {
            WordMap map=new WordMap();
            DirectoryInfo di=new DirectoryInfo(path);
            foreach (var file in di.GetFiles())
            {
                if (file.Extension == ".db")
                {
                    LanDB db = new LanDB(file.FullName);
                    map.Add(db);
                }
            }
            //WriteMap(map);
            List<string> cet6List = ReadList();
            List<string> sameList = map.DictNode.Keys.Intersect(cet6List).ToList();
            Console.WriteLine("Caption Map Count="+map.DictNode.Count+"\nCET 6 Count="+cet6List.Count+ "\nIntersect Count="+sameList.Count+"\nMatching Ratio="+((double)sameList.Count/(double)cet6List.Count));
            Console.ReadLine();
        }


        private static void WriteMap(WordMap map)
        {
            FileStream fs=new FileStream(path+"\\words.map",FileMode.Create);
            StreamWriter sw=new StreamWriter(fs);
            sw.Write(map);
            sw.Close();
            fs.Close();
        }

        private static List<string> ReadList()
        {
            string listPath = @"C:\Users\zhang\Downloads\CET6.txt";
            string content;
            FileStream fs=new FileStream(listPath,FileMode.Open);
            StreamReader reader=new StreamReader(fs);
            content = reader.ReadToEnd();
            reader.Close();
            fs.Close();
            return content.Split(new char[] {'\n'}).ToList();
        }

    }
}
