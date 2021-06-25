using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Photon.Pun;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    private PhotonView PV;

    public bool hasBeenBoosted;
    public int boostValue;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        boostValue = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("SpawnZone"))
        {
            if (collision.GetComponent<EnemyHealth>() != null && gameObject.CompareTag("Ally_shot"))
            {
                collision.GetComponent<EnemyHealth>().DamageEnemy(damage + boostValue);
            } else if (collision.GetComponent<Health>() != null && gameObject.CompareTag("Enemy_shot"))
            {
                collision.GetComponent<Health>().DamagePlayer(damage);
            }
        
            if (PV && ( PV.IsMine))
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
        
}
