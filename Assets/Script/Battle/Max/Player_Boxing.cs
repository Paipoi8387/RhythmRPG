using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Boxing_Max_Turn
{
    public class Player_Boxing : MonoBehaviour
    {
        void Start()
        {

        }


        void Update()
        {
            Attack_Avoid();
        }



        [SerializeField] Animator player_anim;

        void Attack_Avoid()
        {
            if (!player_anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Wait")) return;

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                player_anim.Play("Player_Avoid_Right");
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                player_anim.Play("Player_Avoid_Left");
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                player_anim.Play("Player_Avoid_Up");
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                player_anim.Play("Player_Avoid_Down");
            }
        }

        public bool Get_AnimState(string animstate)
        {
            if (player_anim.GetCurrentAnimatorStateInfo(0).IsName(animstate)) return true;
            else return false;
        }
    }
}