using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunDash : MonoBehaviour
{
    private Rigidbody2D rb;
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private int direction;

    public void SetDirection(int i)
    {
        direction = i;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dashTime = startDashTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (dashTime <= 0)
        {
            direction = 0;
            dashTime = startDashTime;
            rb.velocity = Vector2.zero;
            GetComponent<PlayerMovement>().enabled = true;
            GetComponent<ShotGunDash>().enabled = false;
        }
        else
        {
            dashTime -= Time.deltaTime;
            
            if (direction == 1)
            {
                rb.velocity = Vector2.left * dashSpeed;
            }
            else if (direction == 2)
            {
                rb.velocity = Vector2.right * dashSpeed;
            }
            else if (direction == 3)
            {
                rb.velocity = Vector2.up * dashSpeed;
            }
            else if (direction == 4)
            {
                rb.velocity = Vector2.down * dashSpeed;
            }
        }
    }
}
