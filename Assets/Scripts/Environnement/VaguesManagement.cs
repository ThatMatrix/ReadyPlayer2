using System;
using System.IO;
using System.Linq;
using System.Threading;
using Photon.Pun;
using UnityEngine;

public class VaguesManagement : MonoBehaviour
{
    public int curWave;
    public int maxWave;
    public bool canSpawn;

    public int[] nbMobWave;
    public GameObject[] spawnPoints;
    public string[] nameMobsToSpawn;

    public GameObject Boss;

    private bool HasEnemiesLeftOnTheMap()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] droids = GameObject.FindGameObjectsWithTag("Droid");

        foreach (var enemy in enemies)
        {
            if (enemy.activeSelf)
            {
                return true;
            }
        }

        foreach (var droid in droids)
        {
            if (droid.activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    void DeleteAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] droids = GameObject.FindGameObjectsWithTag("Droid");

        foreach (var enemy in enemies)
        {
            PhotonView photonView = enemy.GetPhotonView();
            
            if (PhotonNetwork.IsMasterClient || photonView.AmOwner || photonView.IsMine)
            {
                Debug.Log("destroyed enemy");
                PhotonNetwork.Destroy(gameObject);
            }
        }
        
        foreach (var droid in droids)
        {
            PhotonView photonView = droid.GetPhotonView();
            
            if (PhotonNetwork.IsMasterClient || photonView.AmOwner || photonView.IsMine)
            {
                Debug.Log("destroyed droid");
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    bool HaveToDelete()
    {
        if (!canSpawn)
        {
            return false;
        }
        
        if (curWave > maxWave)
        {
            Debug.Log("The current Wave is superior to the number of maximum wave : not spawning/deleting enemies");
        }

        return curWave < maxWave;
    }

    private void Start()
    {
        if (!(Boss == null))
        {
            Boss.SetActive(false);
        }
        
        if (nbMobWave.Max() < spawnPoints.Length)
        {
            throw new ArgumentException
                ("the number of spawnPoint is inferior to the number of mobs to spawn at some point");
        }

        if (nbMobWave.Length +  1 < maxWave)
        {
            throw new ArgumentException("The number of waves is superior to the number of mobs to spawn");
        }
        
        if (canSpawn)
        {
            curWave = 0;
            canSpawn = HaveToDelete();
        }
        else
        {
            Debug.Log("canSpawn is at false, not spawning enemies");
        }
    }

    private void Update()
    {
        canSpawn = HaveToDelete();
        if (canSpawn && (!HasEnemiesLeftOnTheMap() || curWave == 0))
        {
            Debug.Log("No enemies left on the map, incrementing the wave");
            curWave += 1;
            
            SpawnWave();
        }
    }

    void SpawnWave()
    {
        if (curWave == maxWave)
        {
            SpawnBossWave();
        }
        else
        {
            int nbMobToSpawn = nbMobWave[curWave - 1];
            
            for (int i = 0; i < nbMobToSpawn; i++)
            {
                GameObject spawnPoint = spawnPoints[i];
                int indexMobToSpawn = new System.Random().Next(nameMobsToSpawn.Length);
                
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", nameMobsToSpawn[indexMobToSpawn]),
                    spawnPoint.transform.position, spawnPoint.transform.rotation);
            }
        }
    }

    void SpawnBossWave()
    {
        if (Boss is null)
        {
            curWave += 1;
            return;
        }
        Boss.SetActive(true);
    }
}