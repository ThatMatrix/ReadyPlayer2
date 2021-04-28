using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ice_trail : MonoBehaviour
{
    private float timeSpawn;

    private PhotonView PV;
    // Start is called before the first frame update
    void Start()
    {
        timeSpawn = Time.time;
        PV = gameObject.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timeSpawn + 0.5f)
        {
            if (PV && ( PV.IsMine))
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
