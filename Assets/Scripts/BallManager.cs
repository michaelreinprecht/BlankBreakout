using UnityEngine;

public class BallManager : MonoBehaviour
{
    [SerializeField] 
    private float ballSpeed = 10f;
    Rigidbody ballRb;

    private void Awake()
    {
        ballRb = GetComponent<Rigidbody>(); //This line throws error   
    }
    
    private void Start()
    {
        Vector3 startVector = Vector3.up * ballSpeed;
        ballRb.AddForce(startVector);
    }

    private void FixedUpdate()
    {
        ballRb = GetComponent<Rigidbody>();
        ballRb.velocity = ballRb.velocity.normalized * ballSpeed;
    }

    public float GetBallSpeed()
    {
        return ballSpeed;
    }

    public void UpdateBallSpeed(float factor)
    {
        ballSpeed *= factor;
    }

    public void ResetBallPhysics()
    {
        ballRb.velocity = Vector3.zero;
        Vector3 startVector = Vector3.up * ballSpeed;
        ballRb.AddForce(startVector);
    }
}
