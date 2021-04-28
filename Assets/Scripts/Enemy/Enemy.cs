using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    [SerializeField] private string enemyName;
    [SerializeField] protected float moveSpeed;
    

    protected Transform target;
    [SerializeField] protected float distance;
    
    //Slow attributes
    [SerializeField] private float resetSlow;
    private float previousMS = 7f;
    [SerializeField] private bool toReset = false;
    
    //Molly attributes
    private float resetBurn;
    private bool burning = false;

    protected SpriteRenderer sprite;

    public float cooldownAttack;
    private float nextAttack;

    public int damage;
    // Start is called before the first frame update
    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        Introduction();
    }

    void Update()
    {
        target = SelectTarget();
        Move();
        if (target != null)
        {
            if (Vector2.Distance(gameObject.transform.position, target.position) <= 2.5f && nextAttack < Time.time)
            {
                target.GetComponent<Health>().DamagePlayer(damage);
                nextAttack = Time.time + cooldownAttack;
            }
        }
        
        if (toReset && Time.time > resetSlow)
        {
            moveSpeed = previousMS;
            toReset = false;
        }

        if (burning)
        {
            if (Time.time > resetBurn)
            {
                burning = false;
            }
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
        Debug.Log(($"name:{enemyName}, speed:{moveSpeed}"));
    }

    protected virtual void Move()
    {
        if (target != null && Vector2.Distance(transform.position, target.position) < distance)
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }
    
    protected bool Turn(float gameObjectX, float targetX)
    {
        return gameObjectX < targetX;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IcePool>() != null)
        {
            if (!toReset)
            {
                Slow();
            }
            else
            {
                resetSlow = Time.time + 5f;
            }
        }
    }
    
    void Slow()
    {
        moveSpeed = moveSpeed - 5f;
        resetSlow = Time.time + 5f;
        toReset = true;
    }
    
}
