using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManagementController : MonoBehaviour
{
    public GameObject[] CarPrefabs;
    public float MinIntervalSec = 1;
    public float MaxIntervalSec = 2;
    public float MinSpeed = 57;
    public float MaxSpeed = 60;
    public Vector3 Point1;
    public Vector3 Point2;

    private float _timeCounterSec = 0;
    private float _intervalSec = 3;
    private void Start ()
    {
        _intervalSec = Random.Range(MinIntervalSec, MaxIntervalSec);
	}
    private void Update ()
    {
        _timeCounterSec += Time.deltaTime;
        if(_timeCounterSec >= _intervalSec)
        {
            LaunchCar();
            _timeCounterSec = 0;
            _intervalSec = Random.Range(MinIntervalSec, MaxIntervalSec);
        }
	}
    private void LaunchCar()
    {
        var carPrefab = CarPrefabs[Random.Range(0, CarPrefabs.Length - 1)];
        var car = Instantiate(carPrefab);
        car.transform.rotation = Quaternion.LookRotation(Point2 - Point1);

        car.transform.position = Point1;
        var carMovement = car.AddComponent<CarMovementConroller>();
        carMovement.Point1 = Point1;
        carMovement.Point2 = Point2;
        carMovement.Speed = Random.Range(MinSpeed, MaxSpeed);
    }
}
