using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cinemachine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameSetup : MonoBehaviour
{
    public static GameSetup GS;

    public Transform[] spawnPoints;

    public byte eventCodeToLaunch;

    private string sceneName;

    private void OnEnable()
    {
        if (GS == null)
        {
            GS = this;
        }
    }

    private void Start()
    {
        FindObjectOfType<AudioManager>().Stop("NPCTheme");
        FindObjectOfType<AudioManager>().GameSetupPlay();
        sceneName = SceneManager.GetActiveScene().name;
        LaunchEvent();
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
        Debug.Log("SceneNmae is " + sceneName);
        if (sceneName == "etageBoss")
        {
            GameObject pos1 = GameObject.FindGameObjectWithTag("SpawnCasper");
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Casper"),
               pos1.transform.position , Quaternion.identity);
        }
    }
}
