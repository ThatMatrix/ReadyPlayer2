using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UltimateBullet : MonoBehaviour
{
    public int damage;
    private PhotonView PV;

    private float spawnTime;

    public bool hasBeenBoosted;
    public int boostValue;

    private void Start()
    {
        spawnTime = Time.time;
        PV = GetComponent<PhotonView>();
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
        
            if (PV && ( PV.IsMine) && !collision.CompareTag("Ice_Pool"))
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
