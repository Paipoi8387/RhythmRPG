using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Score_Manager : MonoBehaviour
{
    /// <summary>
    /// スコア関連
    /// </summary>
    static TextMeshProUGUI score_text;
    private void Start()
    {
        score_text = GetComponent<TextMeshProUGUI>();
    }

    public static int score = 0;

    public static int nolma_score = 0;
    public static int paipoi_score = 0;
    public static int best_score = 0;
    public static void Load_Score()
    {
        string char_name = Beat_Action_Manager.Get_Char_Name();
        int difficulty_num = Beat_Action_Manager.difficulty_num;
        List<List<string>> scoreDatas = Load_Resources.Load_CSV("Battle/" + char_name + "/ScoreList");
        nolma_score = int.Parse(scoreDatas[difficulty_num + 1][1]);
        paipoi_score = int.Parse(scoreDatas[difficulty_num + 1][2]);
        best_score = PlayerPrefs.GetInt(char_name + difficulty_num.ToString(), 0);
    }

    public static void Add_Score(int num)
    {
        score += num;
        score_text.text = score.ToString();
    }

    public static void Overwrite_Score(int num)
    {
        score = num;
        score_text.text = score.ToString();
    }

    public static void Register_BestScore()
    {
        string char_name = Beat_Action_Manager.Get_Char_Name();
        int difficulty_num = Beat_Action_Manager.difficulty_num;

        if(PlayerPrefs.GetInt(char_name + difficulty_num.ToString(), 0) <= score)
        {
            PlayerPrefs.SetInt(char_name + difficulty_num.ToString(), score);
        }
    }


    //スイッチで出す時には必要
    public static void Save_PlayerPrefs()
    {
        string char_name = Beat_Action_Manager.Get_Char_Name();
        int difficulty_num = Beat_Action_Manager.difficulty_num;
        PlayerPrefs.GetInt(char_name + difficulty_num.ToString(), 0);
    }

    public static void Load_PlayerPrefs()
    {

    }
}
