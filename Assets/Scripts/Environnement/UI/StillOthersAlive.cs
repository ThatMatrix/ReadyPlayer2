using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class StillOthersAlive : MonoBehaviour
{
    private GameObject _myCam;
    private int indexOfActualCamera = 0;
    private List<GameObject> _playersCameras;
    
    void OnEnable()
    {
         _playersCameras = GameObject.Find("CameraManager").GetComponent<cameraManagement>().cameras;
         _myCam = GameObject.Find("CameraManager").GetComponent<cameraManagement>().myCam;
         _playersCameras[indexOfActualCamera].GetComponent<CinemachineVirtualCamera>().Priority = 99;
    }

    // Update is called once per frame
    void Update()
    {
        _playersCameras = GameObject.Find("CameraManager").GetComponent<cameraManagement>().cameras;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            int oldIndex = indexOfActualCamera;
            if (indexOfActualCamera - 1 < 0)
            {
                indexOfActualCamera = _playersCameras.Count - 1;
            }
            else
            {
                indexOfActualCamera -= 1;
            }
            
            _playersCameras[oldIndex].GetComponent<CinemachineVirtualCamera>().Priority = 0;
            _playersCameras[indexOfActualCamera ].GetComponent<CinemachineVirtualCamera>().Priority = 99;
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            int oldIndex = indexOfActualCamera;
            if (indexOfActualCamera + 1 >= _playersCameras.Count)
            {
                indexOfActualCamera = 0;
            }
            else
            {
                indexOfActualCamera += 1;
            }
            
            _playersCameras[oldIndex].GetComponent<CinemachineVirtualCamera>().Priority = 0;
            _playersCameras[indexOfActualCamera ].GetComponent<CinemachineVirtualCamera>().Priority = 99;
        }
    }
}
