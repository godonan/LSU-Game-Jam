﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isAlive;
    public float moveSpeed;
    private Rigidbody playerRigidBody;
    private GameObject player;
    private Vector3 camLook;
    private AudioSource footsteps;
    private float camDistance;
    private Vector3 moveVelocity;
    private Vector3 moveInput;
    private Camera mainCamera;

    //private float stepTime = 0.4f;
    //private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        footsteps = player.GetComponent<AudioSource>();
        isAlive = true;
        //camLook = new Vector3(0, 12, 0);
        camLook = new Vector3(0, 10, -8);
        playerRigidBody = GetComponent<Rigidbody>();
        mainCamera = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive && Time.timeScale > 0)
        {
            moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            moveVelocity = moveInput * moveSpeed;

            Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane groundContact = new Plane(Vector3.up, Vector3.zero);
            float rayLength;

            if (groundContact.Raycast(cameraRay, out rayLength))
            {
                Vector3 pointToLook = cameraRay.GetPoint(rayLength);

                transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            }
        }
    }

    void FixedUpdate()
    {
        if (isAlive)
        {
            playerRigidBody.velocity = moveVelocity;
            if (moveVelocity != Vector3.zero)
            {
                if (!footsteps.isPlaying)
                {
                    footsteps.Play();
                }
            }
            else
            {
                if(footsteps.isPlaying)
                    footsteps.Stop();
            }
            
        }
        else
        {
            killSelf();
        }
    }

    void LateUpdate()
    {
        camDistance = Vector2.Distance(mainCamera.transform.position, playerRigidBody.transform.position);
        
        mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, playerRigidBody.transform.position + camLook, moveSpeed * camDistance * Time.deltaTime);
    }
    
    void killSelf()
    {
        Debug.Log("Dying.");

        //Renderer[] renders = player.transform.Find("PlayerMesh").GetComponentsInChildren<Renderer>();
        //foreach (Renderer render in renders)
        //    render.enabled = false;
        //this.enabled = false;
        player.SetActive(false);
    }
}
