using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gootara
{
    public class GroundObj : MonoBehaviour
    {
        void Start()
        {
            start_beat_num = (int)Beat_Manager.beat_num;
            Debug.Log(Mathf.Abs(start_beat_num + moved_time));
        }

        int start_beat_num;
        [SerializeField] Gootara_Beat_Action gootara_beat_action;
        bool is_damaged = false;
        void Update()
        {
            Judge_Avoid();

            if ((int)Beat_Manager.beat_num >= start_beat_num + 13)
            {
                if (gameObject.name.Contains("Spike") && !is_damaged && !is_success)
                {
                    gootara_beat_action.Damage_Heart();
                    is_damaged = true;
                }
            }
            if ((int)Beat_Manager.beat_num >= start_beat_num + 24)
            {
                Destroy(this.gameObject);
            }
        }

        //”»’è
        bool is_success =false;
        [SerializeField] float moved_time = 12;
        float success_range = 0.5f;
        [SerializeField] AudioClip success_sound;
        [SerializeField] Transform player_pos;
        int bonus = 1;
        void Judge_Avoid()
        {
            if (is_success) return;

            float count = (float)(Beat_Manager.beat_num + (1 - Beat_Manager.diff));

            bool success_timing = Mathf.Abs(count - start_beat_num - moved_time) <= success_range;
            bool success_touched = Input.GetKeyDown(KeyCode.Space);
            bool success_positon = player_pos.localPosition.y == 350f;


            if (success_timing && success_touched && success_positon)
            {
                Sub_Beat_Action.Play_Sound(success_sound);
                is_success = true;

                if (gameObject.name.Contains("Coin"))
                {
                    bonus = 2;
                }

                if (Mathf.Abs(count - start_beat_num - moved_time) <= success_range / 2)
                {
                    Sub_Beat_Action.Show_Judge_Text(type: 1, new Vector2(-700f, 500f));
                    Score_Manager.Add_Score(200 * bonus);
                }
                else
                {
                    Sub_Beat_Action.Show_Judge_Text(type: 2, new Vector2(-700f, 500f));
                    Score_Manager.Add_Score(100 * bonus);
                }

                if (gameObject.name.Contains("Coin"))
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}