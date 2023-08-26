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

        while (reader.Peek() != -1) // reader.Peaek‚ª-1‚É‚È‚é‚Ü‚Å
        {
            string line = reader.ReadLine(); // ˆês‚¸‚Â“Ç‚İ‚İ
            line = line.Replace("<>", "\n");
            csvDatas.Add(new List<string>(line.Split(','))); // , ‹æØ‚è‚ÅƒŠƒXƒg‚É’Ç‰Á
        }

        return csvDatas;
    }

    public static Sprite Load_Sprite(string sprite_name)
    {
        Sprite spriteFile = Resources.Load<Sprite>(sprite_name);
        return spriteFile;
    }
}
