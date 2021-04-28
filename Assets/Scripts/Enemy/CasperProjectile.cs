using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CasperProjectile : MonoBehaviour
{
    public float speed;
    public int damage;
    

    private PhotonView PV;
    private Transform player;
    private Vector2 target;
    
    
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (FindObjectOfType<Casper>().target == null)
        {
            if (PV && ( PV.IsMine))
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
            return;
        }

        player = FindObjectOfType<Casper>().target.transform;
        target = new Vector2(player.position.x, player.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            if (PV && ( PV.IsMine))
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Health>() != null)
        {
            collision.GetComponent<Health>().DamagePlayer(damage);
            
        }
        if (PV && ( PV.IsMine))
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
