using System;
using TMPro;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    [SerializeField] 
    private float paddleSpeed = 10f;
    [SerializeField] 
    private float xPosMin = -6.8f;
    [SerializeField] 
    private float xPosMax = 6.6f;
    [SerializeField] 
    private TMP_Text textObject;
    [SerializeField]
    private TMP_Text historyLog;
    private string currentHistoryLine;

    private int value;
    public Action ValueChanged;



    private void Start()
    {
        historyLog.text = "<color=#005500><b>Reached Goals:</b></color>\n\n";
        currentHistoryLine += value.ToString();
    }

    public void SetValue(int startingValue)
    {
        value = startingValue;
        SetContent(value);
    }

    public int GetValue()
    {
        return value;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0f) //Pausing game
        {
            return;
        } else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) 
        {
            float horizontal = Input.GetAxis("Horizontal");
            float xDelta = horizontal * paddleSpeed * Time.deltaTime;
            float newXPosition = transform.position.x + xDelta;

            // Clamp the X position to keep the paddle within bounds
            newXPosition = Mathf.Clamp(newXPosition, xPosMin, xPosMax);

            transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody ballRb = collision.rigidbody;

        if (ballRb != null )
        {
            //Get the exact point where the ball hit the paddle
            Vector3 hitPoint = collision.contacts[0].point;

            float hitFactor = (hitPoint.x - transform.position.x) / transform.localScale.x;

            Vector3 paddleDirection = new Vector3(hitFactor, 1, 0).normalized;
            Vector3 existingDirection = ballRb.velocity.normalized;

            float blendFactor = 0.5f; // 50% new direction, 50% existing direction

            Vector3 combinedDirection = (paddleDirection * blendFactor + existingDirection * (1 - blendFactor)).normalized;

            // Set the new velocity, keeping the ball speed constant
            ballRb.velocity = combinedDirection * ballRb.velocity.magnitude;
        }
    }

    private void SetContent(int number)
    {
        textObject.text = number.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DropDown_MathOperation"))
        {
            other.gameObject.SetActive(false);
            MathOperatorsEnum mathOperator = other.GetComponent<DropDown_MathOp>().mathOperator;
            int termValue = other.GetComponent<DropDown_MathOp>().mathValue;
          
            switch (mathOperator)
                {
                    case MathOperatorsEnum.SUBTRACTION:
                        value -= termValue;
                        ValueChanged();
                        break;
                    case MathOperatorsEnum.ADDITION:
                        value += termValue;
                        ValueChanged();
                        break;
                    case MathOperatorsEnum.MULTIPLICATION:
                        value *= termValue;
                        ValueChanged();
                        break;
                    default:
                        break;
                }
            currentHistoryLine += mathOperator.ToSymbol(); //Update history ...
            currentHistoryLine += termValue.ToString();

            textObject.text = value.ToString();
        }
    }

    public void LogTargetHit()
    {
        historyLog.text += currentHistoryLine + " = <color=#005500>" + value.ToString() + "</color>\n";
    }

    public void LogNonTargetHit()
    {
        historyLog.text += currentHistoryLine + " = <color=#ff0000>" + value.ToString() + "</color>\n";
    }
}
