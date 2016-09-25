using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MusicCallback : MonoBehaviour
{
    AkEventCallbackMsg cookie;
    int m_musicPosition = 0;
    float m_musicDuration = 0;
    float m_musicTimeLeft = 0;
    bool noteOn = false;
    bool _isScoringActive = false;
    bool _isScoring = false;
    uint _scoringPatternID;

    public PartyRoom room;

    private string[] m_patterns;

    void Start()
    {
        AkSoundEngine.SetState("DebugMute", "Off");
        AkSoundEngine.SetState("Scoring", "Off");

        //AkSoundEngine.PostEvent("Play_Music", gameObject, (uint)AkCallbackType.AK_EnableGetSourcePlayPosition, MyCallbackFunction, this);
        m_patterns = new string[]
        {
            "Play_Pattern_01", "Play_Pattern_02", "Play_Pattern_03", "Play_Pattern_04", "Play_Pattern_05"
        };
    }

    public void PlayPattern()
    {
        AkSoundEngine.PostEvent(m_patterns[UnityEngine.Random.Range(0,m_patterns.Length)], gameObject, 0x10101, MIDICallback, this);
    }

    public void StartMusic()
    {
        AkSoundEngine.PostEvent("Play_Music", room.gameObject, 0x7f89, MyCallbackFunction, this);
        AkSoundEngine.PostEvent("Play_AMB", room.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void MyCallbackFunction(object in_cookie, AkCallbackType in_type, object in_callbackInfo)
    {

        switch (in_type)
        {
            case AkCallbackType.AK_Duration:
                AkCallbackManager.AkDurationCallbackInfo duration = (AkCallbackManager.AkDurationCallbackInfo)in_callbackInfo;
                m_musicDuration = duration.fEstimatedDuration / 1000;
                m_musicTimeLeft = m_musicDuration;
                GameManager.Instance.StartCountDown((int)Mathf.Floor(m_musicTimeLeft));
                break;
            case AkCallbackType.AK_MusicSyncBeat:
                AkCallbackManager.AkMusicSyncCallbackInfo musicInfo = in_callbackInfo as AkCallbackManager.AkMusicSyncCallbackInfo;
                OnMusicSyncBeat();
                //Debug.Log(60 / musicInfo.fBeatDuration);
                //AkSoundEngine.PostEvent("Play_Debug", gameObject);
                break;
            case AkCallbackType.AK_MusicSyncBar:
                OnMusicSyncBar();
                break;
            case AkCallbackType.AK_MusicPlayStarted:
                Debug.Log("MusicPlayStarted");
                break;
            case AkCallbackType.AK_EndOfEvent:
                Debug.Log("EndofEvent");
                break;
            default:
                break;
        }
    }
    void MIDICallback(object in_cookie, AkCallbackType in_type, object in_callbackInfo)
    {
        switch (in_type)
        {
            case AkCallbackType.AK_MusicSyncBeat:
                AkCallbackManager.AkMusicSyncCallbackInfo musicInfo = in_callbackInfo as AkCallbackManager.AkMusicSyncCallbackInfo;
                OnMIDISyncBeat();
                break;
            case AkCallbackType.AK_EndOfEvent:
                if (_isScoringActive && _isScoring == false)
                {
                    _isScoringActive = false;
                    _isScoring = true;
                    OnEndPattern();
                    
                    AkSoundEngine.SetState("DebugMute", "On");
                    AkSoundEngine.PostEvent(_scoringPatternID, gameObject, (uint)0x10101, MIDICallback, this);
                    break;
                }
                if (_isScoringActive == false && _isScoring == true)
                {
                    _isScoring = false;
                    AkSoundEngine.SetState("DebugMute", "Off");
                    OnEndScoring();
                    Debug.Log("Fin du scoring");
                }
                Debug.Log("EndofEvent");
                break;
            case AkCallbackType.AK_MIDIEvent:
                AkCallbackManager.AkMidiEventCallbackInfo midiInfo = (AkCallbackManager.AkMidiEventCallbackInfo)in_callbackInfo;
                if (_isScoringActive == false && _isScoring == false)
                {
                    _isScoringActive = true;
                    _scoringPatternID = midiInfo.eventID;
                    OnStartPattern();
                }
                else if(_isScoringActive == false && _isScoring == true)
                {
                    OnStartScoring();
                }
                
                noteOn = !noteOn;
                if (noteOn)
                {
                    OnMusicSyncMIDI();
                }
                break;
            default:
                break;
        }
    }
    void SetStateScoring(string state)
    {
        if(GameManager.Instance.CurrentPlayer.Type == CharacterType.Party)
        {
            AkSoundEngine.SetState("Scoring", state);
        }
    }
    /*void MarkerCallback(object in_callbackInfo)
    {
        AkEventCallbackMsg callbackInfo = (AkEventCallbackMsg)in_callbackInfo;

        AkCallbackManager.AkMarkerCallbackInfo MarkerCallbackInfo = (AkCallbackManager.AkMarkerCallbackInfo)callbackInfo.info;

        m_RawMarkerText.text = MarkerCallbackInfo.strLabel;

        m_SubtitleText.text = ms_EnglishSubtitles[MarkerCallbackInfo.uIdentifier];
    }*/



    public delegate void MusicSyncBarEventHandler(object sender, EventArgs e);

    public event MusicSyncBarEventHandler MusicSyncBarEvent;

    private void OnMusicSyncBar()
    {
        if (MusicSyncBarEvent != null)
        {
            MusicSyncBarEvent.Invoke(this, EventArgs.Empty);
        }
    }

    public delegate void MusicSyncBeatEventHandler(object sender, EventArgs e);

    public event MusicSyncBeatEventHandler MusicSyncBeatEvent;

    private void OnMusicSyncBeat()
    {
        if (MusicSyncBeatEvent != null)
        {
            MusicSyncBeatEvent.Invoke(this, EventArgs.Empty);
        }
    }

    public delegate void MIDISyncBeatEventHandler(object sender, EventArgs e);

    public event MIDISyncBeatEventHandler MIDISyncBeatEvent;

    private void OnMIDISyncBeat()
    {
        if (MIDISyncBeatEvent != null)
        {
            MIDISyncBeatEvent.Invoke(this, EventArgs.Empty);
        }
    }

    public delegate void MusicSyncMIDIEventHandler(object sender, EventArgs e);

    public event MusicSyncMIDIEventHandler MusicSyncMIDIEvent;

    private void OnMusicSyncMIDI()
    {
        if (MusicSyncMIDIEvent != null && (_isScoringActive || _isScoring))
        {
            MusicSyncMIDIEvent.Invoke(this, EventArgs.Empty);
        }
    }

    public delegate void StartPatternEventHandler(object sender, EventArgs e);

    public event StartPatternEventHandler StartPatternEvent;

    private void OnStartPattern()
    {
        if (StartPatternEvent != null && (_isScoringActive || _isScoring))
        {
            StartPatternEvent.Invoke(this, EventArgs.Empty);
            SetStateScoring("On");
        }
    }

    public delegate void EndPatternEventHandler(object sender, EventArgs e);

    public event EndPatternEventHandler EndPatternEvent;

    private void OnEndPattern()
    {
        if (EndPatternEvent != null)
        {
            EndPatternEvent.Invoke(this, EventArgs.Empty);
        }
    }

    public delegate void StartScoringEventHandler(object sender, EventArgs e);

    public event StartScoringEventHandler StartScoringEvent;
    private void OnStartScoring()
    {
        if (StartScoringEvent != null && (_isScoringActive || _isScoring))
        {
            StartScoringEvent.Invoke(this, EventArgs.Empty);
            
        }
    }

    public delegate void EndScoringEventHandler(object sender, EventArgs e);

    public event EndScoringEventHandler EndScoringEvent;

    private void OnEndScoring()
    {
        if (EndScoringEvent != null)
        {
            EndScoringEvent.Invoke(this, EventArgs.Empty);
            SetStateScoring("Off");
        }
    }
}