using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float movespeed ;
    private Rigidbody2D rb;
    private Transform target;
    [SerializeField] private int damage;
    private PhotonView PV;
    private float time;
    private float destroyTime = 3f;
    protected SpriteRenderer sprite;
    
    // Start is called before the first frame update
    void Start()
    {
        sprite = sprite = GetComponent<SpriteRenderer>();
        time = Time.time;
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        target = SelectTarget();
        Transform center = target.transform.GetChild(0).gameObject.transform;
        if (center != null)
        {
            Vector2 moveDirection = (center.position - transform.position).normalized * movespeed;
            if (moveDirection.x < 0)
                sprite.flipX = true;
            rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
        }
        else
        {
            if (PhotonNetwork.IsMasterClient || PV.AmOwner || PV.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    void Update()
    {
        if (Time.time > time + destroyTime)
            if (PV && ( PV.IsMine))
                PhotonNetwork.Destroy(this.gameObject);
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
                if (Vector2.Distance(transform.position, target.position) > //Select the closest player
                    Vector2.Distance(transform.position, target2.position))
                    target = target2;
            }
        }

        return target;
    }


    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.tag.Equals("Droid") && !col.gameObject.tag.Equals("SpawnZone"))
        {
            if (col.GetComponent<Health>() != null)
                col.GetComponent<Health>().DamagePlayer(damage);
            if (PhotonNetwork.IsMasterClient || PV.AmOwner || PV.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
