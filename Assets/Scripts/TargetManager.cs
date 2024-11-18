using System;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField]
    public GameObject targetBoxPrefab;

    private List<int> targets;

    public Action TargetsCleared;

    public void SetTargets(List<int> targets)
    {
        this.targets = targets;
        UpdateUi();
    }

    public bool Contains(int number)
    {
        if (targets.Contains(number))
        {
            targets.Remove(number);
            UpdateUi();

            if (targets.Count == 0)
            {
                TargetsCleared?.Invoke();
            }

            return true;
        }
        return false;
    }

    private void UpdateUi()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var number in targets)
        {
            GameObject newTargetBox = Instantiate(targetBoxPrefab, transform);
            TargetBoxScript targetBoxScript = newTargetBox.GetComponent<TargetBoxScript>();
            if (targetBoxScript != null)
            {
                targetBoxScript.SetContent(number.ToString());
            }
        }
    }
}
