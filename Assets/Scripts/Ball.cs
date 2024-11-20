using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DeathZone"))
        {
            gameController.LooseALife();
            gameController.ResetBall();
        }
    }
}
