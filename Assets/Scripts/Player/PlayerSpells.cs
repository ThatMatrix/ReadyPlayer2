using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Photon.Pun;

public abstract class PlayerSpells : MonoBehaviour
{
    [SerializeField] new private string name;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] private Animator animator;
    private PhotonView PV;
    [SerializeField] private int maxHealth;
    private int health;
    public Vector2 mousePos;
    public Camera cam;
    public Vector2 lookDir;
    public GameObject firePoint;
    public Rigidbody2D rbFirePoint;
    public Vector2 offset;
    protected Animator _animator;

    public GameObject firePointLeft;
    public GameObject firePointRight;
    public GameObject centre;
    public bool right;
    
    
    public float cooldown1;
    public float nextSpell1;
    public float cooldown2;
    public float nextSpell2;
    public float cooldownM;
    public float nextMovement;
    public float cooldownU;
    public float nextUltimate;


    protected void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        rbFirePoint = firePoint.GetComponent<Rigidbody2D>();
        cam = Camera.main;
        health = maxHealth;
        offset = firePoint.transform.position - centre.transform.position;
        SetCooldowns();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            if (cam == null)
            {
                cam = Camera.main;
            }
            else
            {
                mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                
                ChooseFirePoint();
                
                
                if (Input.GetButtonDown("Fire1") && Time.time > nextSpell1)
                {
                    MainSpell();
                    nextSpell1 = Time.time + cooldown1;
                }

                if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && Time.time > nextMovement)
                {
                    MovementSpell();
                    nextMovement = Time.time + cooldownM;
                }

                if (Input.GetButtonDown("Fire2") && Time.time > nextSpell2)
                {
                    SecondarySpell();
                    nextSpell2 = Time.time + cooldown2;
                }

                if (Input.GetKeyDown(KeyCode.E) && Time.time > nextUltimate)
                {
                    Ultimate();
                                                                             nextUltimate = Time.time + cooldownU;
                }
            }
        }
    }

    public void FixedUpdate()
    {
        lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        firePointLeft.transform.position = (Vector2) centre.transform.position + offset;
        firePointRight.transform.position = (Vector2) centre.transform.position - offset;
        rbFirePoint.rotation = angle;
    }

    public abstract void MainSpell();

    public abstract void SecondarySpell();

    public abstract void MovementSpell();

    public abstract void Ultimate();
    public abstract void SetCooldowns();

    private void ChooseFirePoint()
    {
        if (Vector2.Distance(mousePos, firePointLeft.transform.position) < Vector2.Distance(mousePos, firePointRight.transform.position))
        {
            right = true;
            firePoint = firePointLeft;
            rbFirePoint = firePointLeft.GetComponent<Rigidbody2D>();
        }
        else
        {
            right = false;
            firePoint = firePointRight;
            rbFirePoint = firePointRight.GetComponent<Rigidbody2D>();
        }
    }
}
