using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropDown_MathOp : MonoBehaviour
{
    private Rigidbody rb;
    public TextMeshPro mathValueText;
    public int mathValue;
    public MathOperatorsEnum mathOperator;
    private Renderer dropDownRenderer;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetDropDownValue(MathOperatorsEnum brickMathOperator, int brickMathValue)
    {
        mathOperator = brickMathOperator;
        mathValue = brickMathValue;
        mathValueText.text = brickMathOperator.ToSymbol() + mathValue.ToString();
    }
}
