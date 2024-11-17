using GLTFast;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

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

    // Start is called before the first frame update
    void Start()
    {
        SpawnNewBall();
        SpawnAllBricks();
        //InvokeRepeating("CheckForEndOfGame", 20, 3);
        InvokeRepeating("CheckForLowBrickNumber", 20, 3);
    }

    // Update is called once per frame
    void Update()
    {
        inGameUIController.UpdateLives(lives);

        if (lives <= 0)
        {
            gameOverScreen.GetComponent<Canvas>().enabled = true;
            Time.timeScale = 0;
        }
    }

    public void LooseALife()
    {
        lives--;
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
