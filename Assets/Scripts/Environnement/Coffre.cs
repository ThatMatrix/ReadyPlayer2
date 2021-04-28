using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;
using Random = System.Random;

public class Coffre : MonoBehaviour
{
    public bool opened;
    public string[] nameOfItemsToSpawn;

    private Animator _animator;
    private PhotonView PV;
    void Start()
    {
        opened = false;
        _animator = gameObject.GetComponent<Animator>();
        PV = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("CoffreOuvert"))
        {
            if (PV && PV.IsMine)
            {
                AfterChestIsOpened();
                PhotonNetwork.Destroy(gameObject);
                Debug.Log("Chest destroyed");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.audioManager.Play("chest opening");
            _animator.SetBool("HasPlayerInRange", true);
            opened = true;
        }
    }

    private void AfterChestIsOpened()
    {
        if (nameOfItemsToSpawn.Length > 0)
        {
            int indexItemToSpawn = new Random().Next(nameOfItemsToSpawn.Length);
            GameObject o;
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", nameOfItemsToSpawn[indexItemToSpawn]),
                (o = gameObject).transform.position, o.transform.rotation);
        }
        else
        {
            throw new ArgumentException("no itemName in the list");
        }
    }
}
