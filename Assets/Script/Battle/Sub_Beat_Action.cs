using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sub_Beat_Action : MonoBehaviour
{
    private void Start()
    {
        sound_source = GameObject.Find("Sound_Source").GetComponent<AudioSource>();

        debug_text = GameObject.Find("Debug_Text").GetComponent<Text>();

        //Judge_Text�֌W
        judge_text_list = GameObject.Find("JudgeText");
        perfect = judge_text_list.transform.GetChild(0).gameObject;
        good = judge_text_list.transform.GetChild(1).gameObject;
        miss = judge_text_list.transform.GetChild(2).gameObject;
    }

    /// <summary>
    /// ���y��炷
    /// </summary>
    static AudioSource sound_source;

    public static void Play_Sound(AudioClip sound_clip)
    {
        sound_source.PlayOneShot(sound_clip);
    }



    /// <summary>
    /// ����֘A
    /// </summary>
    static GameObject judge_text_list;
    static GameObject perfect;
    static GameObject good;
    static GameObject miss;

    public static void Show_Judge_Text(int type)
    {
        if (type == 1)
        {
            if(!perfect.activeSelf) perfect.SetActive(true);
            perfect.GetComponent<Animator>().Play("JudgeText");
        }
        else if (type == 2)
        {
            if (!good.activeSelf) good.SetActive(true);
            good.GetComponent<Animator>().Play("JudgeText");
        }
        else if (type == 3)
        {
            if (!miss.activeSelf) miss.SetActive(true);
            miss.GetComponent<Animator>().Play("JudgeText");
        }
    }

    public static void Show_Judge_Text(int type, Vector2 pos)
    {
        judge_text_list.transform.localPosition = pos;
        if (type == 1)
        {
            if (!perfect.activeSelf) perfect.SetActive(true);
            perfect.GetComponent<Animator>().Play("JudgeText");
        }
        else if (type == 2)
        {
            if (!good.activeSelf) good.SetActive(true);
            good.GetComponent<Animator>().Play("JudgeText");
        }
        else if (type == 3)
        {
            if (!miss.activeSelf) miss.SetActive(true);
            miss.GetComponent<Animator>().Play("JudgeText");
        }
    }

    /// <summary>
    /// �}�e���A���̐ݒ�
    /// </summary>

    [SerializeField] Material battle_UI_color;

    public void Material_Setting(Color color)
    {
        battle_UI_color.color = color;
    }


    /// <summary>
    /// �E�B���h�E�t���[���̐ݒ�
    /// </summary>

    [SerializeField] GameObject frame_1win;
    [SerializeField] GameObject frame_2win;


    public void Frame_Setting(int win_num)
    {
        if (win_num == 1) frame_1win.SetActive(true);
        else if (win_num == 2) frame_2win.SetActive(true);
    }


    /// <summary>
    /// �f�o�b�O�֘A
    /// </summary>
    static Text debug_text;
    public static void Debug_Text(string content, Color color)
    {
        debug_text.text = content;
        debug_text.color = color;
    }
}
