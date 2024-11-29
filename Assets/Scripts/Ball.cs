using UnityEngine;
using UnityEngine.Playables;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;
    [SerializeField]
    private PlayableDirector bottomCollisionDirector;
    [SerializeField]
    private PlayableDirector topCollisionDirector;
    [SerializeField]
    private PlayableDirector rightCollisionDirector;
    [SerializeField]
    private PlayableDirector leftCollisionDirector;

    // Start is called before the first frame update
    void Start()
    {
        GameObject controllerObject = GameObject.Find("GameController");
        if (controllerObject != null) //Fixed a bug for Melih, where game would crash if ball collided for the first time with a brick
        {
            gameController = controllerObject.GetComponent<GameController>();
        }
        else
        {
            Debug.LogError("GameController not found in the scene!");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayCollisionAnimation(collision);

        if (collision.gameObject.CompareTag("Paddle"))
        {
            SoundManager.Instance.PlaySound("PaddleHit");
        }
        if (collision.gameObject.CompareTag("BrickA") || collision.gameObject.CompareTag("BrickB") || collision.gameObject.CompareTag("BrickC"))
        {
            SoundManager.Instance.PlaySound("BrickHit1");
        }
        if (collision.gameObject.CompareTag("Border"))
        {
            SoundManager.Instance.PlaySound("WallHit");
        }   
    }

    private void PlayCollisionAnimation(Collision collision)
    {
        Vector3 collisionNormal = collision.contacts[0].normal;

        float absX = Mathf.Abs(collisionNormal.x);
        float absY = Mathf.Abs(collisionNormal.y);

        if (absY > absX) //Vertical collision
        {
            if (collisionNormal.y > 0) // Bottom collision
                bottomCollisionDirector.Play();
            else if (collisionNormal.y < 0) // Top collision
                topCollisionDirector.Play();
        }
        else
        { //Horizontal collision
            if (collisionNormal.x < 0) // Right collision
                rightCollisionDirector.Play();
            else if (collisionNormal.x > 0) // Left collision
                leftCollisionDirector.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DeathZone"))
        {
            gameController.LooseALife();
            gameController.ResetBall();
        }
    }
}
