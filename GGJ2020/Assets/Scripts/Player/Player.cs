﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController m_controller;
    public int playerNumber;
    public float playerSpeed = 12f;
    private float gravity = -9.81f;

    private Vector3 m_velocity;

    public Transform playerMesh;

    public float dashSpeed = 5f;
    public float dashTime = 0.2f;
    private bool m_isDash = false;
    private bool m_isCooldownDash = true;
    public float timeDashCooldown = 1f;
    private float m_lastX;
    private float m_lastZ;

    void Start()
    {
        m_controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Dash();
    }

    private void OnTriggerStay(Collider other)
    {

        if ((Input.GetKeyDown(KeyCode.F) && GetComponentInParent<Player>().playerNumber == 1) ||
            (Input.GetKeyDown(KeyCode.Return) && GetComponentInParent<Player>().playerNumber == 2))
        {
            if (other.CompareTag("Puddle") && GetComponent<Catch>().GetObjectInHand() == Catch.ObjectInHand.Mop)
            {
                Destroy(other.gameObject);
                GetComponent<Catch>().DestroyPickable();
            }

            if (other.gameObject.GetComponent<Flammable>() && GetComponent<Catch>().GetObjectInHand() == Catch.ObjectInHand.Extinguisher)
            {
                other.gameObject.GetComponent<Flammable>().SetIsOnFire(false);
                GetComponent<Catch>().DestroyPickable();
            }

            if (other.gameObject.GetComponent<LeverLight>())
            {
                other.gameObject.GetComponent<LeverLight>().PlayerActivate(playerNumber);
            }
        }
    }

    void Move()
    {
        float x = 0, z = 0;

        if (m_isDash)
        {
            Vector3 move = transform.right * m_lastX + transform.forward * m_lastZ;
            m_controller.Move(move * playerSpeed * dashSpeed * Time.deltaTime);
        }
        else
        {
            if (Input.GetJoystickNames().Length > playerNumber - 1 && !string.IsNullOrEmpty(Input.GetJoystickNames()[playerNumber - 1]))
            {
                x = Input.GetAxis("JoyXPlayer" + (playerNumber));
                z = Input.GetAxis("JoyYPlayer" + (playerNumber));
            }
            else
            {
                x = Input.GetAxis("HorizontalPlayer" + (playerNumber));
                z = Input.GetAxis("VerticalPlayer" + (playerNumber));
            }

            if (x != 0.0f || z != 0.0f)
            {
                playerMesh.eulerAngles = new Vector3(-90, 0, (Mathf.Atan2(x, z) * 180 / Mathf.PI) + 90);
            }

            Vector3 move = transform.right * x + transform.forward * z;
            m_controller.Move(move * playerSpeed * Time.deltaTime);

            m_lastX = x;
            m_lastZ = z;

            m_velocity.y += gravity * Time.deltaTime;
            m_controller.Move(m_velocity * Time.deltaTime);
        }
    }

    void Dash()
    {
        if (Input.GetButtonDown("DashPlayer" + playerNumber) && m_isCooldownDash)
        {

            m_isDash = true;
            m_isCooldownDash = false;
            StartCoroutine(MyDash());
            
        }
    }

    IEnumerator MyDash()
    {
        yield return new WaitForSeconds(dashTime);
        m_isDash = false;
        yield return new WaitForSeconds(timeDashCooldown);
        m_isCooldownDash = true;
    }

    
}