using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class ArchitectureList
{
    [System.Serializable]
    public class Architecture{
        public string name;

        [System.Serializable]
        public class Rule{
            public List<int> bricks;
        }
        public List<Rule> rules;
    }

    public List<Architecture> architectures;

    public static ArchitectureList ReadFromJson(string file) {

        StreamReader reader = new StreamReader(file); 
        string jsonString = reader.ReadToEnd();
        reader.Close();
        
        return JsonUtility.FromJson<ArchitectureList>(jsonString);
        
    }

}
