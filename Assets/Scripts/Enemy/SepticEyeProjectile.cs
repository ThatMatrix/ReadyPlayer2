using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SepticEyeProjectile : MonoBehaviour
{
    public int damage;
    private PhotonView PV;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Health>() != null)
        {
            collision.GetComponent<Health>().DamagePlayer(damage);
        }
        
        if (PV && ( PV.IsMine))
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
