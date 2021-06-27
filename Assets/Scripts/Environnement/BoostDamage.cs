using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Rendering;

public class BoostDamage : MonoBehaviour
{
    public float durationOfBoost;
    public int valueOfBoost;
    private enum TypeOfPlayer
    {
        MAGE,
        MIDRANGE,
        CONTACT,
        NOTKNOWN,
    }

    private TypeOfPlayer _typeOfPlayer;
    private PhotonView PV;
    private bool hasBeenActivated;
    private PhotonView PVOfActivator;
    private SpriteRenderer _spriteRenderer;

    private float timeOfEndOfEffect;
    
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _typeOfPlayer = TypeOfPlayer.NOTKNOWN;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasBeenActivated && Time.time < timeOfEndOfEffect)
        {
            GameObject[] shoots = GameObject.FindGameObjectsWithTag("Ally_shot");

            foreach (var shoot in shoots)
            {
                if (_typeOfPlayer == TypeOfPlayer.MAGE)
                {
                    Projectile projectile = shoot.GetComponent<Projectile>();
                    if (!projectile.hasBeenBoosted)
                    {
                        projectile.hasBeenBoosted = true;
                        projectile.boostValue = valueOfBoost;
                    }
                }
                else if (_typeOfPlayer == TypeOfPlayer.MIDRANGE)
                {
                    ShotGunBullet projectile = shoot.GetComponent<ShotGunBullet>();
                    if (!projectile.hasBeenBoosted)
                    {
                        projectile.hasBeenBoosted = true;
                        projectile.boostValue = valueOfBoost;
                    }
                }
            }
        }
        else if (hasBeenActivated)
        {
            Debug.Log("Boost damage ended");
            DeleteObject(PVOfActivator);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasBeenActivated && other.CompareTag("Player"))
        {
            AudioManager.audioManager.Play("power up");

            if (other.GetComponent<MageSpells>() != null)
            {
                _typeOfPlayer = TypeOfPlayer.MAGE;
            }
            else if (other.GetComponent<ShotgunSpells>() != null)
            {
                _typeOfPlayer = TypeOfPlayer.MIDRANGE;
            }
            else if (other.GetComponent<MeleeSpells>() != null)
            {
                if (other.GetComponent<MeleeSpells>().attackDamage != null)
                {
                    Debug.Log("I love bugs");
                    other.GetComponent<MeleeSpells>().attackDamage += valueOfBoost;
                }
            }
            else
            {
                throw new NotImplementedException("not implemented this type of player");
            }

            Debug.Log("Boost damage activated");
            hasBeenActivated = true;
            PVOfActivator = other.GetComponent<PhotonView>();
            _spriteRenderer.enabled = false;
            timeOfEndOfEffect = Time.time + durationOfBoost;
        }
    }

    private void DeleteObject(PhotonView PV)
    {
        PV.TransferOwnership(PV.Controller);
        
        if (PV != null && PV.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
            Debug.Log("Destroyed BoostDamage item");
        }
    }
}
