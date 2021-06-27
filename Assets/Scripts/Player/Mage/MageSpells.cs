using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UIElements;

public class MageSpells : PlayerSpells
{
    public GameObject pied;
    public PlayerMovement movement;
    public GameObject iceShardPrefab;
    public float iceShardForce = 20f;
    private float timeResetSpeed;
    private float timeDuration = 2f;
    private float oldSpeed = 7f;

    
    [SerializeField] private float rangeIcePool;

    public override void SetCooldowns()
    {
        cooldown1 = 2f;
        cooldown2 = 3.5f;
        cooldownM = 3f;
        cooldownU = 7f;
    }


    public override void MainSpell()
    {
        FindObjectOfType<AudioManager>().Play("ice_spell");
        if (!right)
        {
            //_animator.SetBool("ShootR",true);
            _animator.Play("Mage_shootR");
        }
        else
        {
            Debug.Log("not right: " + right);
            //_animator.SetBool("ShootL",true);
            _animator.Play("Mage_shootL");
        }
        
        GameObject ice_shard = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ice_shard(bullet)"), 
            firePoint.transform.position, firePoint.transform.rotation);
        
        Rigidbody2D rbBullet = ice_shard.GetComponent<Rigidbody2D>();
        rbBullet.AddForce(firePoint.transform.up * iceShardForce, ForceMode2D.Impulse);
        
    }

    public override void SecondarySpell()
    {
        FindObjectOfType<AudioManager>().Play("ice_slow");
        if (!right)
        {
            //_animator.SetBool("ShootR",true);
            _animator.Play("Mage_shootR");
        }
        else
        {
            Debug.Log("not right: " + right);
            //_animator.SetBool("ShootL",true);
            _animator.Play("Mage_shootL");
        }
        
        if (rangeIcePool - Vector2.Distance(mousePos, gameObject.transform.position) >= 0)
        {
            Debug.Log("In range");
            GameObject ice_pool = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ice_pool"),
                mousePos, Quaternion.identity);
        }
        else
        {
            Debug.Log("NOT In range");
            GameObject ice_pool = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ice_pool"),
                gameObject.transform.position + (Vector3) lookDir.normalized * rangeIcePool, Quaternion.identity);
        }
        
    }

    public override void MovementSpell()
    {
        FindObjectOfType<AudioManager>().Play("ice_skating");
        oldSpeed = movement.GetMoveSpeed();
        movement.SetMoveSpeed(oldSpeed + 10f);
        timeResetSpeed = Time.time + timeDuration;
    }

    private void LateUpdate()
    {
        if (movement.GetMoveSpeed() > oldSpeed && Time.time > timeResetSpeed)
        {
            movement.SetMoveSpeed(oldSpeed);
        }

        if (movement.GetMoveSpeed() > oldSpeed)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ice_trail"),
                pied.transform.position, Quaternion.identity);
        }
    }

    public override void Ultimate()
    {
        FindObjectOfType<AudioManager>().Play("blizzard");
        if (!right)
        {
            _animator.Play("Mage_shootR");
        }
        else
        {
            _animator.Play("Mage_shootL");
        }
        
        if (rangeIcePool - Vector2.Distance(mousePos, gameObject.transform.position) >= 0)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "MageSnowstorm"),
                mousePos, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "MageSnowstorm"),
                gameObject.transform.position + (Vector3) lookDir.normalized * rangeIcePool, Quaternion.identity);
        }
    }

    
}
