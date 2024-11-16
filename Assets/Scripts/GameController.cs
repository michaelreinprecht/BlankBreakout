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
    private Canvas inGameUiScreen;
    [SerializeField]
    private Canvas gameOverScreen;

        private InGameUIScript inGameUIScript;

    // Start is called before the first frame update
    void Start()
    {
        SpawnNewBall();
        inGameUiScreen = Instantiate(inGameUiScreen);
        inGameUIScript = inGameUiScreen.GetComponentInChildren<InGameUIScript>();
        //InvokeRepeating("CheckForEndOfGame", 20, 3);
    }

    // Update is called once per frame
    void Update()
    {
        inGameUIScript.UpdateLives(lives);

        if (lives <= 0)
        {
            Instantiate(gameOverScreen);
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
