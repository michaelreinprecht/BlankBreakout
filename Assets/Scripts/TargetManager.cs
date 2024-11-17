using System;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField]
    public GameObject targetBoxPrefab;

    public void UpdateTargets(List<int> targets)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var target in targets)
        {
            GameObject newTargetBox = Instantiate(targetBoxPrefab, transform);
            TargetBoxScript targetBoxScript = newTargetBox.GetComponent<TargetBoxScript>();
            if (targetBoxScript != null)
            {
                targetBoxScript.SetContent(target.ToString());
            }
        }
    }
}
