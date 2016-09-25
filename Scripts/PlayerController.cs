using System;
using UnityEngine;
using UnityStandardAssets._2D;
using UnityEngine.Networking;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class PlayerController : NetworkBehaviour
{
    private PlatformerCharacter2D m_Character;
    private Camera2DFollow camera;
    private CharacterType characterType;

    public int MoveDirection { get; set; }

    public CharacterType Type
    {
        get
        {
            return characterType;
        }

        set
        {
            characterType = value;
            AkSoundEngine.SetState("Player_Type", characterType.ToString());
        }
    }

    public override void OnStartLocalPlayer()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera2DFollow>();
        camera.SetTarget(transform);

        var existingPlayers = GameObject.FindGameObjectsWithTag("Player");
        Type = (CharacterType)Enum.GetValues(typeof(CharacterType)).GetValue(existingPlayers.Length - 1);

        GameManager.Instance.CurrentPlayer = this;
    }

    public void Start()
    {
        m_Character = GetComponent<PlatformerCharacter2D>();
        MoveDirection = 0;

        GameManager.Instance.MusicCallback.MusicSyncBeatEvent += Footstep;
    }

    private void Footstep(object sender, EventArgs eventArgs)
    {
        if (MoveDirection == 0)
        {
            return;
        }

        AkSoundEngine.PostEvent("Play_Footsteps", gameObject);
    }

    public void FixedUpdate()
    {
        if (isLocalPlayer && MoveDirection != 0)
        {
            m_Character.Move(MoveDirection, false, false);
        }
    }

    public void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.GetComponent<PartyRoom>() != null)
        {
            MoveDirection = 0;
            m_Character.Move(0, false, false);
        }
    }

    [ClientRpc]
    public void RpcStartGame()
    {
        GameObject.Find(Type + "Actions").SetActiveRecursively(true);
        GameManager.Instance.State = GameState.Selection;
        GameManager.Instance.MusicCallback.StartMusic();
    }

    [ClientRpc]
    private void RpcPlaySound(string soundId, GameObject go)
    {
        AkSoundEngine.PostEvent(soundId, GameManager.Instance.CurrentPlayer.gameObject);
    }

    public void SendSound(string soundId)
    {
        if (isLocalPlayer)
        {
            CmdSendSound(soundId, gameObject);
        }
    }

    [Command]
    private void CmdSendSound(string soundId, GameObject go)
    {
        RpcPlaySound(soundId, go);
    }
}

public enum CharacterType
{
    Janitor,
    Party
}