using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Heart : MonoBehaviour
{
    private PhotonView PV;
    public int healingNb;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        AudioManager.audioManager.Play("power up");
        if (other.CompareTag("Player") && other.GetComponent<Health>() != null)
        {
            Health health = other.GetComponent<Health>();
            health.HealPlayer(healingNb);
            
            PV.TransferOwnership(other.GetComponent<PhotonView>().Controller);
            
            if (PV != null && PV.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
                Debug.Log("Destroyed Heart item");
            }
        }
    }
}
