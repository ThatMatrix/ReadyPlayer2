using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class photonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    private enum PlayerType
    {
        IceMage,
        MidRange,
        Contact,
    }

    public static photonRoom room;
    private PhotonView PV;

    public int currentScene;
    public int multiplayerScene;

    public int playerTypeChosen;

    private void Awake()
    {
        if (photonRoom.room == null)
        {
            room = this;
        }
        else
        {
            if (room != this)
            {
                Destroy(room.gameObject);
                room = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("We have joined a room");
        StartGame();
    }

    void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        Debug.Log("Loading level");
        PhotonNetwork.LoadLevel(multiplayerScene);
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;
        if (currentScene >= multiplayerScene && !AlreadyHasPlayer())
        {
            CreatePlayer();
        }
    }

    // Creates player
    private void CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position,
            Quaternion.identity, 0);
        Debug.Log("Instantiate PhotonNetworkPlayer");
    }

    bool AlreadyHasPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            if (player.GetComponent<PhotonView>() != null && player.GetComponent<PhotonView>().AmOwner)
            {
                return true;
            }
        }

        return false;
    }

    public void PlayerTypeMidRange()
    {
        playerTypeChosen = (int)PlayerType.MidRange;
    }

    public void PlayerTypeIceMage()
    {
        playerTypeChosen = (int)PlayerType.IceMage;
    }

    public void PlayerTypeContact()
    {
        playerTypeChosen = (int)PlayerType.Contact;
    }
}
