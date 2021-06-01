using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

public class MeleeSpells : PlayerSpells
{
    public float dashlength;
    
    public int attackDamage = 20;
    public float attackRange = 1f;
    public Vector3 attackOffset;
    public LayerMask attackMask;

    public Transform player;
    
    
    public override void SetCooldowns()
    {
        cooldown1 = 2.5f;
        cooldown2 = 2f;
        cooldownM = 5f;
        cooldownU = 10f;
    }

    public override void MainSpell()
    {
        Vector2 movement = GetComponent<PlayerMovement>().GetMovement();
        GetComponent<PlayerMovement>().enabled = false;
        
        if (movement.x > 0 && movement.x > movement.y) // Dash Right
        {
            GetComponent<ShotGunDash>().SetDirection(2);
        }
        else if (movement.x < 0 && movement.x < movement.y) // Dash Left
        {
            GetComponent<ShotGunDash>().SetDirection(1);
        }
        else if (movement.y > 0 && movement.y > movement.x) // Dash Up
        {
            GetComponent<ShotGunDash>().SetDirection(3);
        }
        else if (movement.y < 0 && movement.y < movement.x) // Dash Down
        {
            GetComponent<ShotGunDash>().SetDirection(4);
        }

        GetComponent<ShotGunDash>().enabled = true;
        
        
        Attack(movement.x > 0);
    }

    public override void SecondarySpell()
    {
        throw new System.NotImplementedException();
    }

    public override void Ultimate()
    {
        throw new System.NotImplementedException();
    }

    public override void MovementSpell()
    {
        Debug.Log("Got to movement spell melee");
        Vector2 movement = GetComponent<PlayerMovement>().GetMovement();

        if (movement.x > 0 && movement.x > movement.y) // Dash Right
        {
            rb.position = rb.position + Vector2.right * dashlength;
        }
        else if (movement.x < 0 && movement.x < movement.y) // Dash Left
        {
            rb.position = rb.position + Vector2.left * dashlength;
        }
        else if (movement.y > 0 && movement.y > movement.x) // Dash Up
        {
            rb.position = rb.position + Vector2.up * dashlength;
        }
        else if (movement.y < 0 && movement.y < movement.x) // Dash Down
        {
            rb.position = rb.position + Vector2.down * dashlength;
        }
    }
    
    
    public void Attack(bool right)
    {
        Debug.Log("Attack melee facing right :" + right);
        Vector3 pos = transform.position;
        if (right)
        {
            pos += transform.right * attackOffset.x;
        }
        else
        {
            pos += transform.right * -attackOffset.x;
        }
        pos += transform.up * attackOffset.y;
        
        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null && colInfo.CompareTag("Enemy"))
        {
            colInfo.GetComponent<EnemyHealth>().DamageEnemy(attackDamage);
            Debug.Log("Attack done");
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
