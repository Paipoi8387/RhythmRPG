using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instroment : MonoBehaviour
{
    [SerializeField] AudioClip inst_clip;
    [SerializeField] Vector2 start_pos;
    [SerializeField] Vector2 goal_pos;
    [SerializeField] int need_beat_num = 2;
    int start_beat_num;

    [SerializeField] float judge_pos_x = -500;
    [SerializeField] float judge_limit = 50f;
    bool hit_stop = false;

    void Start()
    {
        goal_pos = new Vector2(judge_pos_x - (start_pos.x - judge_pos_x), start_pos.y);
        start_beat_num = (int)Beat_Manager.beat_num + 1;
    }

    void Update()
    {
        if (hit_stop) return;

        Judge_Instroment_Destroy();


        Main_Beat_Action.Move_A_to_B(this.gameObject, start_pos, goal_pos, need_beat_num, start_beat_num);

        if (this.transform.localPosition.x < judge_pos_x - 100f)
        {
            Destroy(gameObject);
            Sub_Beat_Action.Show_Judge_Text(type: 3, text_pos);
        }
    }


    [SerializeField] Vector2 text_pos;
    void Judge_Instroment_Destroy()
    {
        bool is_range = judge_pos_x - judge_limit <= transform.localPosition.x && transform.localPosition.x <= judge_pos_x + judge_limit;
        bool is_perfect = judge_pos_x - judge_limit / 2 <= transform.localPosition.x && transform.localPosition.x <= judge_pos_x + judge_limit / 2;

        bool is_correct_key;
        if(this.gameObject.name.Contains("Bell")) is_correct_key = Input.GetKeyDown(KeyCode.A);
        else is_correct_key = Input.GetKeyDown(KeyCode.B);

        if (is_range && is_correct_key)
        {
            Destroy(gameObject, 0.25f);
            hit_stop = true;
            Sub_Beat_Action.Play_Sound(inst_clip);
            if (is_perfect)
            {
                Score_Manager.Add_Score(2);
                Sub_Beat_Action.Show_Judge_Text(type: 1, text_pos);
            }
            else
            {
                Score_Manager.Add_Score(1);
                Sub_Beat_Action.Show_Judge_Text(type: 2, text_pos);
            }
        }
    }
}
