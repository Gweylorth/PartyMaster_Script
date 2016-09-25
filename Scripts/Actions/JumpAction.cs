using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityStandardAssets._2D;

public class JumpAction : PlayerAction
{
    public JumpAction()
    {
        ScoringValue = 5;
    }

    public override void Score()
    {
        GameManager.Instance.CurrentPlayer.SendSound("Play_Jump");
    }
}