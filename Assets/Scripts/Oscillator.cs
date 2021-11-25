using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 startingPosition;
    [SerializeField] Vector3 movementVector;
    [Range(0, 1)] float movementFactor;
    [SerializeField] float period = 2f;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position; //to get starting position of object in which this script is attached
    }

    // Update is called once per frame
    void Update()
    {
        if(period <= Mathf.Epsilon){     return;    }       //to stop moving of obstacle
        float cycles = Time.time / period;      //continually grow over time
        const float tau = Mathf.PI * 2;         //constant value
        float rawSineWave = Mathf.Sin(cycles * tau);     // going from -1 to 1
        movementFactor = (rawSineWave + 1f)/2f;          // going from 0 to 1

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
