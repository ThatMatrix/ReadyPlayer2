using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class HealthBar : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public Slider healthBar;
    public Health playerHealth;
    private void Start()
    {
        playerHealth = FindMyLife();
        if(playerHealth != null && GameObject.FindGameObjectWithTag("BarDeVie").GetComponent<Slider>() != null)
        {
            healthBar = GameObject.FindGameObjectWithTag("BarDeVie").GetComponent<Slider>();
            Debug.Log($"healthBar max value : " +
                      $"Player Health max Value : {playerHealth.maxHealth}");
            healthBar.maxValue = playerHealth.maxHealth;
            healthBar.value = playerHealth.maxHealth;
        }
    }
    
    

    public void SetHealth(int hp)
    {
        Debug.Log($"healthBar is {healthBar}");
        if (healthBar != null)
        {
            healthBar.value = hp;
        }
    }

    Health FindMyLife()
    {
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in Players)
        {
            if (player.GetPhotonView().IsMine)
            {
                return player.GetComponent<Health>();
            }
        }

        return null;
    }

    public void OnEvent(EventData photonEvent)
    {
        
        if (photonEvent.Code == 1 || photonEvent.Code == 3 || photonEvent.Code == 5 || photonEvent.Code == 6) //Passage etage
        {
            playerHealth = FindMyLife(); //set le player sur le player de l'étage
            Debug.Log($"event {photonEvent.Code} received by HealthBar");
            if (playerHealth != null && GameObject.FindGameObjectWithTag("BarDeVie").GetComponent<Slider>() != null)
            {
                healthBar = GameObject.FindGameObjectWithTag("BarDeVie").GetComponent<Slider>();
                Debug.Log($"healthBar max value : " +
                          $"Player Health max Value : {playerHealth.maxHealth}");
                SetHealth(playerHealth.curHealth); //actualisation de la nouvelle lifebar sur la vie du joueur
            }
        }
    }
}
