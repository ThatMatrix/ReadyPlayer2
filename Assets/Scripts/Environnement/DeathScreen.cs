using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    public GameObject stillOtherAliveScreen;
    public GameObject everyoneDeadScreen;

    private bool _amAlive;
    private GameObject _myPlayer;

    private bool _someoneIsAlive;

    private bool _hasFoundMyPLayerOnce;

    private void Start()
    {
        _hasFoundMyPLayerOnce = false;
        stillOtherAliveScreen.SetActive(false);
        everyoneDeadScreen.SetActive(false);
    }

    private void Init()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (var player in players)
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                _myPlayer = player;
                _amAlive = true;
                _someoneIsAlive = true;
                _hasFoundMyPLayerOnce = true;
                return;
            }
        }

        throw new NotSupportedException("Death screen could not find my own player at the start of the game");
    }

    private void LateUpdate()
    {
        if (!_hasFoundMyPLayerOnce)
        {
            Init();
        }

        if (!_hasFoundMyPLayerOnce)
        {
            return;
        }
        
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");


        if (_someoneIsAlive)
        {
            _someoneIsAlive = false;

            foreach (var player in players)
            {
                if (player.GetComponent<Health>() != null)
                {
                    if (player.GetComponent<Health>().curHealth > 0)
                    {
                        _someoneIsAlive = true;
                    }
                }
                else
                {
                    throw new NotSupportedException($"Player tag object does not have Health script : player {player}");
                }
            }
        }

        if (_amAlive)
        {
            _amAlive = _myPlayer.GetComponent<Health>().curHealth > 0;
        }

        if (_amAlive) return;
        
        if (_myPlayer != null && _myPlayer.GetPhotonView().IsMine)
        {
            PhotonNetwork.Destroy(_myPlayer);
        }
        
        if (_someoneIsAlive && !stillOtherAliveScreen.activeSelf)
        {
            stillOtherAliveScreen.SetActive(true);
            Debug.Log("made still other alive screen appeared");
        }
        
        else if (!_someoneIsAlive)
        {

            if (stillOtherAliveScreen.activeSelf)
            {
                stillOtherAliveScreen.SetActive(false);
            }

            if (!everyoneDeadScreen.activeSelf)
            {
                everyoneDeadScreen.SetActive(true);
                Debug.Log("made everyone dead screen appeared");

            }
        }
    }
}
