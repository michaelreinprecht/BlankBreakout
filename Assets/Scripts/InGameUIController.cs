using System;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIController: MonoBehaviour
{
    [SerializeField]
    private HeartsManager heartsManager;
    [SerializeField]
    private TargetManager targetManager;
    [SerializeField]
    private TargetManager nonTargetManager;

    public Action AllLivesLost;
    public Action TargetsCleared;
    public Action NonTargetCleared;

    public void Start()
    {
        heartsManager.AllLivesLost += () =>
        {
            AllLivesLost.Invoke();
        };
        targetManager.TargetsCleared += () =>
        {
            TargetsCleared.Invoke();
        };
        nonTargetManager.TargetsCleared += () =>
        {
            NonTargetCleared.Invoke();
        };
    }

    public void SetLives(int lives)
    {
        heartsManager.SetHearts(lives);
    }

    public void RemoveLive()
    {
        heartsManager.RemoveHeart();
    }

    public void SetTargets(List<int> numbers)
    {
        targetManager.SetTargets(numbers);
    }

    public bool ContainsTarget(int number)
    {
        return targetManager.Contains(number);
    }

    public void SetNonTargets(List<int> numbers)
    {
        nonTargetManager.SetTargets(numbers);
    }

    public bool ContainsNonTarget(int number)
    {
        bool hit = nonTargetManager.Contains(number);

        if (hit)
        {
            RemoveLive();
        }

        return hit;
    }
}
