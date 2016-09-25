using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyRoom : MonoBehaviour 
{
    public List<PlayerAction> availableActions;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag != "Player" || !collider.GetComponent<PlayerController>().isLocalPlayer)
        {
            return;
        }

        availableActions.ForEach(action => action.CanExecute = true);
        GameManager.Instance.ActionManager.ActionsMenu.SetActive(true);
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag != "Player" || !collider.GetComponent<PlayerController>().isLocalPlayer)
        {
            return;
        }

        availableActions.ForEach(action => action.CanExecute = false);
        GameManager.Instance.ActionManager.ActionsMenu.SetActive(false);
    }
}