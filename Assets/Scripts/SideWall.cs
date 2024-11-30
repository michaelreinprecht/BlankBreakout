using UnityEngine;

public class WallCollision : MonoBehaviour
{
    [SerializeField] private float kickForce = 2f;         // Kick force to apply if the angle is too low

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody ballRb = collision.gameObject.GetComponent<Rigidbody>();

            Vector3 ballVelocity = ballRb.velocity;
            Vector3 wallNormal = collision.contacts[0].normal;

            // Use the dot product to detect small angle (if angle between normal and velocity is small, they're almost parallel)
            float dotProduct = Vector3.Dot(ballVelocity.normalized, wallNormal);

            if (Mathf.Abs(dotProduct) >= 0.95)
            {
                Vector3 kickDirection;
                int random = Random.Range(0, 1);
                if (random == 0) kickDirection = Vector3.up;
                else kickDirection = Vector3.down;
                ballRb.AddForce(kickDirection * kickForce, ForceMode.Impulse); //Apply a little kick up or downwards to the ball.

                Debug.Log("Small angle detected (dot product: " + dotProduct + "). Kick applied with force: " + kickForce);
            }
        }
    }
}
