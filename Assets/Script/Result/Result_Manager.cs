using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Result_Manager : MonoBehaviour
{
    int display_step = 0;

    void Start()
    {
        your_score_num = Score_Manager.score;
        nolma_score_num = Score_Manager.nolma_score;

        Score_Manager.Register_BestScore();
        Display_Sprite();
        Display_Score();
    }

    [SerializeField] Animator battle_warp_anim;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            display_step++;
            Display_Sprite();
            Display_Score();

            if (display_step == 5)
            {
                battle_warp_anim.Play("BattleWarp_Fade");
                Invoke("Change_Map_Scene", 2);
            }
        }
    }


    void Change_Map_Scene()
    {
        SceneManager.LoadScene("Map");
    }


    [SerializeField] Text nolma_your;
    [SerializeField] Text nolma_your_score;
    [SerializeField] Text diff_score;
    [SerializeField] GameObject white_line;
    int your_score_num;
    int nolma_score_num;
    [SerializeField] AudioSource sound_source;
    [SerializeField] AudioClip score_sound;

    void Display_Score()
    {
        if (display_step == 1)
        {
            nolma_your.text = "Ç†Ç»ÇΩ\n";
            nolma_your_score.text = your_score_num.ToString() + "\n";
            sound_source.PlayOneShot(score_sound);
        }
        else if (display_step == 2)
        {
            nolma_your.text += "- ÉmÉãÉ}";
            nolma_your_score.text += nolma_score_num.ToString();
            sound_source.PlayOneShot(score_sound);
        }
        else if (display_step == 3)
        {
            white_line.SetActive(true);
            int diff_score_num = your_score_num - nolma_score_num;
            if (diff_score_num > 0) diff_score.text += "+";
            diff_score.text += diff_score_num.ToString();
            sound_source.PlayOneShot(score_sound);
        }
    }


    [SerializeField] SpriteRenderer enemy_sprite;
    [SerializeField] SpriteRenderer player_sprite;
    [SerializeField] GameObject sad_player;
    [SerializeField] GameObject sad_enemy;
    [SerializeField] GameObject happy_player;
    [SerializeField] GameObject happy_enemy;
    [SerializeField] float sprite_alpha = 0.5f;
    [SerializeField] Text judge;
    [SerializeField] AudioClip judge_sound;
    void Display_Sprite()
    {
        if (display_step == 0)
        {
            string char_name = Beat_Action_Manager.Get_Char_Name();
            enemy_sprite.sprite = Load_Resources.Load_Sprite("Talk_Sprite/" + char_name);
        }
        else if (display_step == 4)
        {
            int diff_score_num = your_score_num - nolma_score_num;
            if (diff_score_num >= 0)
            {
                judge.text += "ÉmÉãÉ} ê¨å˜";
                judge.color = new Color(210 / 255f, 200 / 255f, 90f / 255f);
                player_sprite.color = new Color(1, 1, 1);
                enemy_sprite.color = new Color(sprite_alpha, sprite_alpha, sprite_alpha);
                sad_enemy.SetActive(true);
                happy_player.SetActive(true);
            }
            else if (diff_score_num < 0)
            {
                judge.text += "ÉmÉãÉ} é∏îs";
                judge.color = new Color(90 / 255f, 150 / 255f, 210f / 255f);
                player_sprite.color = new Color(sprite_alpha, sprite_alpha, sprite_alpha);
                enemy_sprite.color = new Color(1, 1, 1);
                sad_player.SetActive(true);
                happy_enemy.SetActive(true);
            }
            sound_source.PlayOneShot(judge_sound);
            Display_Comment();
        }
    }


    [SerializeField] Text comment;
    void Display_Comment()
    {
        string char_name = Beat_Action_Manager.Get_Char_Name();
        int difficutly_num = Beat_Action_Manager.difficulty_num;
        List<List<string>> commentDatas = Load_Resources.Load_CSV("Battle/" + char_name + "/ResultComment");

        if (your_score_num >= Score_Manager.paipoi_score) comment.text = commentDatas[difficutly_num + 1][3];
        else if (your_score_num >= Score_Manager.nolma_score) comment.text = commentDatas[difficutly_num + 1][1];
        else if (your_score_num < Score_Manager.nolma_score) comment.text = commentDatas[difficutly_num + 1][2];

    }
}
