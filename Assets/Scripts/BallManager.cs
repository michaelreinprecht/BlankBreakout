using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    [SerializeField] private float ballSpeed = 10f;
    Rigidbody ballRb;

    private void Start()
    {
        ballRb = GetComponent<Rigidbody>();
        Vector3 startVector = Vector3.down * ballSpeed;
        ballRb.AddForce(startVector);
    }

    private void FixedUpdate()
    {
        ballRb = GetComponent<Rigidbody>();
        ballRb.velocity = ballRb.velocity.normalized * ballSpeed;
    }
}
