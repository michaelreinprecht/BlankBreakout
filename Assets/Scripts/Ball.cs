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
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            //SoundManager.Instance.PlaySound("PaddleHit");
        }
        if (collision.gameObject.CompareTag("BrickA") || collision.gameObject.CompareTag("BrickB") || collision.gameObject.CompareTag("BrickC"))
        {
            //SoundManager.Instance.PlaySound("BrickHit1");
        }
        if (collision.gameObject.CompareTag("Border"))
        {
            //SoundManager.Instance.PlaySound("WallHit");
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
