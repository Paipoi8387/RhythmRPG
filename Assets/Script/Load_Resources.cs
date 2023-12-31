using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Load_Resources : MonoBehaviour
{
    public static List<List<string>> Load_CSV(string csv_name)
    {
        TextAsset csvFile = Resources.Load(csv_name) as TextAsset;
        StringReader reader = new StringReader(csvFile.text);
        List<List<string>> csvDatas = new List<List<string>>();

        while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            line = line.Replace("<>", "\n");
            csvDatas.Add(new List<string>(line.Split(','))); // , 区切りでリストに追加
        }

        return csvDatas;
    }

    public static Sprite Load_Sprite(string sprite_name)
    {
        Sprite spriteFile = Resources.Load<Sprite>(sprite_name);
        return spriteFile;
    }
}
