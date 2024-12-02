using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private int lives = 3;
    [SerializeField]
    private List<MathOperatorsEnum> useOperation = new() { MathOperatorsEnum.ADDITION, MathOperatorsEnum.SUBTRACTION};
    [SerializeField]
    private int maxBrickValue = 10;
    [SerializeField] 
    private int paddleValue = 1;
    [SerializeField]
    private int numberOfTargets = 3;
    [SerializeField]
    private int numberOfNonTargets = 3;
    [SerializeField]
    private int powerupChance = 20;
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
    private GameObject dropDown_MathOpPrefab;

    private List<Brick> bricks = new();


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

        Time.timeScale = 1;
    }

    public void InitGameObjects()
    {
        paddle.SetValue(paddleValue);
        paddle.ValueChanged += CheckTargets;
    }

    public void InitInGameUIController()
    {
        inGameUIController.SetLives(lives);
        inGameUIController.AllLivesLost += GameOver;
        //inGameUIController.TargetsCleared += todo;
        inGameUIController.NonTargetCleared += GameOver;
        inGameUIController.StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CheckTargets()
    {
        inGameUIController.ContainsTarget(paddle.GetValue());
        inGameUIController.ContainsNonTarget(paddle.GetValue());
    }

    public void GameOver()
    {
        gameOverScreen.GetComponent<Canvas>().enabled = true;
        Time.timeScale = 0;
        inGameUIController.StopTimer(); 
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
                brick.ResetBrick(maxBrickValue, useOperation, powerupChance);

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
            var term = brick.SetBrickMathValue(maxBrickValue, useOperation);
            allTerms.Add(term);
        }

        CalculateTargets(allTerms);
    }

    //maybe move some parts of this code out of this class
    public void CalculateTargets(List<DtoTerm> allTerms)
    {
        List<int> targets = new();

        int calculateTarget = paddleValue;
        while (targets.Count != numberOfTargets)
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
            targets.Add(calculateTarget);
        }

        List<int> nonTargets = new();
        while (nonTargets.Count != numberOfNonTargets)
        {
            int randomNonTarget = Random.Range(targets.Min(), targets.Max() + 1);

            if (!targets.Contains(randomNonTarget))
            {
                nonTargets.Add(randomNonTarget);
            }
        }

        inGameUIController.SetTargets(targets);
        inGameUIController.SetNonTargets(nonTargets);
    }
}
