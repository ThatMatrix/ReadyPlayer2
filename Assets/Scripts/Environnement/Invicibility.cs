using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Invicibility : MonoBehaviour
{
    public float timeOfInvincibility;
    private PhotonView PV;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.GetComponent<Health>() != null)
        {
            AudioManager.audioManager.Play("power up");
            Health health = other.GetComponent<Health>();
            health.timeOfInvincibility = timeOfInvincibility;
            health.InvincibilityInit();
            
            PV.TransferOwnership(other.GetComponent<PhotonView>().Controller);
            
            if (PV != null && PV.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
                Debug.Log("Destroyed invincibility item");
            }
        }
    }
}
