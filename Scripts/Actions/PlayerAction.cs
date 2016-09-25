using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerAction : MonoBehaviour
{
    protected const int Length = 4;

    public string Name;

    public Action Execute;

    public int ScoringValue;

    public Button ActionButton;

    public bool CanExecute 
    {
        get 
        {
            return ActionButton.interactable;
        }

        set 
        {
            ActionButton.interactable = value;
        }
    }

    public virtual void Select()
    {
        GameManager.Instance.ActionManager.SetAction(gameObject);
    }

    public virtual void Score()
    {
    }
}