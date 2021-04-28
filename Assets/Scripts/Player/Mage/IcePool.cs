using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class IcePool : MonoBehaviour
{
    public PhotonView PV;
    private float timeDuration = 5f;
    private float timeSpawn;
    
    void Start()
    {
        timeSpawn = Time.time;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timeSpawn + timeDuration)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
