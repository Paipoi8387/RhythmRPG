using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Beat_Manager : MonoBehaviour
{
    static AudioSource bgm_source;
    AudioSource sound_source;
    [SerializeField] AudioClip beat_clip;
    public static bool is_play_beat = true;

    static double bpm = 120; //後ほど、public staticにして他スクリプトから渡せるようにする
    public static double beat_num = 1;
    public static double diff;
    static double frediv;
    static bool is_just_beat = false;
    [SerializeField] bool set_UI = false;

    [SerializeField] float delay_play_second = 1;
    public static float delay_second;
    bool is_finish = false;

    void Start()
    {
        bgm_source = GameObject.Find("BGM_Source").GetComponent<AudioSource>();
        sound_source = GameObject.Find("Sound_Source").GetComponent<AudioSource>();

        delay_second = delay_play_second;
    }

    void OnGUI()
    {
        if (!set_UI) return;
        GUIStyle style = new GUIStyle();
        style.fontSize = 100;
        GUI.Label(new Rect(400, 300, 1000, 300), beat_num.ToString(), style);
        GUI.Label(new Rect(400, 600, 1000, 300), ((float)diff).ToString(), style);
    }

    void Update()
    {
        //Sub_Beat_Action.Debug_Text(GetSet_timSamples.ToString(),new Color(1,1,1));

        diff = beat_num - (bgm_source.timeSamples / frediv);

        if (diff < 0)
        {
            beat_num++;
            diff = beat_num - (bgm_source.timeSamples / frediv);
            is_just_beat = true;
            
            if (is_play_beat) sound_source.PlayOneShot(beat_clip);
        }
        else if (diff > 2 && !is_finish)
        {
            Finish_Operation();
        }
        else
        {
            is_just_beat = false;
        }
    }


    public static void Switch_Play_Beat(bool _is_play_beat)
    {
        is_play_beat = _is_play_beat;
    }

    public static void Play_BGM_Source()
    {
        beat_num = 1;
        diff = 0;
        frediv = 44100 / (bpm / 60);
        bgm_source.Play();
    }

    public static void Set_BGM(AudioClip bgm)
    {
        bgm_source.clip = bgm;
    }

    public static void Set_bpm(double enemy_bpm)
    {
        bpm = enemy_bpm;
    }

    public static string Get_clip_name()
    {
        return bgm_source.clip.name;
    }

    public static bool Get_is_just_beat()
    {
        return is_just_beat;
    }

    public static bool Get_is_audio_play()
    {
        return bgm_source.isPlaying;
    }

    public static int GetSet_timeSamples
    {
        get { return bgm_source.timeSamples; }
        set { bgm_source.timeSamples = value; }
    }


    public void Finish_Operation()
    {
        is_finish = true;
        battle_warp_anim.Play("BattleWarp_Fade");
        Invoke("Change_Result_Scene", 2);
    }

    [SerializeField] Animator battle_warp_anim;
    void Change_Result_Scene()
    {
        SceneManager.LoadScene("Result");
    }
}
