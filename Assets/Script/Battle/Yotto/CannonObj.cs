using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonObj : MonoBehaviour
{
    [SerializeField] int pos_num;
    [SerializeField] Yotto_Beat_Action yotto_beat_action;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (yotto_beat_action.objType[pos_num] < 1)
        {
            Debug.Log("Fish");
            if(yotto_beat_action.catch_time[pos_num] < 0.1f)
            {
                Sub_Beat_Action.Show_Judge_Text(type: 1, new Vector2(0f, 300f));
            }
            else
            {
                Sub_Beat_Action.Show_Judge_Text(type: 2, new Vector2(0f, 300f));
            }
            yotto_beat_action.Catch_Yotto();
        }
        else
        {
            Debug.Log("Cannon");
            yotto_beat_action.Damage_Yotto();
        }

        Debug.Log(yotto_beat_action.catch_time[pos_num]);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        Invoke("Move_Obj", 0.1f);
    }

    void Move_Obj()
    {
        transform.localPosition = new Vector2(transform.localPosition.x, 1000f);
    }
}
