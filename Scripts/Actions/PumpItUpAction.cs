using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PumpItUpAction : PlayerAction
{
    public PumpItUpAction()
    {
        ScoringValue = 2;
    }

    public override void Score()
    {
        GameManager.Instance.CurrentPlayer.SendSound("Play_Claps");
    }
}
