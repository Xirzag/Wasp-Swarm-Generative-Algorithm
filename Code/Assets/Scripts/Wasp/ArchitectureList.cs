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

        #if UNITY_WEBGL
            string jsonString = defaultArchitecture;
        #else
            StreamReader reader = new StreamReader(file); 
            string jsonString = reader.ReadToEnd();
            reader.Close();
        #endif
        
        return JsonUtility.FromJson<ArchitectureList>(jsonString);
        
    }

    static string defaultArchitecture = @"
            {""architectures"": [
            {
                ""name"": ""Architecture D E"",
                ""rules"": [
                    {""bricks"":
                        [
                        2, 2, 2,
                        2, 2, 2,
                        2, 2, 2,

                        0, 0, 0,
                        0, 1, 0,
                        0, 0, 0,
                        
                        0, 0, 0,
                        0, 0, 0,
                        0, 0, 0,


                        0, 0, 0,
                        0, 1, 0,
                        0, 0, 0,

                        0, 0, 0,
                        0, 2, 0,
                        0, 0, 0,
                        
                        0, 0, 0,
                        0, 0, 0,
                        0, 0, 0,


                        0, 1, 0,
                        0, 0, 0,
                        0, 0, 0,

                        0, 2, 0,
                        0, 2, 0,
                        0, 0, 0,
                        
                        0, 0, 0,
                        0, 0, 0,
                        0, 0, 0,


                        0, 1, 0,
                        0, 0, 0,
                        0, 0, 0,

                        2, 2, 2,
                        0, 2, 0,
                        0, 0, 0,
                        
                        0, 0, 0,
                        0, 0, 0,
                        0, 0, 0,


                        1, 0, 0,
                        0, 0, 0,
                        0, 0, 0,

                        2, 2, 0,
                        2, 2, 0,
                        0, 0, 0,
                        
                        0, 0, 0,
                        0, 0, 0,
                        0, 0, 0,


                        0, 0, 0,
                        0, 0, 0,
                        0, 0, 0,

                        2, 2, 2,
                        0, 2, 0,
                        0, 0, 0,
                        
                        0, 0, 0,
                        0, 0, 0,
                        0, 0, 0,


                        0, 0, 0,
                        0, 0, 0,
                        0, 0, 0,

                        2, 2, 2,
                        2, 2, 0,
                        2, 0, 0,
                        
                        0, 0, 0,
                        0, 0, 0,
                        0, 0, 0,



                        0, 0, 0,
                        0, 0, 0,
                        0, 0, 0,

                        2, 2, 0,
                        2, 2, 0,
                        0, 0, 0,
                        
                        0, 0, 0,
                        0, 0, 0,
                        0, 0, 0,


                        0, 0, 0,
                        0, 0, 0,
                        0, 0, 0,

                        2, 2, 2,
                        2, 2, 2,
                        0, 0, 0,
                        
                        0, 0, 0,
                        0, 0, 0,
                        0, 0, 0

                        ]
                    }
                ]
            }
        ]}";

}
