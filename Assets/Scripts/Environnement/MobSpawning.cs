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

    private void Start()
    {
        _hasBeenEnabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && nameMobsToSpawn.Length > 0 && !_hasBeenEnabled)
        {
            _hasBeenEnabled = true;
            foreach (var spawnPoint in mobSpawnPoints)
            {
                int indexMobToSpawn = new Random().Next(nameMobsToSpawn.Length);
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", nameMobsToSpawn[indexMobToSpawn]),
                    spawnPoint.transform.position, spawnPoint.transform.rotation);
            }
        }
    }
}
