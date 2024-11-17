using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        SpawnNewBall();
        //InvokeRepeating("CheckForEndOfGame", 20, 3);
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

    public void SpawnNewBall()
    {
        Instantiate(ballPrefab, ballStartPosition, Quaternion.identity);
    }
}
