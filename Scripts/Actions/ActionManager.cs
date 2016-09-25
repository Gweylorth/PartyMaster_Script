using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class ActionManager : MonoBehaviour 
{
    public GameObject Action1Display;
    public GameObject Action2Display;
    public GameObject ActionsMenu;

    public PlayerAction[] PickedActions { get; private set; }

    public void Start() 
    {
        PickedActions = new PlayerAction[2];
    }

    public void SetAction(GameObject action)
    {
        if (PickedActions[0] == null)
        {
            SetAction(action, 0);
            return;
        }

        SetAction(action, 1);
    }

    private void SetAction(GameObject actionGo, int queueIndex)
    {
        var action = actionGo.GetComponent<PlayerAction>();
        PickedActions[queueIndex] = action;
        var currentDisplay = queueIndex == 0 ? Action1Display : Action2Display;
        currentDisplay.GetComponent<Text>().text = action.Name;
        currentDisplay.SetActive(true);
    }

    public void ClearActions()
    {
        PickedActions = new PlayerAction[2];
        Action1Display.SetActive(false);
        Action2Display.SetActive(false);
    }

    public void FixedUpdate()
    {
        if (GameManager.Instance.State == GameState.Lobby 
            || GameManager.Instance.State == GameState.Selection)
        {
            ActionsMenu.SetActive(true);
        }
        else
        {
            ActionsMenu.SetActive(false);
        }
    }
}
