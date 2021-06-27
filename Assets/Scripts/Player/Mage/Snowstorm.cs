using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Snowstorm : MonoBehaviour
{
    private PhotonView PV;
    private float spawnTime;
    public float duration;
    private List<GameObject> affected;
    
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        affected = new List<GameObject>();
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > spawnTime + duration)
        {
            foreach (GameObject enemy in affected)
            {
                if (enemy.GetComponent<Lizard>() != null)
                {
                    enemy.GetComponent<Lizard>().enabled = true;
                }

                if (enemy.GetComponent<Casper>() != null)
                {
                    enemy.GetComponent<Casper>().enabled = true;
                }

                if (enemy.GetComponent<EnemyAI>() != null)
                {
                    enemy.GetComponent<EnemyAI>().enabled = true;
                }
            }

            if (PV.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("ABCD collider from snowstorm");
        if (other.GetComponent<Lizard>() != null)
        {
            other.GetComponent<Lizard>().enabled = false;
            affected.Add(other.gameObject);
        }

        if (other.GetComponent<Casper>() != null)
        {
            other.GetComponent<Casper>().enabled = false;
            affected.Add(other.gameObject);
        }

        if (other.GetComponent<EnemyAI>() != null)
        {
            other.GetComponent<EnemyAI>().enabled = false;
            affected.Add(other.gameObject);
        }
    }
}
