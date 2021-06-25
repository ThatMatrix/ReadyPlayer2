using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Escalier : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public int sceneToGo;
    private PhotonView PV;

    private void Start()
    {
        Debug.Log("Escalier has been loaded");
        PV = GetComponent<PhotonView>();
    }

    void ResurrectPlayers()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            if (player.GetComponent<Health>() && player.GetComponent<Health>().curHealth <= 0)
            {
                player.GetComponent<Health>().HealPlayer(player.GetComponent<Health>().maxHealth);
            }
        }
        Debug.Log("Resurrected all players");
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("CollisionEnter Escalier");
        Debug.Log($"Tag of collider: {other.gameObject.tag}, is the right tag:{other.gameObject.CompareTag("Player")}");
        ResurrectPlayers();
        if (other.gameObject.CompareTag("Player"))
        {
            if (!other.gameObject.GetComponent<PhotonView>().Owner.IsMasterClient)
            {
                PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
            }
            
            PhotonNetwork.automaticallySyncScene = true;
            PhotonNetwork.LoadLevel(sceneToGo);
        }
    }
    

    public void OnEvent(EventData photonEvent)
    {
        PV = GetComponent<PhotonView>();
        if (photonEvent.Code == 50)
        {
            Debug.Log("recived event 50 which is the stairs event");
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("decided to load the scene because I am the master client");
                int sceneToLoad = (int) photonEvent.CustomData;
                PhotonNetwork.automaticallySyncScene = true;
                PhotonNetwork.LoadLevel(sceneToLoad);
            }
        }
    }
}
