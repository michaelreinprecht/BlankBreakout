using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private PowerupTypes type;
    [SerializeField]
    private float value;
    [SerializeField]
    private int durationInSeconds;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DeathZone"))
        {
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Paddle"))
        {
            Destroy(gameObject);
            PowerupManager.Instance.Activate(type, value, durationInSeconds);
        }
    }
}
