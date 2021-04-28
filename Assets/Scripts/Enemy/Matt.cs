using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matt : MonoBehaviour
{
    public int attackDamage = 20;
    public float attackRange = 1f;
    public Vector3 attackOffset;
    public LayerMask attackMask;

    public Transform player;

    

    private void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length == 0)
        {
            return;
        }
        player = players[0].transform;
        foreach (GameObject p in players)
        {
            if (Vector2.Distance(p.transform.position, transform.position) > Vector2.Distance(player.position, transform.position))
            {
                player = p.transform;
            }
        }
    }

    
    
    public void Attack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null && colInfo.CompareTag("Player"))
        {
            colInfo.GetComponent<Health>().DamagePlayer(attackDamage);
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Gizmos.DrawWireSphere(pos, attackRange);
    }
}
