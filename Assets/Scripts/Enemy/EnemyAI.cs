using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;

    protected Transform target;
    [SerializeField] protected float distance;
    
    [SerializeField] private float resetSlow;
    private float previousMS = 7f;
    [SerializeField] private bool toReset = false;

    protected SpriteRenderer sprite;

    public float cooldownAttack;
    private float nextAttack;

    public int damage;
    
    protected float nextWaypointDistance = 3f;
    protected Path path;
    protected int currentWaypoint = 0;
    protected bool reachEndOfPath = false;

    protected Seeker seeker;

    protected Rigidbody2D rb;
    
    public bool isFlipped = false;
    
    // Start is called before the first frame update
    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        Introduction();
    }

    protected virtual void FixedUpdate()
    {
        target = SelectTarget();
        
        if (toReset && Time.time > resetSlow)
        {
            moveSpeed = previousMS;
            toReset = false;
        }
        
        // if (Vector2.Distance(gameObject.transform.position, target.position) <= 2.5f && nextAttack < Time.time)
        // {
        //     target.GetComponent<Health>().DamagePlayer(damage);
        //     nextAttack = Time.time + cooldownAttack;
        // }
        
        if (path == null)
            return;
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachEndOfPath = true;
            return;
        }
        else
            reachEndOfPath = false;
        
        Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * moveSpeed * 25 * Time.deltaTime;
        

        rb.AddForce(force);
        
        float wayPointDistance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        
        LookAtPlayer();
        
        if (wayPointDistance < nextWaypointDistance)
            currentWaypoint++;


    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > path.vectorPath[currentWaypoint].x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < path.vectorPath[currentWaypoint].x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    Transform SelectTarget()
    {
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        Transform target = null;
        foreach (var player in Players)
        {
            if (target == null)
                target = player.GetComponent<Transform>();
            else
            {
                Transform target2 = player.GetComponent<Transform>();
                if (Vector2.Distance(transform.position, target.position ) > //Select the closest player
                    Vector2.Distance(transform.position, target2.position))
                    target = target2;
            }
        }
        return target;
    }
    
    protected virtual void Introduction()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }
    protected virtual void UpdatePath()
    {
        if (target != null && seeker.IsDone())
            seeker.StartPath(transform.position, target.position, OnPathComplete);
    }
    protected void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0; 
        }
    }
    
    
    protected bool Turn(float gameObjectX, float targetX)
    {
        return gameObjectX < targetX;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!toReset)
        {
            if (collision.GetComponent<IcePool>() != null)
            {
                Slow();
            }
        }
        else
        {
            resetSlow = Time.time + 5f;
        }
    }
    
    void Slow()
    {
        moveSpeed = moveSpeed - 5f;
        resetSlow = Time.time + 5f;
        toReset = true;
    }
}
