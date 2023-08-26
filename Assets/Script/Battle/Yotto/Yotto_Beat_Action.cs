using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Yotto_Beat_Action : Main_Beat_Action
{
    public override void Start()
    {
        base.Start();

        //Load_Attack();
    }

    public override void Just_Beat_Action()
    {
        base.Just_Beat_Action();

        if(Beat_Manager.beat_num % 4 == 0) Set_Cat(Random.Range(0,2));
        Switch_CatImage();
    }

    public override void Progress_Beat_Action()
    {
        base.Progress_Beat_Action();
        Move_Yotto();
    }

    public override void Load_Attack()
    {
        base.Load_Attack();
    }

    private void Update()
    {
        catch_time[0] += Time.deltaTime;
        catch_time[1] += Time.deltaTime;
    }


    [SerializeField] Animator yottoAnim;
    [SerializeField] Transform yottoTrans;
    //ヨットを動かす
    void Move_Yotto()
    {
        if (yottoTrans.localPosition.x != 0) return;

        if (Input.GetKey(KeyCode.RightArrow)) yottoAnim.Play("Yotto_MoveRight");
        else if (Input.GetKey(KeyCode.LeftArrow)) yottoAnim.Play("Yotto_MoveLeft");
    }

    [SerializeField] GameObject[] mobcats;
    int mobcat_num = 0;
    [SerializeField] AudioClip catch_sound;
    public void Catch_Yotto()
    {
        Sub_Beat_Action.Play_Sound(catch_sound);
        Score_Manager.Add_Score(1);

        mobcats[mobcat_num].SetActive(true);
        if(mobcat_num < 2)mobcat_num++;
    }

    [SerializeField] Animator[] heartAnims;
    int heart_num = 2;
    [SerializeField] AudioClip damage_sound;
    [SerializeField] Beat_Manager beat_manager;
    public void Damage_Yotto()
    {
        yottoAnim.Play("Yotto_Damage",1);
        Sub_Beat_Action.Play_Sound(damage_sound);

        if (heart_num == 0)
        {
            beat_manager.Finish_Operation();
        }

        heartAnims[heart_num].Play("Heart_Flash");
        heart_num--;
    }

    //猫とか大砲とか
    [SerializeField] Image[] catImages;
    [SerializeField] Sprite[] kojiroSprites;
    [SerializeField] Sprite[] koumeSprites;
    //これを配列にして、Max値をセットするとデクリメントする
    int[] action_num = new int[2] { -1, -1 };
    void Switch_CatImage()
    {
        action_num[0]--;
        action_num[1]--;

        if (action_num[0] >= 0) catImages[0].sprite = kojiroSprites[action_num[0]];
        if (action_num[1] >= 0) catImages[1].sprite = koumeSprites[action_num[1]];
        
        Switch_ObjPosition(0);
        Switch_ObjPosition(1);

        Cannon_Shoot(0);
        Cannon_Shoot(1);
    }

    const int max_action_num = 3; 
    void Set_Cat(int pos_num)
    {
        action_num[pos_num] = max_action_num;
        //objの種類決定
        objType[pos_num] = Random.Range(0, 2);

        //spriteの変換
        if (objType[pos_num] == 0)
        {
            obj[pos_num].GetComponent<Image>().sprite = objSprite[0];
            obj[pos_num].GetComponent<Animator>().enabled = true;
        }
        else if (objType[pos_num] == 1)
        {
            obj[pos_num].GetComponent<Image>().sprite = objSprite[1];
            obj[pos_num].GetComponent<Animator>().enabled = false;
        }
    }

    [SerializeField] GameObject[] obj;
    public int[] objType = new int[2] { 0, 0 };
    [SerializeField] Sprite[] objSprite;
    public float[] catch_time = new float[2] { 0, 0 };
    [SerializeField] AudioClip display_sound;
    void Switch_ObjPosition(int pos_num)
    {
        if (action_num[pos_num] == 2)
        {
            obj[pos_num].transform.localPosition = new Vector2(obj[pos_num].transform.localPosition.x, 400f);
            obj[pos_num].GetComponent<BoxCollider2D>().enabled = true;
        }
        else if (action_num[pos_num] == 1) obj[pos_num].transform.localPosition = new Vector2(obj[pos_num].transform.transform.localPosition.x, 275f);
        else if (action_num[pos_num] == -1)
        {
            obj[pos_num].transform.localPosition = new Vector2(obj[pos_num].transform.transform.localPosition.x, -300f);
            Sub_Beat_Action.Play_Sound(display_sound);
            catch_time[pos_num] = 0;
        }
        else obj[pos_num].transform.localPosition = new Vector2(obj[pos_num].transform.localPosition.x, 1000f);
    }

    [SerializeField] Animator[] cannon_anim;
    [SerializeField] AudioClip cannon_sound;
    void Cannon_Shoot(int pos_num)
    {
        if (action_num[pos_num] != 0) return;

        cannon_anim[pos_num].Play("Cannon");
        Sub_Beat_Action.Play_Sound(cannon_sound);
    }
}
