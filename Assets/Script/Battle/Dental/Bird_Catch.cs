using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird_Catch : MonoBehaviour
{
    [SerializeField] AudioClip go_bird_clip;

    [SerializeField] GameObject catch_magichat;
    GameObject magichat;
    Animator magichat_anim;
    [SerializeField] GameObject bird_origin;

    
    void Start()
    {
        magichat = catch_magichat.transform.Find("MagicHat").gameObject;
        magichat_anim = catch_magichat.GetComponent<Animator>();
    }


    void Update()
    {
        if (Beat_Manager.Get_is_just_beat())
        {
            Instantiate_Bird();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            catch_magichat.transform.localScale = new Vector2(1, 1);
            magichat_anim.Play("Catch_MagicHat");
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            catch_magichat.transform.localScale = new Vector2(-1, -1);
            magichat_anim.Play("Catch_MagicHat");
        }
    }


    void Instantiate_Bird()
    {
        string beat_num = (Beat_Manager.beat_num).ToString();
        if (Dental_Beat_Action.attackDatas[0].Contains(beat_num))
        {
            GameObject bird_prefab = Instantiate(bird_origin, new Vector2(0, 0), Quaternion.identity);
            bird_prefab.transform.SetParent(this.transform);
            bird_prefab.transform.localScale = bird_origin.transform.localScale;
            bird_prefab.SetActive(true);
            Sub_Beat_Action.Play_Sound(go_bird_clip);
        }
    }
}
