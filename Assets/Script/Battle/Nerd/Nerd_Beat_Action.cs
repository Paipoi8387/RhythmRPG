using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Nerd_Beat_Action : Main_Beat_Action
{
    [SerializeField] AudioClip bgm1;
    [SerializeField] AudioClip bgm2;

    [SerializeField] double bpm1;
    [SerializeField] double bpm2;

    void BGM_Difficulty_Setting()
    {
        int difficutly_num = Beat_Action_Manager.difficulty_num;

        if (difficutly_num == 0)
        {
            bgm = bgm1;
            enemy_bpm = bpm1;
        }
        else if (difficutly_num == 1)
        {
            bgm = bgm2;
            enemy_bpm = bpm2;
        }
    }

    public override void Start()
    {
        BGM_Difficulty_Setting();
        base.Start();



        Load_Attack();
    }

    public override void Just_Beat_Action()
    {
        base.Just_Beat_Action();

        Instantiate_mScale();

        Switch_CharaSprite((int)(Beat_Manager.beat_num % 2));
        Change_barsPattern();
    }

    public override void Progress_Beat_Action()
    {
        base.Progress_Beat_Action();

        Cursor_Move();
    }

    public override void Load_Attack()
    {
        base.Load_Attack();
    }

    int mScale_data_num = 0;
    [SerializeField] GameObject mScale_origin;
    [SerializeField] GameObject mScale_list;
    void Instantiate_mScale()
    {
        //‚±‚±‚ÅCSV‚Ì“Ç‚Ýž‚Ý
        int beat_num = (int)Beat_Manager.beat_num;

        if (attackDatas[0][mScale_data_num] != beat_num.ToString()) return;

        GameObject mScale_prefab = Instantiate(mScale_origin, mScale_origin.transform.localPosition, Quaternion.identity);
        mScale_prefab.transform.SetParent(mScale_list.transform);

       //’·‚³A‰¹ŠK
        mScale_prefab.transform.localScale = new Vector2(float.Parse(attackDatas[1][mScale_data_num]), 0.5f);
        mScale_prefab.transform.localPosition = new Vector2(mScale_origin.transform.localPosition.x, (float)PlayWave.Get_Freq(attackDatas[2][mScale_data_num]));

        mScale_prefab.SetActive(true);

        if (attackDatas[0].Count - 1 > mScale_data_num)
        {
            mScale_data_num++;
        }
    }


    [SerializeField] GameObject cursor;
    [SerializeField] float speed = 0.25f;
    void Cursor_Move()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            cursor.GetComponent<Image>().color = new Color(0,0,0);
        }
        else
        {
            cursor.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f);
        }


        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (cursor.transform.localPosition.y > 550) return;
            cursor.transform.Translate(0, speed, 0);
            PlayWave.freq = cursor.transform.localPosition.y;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (cursor.transform.localPosition.y < 250) return;
            cursor.transform.Translate(0, -speed, 0);
            PlayWave.freq = cursor.transform.localPosition.y;
        }
    }

    [SerializeField] Image playerImage;
    [SerializeField] Image nerdImage;
    [SerializeField] Sprite[] player_sprite;
    [SerializeField] Sprite[] nerd_sprite;
    void Switch_CharaSprite(int pattern)
    {
        int sprite_num_add = 0;
        if (Input.GetKey(KeyCode.Space)) sprite_num_add = 2;

        if(pattern == 0)
        {
            playerImage.sprite = player_sprite[0 + sprite_num_add];
            nerdImage.sprite = nerd_sprite[0];
        }
        else if(pattern == 1)
        {
            playerImage.sprite = player_sprite[1 + sprite_num_add];
            nerdImage.sprite = nerd_sprite[1];
        }
    }

    [SerializeField] GameObject[] bars = new GameObject[5];
    void Change_barsPattern()
    {
        for(int i = 0; i < bars.Length; i++)
        {
            float size = Random.Range(0.5f, 7.5f);
            bars[i].transform.localScale = new Vector2(1, size);
            float r = Random.Range(0, 1f);
            float g = Random.Range(0, 1f);
            float b = Random.Range(0, 1f);
            bars[i].GetComponent<Image>().color = new Color(r, g, b, 1);
        }
    }
}
