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
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("SpawnZone"))
        {
            if (collision.collider.GetComponent<EnemyHealth>() != null)
            {
                collision.collider.GetComponent<EnemyHealth>().DamageEnemy(damage + boostValue);
            }
        
            if (PV && ( PV.IsMine))
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
