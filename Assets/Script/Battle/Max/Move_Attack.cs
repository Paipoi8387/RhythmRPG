using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Boxing_Max_Turn
{
    public class Move_Attack : MonoBehaviour
    {
        int start_beat_num;
        float start_count;
        int direction = 0;
        Text count_text;

        void Start()
        {
            Initialized();
            start_count = (float)(Beat_Manager.beat_num + (1 - Beat_Manager.diff));
            count_text = transform.GetChild(1).gameObject.GetComponent<Text>();
        }


        void Update()
        {
            int diff_beat_num = (int)Beat_Manager.beat_num - start_beat_num;
            Vector2 now_pos = transform.localPosition;
            Move(now_pos,diff_beat_num);

            count_text.text = (4 - diff_beat_num).ToString();

            Judge_Avoid();

            if (Judge_Destroy_Area()) Destroy(gameObject);
        }

        void Initialized()
        {
            start_beat_num = (int)Beat_Manager.beat_num;
            if (this.gameObject.name.Contains("Impact"))
            {
                transform.localPosition = new Vector2(max_trans.localPosition.x, 50);
            }
            else if (this.gameObject.name.Contains("Punch"))
            {
                if (max_trans.localPosition.x > 0) direction = -1;
                else direction = 1;

                transform.localPosition = new Vector2(max_trans.localPosition.x + (direction*50), max_trans.localPosition.y - 5);
                transform.localScale = new Vector2(-direction, 1);
            }
        }

        [SerializeField] float start_punch_pos_x = 400;
        void Move(Vector2 now_pos, int diff_beat_num)
        {
            if (this.gameObject.name.Contains("Impact"))
            {
                transform.localPosition = new Vector2(now_pos.x, (diff_beat_num * -100) + 50);
            }
            else if (this.gameObject.name.Contains("Punch"))
            {
                transform.localPosition = new Vector2(direction * ( (diff_beat_num * 100) - start_punch_pos_x), now_pos.y);
            }
        }

        bool is_success = false;
        [SerializeField] float attack_speed;
        float count;
        [SerializeField] float success_range;
        [SerializeField] AudioClip success_sound;
        [SerializeField] Max_Beat_Action max_beat_action;
        [SerializeField] Transform max_trans;
        [SerializeField] Player_Boxing player_boxing;

        void Judge_Avoid()
        {
            count = (float)(Beat_Manager.beat_num + (1 - Beat_Manager.diff));

            Vector3 now_pos = transform.localPosition;

            bool success_timing = Mathf.Abs(count - start_count - attack_speed) <= success_range;

            bool success_touched = false;
            if (this.gameObject.name.Contains("Impact"))
            {
                 success_touched = (Input.GetKeyDown(KeyCode.RightArrow) && now_pos.x <= 0 && (player_boxing.Get_AnimState("Player_Avoid_Right") || player_boxing.Get_AnimState("Player_Wait")))
                                || (Input.GetKeyDown(KeyCode.LeftArrow) && now_pos.x >= 0 && (player_boxing.Get_AnimState("Player_Avoid_Left") || player_boxing.Get_AnimState("Player_Wait")));
            }
            else if (this.gameObject.name.Contains("Punch"))
            {
                success_touched = (Input.GetKeyDown(KeyCode.UpArrow) && now_pos.y == -415 && (player_boxing.Get_AnimState("Player_Avoid_Up") || player_boxing.Get_AnimState("Player_Wait")))
                               || (Input.GetKeyDown(KeyCode.DownArrow) && now_pos.y == -295 && (player_boxing.Get_AnimState("Player_Avoid_Down") || player_boxing.Get_AnimState("Player_Wait")));
            }

            if (success_timing && success_touched)
            {
                Sub_Beat_Action.Play_Sound(success_sound);
                is_success = true;

                if (Mathf.Abs(count - start_count - attack_speed) <= success_range / 2)
                {
                    Sub_Beat_Action.Show_Judge_Text(type: 1);
                    Score_Manager.Add_Score(200);
                }
                else
                {
                    Sub_Beat_Action.Show_Judge_Text(type: 2);
                    Score_Manager.Add_Score(100);
                }

            }
            else if (Judge_Destroy_Area() && !is_success)
            {
                max_beat_action.Damage_Heart();
                Sub_Beat_Action.Show_Judge_Text(type: 3);
                Destroy(gameObject);
            }
        }

        [SerializeField] float destroy_pos;
        bool Judge_Destroy_Area()
        {
            bool can_destroy = false;

            if (this.gameObject.name.Contains("Impact"))
            {
                if (gameObject.transform.localPosition.y <= -destroy_pos)
                {
                    can_destroy = true;
                }
            }
            else if (this.gameObject.name.Contains("Punch"))
            {
                if (direction == -1 && gameObject.transform.localPosition.x <= -destroy_pos)
                {
                    can_destroy = true;
                }
                else if (direction == 1 && gameObject.transform.localPosition.x >= destroy_pos)
                {
                    can_destroy = true;
                }
            }

            return can_destroy;
        }
    }
}