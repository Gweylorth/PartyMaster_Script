using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class YellAction : PlayerAction
{
    public YellAction()
    {
        ScoringValue = -5;
    }

    public override void Score()
    {
        GameManager.Instance.CurrentPlayer.SendSound("Play_Yell");
    }
}
