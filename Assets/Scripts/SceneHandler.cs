using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SwitchToScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetSharedMaxTargetValue(string newValue)
    {
        if (int.TryParse(newValue, out int parsedValue))
        {
            if (Mathf.Abs(parsedValue) >= 20)
            {
                SharedData.MaxTargetValue = Mathf.Abs(parsedValue);
                Debug.Log($"SharedData.MaxTargetValue updated to: {SharedData.MaxTargetValue}");
            } else
            {
                Debug.Log($"MaxTargetValue to small, set to minimum of 20");
                SharedData.MaxTargetValue = 20;
            }
        }
    }
}
