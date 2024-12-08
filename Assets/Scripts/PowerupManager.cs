using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    private static PowerupManager _instance;
    private List<GameObject> powerups;
    private GameObject ball;
    private GameObject paddle;
    private List<PowerupTypes> activePowerups = new();

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
        ball = GameObject.FindGameObjectWithTag("Ball");
        paddle = GameObject.FindGameObjectWithTag("Paddle");
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

    public void Activate(PowerupTypes type, float value, int duration, bool stackable)
    {
        if (!stackable && activePowerups.Contains(type))
        {
            return;
        }

        StartCoroutine(HandlePowerup(type, value, duration));
    }

    private IEnumerator HandlePowerup(PowerupTypes type, float value, int duration)
    {
        activePowerups.Add(type);

        switch (type)
        {
            case PowerupTypes.Speed:
                yield return StartCoroutine(ActivateSpeedPowerup(value, duration));
                break;
            case PowerupTypes.PaddleWidth:
                yield return StartCoroutine(ActivatePaddleStretching(value, duration));
                break;
            default:
                Debug.LogWarning($"Unhandled powerup type: {type}");
                break;
        }

        activePowerups.Remove(type);
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
        Vector3 targetScale = new Vector3(originalScale.x * value, originalScale.y, originalScale.z);
        float scaleTimer = 0f;
        float scaleDuration = 1f;

        while (scaleTimer < scaleDuration)
        {
            scaleTimer += Time.deltaTime;
            if (paddle != null)
            {
                paddle.transform.localScale = Vector3.Lerp(originalScale, targetScale, scaleTimer / scaleDuration);
            }
            yield return null;
        }

        paddle.transform.localScale = targetScale;

        yield return new WaitForSeconds(duration);

        scaleTimer = 0f;
        while (scaleTimer < scaleDuration)
        {
            scaleTimer += Time.deltaTime;
            if (paddle != null)
            {
                paddle.transform.localScale = Vector3.Lerp(targetScale, originalScale, scaleTimer / scaleDuration);
            }
            yield return null;
        }

        paddle.transform.localScale = originalScale;
    }
}
