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
            SharedData.MaxTargetValue = Mathf.Abs(parsedValue);
            Debug.Log($"SharedData.MaxTargetValue updated to: {SharedData.MaxTargetValue}");
        }
    }
}
