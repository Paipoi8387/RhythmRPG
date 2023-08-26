using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tembo_Beat_Action : Main_Beat_Action
{
    public override void Start()
    {
        base.Start();

        //Load_Attack();
    }

    public override void Just_Beat_Action()
    {
        base.Just_Beat_Action();
        Music_Roop();

        if (Beat_Action_Manager.difficulty_num == 0)
        {
            if (Beat_Manager.beat_num % 4 == 1) Instantiate_block();
        }
        else if (Beat_Action_Manager.difficulty_num == 1)
        {
            if (Beat_Manager.beat_num % 2 == 1) Instantiate_block();
            if (Beat_Manager.beat_num % 4 == 1) tembo_anim.Play("Tembo_Shake");
        }
    }

    public override void Progress_Beat_Action()
    {
        base.Progress_Beat_Action();
    }

    public override void Load_Attack()
    {
        base.Load_Attack();
    }

    /// <summary>
    /// ブロック生成
    /// </summary>
    [SerializeField] GameObject block_list;
    [SerializeField] GameObject red_block_origin;
    [SerializeField] GameObject blue_block_origin;
    [SerializeField] GameObject green_block_origin;

    void Instantiate_block()
    {
        GameObject block_origin;
        int ran_num = Random.Range(0, 3);
        if (ran_num == 0) block_origin = red_block_origin;
        else if (ran_num == 1) block_origin = blue_block_origin;
        else block_origin = green_block_origin;

        GameObject block_prefab = Instantiate(block_origin, block_origin.transform.localPosition, Quaternion.identity);
        block_prefab.transform.SetParent(block_list.transform);
        block_prefab.transform.localScale = new Vector2(1, 1);
        block_prefab.transform.localPosition = block_origin.transform.localPosition;
        block_prefab.SetActive(true);
    }


    /// <summary>
    /// 音楽ループ
    /// </summary>
    int start_timeSamples = 19756;
    int start_beat_num = 2;
    int roop_num = 2;
    [SerializeField] int finish_beat_num = 13;
    [SerializeField] Animator tembo_anim;
    [SerializeField] Text roop_text;
    void Music_Roop()
    {
        if (roop_num == 0) return;

        if ((int)Beat_Manager.beat_num == finish_beat_num)
        {
            Beat_Manager.beat_num = start_beat_num;
            Beat_Manager.GetSet_timeSamples = start_timeSamples;
            roop_text.text = "";
            roop_num--;
        }
        else if ((int)Beat_Manager.beat_num == finish_beat_num - 2)
        {
            tembo_anim.Play("Tembo_Cartain");
            if(roop_num == 2) roop_text.text = "あと２かい";
            else if (roop_num == 1) roop_text.text = "さいご";
        }
    }
}
