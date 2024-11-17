using TMPro;
using UnityEngine;

public class TargetBoxScript : MonoBehaviour
{
    [SerializeField]
    private TMP_Text textObject;

    public void SetContent(string content)
    {
        textObject.text = content;
    }
}
