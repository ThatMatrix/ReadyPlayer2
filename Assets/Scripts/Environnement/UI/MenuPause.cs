using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviourPun
{
    private GameObject player;
    public bool paused;

    public GameObject menuPause;

    public GameObject disconnectButton;
    // Start is called before the first frame update
    void Start()
    {
        paused = false;
        
        disconnectButton.SetActive(false);
        menuPause.SetActive(false);
        
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
                disconnectButton.SetActive(true);
                
                DisablePlayer();
            }
            else
            {
                disconnectButton.SetActive(false);
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
}
