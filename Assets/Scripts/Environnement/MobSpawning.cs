using System;
using System.IO;
using Photon.Pun;
using UnityEngine;
using Random = System.Random;

public class MobSpawning : MonoBehaviour
{
    public GameObject[] mobSpawnPoints;
    public string[] nameMobsToSpawn;
    private bool _hasBeenEnabled;

    private PhotonView PV;

    private void Start()
    {
        _hasBeenEnabled = false;
        PV = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && nameMobsToSpawn.Length > 0 && !_hasBeenEnabled && PV != null 
            && PV.IsMine)
        {
            
            _hasBeenEnabled = true;
            foreach (var spawnPoint in mobSpawnPoints)
            {
                int indexMobToSpawn = new Random().Next(nameMobsToSpawn.Length);
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", nameMobsToSpawn[indexMobToSpawn]),
                    spawnPoint.transform.position, spawnPoint.transform.rotation);
            }
            
            
            if (PV != null && PV.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
                Debug.Log("Destroyed spawnZone item");
            }
        }
    }
        
        
}
