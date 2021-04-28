using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Grenade : MonoBehaviour
{
    public PhotonView PV;
    private float timeDuration = 1f;
    private float timeSpawn;
    public int damage;
    
    void Start()
    {
        PV = GetComponent<PhotonView>();
        timeSpawn = Time.time;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timeSpawn + timeDuration)
        {
            if ( PV.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<EnemyHealth>() != null)
        {
            other.GetComponent<EnemyHealth>().DamageEnemy(damage);
        }
    }
}
