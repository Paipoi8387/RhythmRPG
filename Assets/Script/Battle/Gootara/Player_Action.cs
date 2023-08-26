using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gootara
{
    public class Player_Action : MonoBehaviour
    {
        void Start()
        {
            player_anim = gameObject.GetComponent<Animator>();
            Player_Run();
        }

        void Update()
        {
            Player_All_Action();
        }

        Animator player_anim;
        enum Player_State { run, jump };
        Player_State player_state = Player_State.run;

        void Player_All_Action()
        {
            if (player_state == Player_State.run)
            {
                if (Input.GetKeyDown(KeyCode.Space)) Player_Jump();
            }
        }

        void Player_Run()
        {
            player_state = Player_State.run;
            player_anim.SetInteger("player_state_num", 1);
        }

        [SerializeField] AudioClip jump_sound;
        [SerializeField] float jump_time = 0.3f;
        void Player_Jump()
        {
            player_state = Player_State.jump;
            player_anim.SetInteger("player_state_num", 2);
            transform.localPosition = new Vector2(0, 350f);
            Sub_Beat_Action.Play_Sound(jump_sound);
            Invoke("Player_Landing", jump_time);
        }

        void Player_Landing()
        {
            Player_Run();
            transform.localPosition = new Vector2(0, 250f);
        }
    }
}