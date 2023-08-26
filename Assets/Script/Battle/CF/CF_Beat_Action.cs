using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CF_Beat_Action : Main_Beat_Action
{
    [SerializeField] AudioClip bgm1;
    [SerializeField] AudioClip bgm2;

    [SerializeField] double bpm1;
    [SerializeField] double bpm2;

    int difficutly_num;

    void BGM_Difficulty_Setting()
    {
        difficutly_num = Beat_Action_Manager.difficulty_num;
        
        if (difficutly_num == 0)
        {
            bgm = bgm1;
            enemy_bpm = bpm1;
        }
        else if(difficutly_num == 1)
        {
            bgm = bgm2;
            enemy_bpm = bpm2;
        }
    }

    public override void Start()
    {
        BGM_Difficulty_Setting();

        base.Start();
        Load_Song_Text();
        Load_MK_Act();
        Load_Attack();
    }

    public override void Just_Beat_Action()
    {
        base.Just_Beat_Action();
        Change_Song_Text();


        MK_Action();

        Instantiate_Instroment();
    }

    public override void Progress_Beat_Action()
    {
        base.Progress_Beat_Action();
    }


    private void Update()
    {
        Play_Instroment();
    }


    public override void Load_Attack()
    {
        base.Load_Attack();
    }


    /// <summary>
    /// Ç›Ç¡Ç±ÇËÇÒÇ™îèéË
    /// </summary>
    List<List<string>> mk_act_data;
    void Load_MK_Act()
    {
        mk_act_data = Load_Resources.Load_CSV("Battle/CF/" + difficutly_num.ToString() + "_MK_Act");
    } 

    [SerializeField] Animator mk_anim;
    [SerializeField] Transform mk_trans;
    [SerializeField] GameObject hukidashi_obj;
    void MK_Action()
    {
        int beat_num = (int)Beat_Manager.beat_num;

        int index = mk_act_data[0].IndexOf(beat_num.ToString());
        if (index == -1)
        {
            if (beat_num % 2 == 0 && mk_trans.localPosition.y == 200)
            {
                mk_anim.Play("MK_Clap");
            }
        }
        else
        {
            int mk_act_num = int.Parse(mk_act_data[1][index]);

            if (mk_act_num == 0)
            {
                mk_anim.Play("MK_Appear");
            }
            else if (mk_act_num == 3)
            {
                mk_anim.Play("MK_Disappear");
            }
            else if (mk_act_num == 1)
            {
                hukidashi_obj.SetActive(true);
            }
            else if (mk_act_num == 2)
            {
                hukidashi_obj.SetActive(false);
            }
        }
    }

    /// <summary>
    /// äyäÌ
    /// </summary>
    [SerializeField] GameObject inst_list;
    [SerializeField] GameObject bell_origin;
    [SerializeField] GameObject castanet_origin;
    int inst_data_num = 0;
    void Instantiate_Instroment()
    {
        //Ç±Ç±Ç≈CSVÇÃì«Ç›çûÇ›
        int beat_num = (int)Beat_Manager.beat_num;

        if (attackDatas[0][inst_data_num] != beat_num.ToString()) return;
        GameObject instroment_origin;
        if(attackDatas[1][inst_data_num] == "0") instroment_origin = bell_origin;
        else instroment_origin = castanet_origin;

        GameObject instroment_prefab = Instantiate(instroment_origin, instroment_origin.transform.localPosition, Quaternion.identity);
        instroment_prefab.transform.SetParent(inst_list.transform);
        instroment_prefab.transform.localScale = new Vector2(1, 1);
        instroment_prefab.transform.localPosition = instroment_origin.transform.localPosition;
        instroment_prefab.SetActive(true);

        if (attackDatas[0].Count - 1 > inst_data_num)
        {
            inst_data_num++;
        }
    }

    [SerializeField] AudioClip bell_sound;
    [SerializeField] AudioClip castanets_sound;
    [SerializeField] Image bell_image;
    [SerializeField] Image castanets_image;
    void Play_Instroment()
    {
        if (Input.GetKeyDown(KeyCode.A)) Sub_Beat_Action.Play_Sound(bell_sound);
        else if (Input.GetKeyDown(KeyCode.B)) Sub_Beat_Action.Play_Sound(castanets_sound);

        if (Input.GetKey(KeyCode.A)) bell_image.color = new Color(1f, 1f, 1f);
        else bell_image.color = new Color(0.5f, 0.5f, 0.5f);

        if (Input.GetKey(KeyCode.B)) castanets_image.color = new Color(1f, 1f, 1f);
        else castanets_image.color = new Color(0.5f, 0.5f, 0.5f);
    }


    /// <summary>
    /// âÃéåÇîΩâf
    /// </summary>
    List<List<string>> song_text_data;
    void Load_Song_Text()
    {
        song_text_data = Load_Resources.Load_CSV("Battle/CF/" + difficutly_num.ToString() + "_SongText");
    }

    [SerializeField] Text song_text;
    int song_text_num = 0;
    void Change_Song_Text()
    {
        int beat_num = (int)Beat_Manager.beat_num;
        if (song_text_data[1][song_text_num] != beat_num.ToString()) return;

        song_text.text = song_text_data[0][song_text_num];
        
        if (song_text_data[0].Count - 1 > song_text_num)
        {
            song_text_num++;
        }
    }
}
