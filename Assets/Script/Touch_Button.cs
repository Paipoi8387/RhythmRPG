using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Touch_Button : MonoBehaviour
{
    GameObject up;
    GameObject down;
    GameObject left;
    GameObject right;
    GameObject point;

    GameObject circle;

    // Start is called before the first frame update
    void Start()
    {
        if(this.name == "Cross_Button")
        {
            up = gameObject.transform.Find("Up").gameObject;
            down = gameObject.transform.Find("Down").gameObject;
            left = gameObject.transform.Find("Left").gameObject;
            right = gameObject.transform.Find("Right").gameObject;
            point = gameObject.transform.Find("Point").gameObject;
        }
        else if(this.name == "A_Button") //Aボタン以外も使えそう
        {
            circle = gameObject.transform.Find("Circle").gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.name == "Cross_Button") Cross_Button_Detection();
        else if (this.name == "A_Button") Circle_Button_Detection();
    }

    void Cross_Button_Detection()
    {
        Not_Push_Cross_Button();
        if (Input.GetKey(KeyCode.UpArrow))
        {
            up.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            point.transform.localPosition = new Vector2(0, 35);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            down.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            point.transform.localPosition = new Vector2(0, -35);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            left.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            point.transform.localPosition = new Vector2(-35, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            right.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            point.transform.localPosition = new Vector2(35, 0);
        }
    }

    void Not_Push_Cross_Button()
    {
        up.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1);
        down.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1);
        left.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1);
        right.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1);
        point.transform.localPosition = new Vector2(0, 0);
    }


    void Circle_Button_Detection()
    {
        if (Input.GetKey(KeyCode.A))
        {
            circle.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
        else
        {
            circle.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1);
        }
    }
}
