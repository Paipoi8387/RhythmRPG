using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boxing_Player_Turn
{
    public class Max_Defense : MonoBehaviour
    {
        private void Start()
        {
            speed = speed * (2 - Beat_Action_Manager.difficulty_num);
        }

        public void Just_Beat_Action()
        {
            count++;
            Switch_Start_Goal_Pos();
        }


        [SerializeField] int rest_beat_num = 0;

        void Update()
        {
            if (Beat_Manager.beat_num <= rest_beat_num) return;

            Move_Circle();

            Judge_Punch();
        }


        /// <summary>
        /// 判定サークル
        /// </summary>
        [SerializeField] GameObject move_circle;
        [SerializeField] Vector3 circle_start_pos;
        [SerializeField] Vector3 circle_goal_pos;
        float now_pos = 0;
        [SerializeField] float speed = 2;
        float count = 0;
        bool is_stop = true;

        void Move_Circle()
        {
            if (is_stop) return;
            now_pos = (count + 1 - (float)Beat_Manager.diff) / speed;
            move_circle.transform.localPosition = Vector3.Lerp(circle_start_pos, circle_goal_pos, now_pos);
        }

        void Switch_Start_Goal_Pos()
        {
            if (count >= speed)
            {
                Vector3 tmp_pos = circle_start_pos;
                circle_start_pos = circle_goal_pos;
                circle_goal_pos = tmp_pos;

                count = 0;
            }
        }

        /// <summary>
        /// パンチ
        /// </summary>
        /// 
        [SerializeField] Animator punch_anim;
        [SerializeField] AudioClip punch_clip;
        [SerializeField] AudioClip success_clip;
        [SerializeField] AudioClip fail_clip;

        [SerializeField] float success_range = 3;
        bool is_punched = false;

        void Judge_Punch()
        {
            is_stop = Input.GetKey(KeyCode.A);
            bool is_success_range = Mathf.Abs(move_circle.transform.localPosition.x) <= success_range;

            if (is_stop && !is_punched)
            {
                is_punched = true;

                if (is_success_range)
                {
                    punch_anim.Play("Punch");
                    Sub_Beat_Action.Play_Sound(success_clip);
                    Score_Manager.Add_Score(200);
                }
                else
                {
                    Sub_Beat_Action.Play_Sound(fail_clip);
                }
            }
            else if(!is_stop && is_punched) is_punched = false;
        }



        [SerializeField] Animator max_anim;
        [SerializeField] Animator screen_anim;
        void After_Punch()
        {
            Sub_Beat_Action.Play_Sound(punch_clip);
            screen_anim.Play("Punch_Screen");
            max_anim.Play("Max_Painful");
        }
    }
}
