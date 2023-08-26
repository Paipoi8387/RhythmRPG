using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] AudioClip catch_bird_clip;

    GameObject bird;
    GameObject feather;

    int direction = 1;

    [SerializeField] Vector2 start_pos;
    [SerializeField] Vector2 goal_pos;
    float judge_limit = 50f;
    [SerializeField] int need_beat_num = 3;
    int start_beat_num = -1;


    void Start()
    {
        bird = transform.Find("Bird").gameObject;
        feather = transform.Find("Feather").gameObject;

        if (Random.Range(0, 2) == 1) direction *= -1;

        start_pos = new Vector2(start_pos.x * direction, start_pos.y * direction);
        goal_pos = new Vector2(goal_pos.x * direction, goal_pos.y * direction);
        transform.localPosition = start_pos;
        bird.transform.localScale = new Vector2(direction * bird.transform.localScale.x,direction * bird.transform.localScale.y);

        start_beat_num = (int)Beat_Manager.beat_num + 1;
    }


    void Update()
    {
        if (!bird.activeSelf) return;

        Judge_Bird_Catch();

        Main_Beat_Action.Move_A_to_B(gameObject, start_pos, goal_pos, need_beat_num, start_beat_num);

        if (Mathf.Abs(transform.localPosition.x - goal_pos.x) <= 10f) Destroy(gameObject);
    }


    void Judge_Bird_Catch()
    {
        bool is_range = -judge_limit <= transform.localPosition.x && transform.localPosition.x <= judge_limit;
        bool is_correct_key = (Input.GetKey(KeyCode.UpArrow) && direction == 1) || (Input.GetKey(KeyCode.DownArrow) && direction == -1);

        if (is_range && is_correct_key)
        {
            Destroy(gameObject, 0.5f);
            bird.SetActive(false);
            feather.SetActive(true);
            Sub_Beat_Action.Play_Sound(catch_bird_clip);
            Score_Manager.Add_Score(100);
        }
    }
}
