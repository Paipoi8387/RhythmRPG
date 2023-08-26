using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Beat_Action : MonoBehaviour
{
    [HideInInspector] public Sub_Beat_Action sub_beat_action;
    [SerializeField] bool is_play_beat;

    /// <summary>
    /// �����I�ɍs���鑀��
    /// </summary>

    public virtual void Start()
    {
        sub_beat_action = GameObject.Find("Sub_Beat_Action").GetComponent<Sub_Beat_Action>();
        //�f�U�C��
        sub_beat_action.Material_Setting(color: UI_color);
        sub_beat_action.Frame_Setting(win_num: enemy_win_num);

        Invoke("Start_Action", Beat_Manager.delay_second);

        Beat_Manager.Switch_Play_Beat(is_play_beat);
    }

    public virtual void Just_Beat_Action()
    {
        Sub_Beat_Action.Debug_Text(Beat_Manager.beat_num.ToString(),new Color(1,1,1));
    }

    public virtual void Progress_Beat_Action()
    {

    }

    void Start_Action()
    {
        //���y
        Music_Setting(_bgm:bgm, bpm: enemy_bpm);
        Beat_Manager.Play_BGM_Source();
    }


    /// <summary>
    /// �U���̓ǂݍ���
    /// </summary>

    public static List<List<string>> attackDatas;

    public virtual void Load_Attack()
    {
        string char_name = Beat_Action_Manager.Get_Char_Name();
        int difficutly_num = Beat_Action_Manager.difficulty_num;

        //�t�@�C���ǂݍ���
        attackDatas = Load_Resources.Load_CSV("Battle/" + char_name + "/" + difficutly_num.ToString());
        Debug.Log("Battle/" + char_name + "/" + difficutly_num.ToString());
        //�擪�͓��{��Ȃ̂ō폜
        for (int i = 0; i < attackDatas.Count; i++)
        {
            attackDatas[i].RemoveAt(0);
        }
    }


    /// <summary>
    /// �Ȃ̐ݒ�
    /// </summary>

    public AudioClip bgm;
    public double enemy_bpm;

    public void Music_Setting(AudioClip _bgm, double bpm)
    {
        Beat_Manager.Set_BGM(_bgm);
        Beat_Manager.Set_bpm(bpm);
    }


    /// <summary>
    /// �f�U�C��
    /// </summary>

    [SerializeField] Color UI_color;
    [SerializeField] int enemy_win_num;


    /// <summary>
    /// �I�u�W�F�N�g�̈ړ�
    /// </summary>

    public static void Move_A_to_B(GameObject target, Vector2 start_pos, Vector2 goal_pos, int need_beat_num, int start_beat_num)
    {
        float diff = (float)Beat_Manager.diff;
        int beat_num = (int)Beat_Manager.beat_num;
        float ratio = (beat_num - start_beat_num + 1 - diff) / need_beat_num;
        target.transform.localPosition = Vector2.Lerp(start_pos, goal_pos, ratio);
    }
}
