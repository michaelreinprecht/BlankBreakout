using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

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
    private int currentHitpoints;
    private int mathValue;
    private string mathOperator;


    private enum MathOperatorsEnum
    {
        SUBTRACTION = 0,
        ADDITION = 1,
    }
    // Start is called before the first frame update
    void Start()
    {
        MathOperatorsEnum randomOperator = (MathOperatorsEnum)UnityEngine.Random.Range(0, 2);
        switch (randomOperator)
        {
            case MathOperatorsEnum.SUBTRACTION:
                mathOperator = "-";
                break;
            case MathOperatorsEnum.ADDITION:
                mathOperator = "+";
                break;
            default:
                mathOperator = "+";
                break;
        }
        currentHitpoints = hitPoints;
        mathValue = UnityEngine.Random.Range(1, 9);
        mathValueTextBrick.text = mathOperator + mathValue.ToString();        
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
        currentHitpoints -= damage;

        //destroy brick on 0 health
        if (currentHitpoints <= 0) {
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
        Debug.Log("Collision!");
        if (collision.gameObject.CompareTag("Ball"))
        {
            ReflectBall(collision, (result) =>
            {
                TakeDamage(result);
            });
        }
    }
}
