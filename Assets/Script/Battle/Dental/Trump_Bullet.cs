using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trump_Bullet : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject bullet_prefab;
    [SerializeField] GameObject parent;

    [SerializeField] Sprite enemy_close;
    [SerializeField] Sprite enemy_open;
    [SerializeField] AudioClip shot_clip;

    private void Update()
    {
        if (Beat_Manager.Get_is_just_beat())
        {
            Shoot_Trump();
        }

        Show_Hands();
    }

    public void Shoot_Trump()
    {
        Vector2 goal_pos;
        if (Random.Range(0, 2) == 0) goal_pos = new Vector2(-300, 300);
        else goal_pos = new Vector2(300, 300);
        enemy.transform.localPosition = goal_pos;

        //íeä€ê∂ê¨
        string beat_num = (Beat_Manager.beat_num).ToString();
        if (Dental_Beat_Action.attackDatas[0].Contains(beat_num))
        {
            enemy.GetComponent<Image>().sprite = enemy_open;
            GameObject bullet = Instantiate(bullet_prefab, goal_pos, Quaternion.identity);
            bullet.transform.SetParent(parent.transform);
            bullet.transform.localScale = new Vector2(1, 1);
            bullet.transform.localPosition = new Vector2(goal_pos.x, goal_pos.y - 100);
            Sub_Beat_Action.Play_Sound(shot_clip);
        }
        else
        {
            enemy.GetComponent<Image>().sprite = enemy_close;
        }
    }


    [SerializeField] GameObject hands;
    void Show_Hands()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (!hands.activeSelf) hands.SetActive(true);
            if (hands.transform.localPosition.x != -300) hands.transform.localPosition = new Vector2(-300, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (!hands.activeSelf) hands.SetActive(true);
            if (hands.transform.localPosition.x != 300) hands.transform.localPosition = new Vector2(300, 0);
        }
        else
        {
            if (hands.activeSelf) hands.SetActive(false);
        }
    }
}
