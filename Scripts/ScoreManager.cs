using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{

    private static ScoreManager _instance;
    private PlayerAction currentAction;

    bool _isScoring = false;

    bool _isPattern = false;
    int _patternCountDown = 3;
    int _patternScore = 0;

    bool _pulseMIDI = false;
    bool _pulsePlayer = false;

    float _pulseMIDITime = 0f;
    float _pulsePlayerTime = 0f;

    int _nbNotes = 0;
    int _scoreIndex = 0;
    bool[] _scores;


    public static ScoreManager Instance;

    public MusicCallback MusicCallback { get; private set; }

    public void Execute(PlayerAction action)
    {
        currentAction = action;
        GameManager.Instance.MusicCallback.MIDISyncBeatEvent += BeatEvent;
        GameManager.Instance.MusicCallback.MusicSyncMIDIEvent += ScorePattern;
        GameManager.Instance.MusicCallback.StartPatternEvent += StartPattern;
        GameManager.Instance.MusicCallback.EndPatternEvent += EndPattern;
        GameManager.Instance.MusicCallback.StartScoringEvent += StartScoring;
        GameManager.Instance.MusicCallback.EndScoringEvent += EndScoring;

        GameManager.Instance.MusicCallback.PlayPattern();
    }

    public void Finish()
    {
        GameManager.Instance.MusicCallback.MIDISyncBeatEvent -= BeatEvent;
        GameManager.Instance.MusicCallback.MusicSyncMIDIEvent -= ScorePattern;
        GameManager.Instance.MusicCallback.StartPatternEvent -= StartPattern;
        GameManager.Instance.MusicCallback.EndPatternEvent -= EndPattern;
        GameManager.Instance.MusicCallback.StartScoringEvent -= StartScoring;
        GameManager.Instance.MusicCallback.EndScoringEvent -= EndScoring;

        _patternCountDown = 3;
        _patternScore = 0;

        GameManager.Instance.FinishedAction();
    }

    void Start()
    {
        Instance = this;
    }

    void Update()
    {
        if(_isScoring)
        {
            bool tapRegistered = false;
            if (Input.touchCount >= 1)
            {
                var touchInput = Input.GetTouch(0);
                tapRegistered = touchInput.phase == TouchPhase.Ended && touchInput.tapCount == 1;
            }

            if ((Input.GetKey(KeyCode.P) 
                || tapRegistered) 
                && !_pulsePlayer)
            {
                currentAction.Score();
                _pulsePlayer = true;
                _pulsePlayerTime = Time.time;
            }
        }
    }

    private void StartPattern(object sender, EventArgs eventArgs)
    {
        _isPattern = true;
    }

    private void EndPattern(object sender, EventArgs eventArgs)
    {
        _isPattern = false;
    }

    private void BeatEvent(object sender, EventArgs eventArgs)
    {
        
        if (_isPattern)
        {
            Debug.Log("beat");
            --_patternCountDown;
            Debug.Log("pattern CountDown : " + _patternCountDown);
            if (_patternCountDown == 0)
            {
                _isScoring = true;
            }         
        }
    }

    private void StartScoring(object sender, EventArgs eventArgs)
    {
        //_isScoring = true;
        
    }

    private void EndScoring(object sender, EventArgs eventArgs)
    {
        _isScoring = false;
        _nbNotes = 0;

        GameManager.Instance.PartyGuests += _patternScore;
        Finish();

    }

    private void ScorePattern(object sender, EventArgs eventArgs)
    {
        if (!_isScoring)
        {
            _nbNotes++;
            return;
        }
        _pulseMIDI = true;
        _pulseMIDITime = Time.time;
        StartCoroutine("ScoreCountDown");
        
    }

    private IEnumerator ScoreCountDown()
    {
        yield return new WaitForSeconds(0.2F);
        GameManager.Instance.UIManager.Yeah = false;
        GameManager.Instance.UIManager.Oops = false;

        if (Mathf.Abs(_pulseMIDITime - _pulsePlayerTime) > 0.2f)
        {
            GameManager.Instance.UIManager.Oops = true;
            Debug.Log("Noob");
        }
        else
        {
            GameManager.Instance.UIManager.Yeah = true;
            Debug.Log("Yeah");
            _patternScore += currentAction.ScoringValue;
        }

        _pulseMIDI = false;
        _pulsePlayer = false;
        _scoreIndex++;
    }
}
