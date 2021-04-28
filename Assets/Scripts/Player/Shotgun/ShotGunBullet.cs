using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class ShotGunBullet : MonoBehaviour
{
    public int damage;
    private PhotonView PV;

    private float spawnTime;
    private float duration = 0.2f;
    
    public bool hasBeenBoosted;
    public int boostValue;

    private void Start()
    {
        spawnTime = Time.time;
        PV = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (Time.time > spawnTime + duration)
        {
            if (PV && ( PV.IsMine))
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyHealth>() != null && gameObject.CompareTag("Ally_shot"))
        {
            collision.GetComponent<EnemyHealth>().DamageEnemy(damage + boostValue);
        }

        if (collision.GetComponent<Health>() != null && gameObject.CompareTag("Enemy_shot"))
        {
            collision.GetComponent<Health>().DamagePlayer(damage);
        }
        if (PV && ( PV.IsMine))
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
