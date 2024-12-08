using UnityEngine;
using TMPro;

public class HighscoreData : MonoBehaviour
{
    public static HighscoreData Instance { get; private set; }

    [SerializeField]
    private TMP_Text averageTimeTextAddition;
    [SerializeField]
    private TMP_Text bestTimeTextAddition;
    [SerializeField]
    private TMP_Text gamesPlayedTextAddition;
    [SerializeField]
    private TMP_Text averageTimeTextSubtraction;
    [SerializeField]
    private TMP_Text bestTimeTextSubtraction;
    [SerializeField]
    private TMP_Text gamesPlayedTextSubtraction;
    [SerializeField]
    private TMP_Text averageTimeTextMultiplication;
    [SerializeField]
    private TMP_Text bestTimeTextMultiplication;
    [SerializeField]
    private TMP_Text gamesPlayedTextMultiplication;

    private float averageTimeAddition;
    private float bestTimeAddition;
    private int gamesPlayedAddition;
    private float averageTimeSubtraction;
    private float bestTimeSubtraction;
    private int gamesPlayedSubtraction;
    private float averageTimeMultiplication;
    private float bestTimeMultiplication;
    private int gamesPlayedMultiplication;

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate instances
        }
    }

    private void Start()
    {
        LoadHighScores();
        SetTextFields();
    }

    public void SaveHighScore(string level, float newTime)
    {
        // Load current data
        float currentAverage = PlayerPrefs.GetFloat($"{level}_AverageTime", 0);
        float currentBest = PlayerPrefs.GetFloat($"{level}_BestTime", float.MaxValue);
        int currentGamesPlayed = PlayerPrefs.GetInt($"{level}_GamesPlayed", 0);

        // Update data
        currentGamesPlayed++;
        currentAverage = (currentAverage * (currentGamesPlayed - 1) + newTime) / currentGamesPlayed;
        currentBest = Mathf.Min(currentBest, newTime);

        // Save updated data
        PlayerPrefs.SetFloat($"{level}_AverageTime", currentAverage);
        PlayerPrefs.SetFloat($"{level}_BestTime", currentBest);
        PlayerPrefs.SetInt($"{level}_GamesPlayed", currentGamesPlayed);
    }

    public void LoadHighScores()
    {
        // Load Addition data
        averageTimeAddition = PlayerPrefs.GetFloat("Addition_AverageTime", 0);
        bestTimeAddition = PlayerPrefs.GetFloat("Addition_BestTime", 0);
        gamesPlayedAddition = PlayerPrefs.GetInt("Addition_GamesPlayed", 0);

        // Load Subtraction data
        averageTimeSubtraction = PlayerPrefs.GetFloat("Subtraction_AverageTime", 0);
        bestTimeSubtraction = PlayerPrefs.GetFloat("Subtraction_BestTime", 0);
        gamesPlayedSubtraction = PlayerPrefs.GetInt("Subtraction_GamesPlayed", 0);

        // Load Multiplication data
        averageTimeMultiplication = PlayerPrefs.GetFloat("Multiplication_AverageTime", 0);
        bestTimeMultiplication = PlayerPrefs.GetFloat("Multiplication_BestTime", 0);
        gamesPlayedMultiplication = PlayerPrefs.GetInt("Multiplication_GamesPlayed", 0);
    }

    public void SetTextFields()
    {
        // Set Addition data
        averageTimeTextAddition.text = FormatTime(averageTimeAddition);
        bestTimeTextAddition.text = FormatTime(bestTimeAddition);
        gamesPlayedTextAddition.text = gamesPlayedAddition == 0 ? "-" : gamesPlayedAddition.ToString();

        // Set Subtraction data
        averageTimeTextSubtraction.text = FormatTime(averageTimeSubtraction);
        bestTimeTextSubtraction.text = FormatTime(bestTimeSubtraction);
        gamesPlayedTextSubtraction.text = gamesPlayedSubtraction == 0 ? "-" : gamesPlayedSubtraction.ToString();

        // Set Multiplication data
        averageTimeTextMultiplication.text = FormatTime(averageTimeMultiplication);
        bestTimeTextMultiplication.text = FormatTime(bestTimeMultiplication);
        gamesPlayedTextMultiplication.text = gamesPlayedMultiplication == 0 ? "-" : gamesPlayedMultiplication.ToString();
    }

    private string FormatTime(float time)
    {
        if (time <= 0 || time == float.MaxValue)
        {
            return "-";
        }
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}
