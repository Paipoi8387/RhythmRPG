using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tetuman_Beat_Action : Main_Beat_Action
{
    public override void Start()
    {
        base.Start();

        //Load_Attack();
    }

    public override void Just_Beat_Action()
    {
        base.Just_Beat_Action();
        Tetuman_Roulette();
    }

    public override void Progress_Beat_Action()
    {
        base.Progress_Beat_Action();
    }

    public override void Load_Attack()
    {
        base.Load_Attack();
    }

    [SerializeField] Image[] tetumanImages;
    int tetuman_num = 0;
    void Tetuman_Roulette()
    {
        tetuman_num = Random.Range(0, 4);
        for (int i = 0; i < 4; i++)
        {
            if (i == tetuman_num) tetumanImages[i].color = new Color(1, 1, 1);
            else tetumanImages[i].color = new Color(0.1f, 0.1f, 0.1f);
        }
    }
}
