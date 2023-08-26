using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] AudioClip get_trump_clip;
    Vector2 start_pos;
    Vector2 goal_pos;
    [SerializeField] int need_beat_num = 2;
    int start_beat_num;

    float judge_pos_y = -300;
    float judge_limit = 50f;
    bool hit_stop = false;

    void Start()
    {
        Vector2 now_pos = this.transform.localPosition;
        start_pos = now_pos;
        goal_pos = new Vector2(now_pos.x, judge_pos_y - (now_pos.y - judge_pos_y)); //”­ËˆÊ’u‚ª200A”»’è‰ÓŠ‚Í-300‚Å‹——£‚ª500
        start_beat_num = (int)Beat_Manager.beat_num + 1;
    }

    void Update()
    {
        if (hit_stop) return;

        Judge_Trump_Destroy();


        Main_Beat_Action.Move_A_to_B(this.gameObject, start_pos, goal_pos, need_beat_num, start_beat_num);

        if (this.transform.localPosition.y < judge_pos_y - 100f) Destroy(gameObject);
    }


    void Judge_Trump_Destroy()
    {
        bool is_range = judge_pos_y - judge_limit <= transform.localPosition.y && transform.localPosition.y <= judge_pos_y + judge_limit;
        bool is_correct_key = (Input.GetKeyDown(KeyCode.LeftArrow) && start_pos.x <= 0) || (Input.GetKeyDown(KeyCode.RightArrow) && start_pos.x > 0);

        if (is_range && is_correct_key)
        {
            Score_Manager.Add_Score(100);
            Destroy(gameObject, 0.25f);
            hit_stop = true;
            Sub_Beat_Action.Play_Sound(get_trump_clip);
        }
    }
}
