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
    private MathOperatorsEnum mathOperator;
    private DropDown_MathOp linkedDropDown;
    private Rigidbody rb;

    public void SetIsPowerup(int chance)
    {
        if (UnityEngine.Random.Range(0, 100) <= chance)
        {
            isPowerUp = true;
        }
    }

    // Start is called before the first frame update
    public DtoTerm SetBrickMathValue(int maxValue, List<MathOperatorsEnum> validOperations)
    {
        MathOperatorsEnum mathOperator = (MathOperatorsEnum)validOperations[UnityEngine.Random.Range(0, validOperations.Count)];
        
        currentHitPoints = hitPoints;
        mathValue = UnityEngine.Random.Range(1, maxValue);
        mathValueTextBrick.text = mathOperator.ToSymbol() + mathValue.ToString();

        return new DtoTerm() { MathOperator = mathOperator, Value = mathValue };
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
        SoundManager.Instance.PlaySound(destroySound);
        if (director)
        {
            director.Play();
        }
        if (destructionParticles)
        {
            destructionParticles.Play();
        }
        Destroy(gameObject, 4f); //Destroy game object after 4 seconds
        mathValueTextBrick.text = "";     
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
        var rb = linkedDropDown.GetComponent<Rigidbody>();
        rb.GetComponent<Rigidbody>().useGravity = true;
        linkedDropDown.SetVisibility(true);
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
    public void LinkDropDown(DropDown_MathOp dropDown)
    {
        linkedDropDown = dropDown;
        linkedDropDown.SetDropDownValue(mathOperator, mathValue);
        linkedDropDown.transform.position = transform.position;
    }
}
