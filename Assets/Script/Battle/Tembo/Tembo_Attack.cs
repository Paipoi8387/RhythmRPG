using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tembo_Attack : MonoBehaviour
{
    [SerializeField] Animator screen_anim;
    [SerializeField] AudioClip impact_clip;
    void Screen_Shake()
    {
        screen_anim.Play("Damage_Screen");
        Sub_Beat_Action.Play_Sound(impact_clip);
    }
}
