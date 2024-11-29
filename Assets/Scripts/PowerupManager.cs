using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    private static PowerupManager _instance;
    private List<GameObject> powerups;
    private GameObject ball;
    private GameObject paddle;

    public static PowerupManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<PowerupManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new("PowerupManager");
                    _instance = singletonObject.AddComponent<PowerupManager>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetPowerups(List<GameObject> gameObjects)
    {
        powerups = gameObjects;
        ball = GameObject.Find("BouncingBall(Clone)");
        paddle = GameObject.Find("Paddle");
    }

    public GameObject GetRandomPowerup()
    {
        if (powerups.Count == 0)
        {
            return null;
        }

        int index = Random.Range(0, powerups.Count);
        return powerups[index];
    }

    public void Activate(PowerupTypes type, float value, int duration)
    {
        switch (type)
        {
            case PowerupTypes.Speed:
                StartCoroutine(ActivateSpeedPowerup(value, duration));
                break;
            case PowerupTypes.PaddleWidth:
                StartCoroutine(ActivatePaddleStretching(value, duration));
                break;
            default:
                break;
        }
    }

    private IEnumerator ActivateSpeedPowerup(float value, int duration)
    {
        BallManager ballManager = ball.GetComponent<BallManager>();

        ballManager.UpdateBallSpeed(value);

        yield return new WaitForSeconds(duration);

        ballManager.UpdateBallSpeed(1 / value);
    }

    private IEnumerator ActivatePaddleStretching(float value, int duration)
    {
        Vector3 originalScale = paddle.transform.localScale;

        paddle.transform.localScale = new Vector3(originalScale.x * value, originalScale.y, originalScale.z);

        yield return new WaitForSeconds(duration);

        paddle.transform.localScale = originalScale;
    }
}
