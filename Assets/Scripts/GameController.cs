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
    private GameObject ballPrefab;
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
    private GameObject brickAPrefab;
    [SerializeField]
    private GameObject brickBPrefab;
    [SerializeField]
    private GameObject brickCPrefab;
    [SerializeField]
    private int brickRows = 3;
    [SerializeField] 
    private int brickColumns = 5;
    [SerializeField]
    private int minimumBrickNumber = 1;
     [SerializeField]
    private GameObject dropDown_MathOpPrefab;

    // Start is called before the first frame update
    void Start()
    {
        SpawnNewBall();
        SpawnAllBricks();
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
    }

    public void LooseALife()
    {
        inGameUIController.RemoveLive();
    }

    public void ResetBall()
    {
        if (GameObject.FindGameObjectWithTag("Ball"))
        {
            GameObject.FindGameObjectWithTag("Ball").transform.position = ballStartPosition;
            BallManager ballManager = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallManager>();
            ballManager.ResetBallPhysics();
        } else
        {
            Instantiate(ballPrefab, ballStartPosition, Quaternion.identity);
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
        if (GameObject.FindGameObjectsWithTag("Brick").Length < minimumBrickNumber) {
            SpawnAllBricks(); //TODO -> change this so we can spawn new bricks in new open spaces instead of respawning all (would need to track all bricks and listen to some brick destroyed events)
        }
    }

    public void SpawnNewBall()
    {
        Instantiate(ballPrefab, ballStartPosition, Quaternion.identity);
    }

    public void SpawnAllBricks()
    {
        //TODO change prefabs for different brick rows and types (e.g. for powerups)
        List<DtoTerm> allTerms = new();
        GameObject prefab = brickAPrefab;
        GameObject dropDownPrefab = dropDown_MathOpPrefab;
        for (int i = 0; i < brickRows; i++)
        {
            if (i == 1) prefab = brickBPrefab;
            if (i == 2) prefab = brickCPrefab;
            for (int j = 0; j < brickColumns; j++)
            {
                Vector3 brickPosition = new Vector3((j*2.5f)-6.5f, (i*1.1f)+1.0f, 0);
                GameObject prefabInstance = Instantiate(prefab, brickPosition, Quaternion.identity);
                Brick brickScript = prefabInstance.GetComponent<Brick>();
                brickScript.SetIsPowerup(powerupChance);
                var term = brickScript.SetBrickMathValue(maxBrickValue, useOperation);
                
                // Instantiate the math operation dropdown at the brick's position
                GameObject dropDownInstance = Instantiate(dropDownPrefab, brickPosition, Quaternion.identity);
                dropDownInstance.transform.SetParent(prefabInstance.transform, true); // keep world position
                
                DropDown_MathOp dropDownScript = dropDownInstance.GetComponent<DropDown_MathOp>();
                dropDownInstance.transform.localPosition = brickPosition;
                dropDownScript.SetVisibility(false);

                brickScript.LinkDropDown(dropDownScript);

                allTerms.Add(term);
            }
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
