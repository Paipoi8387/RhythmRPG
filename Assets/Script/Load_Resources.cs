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

        while (reader.Peek() != -1) // reader.Peaek��-1�ɂȂ�܂�
        {
            string line = reader.ReadLine(); // ��s���ǂݍ���
            line = line.Replace("<>", "\n");
            csvDatas.Add(new List<string>(line.Split(','))); // , ��؂�Ń��X�g�ɒǉ�
        }

        return csvDatas;
    }

    public static Sprite Load_Sprite(string sprite_name)
    {
        Sprite spriteFile = Resources.Load<Sprite>(sprite_name);
        return spriteFile;
    }
}
