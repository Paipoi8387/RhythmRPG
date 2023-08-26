using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boxing_Max_Turn;
using Boxing_Player_Turn;
using UnityEngine.UI;
using TMPro;

public class Max_Beat_Action : Main_Beat_Action
{
    enum Max_State {Max_Attack, Max_Defense ,Max_Rest};
    [SerializeField] Max_State max_state = Max_State.Max_Attack;

    [SerializeField] GameObject max_attack_obj;
    [SerializeField] GameObject max_defense_obj;
    [SerializeField] GameObject max_rest_obj;
    Max_Attack max_attack;
    Max_Defense max_defense;

    [SerializeField] Animator screen_anim;

    public override void Start()
    {
        base.Start();
        max_attack = max_attack_obj.GetComponent<Max_Attack>();
        max_defense = max_defense_obj.GetComponent<Max_Defense>();

        Action_Setting(max_state);

        heart_array = new GameObject[] {heart_l,heart_c,heart_r };
        Load_Attack();
    }

    public override void Just_Beat_Action()
    {
        base.Just_Beat_Action();

        Switch_State();
        Change_Round_Text();

        if (max_state == Max_State.Max_Attack)
        {
            int _beat_num = int.Parse(attackDatas[0][csv_row_num]) + (round_num - 2) * (round2_start - round1_start);
            int _attack_direction = int.Parse(attackDatas[1][csv_row_num]);
            max_attack.Just_Beat_Action(_beat_num, _attack_direction);

            if (_beat_num == (int)Beat_Manager.beat_num && csv_row_num < attackDatas[0].Count - 1) csv_row_num++;
        }
        else if (max_state == Max_State.Max_Defense)
        {
            max_defense.Just_Beat_Action();
        }

        Audience_Jump();
    }


    public override void Progress_Beat_Action()
    {
        base.Progress_Beat_Action();

        //Sub_Beat_Action.Debug_Text(Beat_Manager.beat_num.ToString(), new Color(1, 1, 1));
    }

    int csv_row_num = 0;
    public override void Load_Attack()
    {
        base.Load_Attack();
    }

    [SerializeField] int round1_start;
    [SerializeField] int round1_finish;
    [SerializeField] int round2_start;
    [SerializeField] int round2_finish;
    [SerializeField] int round3_start;
    [SerializeField] int round3_finish;
    void Switch_State()
    {
        if (Beat_Manager.beat_num == round1_start)
        {
            Action_Setting(Max_State.Max_Attack);
            Change_Explain_Text(0);
        }
        else if (Beat_Manager.beat_num == round1_finish)
        {
            Action_Setting(Max_State.Max_Rest);
            Change_Explain_Text(2);
        }
        else if (Beat_Manager.beat_num == round2_start)
        {
            Change_Explain_Text(0);
            Action_Setting(Max_State.Max_Defense);
        }
        else if (Beat_Manager.beat_num == round2_finish)
        {
            Action_Setting(Max_State.Max_Rest);
            Change_Explain_Text(1);
        }
        else if (Beat_Manager.beat_num == round3_start)
        {
            Action_Setting(Max_State.Max_Attack);
            Change_Explain_Text(0);
        }
        else if (Beat_Manager.beat_num == round3_finish) Action_Setting(Max_State.Max_Rest);
    }

    [SerializeField] TextMeshProUGUI round_text;
    [SerializeField] AudioClip bell_sound;
    [SerializeField] AudioClip show_sound;
    int round_num = 1;

    void Change_Round_Text()
    {
        double beat_num = Beat_Manager.beat_num;

        if(beat_num == round1_start - 12 || beat_num == round2_start - 12 || beat_num == round3_start - 12)
        {
            round_text.text = "ROUND";
            Sub_Beat_Action.Play_Sound(show_sound);
        }
        else if (beat_num == round1_start - 8 || beat_num == round2_start - 8 || beat_num == round3_start - 8)
        {
            round_text.text = round_num.ToString();
            Sub_Beat_Action.Play_Sound(show_sound);
        }
        else if (beat_num == round1_start - 4 || beat_num == round2_start - 4 || beat_num == round3_start - 4)
        {
            round_text.text = "FIGHT";
            Sub_Beat_Action.Play_Sound(bell_sound);
        }
        else if (beat_num == round1_start || beat_num == round2_start || beat_num == round3_start)
        {
            round_num++;
            round_text.text = "";
        }
    }

    [SerializeField] Animator max_anim;
    [SerializeField] GameObject max_obj;
    [SerializeField] GameObject move_attack_area;
    void Action_Setting(Max_State _max_state)
    {
        max_anim.enabled = true;
        max_state = _max_state;
        max_obj.transform.localPosition = new Vector2(0, 100);

        if (_max_state == Max_State.Max_Attack)
        {
            max_attack_obj.SetActive(true);
            max_defense_obj.SetActive(false);
            max_rest_obj.SetActive(false);
            screen_anim.Play("Nomal_Screen");
            max_anim.Play("Max_Stand");
            csv_row_num = 0;
        }
        else if (_max_state == Max_State.Max_Defense)
        {
            max_attack_obj.SetActive(false);
            max_defense_obj.SetActive(true);
            max_rest_obj.SetActive(false);
            screen_anim.Play("Near_Screen");
            max_anim.Play("Max_Exhaust");
        }
        else if (_max_state == Max_State.Max_Rest)
        {
            max_attack_obj.SetActive(false);
            max_defense_obj.SetActive(false);
            max_rest_obj.SetActive(true);
            screen_anim.Play("Nomal_Screen");
            max_anim.Play("Max_Stand");

            for (int i = 5; i < move_attack_area.transform.childCount; i++)
            {
                Destroy(move_attack_area.transform.GetChild(i).gameObject);
            }
        }
    }


    /// <summary>
    /// 観客
    /// </summary>
    [SerializeField] Transform audience_trans;

    void Audience_Jump()
    {
        if (audience_trans.localPosition.y == 0)
        {
            audience_trans.localPosition = new Vector2(0, 10);
        }
        else if (audience_trans.localPosition.y == 10)
        {
            audience_trans.localPosition = new Vector2(0, 0);
        }
    }

    [SerializeField] Text explain_text;
    void Change_Explain_Text(int state_num)
    {
        if(state_num == 0)
        {
            explain_text.text = "";
        }
        else if(state_num == 1)
        {
            explain_text.text = "タイミング良く～\n\n十字キーで避けろ～～！！";
        }
        else if (state_num == 2)
        {
            explain_text.text = "カーソルの真ん中で～\n\nAボタンでパンチ～～！！";
        }
    }


    /// <summary>
    /// ライフ管理
    /// </summary>

    int heart_num = 3;
    [SerializeField] GameObject heart_l;
    [SerializeField] GameObject heart_c;
    [SerializeField] GameObject heart_r;
    GameObject[] heart_array;
    [SerializeField] Beat_Manager beat_manager;

    [SerializeField] AudioClip damage_sound;

    public void Damage_Heart()
    {
        heart_num--;
        if (heart_num >= 0) heart_array[heart_num].GetComponent<Animator>().Play("Heart_Flash");

        Sub_Beat_Action.Play_Sound(damage_sound);

        screen_anim.Play("Damage_Screen");

        if(heart_num == 0)
        {
            beat_manager.Finish_Operation();
        }
    }
}
