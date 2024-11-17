using TMPro;
using UnityEngine;

public class InGameUIController: MonoBehaviour
{
    [SerializeField]
    private TMP_Text livesTextInfo;

    public void UpdateLives(int lives)
    {
        livesTextInfo.text = "Lives: " + lives.ToString();
    }
}
