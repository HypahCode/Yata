using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HellTech.Editor
{
    public struct KeyValue
    {
        public string Key;
        public string Value;
        public KeyValue(string key, string value)
        {
            Key = key;
            Value = value;
        }
        public float AsFloat()
        {
            return float.Parse(Value);
        }
        public int AsInt()
        {
            return int.Parse(Value);
        }
        public bool AsBool()
        {
            return Value.ToLower() == "true";
        }
    }

    public sealed class DataNode
    {
        public DataNode Parent = null;
        public List<DataNode> Childeren = new List<DataNode>();
        public List<KeyValue> Keys = new List<KeyValue>();
        private string path;

        private string name;

        public DataNode(string name, string filePath)
        {
            path = filePath;
            this.name = name;
        }

        public string Name
        {
            get { return name; }
        }

        public DataNode AddChild(string name, string filePath)
        {
            DataNode newNode = new DataNode(name, filePath);
            newNode.Parent = this;
            Childeren.Add(newNode);
            return newNode;
        }

        public void AddValue(string key, string value)
        {
            Keys.Add(new KeyValue(key, value));
        }

        public DataNode GetChildByName(string name)
        {
            for (int i = 0; i < Childeren.Count; i++)
            {
                if (Childeren[i].name == name)
                {
                    return Childeren[i];
                }
            }
            return null;
        }

        public string GetKeyByIndex(int idx)
        {
            return RemoveVarsFromStr(Keys[idx].Value);
        }

        public string GetKeyByName(string keyName, string defaultValue)
        {
            for (int i = 0; i < Keys.Count; i++)
            {
                if (Keys[i].Key == keyName)
                {
                    return RemoveVarsFromStr(Keys[i].Value);
                }
            }
            return defaultValue;
        }

        private string RemoveVarsFromStr(string str)
        {
            string s = str;
            int idx = s.IndexOf("(@path)");
            if (idx >= 0)
            {
                s = s.Replace("(@path)", path);
            }
            return s;
        }
    }

    public sealed class DataFile
    {
        public DataNode Root;
        public DataNode Parent;
        private string path;

        public DataFile()
        {
            Root = new DataNode("root", "");
            path = "";
        }

        public DataFile(string data, string filePath)
            : this()
        {
            path = filePath;
            DataNode node = Root;

            char[] delimiters = new char[] { '\r', '\n' };
            string[] lines = data.Split(delimiters);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if ((line != "") && (!line.StartsWith("//")))
                {
                    if (line.StartsWith("{"))
                    {
                        node = node.AddChild(line.Substring(1), filePath);
                    }
                    else if (line.StartsWith("}"))
                    {
                        node = node.Parent;
                    }
                    else if (line.Contains("="))
                    {
                        int pos = line.IndexOf('=');
                        node.AddValue(line.Substring(0, pos), line.Substring(pos + 1, line.Length - pos - 1));
                    }
                }
            }
        }

        static public DataFile CreateFromDisk(string filename)
        {
            DataFile file = null;

            string data;
            using (StreamReader reader = new StreamReader(filename))
            {
                data = reader.ReadToEnd();
                file = new DataFile(data, Path.GetDirectoryName(filename) + "\\");
            }
            return file;
        }

        public void SaveToFile(string filename)
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < Root.Childeren.Count; i++)
            {
                SaveNode(stringBuilder, Root.Childeren[i], 0);
            }

            using (TextWriter tw = new StreamWriter(filename))
            {
                tw.Write(stringBuilder.ToString());
            }
        }

        private void SaveNode(StringBuilder sb, DataNode node, int level)
        {
            string tabs = "";
            for (int i = 0; i < level; i++)
            {
                tabs += "\t";
            }

            sb.AppendLine(tabs + "{" + node.Name);

            for (int i = 0; i < node.Keys.Count; i++)
            {
                sb.AppendLine(tabs + "\t" + node.Keys[i].Key + "=" + node.Keys[i].Value);
            }

            for (int i = 0; i < node.Childeren.Count; i++)
            {
                SaveNode(sb, node.Childeren[i], level + 1);
            }

            sb.AppendLine(tabs + "}");
        }
    }
}
