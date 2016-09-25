using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{
    public const int SelectionBarTime = 4;
    public const int ExecutionBarTime = 4;

    [SyncVar]
    private int partyGuests = 50;

    [SyncVar]
    private int timeLeft;

    private int currentAction = 0;

    private bool _scoreActive = false;

    private int holdCount = 0;

    public GameState State;

    public PlayerController CurrentPlayer { get; set; }

    public int PartyGuests
    {
        get
        {
            return partyGuests;
        }

        set
        {
            partyGuests = value;
        }
    }

    public int TimeLeft
    {
        get
        {
            return timeLeft;
        }

        set
        {
            timeLeft = value;
        }
    }

    private int SelectionTimeLeft { get; set; }

    private int ExecutionTimeLeft { get; set; }

    private ActionManager _actionManager;

    private UIManager _uiManager;

    public static GameManager Instance;

    public ActionManager ActionManager
    {
        get
        {
            if (_actionManager == null)
            {
                _actionManager = this.gameObject.GetComponentInChildren<ActionManager>();
            }

            return _actionManager;
        }
    }

    public UIManager UIManager
    {
        get
        {
            if (_uiManager == null)
            {
                _uiManager = this.gameObject.GetComponentInChildren<UIManager>();
            }

            return _uiManager;
        }
    }

    public MusicCallback MusicCallback { get; private set; }

    void Awake()
    {
        Debug.Log("GameManager");
        Instance = this;
        MusicCallback = gameObject.GetComponent<MusicCallback>();
        MusicCallback.MusicSyncBarEvent += SyncBar;
        MusicCallback.MusicSyncBeatEvent += SyncBeat;
        MusicCallback.StartScoringEvent += StartScoring;
        SelectionTimeLeft = SelectionBarTime;
        ExecutionTimeLeft = ExecutionBarTime;
        TimeLeft = 100;
        PartyGuests = 55;
    }

    private void StartScoring(object sender, EventArgs eventArgs)
    {
        _scoreActive = true;
        Debug.Log("StartScoring");
    }

    private void SyncBar(object sender, EventArgs eventArgs)
    {
        if (State == GameState.Selection)
        {
            SelectionTimeLeft--;
            if (SelectionTimeLeft == -1)
            {
                State = GameState.Listen;
                SelectionTimeLeft = SelectionBarTime;
            }
        }
    }

    private void SyncBeat(object sender, EventArgs e)
    {
        UIManager.SwapColors();

        if (State == GameState.Listen && ExecutionTimeLeft == ExecutionBarTime)
        {
            var action = ActionManager.PickedActions[currentAction];

            if (action == null || action.GetType().IsSubclassOf(typeof(MoveAction)))
            {
                State = GameState.Hold;
                holdCount++;
                if (holdCount == 2)
                {
                    holdCount = 0;
                    FinishedAction();
                }
            }
            else 
            {
                ScoreManager.Instance.Execute(action);
            }
        }

        if (State == GameState.Listen || State == GameState.Repro || State == GameState.Hold)
        {
            ExecutionTimeLeft--;
            if (ExecutionTimeLeft == -1)
            {
                State = (State == GameState.Listen && State != GameState.Hold) ? GameState.Repro : GameState.Listen;
                ExecutionTimeLeft = ExecutionBarTime;
            }
        }

        switch (State)
        {
            case GameState.Selection:
                UIManager.SelectionTimeLeftText.text = SelectionTimeLeft.ToString();
                break;

            case GameState.Listen:
            case GameState.Repro:
            case GameState.Hold:
                UIManager.SelectionTimeLeftText.text = State.ToString();
                break;
        }
    }

    public void StartCountDown(int initialValue)
    {
        TimeLeft = initialValue;
        StartCoroutine("CountDown");
    }

    private IEnumerator CountDown()
    {
        yield return new WaitForSeconds(1F);
        TimeLeft--;
        if (TimeLeft == 0)
        {
            EndGame();
        }

        StartCoroutine("CountDown");
    }

    private void EndGame()
    {
        UIManager.GameOver();
        AkSoundEngine.StopAll();
        State = GameState.Lobby;
        StopAllCoroutines();
    }

    public void FixedUpdate()
    {
        if (TimeLeft <= 0 || PartyGuests == 100 || PartyGuests == 0)
        {
            EndGame();
        }

        UIManager.PartyDudesText.text = string.Format("{0}/100", partyGuests);
        var minutes = Mathf.Floor(timeLeft / 60).ToString("00");
        var seconds = (timeLeft % 60).ToString("00");
        UIManager.TimeLeftText.text = string.Format("{0}:{1}", minutes, seconds);
    }

    public void FinishedAction()
    {
        UIManager.Yeah = false;
        UIManager.Oops = false;

        if (currentAction == 1)
        {
            State = GameState.Selection;
            ActionManager.ClearActions();
            currentAction = 0;
            ExecutionTimeLeft = ExecutionBarTime;
            return;
        }

        currentAction++;
    }
}

public enum GameState
{
    Lobby,
    Selection,
    Listen,
    Repro,
    Hold
}
