using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropDown_MathOp : MonoBehaviour
{
    private Rigidbody rb;
    public TextMeshPro mathValue;


    // Start is called before the first frame update
    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        rb.GetComponent<Rigidbody>().useGravity = true;
        mathValue.text = "+4";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
