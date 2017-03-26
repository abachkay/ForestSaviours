using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliRotors : MonoBehaviour {
    public Transform MainRotor;
    public Transform RearRotor;
    public float MainRotorSpeed;
    public float RearRotorSpeed;
	void Update ()
    {
        MainRotor.Rotate(0, 0, MainRotorSpeed*60*Time.deltaTime);
        RearRotor.Rotate(RearRotorSpeed* 60 * Time.deltaTime, 0,0);
	}
}
