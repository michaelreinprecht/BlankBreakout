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

    public void UpdateLives(int lives)
    {
        heartsManager.UpdateHearts(lives);
    }

    public void UpdateTarget(List<int> numbers)
    {
        targetManager.UpdateTargets(numbers);
    }

    public void UpdateNonTarget(List<int> numbers)
    {
        nonTargetManager.UpdateTargets(numbers);
    }
}
