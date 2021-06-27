using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;


public class GameSetup : MonoBehaviour
{
    public static GameSetup GS;

    public Transform[] spawnPoints;

    public byte eventCodeToLaunch;

    private void OnEnable()
    {
        if (GS == null)
        {
            GS = this;
        }
    }

    private void Start()
    {
        LaunchEvent();
        FindObjectOfType<AudioManager>().Stop("NPCTheme");
        FindObjectOfType<AudioManager>().GameSetupPlay();
    }

    void LaunchEvent()
    {
        Debug.Log("gameSetup started");
        RaiseEventOptions options = new RaiseEventOptions()
        {
            Receivers = ReceiverGroup.All
        };
        PhotonNetwork.RaiseEvent(eventCodeToLaunch, 3, options, SendOptions.SendReliable);
        Debug.Log("Event Sent");
    }
}
