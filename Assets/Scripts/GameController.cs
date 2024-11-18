using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private int lives = 3;
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

    //Melih: FOR Testing
    private List<int> targets = new() { 2, 4, 8, 6 };
    private List<int> nonTargets = new() { 2, 4, 8, 6 };

    // Start is called before the first frame update
    void Start()
    {
        SpawnNewBall();
        SpawnAllBricks();
        //InvokeRepeating("CheckForEndOfGame", 20, 3);
        InvokeRepeating("CheckForLowBrickNumber", 20, 3);
        Time.timeScale = 1;
        InitInGameUIController();
    }

    public void InitInGameUIController()
    {
        inGameUIController.SetLives(lives);
        inGameUIController.AllLivesLost += GameOver;
        inGameUIController.SetTargets(targets);
        inGameUIController.SetNonTargets(nonTargets);
        //inGameUIController.TargetsCleared += todo;
        inGameUIController.NonTargetCleared += GameOver;
    }

    // Update is called once per frame
    void Update()
    {
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
        for (int i = 0; i < brickRows; i++)
        {
            for (int j = 0; j < brickColumns; j++)
            {
                Instantiate(brickAPrefab, new Vector3((j*2.5f)-6.5f, (i*1.1f)-2.8f, 0), Quaternion.identity);
            }
        }
    }
}
