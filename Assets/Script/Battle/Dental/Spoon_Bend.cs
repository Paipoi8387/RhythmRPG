using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spoon_Bend : MonoBehaviour
{
    [SerializeField] AudioClip instraction_clip;
    [SerializeField] AudioClip switch_turn_clip;
    [SerializeField] AudioClip success_clip;

    [SerializeField] Image enemy_image;
    [SerializeField] Sprite dental_nomal;
    [SerializeField] Sprite dental_right;
    [SerializeField] Sprite dental_left;

    [SerializeField] GameObject spoon;
    [SerializeField] Sprite spoon_nomal;
    [SerializeField] Sprite spoon_bend;
    [SerializeField] GameObject particle;
    int spoon_direction = 0;

    bool instraction_turn = true;
    int turn_count = 0; //2拍に1回
    int judge_turn_count = 0; //1拍に1回

    List<int> direction_list = new List<int>();


    // Start is called before the first frame update
    void Start()
    {
        Make_Instraction();   
    }

    // Update is called once per frame
    void Update()
    {
        if (Beat_Manager.Get_is_just_beat())
        {
            judge_turn_count++;

            if ((int)Beat_Manager.beat_num % 2 == 1)
            {
                //指示出しは5から始まるけど、csvはそれより2だけ小さい、3から書く
                //sub_beat_action.Debug_Text(Beat_Manager.Get_beat_num().ToString(),new Color(1,1,1,1));

                //敵と自分のターンの切り替え
                if (turn_count == direction_list.Count)
                {
                    Switch_Turn();
                    return;
                }

                if (instraction_turn)
                {
                    Enemy_Instraction(direction_list[turn_count]);
                }
                turn_count++;
            }
            else
            {
                Enemy_Instraction(0);
            }
        }

        Spoon_Action();
    }


    void Make_Instraction()
    {
        direction_list.Clear();
        int direction_length = 3;

        for (int i = 0; i < direction_length; i++)
        {
            //direction_list.Add(Random.Range(0, 3) - 1);

            string beat_num = (Beat_Manager.beat_num + 2*i).ToString();
            if (Dental_Beat_Action.attackDatas[0].Contains(beat_num))
            {
                if (Random.Range(0, 2) == 1) direction_list.Add(1);
                else direction_list.Add(-1);
            }
            else direction_list.Add(0);
        }
    }


    void Switch_Turn()
    {
        Sub_Beat_Action.Play_Sound(switch_turn_clip);
        turn_count = 0;
        judge_turn_count = 0;

        instraction_turn = !instraction_turn;
        if (instraction_turn)
        {
            Make_Instraction();
            spoon.GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 1);
            enemy_image.color = new Color(1, 1, 1, 1);
        }
        else
        {
            spoon.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            enemy_image.color = new Color(0.3f, 0.3f, 0.3f, 1);
        }
    }


    void Enemy_Instraction(int direction)
    {
        if (direction == 0)
        {
            enemy_image.sprite = dental_nomal;
        }
        else
        {
            if (direction == 1) enemy_image.sprite = dental_right;
            else if (direction == -1) enemy_image.sprite = dental_left;

            Sub_Beat_Action.Play_Sound(instraction_clip);
        }
    }


    void Spoon_Action()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            spoon.GetComponent<Image>().sprite = spoon_bend;
            spoon_direction = -1;
            spoon.transform.localScale = new Vector2(spoon_direction, 1);
            Judge_Direction();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            spoon.GetComponent<Image>().sprite = spoon_bend;
            spoon_direction = 1;
            spoon.transform.localScale = new Vector2(spoon_direction, 1);
            Judge_Direction();
        }
        else if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            particle.GetComponent<ParticleSystem>().Stop();
            spoon.GetComponent<Image>().sprite = spoon_nomal;
            spoon_direction = 0;
            spoon.transform.localScale = new Vector2(1, 1);
        }
    }

    void Judge_Direction()
    {
        int about_passed_time = (int)Mathf.Round(judge_turn_count + 1 - (float)Beat_Manager.diff);

        if (instraction_turn || about_passed_time < 2 || about_passed_time > 6) return;
        if (direction_list[about_passed_time / 2 - 1] == spoon_direction && spoon_direction != 0)
        {
            Score_Manager.Add_Score(100);
            Sub_Beat_Action.Play_Sound(success_clip);
            //本当は良くないかもだけど、同じcountで数回判定しないように
            direction_list[about_passed_time / 2 - 1] = 0;
            particle.GetComponent<ParticleSystem>().Play();
        }
    }
}
