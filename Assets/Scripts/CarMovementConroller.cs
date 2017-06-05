using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovementConroller: MonoBehaviour
{
    public Vector3 Point1;
    public Vector3 Point2;
    public float Speed = 80;

    private Transform _carTransform;
    void Start ()
    {
        _carTransform = GetComponent<Transform>();
        _carTransform.position = Point1;
    }		
	void Update ()
    {
        _carTransform.position = Vector3.MoveTowards(_carTransform.position, Point2, Time.deltaTime * Speed);        
        if(Vector3.Distance(_carTransform.position,Point2)<0.1f)
        {
            Destroy(gameObject);
        }
    }
}
