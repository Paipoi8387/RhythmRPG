using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tembo
{
    public class Player_Action : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            player_anim = gameObject.GetComponent<Animator>();
            player_rb = gameObject.GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            Player_All_Action();
        }

        void FixedUpdate()
        {
            if(player_state != Player_State.damage) Player_Move();
        }



        Rigidbody2D player_rb;
        [SerializeField] float upforce = 550;
        [SerializeField] float speed = 0.01f;

        Animator player_anim;
        enum Player_State { wait, run, jump, damage };
        Player_State player_state = Player_State.wait;

        void Player_All_Action()
        {
            if (player_state == Player_State.wait)
            {
                if (Input.GetKeyDown(KeyCode.Space)) Player_Jump();
                else if (Player_Direction().x != 0) Player_Run();
            }
            else if (player_state == Player_State.run)
            {
                if (Input.GetKeyDown(KeyCode.Space)) Player_Jump();
                else if (Player_Direction().x != 0) Player_Run();
                else Player_Wait();
            }
        }


        void Player_Wait()
        {
            player_state = Player_State.wait;
            player_anim.SetInteger("player_state_num", 0);
        }

        Vector2 Player_Direction()
        {
            int x = 0;

            if (Input.GetKey(KeyCode.RightArrow)) x = 1;
            else if (Input.GetKey(KeyCode.LeftArrow)) x = -1;

            return new Vector2(x, 0);
        }

        void Player_Move()
        {
            Vector2 now_pos = transform.localPosition;

            float move_x = Player_Direction().x;

            if (now_pos.x + move_x * speed < -1000)
            {
                transform.localPosition = new Vector2(950, now_pos.y);
            }
            else if (now_pos.x + move_x * speed > 1000)
            {
                transform.localPosition = new Vector2(-950, now_pos.y);
            }
            else
            {
                transform.localPosition = new Vector2(now_pos.x + move_x * speed, now_pos.y);
            }
        }

        void Player_Run()
        {
            player_state = Player_State.run;

            if (Player_Direction().x == 1) gameObject.transform.localScale = new Vector2(1, 1);
            else if (Player_Direction().x == -1) gameObject.transform.localScale = new Vector2(-1, 1);

            player_anim.SetInteger("player_state_num", 1);
        }

        [SerializeField] float y_ground;
        [SerializeField] float y_one_block;
        [SerializeField] float y_two_block;
        [SerializeField] float y_diff;

        void Player_Jump()
        {
            float y = transform.localPosition.y;
            if (Mathf.Abs(y - y_ground) < y_diff || Mathf.Abs(y - y_one_block) < y_diff || Mathf.Abs(y - y_two_block) < y_diff)
            {
                player_state = Player_State.jump;
                player_anim.SetInteger("player_state_num", 2);

                player_rb.AddForce(new Vector2(0, upforce));
            }
        }

        public static bool is_ceiling = false;
        public static string block_name = "";
        [SerializeField] AudioClip damage_sound;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Ground")
            {
                if (Player_Direction().x != 0) Player_Run();
                else Player_Wait();
            }
            else if (collision.gameObject.tag == "Ceiling")
            {
                is_ceiling = true;
                block_name = collision.gameObject.transform.parent.gameObject.name;
                Play_Block_Anim(block_name);
            }
            
            if (collision.gameObject.tag == "Enemy")
            {
                player_anim.SetInteger("player_state_num", 3);
                player_state = Player_State.damage;
                Invoke("Player_Wait",3);
                Sub_Beat_Action.Play_Sound(damage_sound);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Ceiling")
            {
                is_ceiling = false;
                block_name = "";
            }
        }


        [SerializeField] Animator red_block_anim;
        [SerializeField] Animator blue_block_anim;
        [SerializeField] Animator green_block_anim;
        void Play_Block_Anim(string block_name)
        {
            if (block_name == "›_Block") red_block_anim.Play("Clap_Block");
            else if (block_name == "¢_Block") green_block_anim.Play("Clap_Block");
            else if (block_name == " _Block") blue_block_anim.Play("Clap_Block");
        }
    }
}