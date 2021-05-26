using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using Cinemachine;
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using UnityEngine;
using Random = System.Random;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using Photon;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class PhotonPlayer : MonoBehaviourPunCallbacks, IOnEventCallback
{
    private enum PlayerType
    {
        IceMage,
        MidRange,
        Contact,
    }
    
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    public Animator animator;
    private Vector2 movement;
    private PhotonView PV;
    public GameObject myAvatar;
    public CinemachineVirtualCamera MyCamera;
    public GameObject myLifeBar;
    private int _currentscene;
    private int spawnPicker;

    private string _prefabName;
    private PlayerType _playerTypeChosen;

    void Awake()
    {
        GameObject photonRoom = GameObject.FindGameObjectWithTag("RoomController");
        int playerType = photonRoom.GetComponent<photonRoom>().playerTypeChosen;
        _playerTypeChosen = (PlayerType) playerType;
        
        switch (_playerTypeChosen)
        {
            case PlayerType.IceMage:
                _prefabName = Path.Combine("PhotonPrefabs", "PlayerAvatar");
                break;
            case PlayerType.MidRange:
                _prefabName = Path.Combine("PhotonPrefabs", "MidRange");
                break;
            case PlayerType.Contact:
                _prefabName = Path.Combine("PhotonPrefabs", "Melee");
                break;
        }
    }
    
    

    void Start()
    {
        DontDestroyOnLoad(this);
        Debug.Log($"Added to don't destroy on load{this}");
        PV = GetComponent<PhotonView>();
        Debug.Log($"isMaster of the Room:{PV.Owner.IsMasterClient}");
        
        if (PV.Owner.IsMasterClient)
        {
            PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
            Debug.Log($"{PhotonNetwork.LocalPlayer} has been set as master client");
        }


        if (PV.IsMine)
        {
            Scene scene = SceneManager.GetActiveScene();
            _currentscene = scene.buildIndex;
            Vector3 position = Vector3.positiveInfinity; //never used but because we cannot set it to null
                                                         //we use a random value to check if it has been initialized
            Quaternion quaternion = Quaternion.Euler(0,0,0); //never used but because we cannot set it to null
                                                             //we use a random value to check if it has been initialized
             
            spawnPicker = new Random().Next(GameSetup.GS.spawnPoints.Length);
            
            position = GameSetup.GS.spawnPoints[spawnPicker].position; //position pour spawn avatar
            quaternion = GameSetup.GS.spawnPoints[spawnPicker].rotation; //rotation pour spawn avatar

            if (_prefabName == null)
            {
                Debug.Log("PhotonPlayer: Could not generate the Player avatar, reason: name is null");
                throw new Exception("PhotonPlayer: Could not generate the Player avatar, reason: name is null");
            }
            
            myAvatar = PhotonNetwork.Instantiate(_prefabName,
                    position, quaternion, 0); //spawns the PlayerAvatar into the game
            
            if (myAvatar == null)
            {
                Debug.Log("PhotonPlayer: Could not generate the Player avatar");
                throw new Exception("PhotonPlayer: Exception: Could not generate the Player avatar");
            }

            Debug.Log("Avatar Genrated"); //Log that PlayerAvatar has been spawned

            myAvatar.GetComponent<Health>().hp = gameObject.AddComponent<HealthBar>(); //ajoute la barre de vie
            myAvatar.GetComponent<Health>().hp.tag = "BarDeVie"; //lui donne un tag pour la retrouver
        }
    }

    public void OnEvent(EventData data) //gère chgmt étages
    {
        if (myAvatar == null) //if PlayerAvatar does not exists we don't care
        {
            Debug.Log("playerAvatar does not exist");
            return;
        }
        
        byte eventcode = data.Code; //get the code of the event recieved

        if (eventcode == 2 || eventcode == 3 || eventcode == 5 || eventcode == 4) //if the code is the code to go to floor2
        {
            if (myAvatar.GetComponent<PlayerMovement>() != null && myAvatar.GetComponent<PlayerMovement>().HasCamera) //avoid null object references
            {
                myAvatar.GetComponent<PlayerMovement>().HasCamera = false; //update the camera get the ones of the new floor
                Debug.Log("Setup Camera complete"); //log the update
            }
            else
            {
                Debug.Log("Could not Setup the Cameras"); //log the failure of update cameras
            }
            SetupHealth(); //update the healthbar of the player for the new floor
            if (GameSetup.GS != null && GameSetup.GS.spawnPoints != null) //avoid null object references
            {
                UpdatePosPerso(GameSetup.GS.spawnPoints); //update the position of the PlayerAvatar to a spawn point
                Debug.Log("Position Updated"); //log the update
            }
            Debug.Log("Setup etage complete");
        }
    }

    private void SetupHealth()
    {
        GameObject BarreDeVieGameObject = GameObject.FindWithTag("BarDeVie");
        if (BarreDeVieGameObject.GetComponent<HealthBar>() != null)
        {
            myAvatar.GetComponent<Health>().hp = BarreDeVieGameObject.GetComponent<HealthBar>();
            myAvatar.GetComponent<Health>().hp.tag = "BarDeVie";
            Debug.Log("Setup health for floor complete"); //log the update of the playerHealthBar
        }
        else
        {
            Debug.Log("Could not update the Healthbar of the player"); //log the failure
        }
    }

    private void UpdatePosPerso(Transform[] spawnpoints)
    {
        int spawnPicker = new Random().Next(spawnpoints.Length);
        myAvatar.transform.position = spawnpoints[spawnPicker].transform.position;
    }
}