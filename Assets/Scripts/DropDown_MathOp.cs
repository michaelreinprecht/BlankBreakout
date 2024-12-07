using UnityEngine;
using TMPro;

public class DropDown_MathOp : MonoBehaviour
{
    private Rigidbody rb;
    public TextMeshPro mathValueText;
    public int mathValue;
    public MathOperatorsEnum mathOperator;

    public void SetDropDownValue(MathOperatorsEnum brickMathOperator, int brickMathValue)
    {
        mathOperator = brickMathOperator;
        mathValue = brickMathValue;
        mathValueText.text = brickMathOperator.ToSymbol() + mathValue.ToString();
    }
}
