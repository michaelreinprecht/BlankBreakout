using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    [SerializeField] private float paddleSpeed = 10f;

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0f) //Pausing game
        {
            return;
        } else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) 
        {
            float horizontal = Input.GetAxis("Horizontal");
            Vector3 newPosition = transform.position + new Vector3(horizontal * paddleSpeed * Time.deltaTime, 0, 0);

            //TODO: set bounds for border
            transform.position = newPosition;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody ballRb = collision.rigidbody;

        if (ballRb != null )
        {
            //Get the exact point where the ball hit the paddle
            Vector3 hitpoint = collision.contacts[0].point;

            float hitfactor = (hitpoint.x - transform.position.x) / transform.localScale.x;

            Vector3 newDirection = new Vector3(hitfactor, 1, 0).normalized;
            ballRb.velocity = newDirection;
        }
    }
}
