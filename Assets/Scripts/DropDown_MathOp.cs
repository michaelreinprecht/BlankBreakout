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
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        dropDownRenderer = GetComponentInChildren<Renderer>();
        if (dropDownRenderer == null)
        {
            Debug.LogError("Renderer not found on DropDown_MathOp or its children.");
            return;
        }
        //dropDownRenderer.enabled = false;
        SetVisibility(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDropDownValue(MathOperatorsEnum brickMathOperator, int brickMathValue)
    {
        mathOperator = brickMathOperator;
        mathValue = brickMathValue;
        mathValueText.text = brickMathOperator.ToSymbol() + mathValue.ToString();
    }
    
    public void SetVisibility(bool isVisible)
    {
        if (dropDownRenderer != null)
        {
            dropDownRenderer.enabled = isVisible;
        }
        if (mathValueText != null)
        {
            mathValueText.enabled = isVisible;
        }
    }

}
