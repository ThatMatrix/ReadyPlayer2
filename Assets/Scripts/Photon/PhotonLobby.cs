using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = System.Random;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby;

    public GameObject battleButton;
    public GameObject cancelButton;
    public GameObject offlineButton;
    public GameObject settingsButton;
    public List<GameObject> uiGameObjectsMenuBase;
    public List<GameObject> uiGameObjectsSettings;
    public Dropdown resolutionDropdown;
    Resolution[] resolutions;
    public GameObject menuButton;
    public GameObject secondRoomButton;

    public GameObject iceMageButton;
    public GameObject midRangeButton;
    public GameObject contactButton;

    public UnityEvent playerTypeIceMage;    
    public UnityEvent playerTypeMidRange;
    public UnityEvent playerTypeContact;

    public GameObject LoadButton;

    //private bool _connectedToMaster;

    
    private void Awake()
    {
        lobby = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //_connectedToMaster = false;
        PhotonNetwork.ConnectUsingSettings(); //Connects to photon MasterServer
        MenuUiListInitialize();
        SettingsUiListInitialize();
        HideAllUiMenu();
        HideAllUiSettings();
        ResolutionDropdownInit();
        Screen.SetResolution(1000, 600, Screen.fullScreen);
        iceMageButton.SetActive(false);
        midRangeButton.SetActive(false);
        contactButton.SetActive(false);
        offlineButton.SetActive(true);
    }

    void CharacterSelection()
    {
        iceMageButton.SetActive(true);
        midRangeButton.SetActive(true);
        contactButton.SetActive(true);
    }

    public void OnIceMageButtonClicked()
    {
        playerTypeIceMage.Invoke();
        AfterSelectionOfCharacter();
    }

    public void OnMidRangeButtonClicked()
    {
        playerTypeMidRange.Invoke();
        AfterSelectionOfCharacter();
    }
    
    public void ContactButtonClicked()
    {
        playerTypeContact.Invoke();
        AfterSelectionOfCharacter();
    }

    void AfterSelectionOfCharacter()
    {
        iceMageButton.SetActive(false);
        midRangeButton.SetActive(false);
        contactButton.SetActive(false);

        PhotonNetwork.JoinRandomRoom();
        Debug.Log("trying To Join a Room");
    }
    

    private void MenuUiListInitialize()
    {
        uiGameObjectsMenuBase = new List<GameObject>();
        uiGameObjectsMenuBase.Add(battleButton);
        uiGameObjectsMenuBase.Add(LoadButton);
        uiGameObjectsMenuBase.Add(offlineButton);
        uiGameObjectsMenuBase.Add(cancelButton);
        uiGameObjectsMenuBase.Add(settingsButton);
        uiGameObjectsSettings.Add(secondRoomButton);
    }

    private void SettingsUiListInitialize()
    {
        uiGameObjectsSettings = new List<GameObject>();
        uiGameObjectsSettings.Add(menuButton);
    }

    void ResolutionDropdownInit()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;
        
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + 
                            resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width 
                && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
    }

    public override void OnConnectedToMaster()//OnConnectedToMaster ?
    {
        Debug.Log("Connection to the master server successful");
        PhotonNetwork.AutomaticallySyncScene = true;
        //_connectedToMaster = true;
        Debug.Log($"isMaster: {PhotonNetwork.IsMasterClient}");
        offlineButton.SetActive(false);
        settingsButton.SetActive(true);
        battleButton.SetActive(true);
        LoadButton.SetActive(true);
        secondRoomButton.SetActive(true);
    }

    public void OnBattleButtonClicked()
    {
        Debug.Log("BattleButton was clicked");
        battleButton.SetActive(false);
        LoadButton.SetActive(false);
        settingsButton.SetActive(false);
        secondRoomButton.SetActive(false);
        
        cancelButton.SetActive(true);
        
        CharacterSelection();

    }

    public void OnSecondRoomButtonClicked()
    {
        Debug.Log("Clicked SecondRoom Button");
        CreateRoomTest();
    }
    
    void CreateRoomTest()
    {
        Debug.Log("trying to create a room ...");
        int randomRoomName = new Random().Next(0, 10000);
        RoomOptions roomOps = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = false,
            MaxPlayers = 4
        };
        PhotonNetwork.CreateRoom("Room " + randomRoomName, roomOps);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join a random Room but failed. There must be no game available");
        CreateRoom();
    }

    void CreateRoom()
    {
        Debug.Log("trying to create a room ...");
        int randomRoomName = new Random().Next(0, 10000);
        RoomOptions roomOps = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 4
        };
        PhotonNetwork.CreateRoom("Room " + randomRoomName, roomOps);
    }


    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a room but failed, there must be a room with the same name");
        CreateRoom();
    }

    public void OnCancelButtonClicked()
    {
        Debug.Log("CancelButton was clicked");
        cancelButton.SetActive(false);
        iceMageButton.SetActive(false);
        midRangeButton.SetActive(false);
        contactButton.SetActive(false); 
        
        battleButton.SetActive(true);
        LoadButton.SetActive(true);
        settingsButton.SetActive(true);
        secondRoomButton.SetActive(true);
    }

    public void HideAllUiMenu()
    {
        foreach (var button in uiGameObjectsMenuBase)
        {
            button.SetActive(false);
        }
        Debug.Log("All buttons in BaseMenu have been hidden");
    }

    private void HideAllUiSettings()
    {
        foreach (var objects in uiGameObjectsSettings)
        {
            objects.SetActive(false);
        }

        resolutionDropdown.gameObject.SetActive(false);
        Debug.Log("All Button from Settings Menu have been hidden");
    }
    
    private void ShowAllUiSettings()
    {
        foreach (var objects in uiGameObjectsSettings)
        {
            objects.SetActive(true);
        }

        resolutionDropdown.gameObject.SetActive(true);
        Debug.Log("All Button from Settings Menu have been hidden");
    }

    public void SettingsButtonClicked()
    {
        Debug.Log("Settings button have been clicked");
        HideAllUiMenu();
        ShowAllUiSettings();
    }

    public void MenuButtonClicked()
    {
        Debug.Log("Menu Button has been clicked");
        HideAllUiSettings();
        battleButton.SetActive(true);
        settingsButton.SetActive(true);
    }
    
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, 
            resolution.height, Screen.fullScreen);
    }

    public void LoadButtonClicked()
    {
        //PlayerPrefs.SetInt("SceneIndex", sceneIndex);
        //PlayerPrefs.SetInt("PlayerType", playerType);
        if (PlayerPrefs.HasKey("SceneIndex"))
        {
            int sceneIndex = PlayerPrefs.GetInt("SceneIndex");
            photonRoom.room.multiplayerScene = sceneIndex;

        }

        if (PlayerPrefs.HasKey("PlayerType"))
        {
            int playerType = PlayerPrefs.GetInt("PlayerType");
            photonRoom.room.playerTypeChosen = playerType;
        }
        
        CreateRoom();
        
    }
}
