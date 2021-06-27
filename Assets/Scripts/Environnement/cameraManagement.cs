using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cinemachine;
using Photon.Pun;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cameraManagement : MonoBehaviour
{
    public List<GameObject> cameras;
    public GameObject myCam;

    public GameObject prefab;
    
    private int sceneIndex;

    public Dictionary<int, bool> players;

    public Dictionary<int, GameObject> DicoCams;

    public List<int> playerListStock;

    private void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (prefab == null)
        {
            throw new ArgumentException("prefab set to null, could not instantiate camera");
        }
        
        players = new Dictionary<int, bool>();
        DicoCams = new Dictionary<int, GameObject>();

        playerListStock = new List<int>();

    }

    private void LateUpdate()
    {
        GameObject[] playerList = GameObject.FindGameObjectsWithTag("Player");
        
        foreach (var player in playerList)//do while in game
        {

            if (!playerListStock.Contains(player.GetInstanceID()))
            {
                playerListStock.Add(player.GetInstanceID());
            }
            
            try
            {
                if (player.GetComponent<Health>() != null && player.GetComponent<Health>().curHealth > 0)
                {
                    players.Add(player.GetInstanceID(), false);
                }
            }
            catch (Exception)
            {
                // ignored
            }

            if (players.ContainsKey(player.GetInstanceID()) && players[player.GetInstanceID()] == false)
            {
                players[player.GetInstanceID()] = true;
                
                Debug.Log("instantiated 1 camera");
                player.GetComponent<PlayerMovement>().HasCamera = true;

                //For each player we instantiate a camera at its position
                GameObject cinemachineVirtualCamera = Instantiate(prefab, player.transform.position, player.transform.rotation);

                DicoCams.Add(player.GetInstanceID(), cinemachineVirtualCamera);
                
                if (sceneIndex == 4) //we need this condition because in the bossRoom the camera must unzoomed a bit
                {
                    cinemachineVirtualCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = 27.02f;
                }
                if (sceneIndex == 6) //we need this condition because in the bossRoom the camera must unzoomed a bit
                {
                    cinemachineVirtualCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = 10f;
                }
                
                //we make it so the camera follows the player
                Transform toFollow = player.GetComponentInChildren<CenterManager>().GetCenter();
                cinemachineVirtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = toFollow;
                cinemachineVirtualCamera.GetComponent<CinemachineVirtualCamera>().LookAt = toFollow;
                cinemachineVirtualCamera.GetComponent<CinemachineVirtualCamera>().enabled = true;

                //we add the new camera to the camera list
                cameras.Add(cinemachineVirtualCamera);


                if (player.GetPhotonView() != null && player.GetPhotonView().IsMine)
                {
                    myCam = cinemachineVirtualCamera;
                    cinemachineVirtualCamera.GetComponent<CinemachineVirtualCamera>().Priority = 99;
                }
                else
                {
                    cinemachineVirtualCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;
                }
            }
            else
            {
                for(int i = 0; i < playerListStock.Count;i++)
                {
                    int stockPlayer = playerListStock[i];
                    bool found = false;

                    foreach (var item in playerList)
                    {
                        if (item.GetInstanceID() == stockPlayer)
                        {
                            found = true;
                            break;
                        }
                    }
                    
                    if (!found)
                    {
                        GameObject cam = DicoCams[stockPlayer];
                        if (cam != myCam)
                        {
                            cameras.Remove(cam);
                        
                            DicoCams.Remove(stockPlayer);
                            playerListStock.Remove(stockPlayer);
                            players.Remove(stockPlayer);
                        }
                    }
                }
            }
            
            GameObject[] cameraGameList = GameObject.FindGameObjectsWithTag("Camera");

            for (int i = 0; i < cameraGameList.Length; i++)
            {
                if (!cameras.Contains(cameraGameList[i]))
                {
                    DestroyImmediate(cameraGameList[i]);
                    Debug.Log("destroyed camera");
                }
            }
        }
    }
}
