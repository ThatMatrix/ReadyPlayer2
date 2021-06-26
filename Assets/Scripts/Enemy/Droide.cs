using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droide : MonoBehaviour
{
    [SerializeField] private GameObject shoot;
    private float fireRate;
    private float nextFire;
    public bool isFlipped = false;
    [SerializeField] private Transform spawn;
    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        fireRate = 5f;
        nextFire = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        target = SelectTarget();
        if (target == null)
            return;
        LookAtPlayer();
        CheckIfShoot();
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
    
    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > target.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < target.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }
    void CheckIfShoot()
    {
        if (Time.time > nextFire)
        {
            Instantiate(shoot, spawn.position, Quaternion.identity);
            nextFire = Time.time + fireRate;
        }
    }
}
