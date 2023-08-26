using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Manager : MonoBehaviour
{

    GameObject red_block;
    GameObject green_block;
    GameObject blue_block;
    GameObject nomal_block;

    void Start()
    {
        red_block = transform.GetChild(0).gameObject;
        green_block = transform.GetChild(1).gameObject;
        blue_block = transform.GetChild(2).gameObject;
        nomal_block = transform.GetChild(3).gameObject;

        Switch_Block();
    }

    [SerializeField] float speed = 0.5f;
    [SerializeField] int direction = 1;
    void FixedUpdate()
    {
        transform.Translate(speed * direction, 0, 0);
        if(direction == 1 && transform.localPosition.x > 1000)
        {
            transform.localPosition = new Vector2(-1000, 0);
            Switch_Block();
        }
        else if (direction == -1 && transform.localPosition.x < -1000)
        {
            transform.localPosition = new Vector2(1000, 0);
            Switch_Block();
        }
    }

    void Switch_Block()
    {
        red_block.SetActive(false);
        green_block.SetActive(false);
        blue_block.SetActive(false);
        nomal_block.SetActive(false);

        int ran_num = Random.Range(-3, 5);
        if (ran_num == 0) red_block.SetActive(true);
        else if (ran_num == 1) green_block.SetActive(true);
        else if (ran_num == 2) blue_block.SetActive(true);
        else if (ran_num >= 3) nomal_block.SetActive(true);
    }
}
