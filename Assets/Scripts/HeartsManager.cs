using UnityEngine;

public class HeartsManager : MonoBehaviour
{
    [SerializeField]
    public GameObject fullHeartPrefab;
    [SerializeField]
    public GameObject MissingHeartPrefab;

    public int? numberOfHearts;

    public void UpdateHearts(int numberOfRemainingHearts)
    {
        if (numberOfHearts == null)
        {
            numberOfHearts = numberOfRemainingHearts;
        }

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < numberOfRemainingHearts; i++)
        {
            Instantiate(fullHeartPrefab, transform);
        }

        for (int i = numberOfRemainingHearts; i < numberOfHearts; i++)
        {
            Instantiate(MissingHeartPrefab, transform);
        }
    }
}
