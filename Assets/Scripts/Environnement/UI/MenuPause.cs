using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviourPun
{
    private GameObject player;
    public bool paused;

    public GameObject menuPause;

    public GameObject gameSaved;
    //public GameObject disconnectButton;
    //public GameObject saveButton;

    public GameObject[] listButtons;
    // Start is called before the first frame update
    void Start()
    {
        paused = false;
        //disconnectButton.SetActive(false);
        DeactivateAllButtons();
        menuPause.SetActive(false);
        gameSaved.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("MenuPause: Pressed keyEscape");
            if (!paused)
            {
                menuPause.SetActive(true);
                ActivateAllButtons();
                DisablePlayer();
            }
            else
            {
                DeactivateAllButtons();
                gameSaved.SetActive(false);
                menuPause.SetActive(false);

                if (player != null)
                {
                    player.SetActive(true);
                }
            }

            paused = !paused;
        }
    }

    void DisablePlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                this.player = player;
                player.SetActive(false);
            }
        }
    }
    
    public void onDisconnectButtonClicked()
    {
        Debug.Log("Clicked disconnect button");
        if (photonRoom.room != null)
        {
            Destroy(photonRoom.room.gameObject);
        }
        if (AudioManager.audioManager != null)
        {
            Destroy(AudioManager.audioManager.gameObject);
        }
        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom)
        {
            yield return null;
        }
        PhotonNetwork.automaticallySyncScene = false;
        SceneManager.LoadScene(0);
    }

    void DeactivateAllButtons()
    {
        foreach (var button in listButtons)
        {
            button.SetActive(false);
        }
    }

    void ActivateAllButtons()
    {
        foreach (var button in listButtons)
        {
            button.SetActive(true);
        }
    }

    public enum PlayerType
    {
        Mage,
        Midrange,
        Contact,
    }

    PlayerType GetPlayerType()
    {
        if (player.GetComponent<MageSpells>() != null)
        {
            return PlayerType.Mage;
        }

        if (player.GetComponent<ShotgunSpells>() != null)
        { 
            return PlayerType.Midrange;
        }

        throw new NotImplementedException("not implemented this type of player");
    }
    
    public void OnSaveButtonClicked()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        int playerType = (int) GetPlayerType();
        PlayerPrefs.SetInt("SceneIndex", sceneIndex);
        PlayerPrefs.SetInt("PlayerType", playerType);
        Debug.Log($"Game Saved: SceneIndex = {sceneIndex}, PlayerType = {(PlayerType) playerType}");

        PopGameSaved();
    }

    void PopGameSaved()
    {
        gameSaved.SetActive(true);
    }
}
