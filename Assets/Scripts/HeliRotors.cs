using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliRotors : MonoBehaviour {
    public Transform MainRotor;
    public Transform RearRotor;
    public float MainRotorSpeed;
    public float RearRotorSpeed;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        MainRotor.Rotate(0, 0, MainRotorSpeed);
        RearRotor.Rotate(RearRotorSpeed,0,0);
	}
}
