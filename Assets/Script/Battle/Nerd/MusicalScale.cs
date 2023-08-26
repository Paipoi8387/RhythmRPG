using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalScale : MonoBehaviour
{
    [SerializeField] Vector2 start_pos;
    [SerializeField] Vector2 goal_pos;
    [SerializeField] int need_beat_num = 2;
    int start_beat_num;

    [SerializeField] float judge_pos_x = -750;

    void Start()
    {
        start_pos.y = transform.localPosition.y;
        goal_pos.y = transform.localPosition.y;
        start_beat_num = (int)Beat_Manager.beat_num + 1;
    }

    void Update()
    {
        Main_Beat_Action.Move_A_to_B(this.gameObject, start_pos, goal_pos, need_beat_num, start_beat_num);

        if (this.transform.localPosition.x < goal_pos.x + 100)
        {
            Destroy(gameObject);
        }
    }
}
