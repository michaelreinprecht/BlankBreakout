using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    [SerializeField]
    private int lives = 3;
    [SerializeField]
    private List<MathOperatorsEnum> useOperation = new() { MathOperatorsEnum.ADDITION, MathOperatorsEnum.SUBTRACTION};
    [SerializeField]
    private int overflowValue = 0;
    [SerializeField]
    private int minTargetValue = 0;
    [SerializeField]
    private int maxTargetValue = 20;
    [SerializeField] 
    private int paddleValue = 0;
    [SerializeField]
    private int numberOfTargets = 3;
    [SerializeField]
    private int numberOfNonTargets = 3;
    [SerializeField]
    private int powerupChance = 20;
    [SerializeField]
    private int brickMaxValueRatio = 4;
    [SerializeField]
    private int brickMaxValueFlat = 0;
    [SerializeField]
    private List<GameObject> usePowerup = new();
    [SerializeField]
    private Vector3 ballStartPosition;
    [SerializeField]
    private Vector3 paddleStartPosition;
    [SerializeField]
    private PaddleController paddle;
    [SerializeField]
    private Canvas gameOverScreen;
    [SerializeField]
    private Canvas LevelWonScreen;
    [SerializeField]
    private InGameUIController inGameUIController;

    [SerializeField]
    private int minimumBrickNumber = 6;

    [SerializeField]
    private List<Brick> bricksRowA = new();
    [SerializeField]
    private List<Brick> bricksRowB = new();
    [SerializeField]
    private List<Brick> bricksRowC = new();
    [SerializeField]
    private List<Brick> bricks = new();
    [SerializeField]
    private TMP_Text levelEndTimeGameOver;
    [SerializeField]
    private TMP_Text levelEndTimeLevelWon;

    private string path;

    // Start is called before the first frame update
    void Start()
    {
        SetupBall();
        SetupBricks();
        //InvokeRepeating("CheckForEndOfGame", 20, 3);
        InvokeRepeating("CheckForLowBrickNumber", 20, 3);
        InitGameObjects();
        InitInGameUIController();
        PowerupManager.Instance.SetPowerups(usePowerup);
        path = PlayerSave.GetFilePath();
    }

    public void InitGameObjects()
    {
        paddle.SetValue(paddleValue, maxTargetValue, minTargetValue, overflowValue);
        paddle.ValueChanged += CheckTargets;
    }

    public void InitInGameUIController()
    {
        inGameUIController.SetLives(lives);
        inGameUIController.AllLivesLost += GameOver;
        inGameUIController.TargetsCleared += LevelWon;
        inGameUIController.NonTargetCleared += () => { };
        inGameUIController.StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CheckTargets()
    {
        if (inGameUIController.ContainsTarget(paddle.GetValue()))
        {
            paddle.LogTargetHit();
            SoundManager.Instance.PlaySound("TargetReached", 1f);
        }
        if (inGameUIController.ContainsNonTarget(paddle.GetValue()))
        {
            paddle.LogNonTargetHit();
            SoundManager.Instance.PlaySound("nonTargetReached", 1f);
        }
    }

    public void GameOver()
    {
        gameOverScreen.GetComponent<Canvas>().enabled = true;
        Time.timeScale = 0;
        inGameUIController.StopTimer();
        levelEndTimeGameOver.text = inGameUIController.GetTimeAsString();
    }

    public void LooseALife()
    {
        inGameUIController.RemoveLive();
    }

    public void SetupBall()
    {
        if (GameObject.FindGameObjectWithTag("Ball"))
        {
            GameObject.FindGameObjectWithTag("Ball").transform.position = ballStartPosition;
            BallManager ballManager = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallManager>();
            ballManager.ResetBallPhysics();
        }
    }

    public void LevelWon()
    {
        inGameUIController.StopTimer();
        Time.timeScale = 0;
        LevelWonScreen.GetComponent<Canvas>().enabled = true;
        SaveTime();
        levelEndTimeLevelWon.text = inGameUIController.GetTimeAsString();
    }

    private void SaveTime()
    {
        PlayerSave playerSave = LoadPlayerSave();
        if (playerSave == null)
        {
            playerSave = new PlayerSave();
        }

        float levelTime = inGameUIController.GetTime();
        if (SceneManager.GetActiveScene().name == "Level_1")
        {
            playerSave.TimesList_Level_1.Add(levelTime);
        }
        if (SceneManager.GetActiveScene().name == "Level_2")
        {
            playerSave.TimesList_Level_2.Add(levelTime);
        }
        if (SceneManager.GetActiveScene().name == "Level_3")
        {
            playerSave.TimesList_Level_3.Add(levelTime);
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);
        bf.Serialize(file, playerSave);
        file.Close();
    }

    private PlayerSave LoadPlayerSave()
    {
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            try 
            {
                PlayerSave playerSave = (PlayerSave)bf.Deserialize(file);
                file.Close();
                return playerSave;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error loading save file: " + e.Message);
                file.Close();
            }
            
        }
        return null;
    }

    public void CheckForEndOfGame()
    {        
        //if (GameObject.Find("BrickLineC").transform.childCount == 0)
        //{
        //    SceneManager.LoadScene(0);
        //}
    }

    public void CheckForLowBrickNumber()
    {
        int activeBrickCount = bricks.Count(brick => brick.gameObject.activeSelf);
        if (activeBrickCount < minimumBrickNumber) {
            // Reset inactive bricks to replenish the number
            var inactiveBricks = bricks.Where(brick => !brick.gameObject.activeSelf).ToList();

            // Shuffle the list to introduce randomness in selection
            for (int i = 0; i < inactiveBricks.Count; i++)
            {
                int randomIndex = Random.Range(i, inactiveBricks.Count);
                var temp = inactiveBricks[i];
                inactiveBricks[i] = inactiveBricks[randomIndex];
                inactiveBricks[randomIndex] = temp;
            }

            foreach (Brick brick in inactiveBricks)
            {
                if (brickMaxValueFlat > 0)
                {
                    brick.ResetBrick((int)Mathf.Round(brickMaxValueFlat), useOperation, powerupChance);
                } else
                {
                    brick.ResetBrick((int)Mathf.Round(maxTargetValue/brickMaxValueRatio), useOperation, powerupChance);
                }

                // Stop resetting once the minimum is met
                activeBrickCount++;
                var randomExtra = Random.Range(0, 5); //Add a random number of extra bricks back
                if (activeBrickCount >= minimumBrickNumber + randomExtra) break;
            }
        }
    }

    private void SetupBricks()
    {
        List<DtoTerm> allTerms = new();

        bricks.AddRange(bricksRowC);
        bricks.AddRange(bricksRowB);
        bricks.AddRange(bricksRowA);

        foreach (Brick brick in  bricks)
        {
            brick.SetIsPowerup(powerupChance);
            var term = new DtoTerm();
            if (brickMaxValueFlat > 0)
            {
                term = brick.SetBrickMathValue((int)Mathf.Round(brickMaxValueFlat), useOperation);
            }
            else
            {
                term = brick.SetBrickMathValue((int)Mathf.Round(maxTargetValue / brickMaxValueRatio), useOperation);
            }
            allTerms.Add(term);
        }

        CalculateTargets(allTerms);
    }

    public void CalculateTargets(List<DtoTerm> allTerms)
    {
        List<int> targets = new();

        int calculateTarget = paddleValue;
        while (targets.Count < numberOfTargets)
        {
            int termLength = Random.Range(2, 4); //To reach next target you ideally need 2 or 3 bricks

            for (int i = 0; i < termLength; i++)
            {
                int operationIndex = Random.Range(0, allTerms.Count);
                DtoTerm term = allTerms[operationIndex];
                allTerms.Remove(term);

                switch (term.MathOperator)
                {
                    case MathOperatorsEnum.SUBTRACTION:
                        calculateTarget -= term.Value;
                        break;
                    case MathOperatorsEnum.ADDITION:
                        calculateTarget += term.Value;
                        break;
                    case MathOperatorsEnum.MULTIPLICATION:
                        calculateTarget *= term.Value;
                        break;
                    default:
                        break;
                }
            }

            while (calculateTarget < minTargetValue || calculateTarget > maxTargetValue)
            {
                int overflow;
                if (calculateTarget < minTargetValue)
                {
                    overflow = minTargetValue - calculateTarget;
                    calculateTarget = overflowValue + overflow;
                }
                else if (calculateTarget > maxTargetValue)
                {
                    overflow = calculateTarget - maxTargetValue;
                    calculateTarget = overflowValue - overflow;
                }
            }

            if (!targets.Contains(calculateTarget) && calculateTarget != paddleValue)
            {
                targets.Add(calculateTarget);
            }
        }

        List<int> nonTargets = new();
        while (nonTargets.Count < numberOfNonTargets)
        {
            int randomNonTarget = Random.Range(minTargetValue, maxTargetValue + 1);

            if (!targets.Contains(randomNonTarget) && !nonTargets.Contains(randomNonTarget) && randomNonTarget != paddleValue)
            {
                nonTargets.Add(randomNonTarget);
            }
        }

        inGameUIController.SetTargets(targets);
        inGameUIController.SetNonTargets(nonTargets);
    }
}
