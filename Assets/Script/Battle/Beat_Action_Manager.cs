using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Beat_Action_Manager : MonoBehaviour
{
    [SerializeField] bool use_map_num = true;
    public enum Char_Name { Dental, Max, CF, Tembo, Gootara, Nerd, Yotto, Tetuman };
    [SerializeField] Char_Name char_name = Char_Name.Dental;
    public static int char_num;
    public static int difficulty_num;
    [SerializeField] bool use_d_difficulty_num;
    [SerializeField] int d_difficulty_num = 0;


    //各キャラのobj
    GameObject[] char_obj_array;
    [SerializeField] GameObject dental_obj;
    [SerializeField] GameObject max_obj;
    [SerializeField] GameObject cf_obj;
    [SerializeField] GameObject tembo_obj;
    [SerializeField] GameObject gootara_obj;
    [SerializeField] GameObject nerd_obj;
    [SerializeField] GameObject yotto_obj;
    [SerializeField] GameObject tetuman_obj;

    //各キャラのbeat_action
    Main_Beat_Action[] beat_action_array;


    void Start()
    {
        if (use_d_difficulty_num) difficulty_num = d_difficulty_num;

        if (!use_map_num) char_num = (int)char_name;

        char_obj_array = new GameObject[] { dental_obj, max_obj, cf_obj, tembo_obj, gootara_obj, nerd_obj, yotto_obj, tetuman_obj };
        beat_action_array = new Main_Beat_Action[]
        {
            dental_obj.GetComponent<Dental_Beat_Action>(),
            max_obj.GetComponent<Main_Beat_Action>(),
            cf_obj.GetComponent<Main_Beat_Action>(),
            tembo_obj.GetComponent<Main_Beat_Action>(),
            gootara_obj.GetComponent<Main_Beat_Action>(),
            nerd_obj.GetComponent<Main_Beat_Action>(),
            yotto_obj.GetComponent<Main_Beat_Action>(),
            tetuman_obj.GetComponent<Main_Beat_Action>()
        };

        Char_Beat_Action_Setting();

        Score_Manager.score = 0;
    }

    void Update()
    {
        if (Beat_Manager.Get_is_just_beat())
        {
            Just_Beat_Action_Manage();
        }
        else
        {
            Progress_Beat_Action_Manage();
        }
    }


    void Char_Beat_Action_Setting()
    {
        for(int i = 0; i < char_obj_array.Length; i++)
        {
            if(i == char_num) char_obj_array[i].SetActive(true);
        }
    }


    void Just_Beat_Action_Manage()
    {
        beat_action_array[char_num].Just_Beat_Action();
    }

    public void Progress_Beat_Action_Manage()
    {
        beat_action_array[char_num].Progress_Beat_Action();
    }


    public static string Get_Char_Name()
    {
        return Enum.ToObject(typeof(Char_Name),char_num).ToString();
    }
}
