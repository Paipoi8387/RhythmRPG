using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gootara_Beat_Action : Main_Beat_Action
{
    [SerializeField] GameObject hearts;
    public override void Start()
    {
        base.Start();
        Load_Attack();

        if (Beat_Action_Manager.difficulty_num == 0) hearts.SetActive(false);

        heart_array = new GameObject[] { heart_l, heart_c, heart_r };
    }

    public override void Just_Beat_Action()
    {
        base.Just_Beat_Action();
        Rotate_Ground();

        Instantiate_Obj();

        Character_HandsUp();
        Character_HandsDown();
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
    /// 地面の回転
    /// </summary>
    [SerializeField] GameObject ground;
    [SerializeField] float rotate_speed = 10;
    void Rotate_Ground()
    {

        ground.transform.Rotate(0, 0, rotate_speed);
    }

    /// <summary>
    /// オブジェクト生成
    /// </summary>
    [SerializeField] GameObject coin_origin;
    [SerializeField] GameObject spike_origin;
    [SerializeField] GameObject prefab_parent;
    [SerializeField] GameObject origin_parent;
    int obj_data_num = 0;
    void Instantiate_Obj()
    {
        //ここでCSVの読み込み
        
        int beat_num = (int)Beat_Manager.beat_num;
        if (attackDatas[0][obj_data_num] != beat_num.ToString()) return;

        GameObject obj_origin;
        if (attackDatas[1][obj_data_num] == "0") obj_origin = spike_origin;
        else obj_origin = coin_origin;

        //GameObject obj_origin = coin_origin;

        GameObject obj_prefab = Instantiate(obj_origin, obj_origin.transform.localPosition, Quaternion.identity);
        obj_prefab.transform.SetParent(origin_parent.transform);
        obj_prefab.transform.localScale = new Vector2(1, 1);
        obj_prefab.transform.localPosition = obj_origin.transform.localPosition;
        obj_prefab.transform.localRotation = obj_origin.transform.localRotation;
        obj_prefab.transform.SetParent(prefab_parent.transform);
        obj_prefab.SetActive(true);

        if (attackDatas[0].Count - 1 > obj_data_num)
        {
            obj_data_num++;
        }

        handsup_queue.Enqueue(beat_num + add_timing);
        handsdown_queue.Enqueue(beat_num + add_timing + 1);
    }


    ///各キャラのanimation
    [SerializeField] int add_timing = 10;
    Queue<int> handsup_queue = new Queue<int>();
    [SerializeField] GameObject hukidasi;
    [SerializeField] Animator gooteru_anim;
    [SerializeField] Animator liza_anim;
    [SerializeField] Animator deen_anim;
    void Character_HandsUp()
    {
        if (handsup_queue.Count == 0) return;

        if (handsup_queue.Peek() == (int)Beat_Manager.beat_num)
        {
            gooteru_anim.Play("Gooteru_HandsUp");
            liza_anim.Play("Liza_HandsUp");
            deen_anim.Play("Deen_HandsUp");
            handsup_queue.Dequeue();
            hukidasi.SetActive(true);
        }
    }

    Queue<int> handsdown_queue = new Queue<int>();
    void Character_HandsDown()
    {
        if (handsdown_queue.Count == 0) return;

        if (handsdown_queue.Peek() == (int)Beat_Manager.beat_num)
        {
            gooteru_anim.Play("Gooteru_Stand");
            liza_anim.Play("Liza_Stand");
            deen_anim.Play("Deen_Stand");
            handsdown_queue.Dequeue();
            hukidasi.SetActive(false);
        }
    }

    ///ダメージ
    /// <summary>
    /// ライフ管理
    /// </summary>

    int heart_num = 3;
    [SerializeField] GameObject heart_l;
    [SerializeField] GameObject heart_c;
    [SerializeField] GameObject heart_r;
    GameObject[] heart_array;
    [SerializeField] Beat_Manager beat_manager;

    [SerializeField] AudioClip damage_sound;

    public void Damage_Heart()
    {
        if (Beat_Action_Manager.difficulty_num == 0) return;

        heart_num--;
        if (heart_num >= 0) heart_array[heart_num].GetComponent<Animator>().Play("Heart_Flash");

        Sub_Beat_Action.Play_Sound(damage_sound);

        if (heart_num == 0)
        {
            beat_manager.Finish_Operation();
        }
    }
}
