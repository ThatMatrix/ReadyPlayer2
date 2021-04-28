using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lizard : EnemyAI
{
    public Transform leftPatrolPoint, rightPatrolPoint;
    private Transform targetPatrolPoint;
    public int lizardDamage;

    public float lizardCooldown;
    
    public float attackRange = 1f;
    public Vector3 attackOffset;
    public LayerMask attackMask;
    
    protected override void Introduction()
    {
        cooldownAttack = lizardCooldown;
        damage = lizardDamage;
        targetPatrolPoint = leftPatrolPoint;
        base.Introduction();
        Debug.Log("Lizard class call");
        
    }

    protected override void UpdatePath()
    {
        if (target == null)
        {
            return;
        }
        if (Vector2.Distance(transform.position, target.position) > distance) //Player out of range
        {
            if (Vector2.Distance(transform.position, leftPatrolPoint.position) < 2f)
            {
                targetPatrolPoint = rightPatrolPoint;
            }
            if (Vector2.Distance(transform.position, rightPatrolPoint.position) < 2f)
            {
                targetPatrolPoint = leftPatrolPoint;
            }
            
            if (seeker.IsDone())
                seeker.StartPath(transform.position, targetPatrolPoint.position, OnPathComplete);
        }

        else
        {
            base.UpdatePath();
            
        }
        
        
    }
    
    public void Attack()
    {
        //attackOffset.x *= sprite.flipX;
        if (sprite.flipX)
        {
            attackOffset.x = 3f;
        }
        else
        {
            attackOffset.x = -3f;
        }
        
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null && colInfo.CompareTag("Player"))
        {
            colInfo.GetComponent<Health>().DamagePlayer(damage);
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
