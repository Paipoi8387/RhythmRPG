using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Boxing_Max_Turn
{
    public class Max_Attack : MonoBehaviour
    {
        /// <summary>
        /// çUåÇÇÃÉÇÅ[ÉVÉáÉì
        /// </summary>
        /// 

        public void Just_Beat_Action(int _beat_num, int _attack_direction)
        {
            Attack_Motion(_beat_num,_attack_direction);
        }

        [SerializeField] Transform max_trans;
        [SerializeField] float attack_distance = 60;

        enum Max_Attack_State { Wait, Prepare, Swing };
        [SerializeField] Max_Attack_State max_attack_state = Max_Attack_State.Wait;

        int attack_direction;
        [SerializeField] Image max_image;
        [SerializeField] Animator max_anim;
        [SerializeField] Sprite max_prepare_front;
        [SerializeField] Sprite max_swing_front;
        [SerializeField] Sprite max_prepare_side;
        [SerializeField] Sprite max_swing_side;

        [SerializeField] AudioClip prepare_sound;
        [SerializeField] AudioClip swing_sound;

        [SerializeField] int rest_beat_num = 0;

        void Attack_Motion(int _beat_num, int _attack_direction)
        {
            if ((int)Beat_Manager.beat_num < rest_beat_num) return;

            if (max_attack_state == Max_Attack_State.Wait && _beat_num == (int)Beat_Manager.beat_num)
            {
                max_attack_state = Max_Attack_State.Prepare;
                attack_direction = _attack_direction;
            }

            if (max_attack_state == Max_Attack_State.Prepare)
            {
                if (attack_direction == 0)
                {
                    int ran_num = Random.Range(-1, 2);
                    max_trans.localPosition = new Vector2(ran_num * attack_distance, 100f);
                    max_anim.enabled = false;
                    max_image.sprite = max_prepare_front;
                }
                else if (attack_direction == 1)
                {
                    int ran_num = Random.Range(0, 2);
                    if (ran_num == 0) ran_num = -1;
                    max_trans.localPosition = new Vector2(ran_num * 450f, -350f + ran_num * attack_distance);
                    max_trans.localScale = new Vector2(ran_num, 1);
                    max_anim.enabled = false;
                    max_image.sprite = max_prepare_side;
                }
                Sub_Beat_Action.Play_Sound(prepare_sound);
                max_attack_state = Max_Attack_State.Swing;
            }
            else if (max_attack_state == Max_Attack_State.Swing)
            {
                if (attack_direction == 0)
                {
                    max_image.sprite = max_swing_front;
                    if(max_trans.localPosition.x == attack_distance) Instantiate_Move_Attack(impact_right_origin);
                    else if (max_trans.localPosition.x == -attack_distance) Instantiate_Move_Attack(impact_left_origin);
                    else if (max_trans.localPosition.x == 0) Instantiate_Move_Attack(impact_center_origin);
                }
                else if (attack_direction == 1)
                {
                    max_image.sprite = max_swing_side;
                    if(max_trans.localPosition.y == -410) Instantiate_Move_Attack(punch_down_origin);
                    else if (max_trans.localPosition.y == -290) Instantiate_Move_Attack(punch_up_origin);
                }
                max_attack_state = Max_Attack_State.Wait;
            }
            else if (max_attack_state == Max_Attack_State.Wait)
            {
                max_anim.enabled = true;
            }
        }

        [SerializeField] GameObject move_attack_area;
        [SerializeField] GameObject impact_center_origin;
        [SerializeField] GameObject impact_right_origin;
        [SerializeField] GameObject impact_left_origin;
        [SerializeField] GameObject punch_up_origin;
        [SerializeField] GameObject punch_down_origin;

        void Instantiate_Move_Attack(GameObject move_attack_origin)
        {
            GameObject move_attack_prefab = Instantiate(move_attack_origin, new Vector2(0, 0), Quaternion.identity);
            move_attack_prefab.transform.SetParent(move_attack_area.transform);
            move_attack_prefab.transform.localScale = move_attack_origin.transform.localScale;
            move_attack_prefab.SetActive(true);
            Sub_Beat_Action.Play_Sound(swing_sound);
        }
    }
}