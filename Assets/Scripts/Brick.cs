using System;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;
using System.Collections.Generic;

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
    
    public bool IsPowerUp = false; //might need for adding powerups ...
    private int currentHitPoints;
    private int mathValue;
    private string mathOperator;

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
        if (IsPowerUp)
        {
            DropPowerUp();
        }
        else
        {
            DropMathTerm();
        }

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
        //TODO
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
}
