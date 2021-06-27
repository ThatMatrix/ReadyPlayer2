using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Pun;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class Casper : MonoBehaviour
{
    public static Casper Cs;
    
    public Animator animator;
    public GameObject firePoint;
    public Rigidbody2D rbFirePoint;

    private float timeBtwShots;
    public float startTimeBtwShots;
    public GameObject target;

    public bool hasTP;
    public GameObject[] spots;
    private bool[] rotate = { true,true,false,false,true};
    GameObject newPos;
    private SpriteRenderer sprite;

    private bool hasTargetDash = false;
    public bool arrived = false;
    private GameObject targetDash;
    [SerializeField] private float dashSpeed;
    private int rotationAfter;
    
    public GameObject firePointRight;
    public Rigidbody2D rbFirePointRight;
    public GameObject firePointLeft;
    public Rigidbody2D rbFirePointLeft;

    public int damage;

    private PhotonView PV;
    private bool died;
    
    private void OnEnable()
    {
        if (Cs == null)
        {
            Cs = this;
        }
        else
        {
            if (Cs != this)
            {
                PhotonNetwork.Destroy(Cs.gameObject);
            }
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("CasperTheme");
        spots = FindObjectOfType<CasperSpotReference>().spots;
        timeBtwShots = startTimeBtwShots;
        sprite = gameObject.GetComponent<SpriteRenderer>();
        animator.SetBool("Dead", false);
        //offset = firePoint.transform.position - animator.GetComponent<Transform>().position;
        newPos = spots[2];
        PV = GetComponent<PhotonView>();
        FindObjectOfType<AudioManager>().Stop("UsualStage");
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            if (Vector2.Distance(player.transform.position, gameObject.transform.position) <= 1f)
            { 
                player.GetComponent<Health>().DamagePlayer(damage);
            }
        }
        //Debug.Log("Position player 0: " + players[0].transform.position.x + "-" + players[0].transform.position.y);
        
        
        if (!animator.GetBool("Dead") && animator.GetBool("Tir"))
        {
            hasTargetDash = false;
            arrived = false;
            if (timeBtwShots <= 0)
            {
                players = GameObject.FindGameObjectsWithTag("Player");
                target = players[ Random.Range(0, players.Length)];
                    
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Tear(boss shot)"),
                    firePoint.transform.position, quaternion.identity);

                timeBtwShots = startTimeBtwShots;
            }
            else
            {
                timeBtwShots -= Time.deltaTime;
            }
        }

        else if (!animator.GetBool("Dead") && animator.GetBool("Tp") && !hasTP)
        {
            int i;
            GameObject tmp = spots[i = Random.Range(0, spots.Length)];
            while (newPos == tmp)
            {
                tmp = spots[i = Random.Range(0, spots.Length)];
            }

            if (rotate[i])
            {
                firePoint = firePointRight;
                rbFirePoint = rbFirePointRight;
            }
            else
            {
                firePoint = firePointLeft;
                rbFirePoint = rbFirePointLeft;
            }
            
            sprite.flipX = rotate[i];
            newPos = tmp;
            gameObject.transform.position = newPos.transform.position;
            
            
            hasTP = true;
        }

        else if (!animator.GetBool("Dead") && animator.GetBool("Dash"))
        {
            if (!hasTargetDash)
            {
                int i;
                GameObject tmp = spots[i = Random.Range(0, spots.Length)];
                while (newPos == tmp)
                {
                    tmp = spots[i = Random.Range(0, spots.Length)];
                }

                rotationAfter = i;
                targetDash = tmp;
                hasTargetDash = true;
            }
            
            transform.position = Vector2.MoveTowards(transform.position, targetDash.transform.position,
                dashSpeed * Time.deltaTime);
            if (transform.position.x == targetDash.transform.position.x && transform.position.y == targetDash.transform.position.y)
            {
                newPos = targetDash;
                if (rotate[rotationAfter])
                {
                    firePoint = firePointRight;
                    rbFirePoint = rbFirePointRight;
                }
                else
                {
                    firePoint = firePointLeft;
                    rbFirePoint = rbFirePointLeft;
                }

                sprite.flipX = rotate[rotationAfter];
                arrived = true;
            }
            
        }
        
    }
}
