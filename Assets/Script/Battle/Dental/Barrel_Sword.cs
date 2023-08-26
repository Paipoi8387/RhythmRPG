using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel_Sword : MonoBehaviour
{
    [SerializeField] GameObject sword_origin;

    float sword_goal_pos_x = -150f;

    [SerializeField] GameObject line;
    [SerializeField] Vector2 line_start_pos;
    [SerializeField] Vector2 line_goal_pos;
    float line_pos_y;
    [SerializeField] int need_beat_num = 3;
    [SerializeField] int start_beat_num;

    [SerializeField] GameObject hall_origin;
    List<GameObject> hall_list = new List<GameObject>();
    int max_hall_num = 3;
    int success_hall_num = 0;
    [SerializeField] float judge_limit = 15;

    [SerializeField] Animator jump_anim;

    [SerializeField] AudioClip insert_sword_clip;
    [SerializeField] AudioClip insert_barrel_clip;
    [SerializeField] AudioClip jump_barrel_clip;

    void Start()
    {
        Instantiate_All_Hall();

        start_beat_num = (int)Beat_Manager.beat_num + 1;
    }

    void Update()
    {
        //Œ•‚ÌˆÚ“®
        if (Input.GetKeyDown(KeyCode.A))
        {
            line_pos_y = line.transform.localPosition.y;
            Instantiate_Sword();
            Sub_Beat_Action.Play_Sound(insert_sword_clip);

            Judge_Sword_in_Hall();
        }

        //”»’è‰ÓŠ‚ÌˆÚ“®
        Main_Beat_Action.Move_A_to_B(line, line_start_pos, line_goal_pos, need_beat_num, start_beat_num);
    }


    void Instantiate_All_Hall()
    {
        for (int i = 0; i < max_hall_num; i++)
        {
            string beat_num = (Beat_Manager.beat_num + i).ToString();
            if (Dental_Beat_Action.attackDatas[0].Contains(beat_num))
            {
                GameObject hall_prefab = Instantiate(hall_origin, new Vector2(0, 0), Quaternion.identity);
                hall_prefab.transform.SetParent(this.transform);
                hall_prefab.transform.localScale = new Vector2(1, 1);
                hall_prefab.transform.localPosition = new Vector2(hall_origin.transform.localPosition.x, (i * -100) - 130);

                hall_list.Add(hall_prefab);
            }
        }
    }

    void Instantiate_Sword()
    {
        GameObject sword_prefab = Instantiate(sword_origin, new Vector2(0, 0), Quaternion.identity);
        sword_prefab.transform.SetParent(this.transform);
        sword_prefab.transform.localScale = new Vector2(1, 1);
        sword_prefab.transform.localPosition = new Vector2(sword_goal_pos_x, line_pos_y);
        sword_prefab.GetComponent<ParticleSystem>().Play();
    }

    void Judge_Sword_in_Hall()
    {
        for (int i = 0; i < hall_list.Count; i++)
        {
            float hall_pos_y = hall_list[i].transform.localPosition.y;

            if (hall_pos_y - judge_limit <= line_pos_y && line_pos_y <= hall_pos_y + judge_limit)
            {
                success_hall_num++;
                Score_Manager.Add_Score(100);
                Sub_Beat_Action.Play_Sound(insert_barrel_clip);
                if (success_hall_num == hall_list.Count)
                {
                    jump_anim.SetBool("jump", true);
                    Sub_Beat_Action.Play_Sound(jump_barrel_clip);
                    Score_Manager.Add_Score(100);
                }
            }
        }
    }
}
