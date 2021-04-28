using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ExitGames.Client.Photon.StructWrapping;
using NUnit.Framework.Constraints;
using Photon.Pun;
using UnityEngine;
using Random = System.Random;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1000f; 
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    private Vector2 movement;
    private PhotonView PV;

    public bool HasCamera;

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public Vector2 GetMovement()
    {
        return movement;
    }

    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    void Start()
    {
        DontDestroyOnLoad(this);
        Debug.Log($"Added to don't destroy on load{this}");
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        HasCamera = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine) //Inputs
        {
            //Input.GetKeyDown(KeyCode.Escape)
            /*if (Input.GetKeyDown(KeyCode.Q))
            {
                movement.x = -1;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                movement.x = 1;
            }
            else
            {
                movement.x = 0;
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("Z key was pressed");
                movement.y = 1;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                movement.y = -1;
            }
            else
            {
                movement.y = 0;
            }*/

            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
            
        }
    }

    void FixedUpdate()
    {
        //Actual Movement
        if (PV.IsMine)
        {
            rb.velocity = movement.normalized * moveSpeed;
        }
    }
}