using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int curHealth;
    public int maxHealth;
    
    public bool invincible;
    public float timeOfInvincibility;
    private float timeEndInvincibility;
    

    public HealthBar hp;
    
    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
        invincible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // if( Input.GetKeyDown( KeyCode.Space ) )
        // {
        //     DamagePlayer(10);
        // }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            if (Vector2.Distance(enemy.transform.position, gameObject.transform.position) <= 0.1f)
            {
                DamagePlayer(enemy.GetComponent<Enemy>().damage);
            }
        }

        if (invincible && Time.time > timeEndInvincibility)
        {
            invincible = false;
            Debug.Log("Invincibility status ended");
        }
    }

    public void DamagePlayer(int damage)
    {
        if (hp != null && !invincible)
        {
            if (curHealth - damage <= 0)
            {
                curHealth = 0;
            }
            else
            {
                curHealth -= damage;
            }
            Debug.Log($"Hp is : {curHealth}");
            hp.SetHealth(curHealth);
        }
    }

    public void HealPlayer(int heal)
    {
        if (hp != null)
        {
            if (curHealth + heal > maxHealth)
            {
                curHealth = maxHealth;
            }
            else
            {
                curHealth += heal;
            }
            Debug.Log($"Hp is {curHealth}");
            hp.SetHealth(curHealth);
        }
    }

    public void InvincibilityInit()
    {
        invincible = true;
        timeEndInvincibility = Time.time + timeOfInvincibility;
        Debug.Log($"Invincibility status initiated for {timeOfInvincibility} seconds");
    }
}
