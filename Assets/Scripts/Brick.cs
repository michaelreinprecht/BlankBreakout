using System;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class Brick : MonoBehaviour
{
    [SerializeField]
    private int hitPoints = 1;
    [SerializeField]
    private float reflectingForce = 0.0f; //Set to 0 for constant speed
    [SerializeField]
    private BoxCollider brickCollider;
    [SerializeField]
    private ParticleSystem destructionParticles;
    [SerializeField]
    private string destroySound;
    [SerializeField]
    private PlayableDirector director;
    [SerializeField]
    private TextMeshPro mathValueTextBrick;

    private bool isPowerUp;
    private int currentHitPoints;
    private int mathValue;
    private string mathOperator;

    private float scaleTimerMax = 1f;
    private float scaleTimer = 1f;

    public void SetIsPowerup(int chance)
    {
        if (UnityEngine.Random.Range(0, 100) <= chance)
        {
            isPowerUp = true;
        }
    }

    private void Update()
    {
        if (scaleTimer < scaleTimerMax)
        {
            scaleTimer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(0.5f, 0.5f, 0.5f), scaleTimer / scaleTimerMax);
        }
    }

    private void ScaleInBrick()
    {
        scaleTimer = 0f;
    }

    // Start is called before the first frame update
    public DtoTerm SetBrickMathValue(int maxValue, List<MathOperatorsEnum> validOperations)
    {
        MathOperatorsEnum randomOperator = (MathOperatorsEnum)validOperations[UnityEngine.Random.Range(0, validOperations.Count)];
        switch (randomOperator)
        {
            case MathOperatorsEnum.SUBTRACTION:
                mathOperator = "-";
                break;
            case MathOperatorsEnum.ADDITION:
                mathOperator = "+";
                break;
            case MathOperatorsEnum.MULTIPLICATION:
                mathOperator = "*";
                break;
            default:
                mathOperator = "+";
                break;
        }
        currentHitPoints = hitPoints;
        mathValue = UnityEngine.Random.Range(1, maxValue);
        mathValueTextBrick.text = mathOperator + mathValue.ToString();

        return new DtoTerm() { MathOperator = randomOperator, Value = mathValue };
    }

    //Add force to ball when reflecting form brick if needed and invoke callback with damage number
    private void ReflectBall(Collision collision, Action<int> callback) 
    {
        Rigidbody ballRb = collision.gameObject.GetComponent<Rigidbody>();

        ballRb.AddForce(ballRb.velocity * reflectingForce, ForceMode.VelocityChange); //Not needed for constant speed
        callback.Invoke(1);
    }

    //Handle damage brick is taking and initiate destruction if needed
    public void TakeDamage(int damage)
    {
        currentHitPoints -= damage;

        //destroy brick on 0 health
        if (currentHitPoints <= 0) {
            HandleDestruction();
            return;
        }
    }

    public void HandleDestruction()
    {
        //TODO drop terms or powerups ...
        if (isPowerUp)
        {
            DropPowerUp();
        }
        
        DropMathTerm();
        
        if (brickCollider)
        {
            brickCollider.enabled = false; //deactivate collider once brick is destroyed
        }
        SoundManager.Instance.PlaySound(destroySound, 1f);
        CinemachineShake.Instance.ShakeCamera(5f, .1f);
        if (director)
        {
            director.Play();
        }
        if (destructionParticles)
        {
            destructionParticles.Play();
        }
        mathValueTextBrick.text = "";     
        StartCoroutine(DeactivateBrickAfterDelay(1.5f));
    }

    private IEnumerator DeactivateBrickAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay

        // Now deactivate the brick after the delay
        gameObject.SetActive(false);
    }

    public void ResetBrick(int maxMathValue, List<MathOperatorsEnum> validOperations, int isPowerupChance)
    {
        currentHitPoints = hitPoints; // Reset health
        SetBrickMathValue(maxMathValue, validOperations);
        SetIsPowerup(isPowerupChance);

        // Reactivate collider and visuals
        if (brickCollider) brickCollider.enabled = true;
        if (destructionParticles) destructionParticles.Stop(); // Reset any visual effects
                                                               // Reset the animation and ensure it's playing again
        if (director)
        {
            director.Stop();            // Stop the director
            director.time = 0;          // Set the director's time to the start
            director.Evaluate();        // Ensure it updates immediately to the first frame
            director.Play();            // Play the director (starts the animation again)
        }

        // Ensure the mesh renderer is enabled
        Renderer brickRenderer = GetComponent<Renderer>();
        if (brickRenderer)
        {
            brickRenderer.enabled = true; // Ensure the brick is visible
        }
        ScaleInBrick();
        gameObject.SetActive(true); // Ensure the brick is visible
    }

    private void DropPowerUp()
    {
        GameObject powerUp = PowerupManager.Instance.GetRandomPowerup();
        if (powerUp == null)
        {
            return;
        }

        GameObject powerUpInstance = Instantiate(powerUp, transform.position, Quaternion.identity);
        Rigidbody powerUpRb = powerUpInstance.GetComponent<Rigidbody>();
    
        if (powerUpRb != null)
        {
            Vector3 randomDirection = new Vector3(
                UnityEngine.Random.Range(-1f, 1f),
                1f,
                UnityEngine.Random.Range(-1f, 1f)
            ).normalized;

            powerUpRb.AddForce(randomDirection * 3f, ForceMode.Impulse);
        }

        StartCoroutine(EnableGravityAfterDelay(powerUpInstance, 0.5f));
    }


    private void DropMathTerm()
    {
        //TODO
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            ReflectBall(collision, (result) =>
            {
                TakeDamage(result);
            });
        }
    }

    private IEnumerator EnableGravityAfterDelay(GameObject powerUpInstance, float delay)
    {
        yield return new WaitForSeconds(delay);

        Rigidbody powerUpRb = powerUpInstance.GetComponent<Rigidbody>();
        if (powerUpRb != null)
        {
            powerUpRb.useGravity = true;
        }
    }
}
