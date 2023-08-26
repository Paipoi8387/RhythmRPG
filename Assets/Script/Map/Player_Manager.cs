using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KoganeUnityLib.Example;
using UnityEngine.SceneManagement;
using TMPro;

public class Player_Manager : MonoBehaviour
{
    enum Operation_State { Move, Warp, Talk , Before_Battle};
    Operation_State ope_state = Operation_State.Move;
    [SerializeField] bool is_initialize = true;

    void Start()
    {
        player_anim = GetComponent<Animator>();
        if(is_initialize) Initialize();
    }


    void Update()
    {
        if (ope_state == Operation_State.Move)
        {
            if (can_talk)
            {
                Talk_Operation();
            }
        }
        else if (ope_state == Operation_State.Talk)
        {
            Talk_Operation();
        }
        else if (ope_state == Operation_State.Before_Battle)
        {
            Before_Battle_Operation();
        }
    }

    void FixedUpdate()
    {
        if (ope_state == Operation_State.Move)
        {
            Walk_Operation();
        }
    }


    static int zone_num = 0;
    static float pos_x = 0;
    static float pos_y = 0;
    void Initialize()
    {
        transform.localPosition = new Vector2(pos_x, pos_y);

        zone_parent[zone_parent_num].transform.GetChild(zone_num).gameObject.SetActive(true);
    }

    void Set_OpeState(Operation_State _ope_state)
    {
        ope_state = _ope_state;
    }

    /// <summary>
    /// 歩く
    /// </summary>
    Animator player_anim;
    int direction = 0;
    [SerializeField] float speed = 0.1f;
    [SerializeField] GameObject main_camera;

    void Walk_Operation()
    {
        int x = 0;
        if (Input.GetKey(KeyCode.LeftArrow)) x = -1;
        else if (Input.GetKey(KeyCode.RightArrow)) x = 1;

        int y = 0;
        if (Input.GetKey(KeyCode.DownArrow)) y = -1;
        else if (Input.GetKey(KeyCode.UpArrow)) y = 1;

        transform.Translate(x * speed, y * speed, 0);


        if (!player_anim.enabled && !(x == 0 && y == 0)) player_anim.enabled = true;

        if (y > 0) { player_anim.Play("P_Walk_Back"); direction = 1; }
        else if (y < 0) { player_anim.Play("P_Walk_Front"); direction = 0; }
        else if (x > 0) { player_anim.Play("P_Walk_Right"); direction = 2; }
        else if (x < 0) { player_anim.Play("P_Walk_Left"); direction = 3; }
        else if (x == 0 && y == 0)
        {
            player_anim.enabled = false;
            Set_Wait_Sprite();
        }

        Camera_Positioning();
    }

    [SerializeField] SpriteRenderer player_sprite;
    [SerializeField] Sprite player_front;
    [SerializeField] Sprite player_back;
    [SerializeField] Sprite player_right;
    [SerializeField] Sprite player_left;
    void Set_Wait_Sprite()
    {
        if (direction == 0) player_sprite.sprite = player_front;
        else if (direction == 1) player_sprite.sprite = player_back;
        else if (direction == 2) player_sprite.sprite = player_right;
        else if (direction == 3) player_sprite.sprite = player_left;
    }


    [SerializeField] bool cameraMove_x = false;
    [SerializeField] bool cameraMove_y = false;
    void Camera_Positioning()
    {
        if (cameraMove_x && cameraMove_y)
        {
            main_camera.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -10f);
        }
        else if (cameraMove_x && !cameraMove_y)
        {
            main_camera.transform.localPosition = new Vector3(transform.localPosition.x, 0, -10f);
        }
        else if (!cameraMove_x && cameraMove_y)
        {
            main_camera.transform.localPosition = new Vector3(0, transform.localPosition.y, -10f);
        }
        else
        {
            main_camera.transform.localPosition = new Vector3(0, 0, -10f);
        }
    }

    void Reset_Operation_State()
    {
        warp_anim.Play("Start");
        Set_OpeState(Operation_State.Move);
    }



    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Warp" || collision.tag == "LongWarp")
        {
            Warp warp = collision.GetComponent<Warp>();
            Warp_Operation(warp,collision.tag);

            cameraMove_x = warp.cameraMove_x;
            cameraMove_y = warp.cameraMove_y;
            Camera_Positioning();
        }
        else if (collision.tag == "Character")
        {
            string talk_csv_name = "Talk_Text/" + collision.name;
            talkDatas = Load_Resources.Load_CSV(talk_csv_name);
            chara_sprite.color = collision.gameObject.GetComponent<SpriteRenderer>().color;
            can_talk = true;
        }
        else if (collision.tag == "BattleWarp")
        {
            int char_num = int.Parse(collision.name.Replace("BattleWarp_", ""));
            Beat_Action_Manager.char_num = char_num;

            player_anim.enabled = false;
            Set_Wait_Sprite();

            before_battle.SetActive(true);
            Select_Difficulty_Anim();
            Show_Score_Text();

            Set_OpeState(Operation_State.Before_Battle);

        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Character")
        {
            can_talk = false;
        }
    }




    /// <summary>
    /// ワープ
    /// </summary>
    [SerializeField] int zone_parent_num = 0;
    [SerializeField] Animator warp_anim;
    GameObject active_zone;
    [SerializeField] GameObject[] zone_parent;

    void Warp_Operation(Warp warp, string warp_tag)
    {
        if (warp_tag == "Warp")
        {
            warp_anim.Play("Warp_Fade");
            Invoke("Reset_Operation_State", 0.5f);
        }
        if (warp_tag == "LongWarp")
        {
            warp_anim.Play("LongWarp_Fade");
            Invoke("Reset_Operation_State", 3f);
        }

        Set_OpeState(Operation_State.Warp);

        if(active_zone != null) active_zone.SetActive(false);
        else
        {
            for (int i = 0; i < zone_parent[zone_parent_num].transform.childCount; i++)
            {
                zone_parent[zone_parent_num].transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        zone_num = warp.next_zone_num;
        GameObject next_zone = zone_parent[zone_parent_num].transform.GetChild(warp.next_zone_num).gameObject;
        active_zone = next_zone;
        next_zone.SetActive(true);

        Vector3 player_pos = transform.localPosition;
        if (!warp.fix_x) player_pos.x = warp.next_player_pos.x;
        if (!warp.fix_y) player_pos.y = warp.next_player_pos.y;
        transform.localPosition = player_pos;

        if (direction == 1 && warp.reverse_direction_y) direction = 0;
        else if (direction == 0 && warp.reverse_direction_y) direction = 1;
        else if (direction == 2 && warp.reverse_direction_x) direction = 3;
        else if (direction == 3 && warp.reverse_direction_x) direction = 2;
        player_anim.enabled = false;
        Set_Wait_Sprite();
    }


    [SerializeField] GameObject before_battle;
    void Before_Battle_Operation()
    {
        Select_Difficulty();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            pos_x = transform.localPosition.x;
            pos_y = transform.localPosition.y - 1;
            warp_anim.Play("BattleWarp_Fade");
            Invoke("BattleWarp_Operation", delay_battlewarp_second);
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Set_OpeState(Operation_State.Move);
            player_anim.enabled = true;
            before_battle.SetActive(false);
        }
    }

    [SerializeField] GameObject difficulty;
    int difficulty_num = 0;
    void Select_Difficulty()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            difficulty_num = 1;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            difficulty_num = 0;
        }
        else
        {
            return;
        }

        //キー入力してればここにたどり着く
        Beat_Action_Manager.difficulty_num = difficulty_num;
        Select_Difficulty_Anim();
        Show_Score_Text();
    }

    void Select_Difficulty_Anim()
    {
        for (int i = 0; i < 2; i++)
        {
            Animator difficulty_anim = difficulty.transform.GetChild(i).gameObject.GetComponent<Animator>();
            if (i == difficulty_num) difficulty_anim.Play("Select_Difficulty");
            else difficulty_anim.Play("Wait");
        }
    }

    [SerializeField] TextMeshProUGUI best_score_text;
    [SerializeField] TextMeshProUGUI best_text;
    [SerializeField] TextMeshProUGUI nolma_score_text;
    [SerializeField] TextMeshProUGUI paipoi_score_text;
    void Show_Score_Text()
    {
        Score_Manager.Load_Score();
        //scoreにたいして何かしらの操作

        best_score_text.text = Score_Manager.best_score.ToString();
        best_score_text.color = difficulty.transform.GetChild(difficulty_num).gameObject.GetComponent<Image>().color;
        best_text.color = difficulty.transform.GetChild(difficulty_num).gameObject.GetComponent<Image>().color;

        nolma_score_text.text = Score_Manager.nolma_score.ToString();
        paipoi_score_text.text = Score_Manager.paipoi_score.ToString();
    }


    [SerializeField] float delay_battlewarp_second = 1;
    void BattleWarp_Operation()
    {
        SceneManager.LoadScene("Battle");
    }



    /// <summary>
    /// 会話
    /// </summary>
    [SerializeField] Talk_Manager talk_manager;
    [SerializeField] GameObject talk_box;
    [SerializeField] SpriteRenderer chara_sprite;
    List<List<string>> talkDatas;
    int talk_count = 0;

    bool can_talk = false;

    void Talk_Operation()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (ope_state != Operation_State.Talk) Set_OpeState(Operation_State.Talk);
            if (talk_count == 0) talk_box.SetActive(true);

            CancelInvoke("Play_Talk_Sound");
            if (talk_manager.show_all_content)
            {
                if (talk_count == talkDatas.Count)
                {
                    talk_count = 0;
                    talk_box.SetActive(false);
                    Set_OpeState(Operation_State.Move);
                    return;
                }

                talk_manager.show_all_content = false;
                talk_manager.Show_Content(talkDatas[talk_count][0]);
                chara_sprite.sprite = Load_Resources.Load_Sprite("Talk_Sprite/" + talkDatas[talk_count][1]);

                talk_count++;
            }
            else
            {
                talk_manager.Skip_Content();
            }
        }
    }
}