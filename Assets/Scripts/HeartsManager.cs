using System;
using UnityEngine;

public class HeartsManager : MonoBehaviour
{
    [SerializeField]
    public GameObject fullHeartPrefab;
    [SerializeField]
    public GameObject MissingHeartPrefab;

    public int numberOfMaxHearts;
    public int numberOfHearts;
    public Action AllLivesLost;

    public void SetHearts(int numberOfHearts)
    {
        this.numberOfMaxHearts = numberOfHearts;
        this.numberOfHearts = numberOfHearts;

        UpdateUi();
    }

    public void RemoveHeart()
    {
        SoundManager.Instance.PlaySound("LifeLost", 1f);
        numberOfHearts--;
        UpdateUi();
        
        if (numberOfHearts == 0)
        {
            AllLivesLost.Invoke();
        }
    }

    private void UpdateUi()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < numberOfHearts; i++)
        {
            Instantiate(fullHeartPrefab, transform);
        }

        for (int i = numberOfHearts; i < numberOfMaxHearts; i++)
        {
            Instantiate(MissingHeartPrefab, transform);
        }
    }
}
