using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HecicopterController : MonoBehaviour
{
    public float MainRotorTurningSpeed = 1;
    public float RearRotorTurningSpeed = 1;
    public float MovementSpeed = 3;
    public float RotationSpeed = 6;
    public float RefilSpeed = 0.3f;
    public float AmountsOfOneSpray = 0.24999f;

    public Transform MainRotorTransform;
    public Transform RearRotorTransform;
    public Transform TargetOfMovementSpriteTransform;
    public Transform HealthBarTransform;
    public GameObject WaterSpray;

    public bool IsOnMove = false;
    public bool IsGoingToWater = false;
    private float _waterAmount = 1;

    private void Update ()
    {
        // Rotors turning.
        MainRotorTransform.Rotate(0, 0, MainRotorTurningSpeed * 60 * Time.deltaTime);
        RearRotorTransform.Rotate(RearRotorTurningSpeed * 60 * Time.deltaTime, 0, 0);

        // Helicopter moves and rotates.
        if (IsOnMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(TargetOfMovementSpriteTransform.transform.position.x, 
                transform.position.y, TargetOfMovementSpriteTransform.transform.position.z), MovementSpeed * Time.deltaTime);
            var q = Quaternion.LookRotation(new Vector3(TargetOfMovementSpriteTransform.transform.position.x, transform.position.y, 
                TargetOfMovementSpriteTransform.transform.position.z) - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, RotationSpeed * Time.deltaTime);
        }

        // Helicopter sprays water.
        if (IsOnMove && Mathf.Abs(transform.position.x - TargetOfMovementSpriteTransform.position.x) < 0.1 && Mathf.Abs(transform.position.z - TargetOfMovementSpriteTransform.transform.position.z) < 0.1)
        {
            IsOnMove = false;
            if (!IsGoingToWater && _waterAmount >= AmountsOfOneSpray)
            {
                var water = Instantiate(WaterSpray);
                water.transform.position = transform.position;
                var waterController = water.GetComponent<WaterController>();
                //waterController.TerrainObject = TerrainObject;
                _waterAmount -= AmountsOfOneSpray;
                //GetComponents<AudioSource>()[2].Play();
            }
            HealthBarTransform.localScale = new Vector3(_waterAmount, 1, 1);
        }

        // Helicopter refils water.
        if (!IsOnMove && IsGoingToWater && _waterAmount < 1)
        {
            _waterAmount += RefilSpeed  * Time.deltaTime;
            HealthBarTransform.localScale = new Vector3(_waterAmount, 1, 1);
        }
    }
}
