using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Pun;
using Unity.Mathematics;

public class EnemyHealth : MonoBehaviour
{
    public int curHealth;
    public int maxHealth;
    private PhotonView PV;

    private int sceneToGo;
    
    private bool killOnce;
    
    
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        curHealth = maxHealth;
        killOnce = false;
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     DamageEnemy(20);
        // }
        
        if (curHealth <= 0)
        {
            FindObjectOfType<AudioManager>().Play("enemy death");
            
            if (gameObject.GetComponent<SecticEye>() != null)
            {
                gameObject.GetComponent<Animator>().SetTrigger("Dead");
                FindObjectOfType<AudioManager>().Stop("SepticEyeTheme");
                GameObject.FindGameObjectWithTag("NPC").SetActive(true);
            }

            if (gameObject.GetComponent<Matt>() != null)
            {
                FindObjectOfType<AudioManager>().Stop("MattTheme");
                if (!killOnce)
                {
                    sceneToGo = 6;
                    Kill();
                    killOnce = true;
                }

                GameObject.FindGameObjectWithTag("NPC").GetComponent<NPCDialogBox>().go = true;
            }

            if (gameObject.GetComponent<Casper>() != null)
            {
                FindObjectOfType<AudioManager>().Stop("CasperTheme");
                gameObject.GetComponent<Casper>().animator.SetBool("Dead", true);
                Debug.Log("Casper hp are at 0");
                if (!killOnce)
                {
                    sceneToGo = 5;
                    Invoke("Kill", 2.5f);
                    killOnce = true;
                }

                GameObject.FindGameObjectWithTag("NPC").GetComponent<NPCDialogBox>().go = true;
            }
            else
            {
                if (PhotonNetwork.IsMasterClient || PV.AmOwner || PV.IsMine)
                {
                    Debug.Log("destroyed enemy");
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }
    }

    public void DamageEnemy( int damage )
    {
        curHealth -= damage;
    }
    
    void Kill()
    {
        if (GetComponent<PhotonView>() && ( PV.IsMine ||PV.AmOwner || PhotonNetwork.IsMasterClient))
        {
            GameObject portail = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Portail"),
                gameObject.transform.position, quaternion.identity);
            if (portail.GetComponent<Escalier>() != null)
            {
                portail.GetComponent<Escalier>().sceneToGo = this.sceneToGo;
            }
            else
            {
                throw new NotImplementedException("aled");
            }

            PhotonNetwork.Destroy(gameObject);
        }        
    }
}
