using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dental_Beat_Action : Main_Beat_Action
{
    [SerializeField] GameObject dental_anim;
    [SerializeField] GameObject trump_bullet;
    [SerializeField] GameObject barrel_sword;
    [SerializeField] GameObject bird_catch;
    [SerializeField] GameObject spoon_bend;

    public override void Start()
    {
        base.Start();
        
        //攻撃
        switch_anim = dental_anim;
        action1 = trump_bullet;
        action2 = barrel_sword;
        action3 = bird_catch;
        action4 = spoon_bend;

        Load_Attack();
        Instantiate_Action_Object();
    }

    public override void Just_Beat_Action()
    {
        base.Just_Beat_Action();
        Change_Action_Object();

        Scale_Score_Bar();
    }

    public override void Progress_Beat_Action()
    {
        base.Progress_Beat_Action();

        Scale_Score_Bar();
    }



    public override void Load_Attack()
    {
        base.Load_Attack();

        switch_action_mode(attackDatas[1][0]);
        duration = int.Parse(attackDatas[2][0]);
        max_score = float.Parse(attackDatas[3][0]);
    }


    /// <summary>
    /// スコアバー
    /// </summary>
    [SerializeField] Transform score_bar_trans;
    float max_score = 0;
    void Scale_Score_Bar()
    {
        float rate = Score_Manager.score / max_score;
        score_bar_trans.localScale = new Vector3(rate, 1,1);
    }


    /// <summary>
    /// 攻撃の切り替え
    /// </summary>
    public enum Action_Mode { Switch_Anim, Action1, Action2, Action3, Action4 };
    public Action_Mode action_mode = Action_Mode.Switch_Anim;

    [HideInInspector] public GameObject switch_anim;
    [HideInInspector] public GameObject action1;
    [HideInInspector] public GameObject action2;
    [HideInInspector] public GameObject action3;
    [HideInInspector] public GameObject action4;
    [SerializeField] GameObject trump_explain;
    [SerializeField] GameObject sword_explain;
    [SerializeField] GameObject bird_explain;
    [SerializeField] GameObject spoon_explain;

    int duration;
    int start_beat_num = 0;
    public AudioClip switch_audio_clip;


    void Destroy_Action_Object()
    {
        GameObject target = gameObject.transform.Find("Prefab_Action_Object").gameObject;
        Destroy(target);
    }


    void Instantiate_Action_Object()
    {
        GameObject action_object = null;
        if (action_mode == Action_Mode.Action1) action_object = action1;
        else if (action_mode == Action_Mode.Action2) action_object = action2;
        else if (action_mode == Action_Mode.Action3) action_object = action3;
        else if (action_mode == Action_Mode.Action4) action_object = action4;

        if (action_object == null) return;
        GameObject prefab_action_object = Instantiate(action_object, new Vector2(0, 0), Quaternion.identity);
        prefab_action_object.name = "Prefab_Action_Object";
        prefab_action_object.transform.SetParent(this.gameObject.transform);
        prefab_action_object.transform.localScale = new Vector2(1, 1);
        prefab_action_object.transform.localPosition = new Vector2(0, 0);
    }

    void switch_action_mode(string action_mode_name)
    {
        if (action_mode_name == "Action1")
        {
            action_mode = Action_Mode.Action1;
            Switch_Explain(1);
        }
        else if (action_mode_name == "Action2")
        {
            action_mode = Action_Mode.Action2;
            Switch_Explain(2);
        }
        else if (action_mode_name == "Action3")
        {
            action_mode = Action_Mode.Action3;
            Switch_Explain(3);
        }
        else if (action_mode_name == "Action4")
        {
            action_mode = Action_Mode.Action4;
            Switch_Explain(4);
        }
    }

    void Change_Action_Object()
    {
        int beat_num = (int)Beat_Manager.beat_num;
        int playing_beat_num = beat_num - start_beat_num;

        if (playing_beat_num + 1 == duration)
        {
            Destroy_Action_Object();

            Sub_Beat_Action.Play_Sound(switch_audio_clip);
            action_mode = Action_Mode.Switch_Anim;
            switch_anim.SetActive(true);
        }
        else if (playing_beat_num == duration)
        {
            switch_anim.SetActive(false);

            int index = attackDatas[0].IndexOf(beat_num.ToString());

            if (index == -1) return; 
            
            switch_action_mode(attackDatas[1][index]);
            

            //現時点ではdurationの設定ができてないので、失敗する
            Instantiate_Action_Object();
            start_beat_num = beat_num;
            duration = int.Parse(attackDatas[2][index]);
        }
    }


    void Switch_Explain(int action_mode_num)
    {
        trump_explain.SetActive(false);
        sword_explain.SetActive(false);
        bird_explain.SetActive(false);
        spoon_explain.SetActive(false);

        if (action_mode_num == 1) trump_explain.SetActive(true);
        else if (action_mode_num == 2) sword_explain.SetActive(true);
        else if (action_mode_num == 3) bird_explain.SetActive(true);
        else if (action_mode_num == 2) spoon_explain.SetActive(true);
    }
}
