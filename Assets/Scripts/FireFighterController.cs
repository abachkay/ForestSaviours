using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FireFighterController : MonoBehaviour
{
    public float MovementSpeed = 3;
    public float RotationSpeed = 6;
    public float RefilSpeed = 0.1f;
    public float AmountsOfOneSpray = 0.24999f;
    public FireFighterType Type = FireFighterType.AerialWaterSpraying;
   
    public Transform HealthBarTransform;
    public List<GameObject> TargetsOfMovementList;
    public GameObject WaterPointingTargetOfMovement = null;
    public GameObject TargetOfMovementPrefab;    
    public GameObject WaterSpray;

    public bool IsOnMove = false;
    public bool IsMovingToWater = false;
    private bool IsRefilling = false;
    private float _waterAmount = 1;
    private FireFighterState _state = FireFighterState.Idle;
    private void Start()
    {
        TargetsOfMovementList = new List<GameObject>();
    }
    private void Update ()
    {
        if(_state == FireFighterState.Idle && TargetsOfMovementList.Count > 0)
        {
            _state = FireFighterState.Moving;
        }
       
        if (_state == FireFighterState.Moving && TargetsOfMovementList.Count > 0)
        {
            // Helicopter moves and rotates.
            var targetOfMovement = TargetsOfMovementList.First();
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetOfMovement.transform.position.x,
                transform.position.y, targetOfMovement.transform.position.z), MovementSpeed * Time.deltaTime);
            if (new Vector3(targetOfMovement.transform.position.x, transform.position.y,
                targetOfMovement.transform.position.z) != transform.position)
            {
                var q = Quaternion.LookRotation(new Vector3(targetOfMovement.transform.position.x, transform.position.y,
                        targetOfMovement.transform.position.z) - transform.position);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, q, RotationSpeed * Time.deltaTime);
            }
            if (Mathf.Abs(transform.position.x - targetOfMovement.transform.position.x) < 0.1 && Mathf.Abs(transform.position.z - targetOfMovement.transform.position.z) < 0.1)
            {
                if(targetOfMovement == WaterPointingTargetOfMovement)
                {
                    _state = FireFighterState.Refilling;
                }
                else if (_waterAmount >= AmountsOfOneSpray)
                {
                    // Spray water
                    var water = Instantiate(WaterSpray);
                    if (Type == FireFighterType.AerialChemicalBombing)
                    {
                        water.transform.position = targetOfMovement.transform.position;
                    }
                    else
                    {
                        water.transform.position = transform.position;
                    }
                    var waterController = water.GetComponent<WaterController>();
                    _waterAmount -= AmountsOfOneSpray;
                    if (GetComponent<AudioSource>())
                    {
                        GetComponent<AudioSource>().Play();
                    }
                    if (HealthBarTransform != null)
                    {
                        HealthBarTransform.localScale = new Vector3(_waterAmount, 1, 1);
                    }
                    _state = FireFighterState.Idle;
                }
                else
                {
                    _state = FireFighterState.Idle;
                }

                // Destroy target sprite.
                Destroy(targetOfMovement);
                TargetsOfMovementList.RemoveAt(0);                
            }
        }

        if (_state == FireFighterState.Refilling)
        {    
            // Refils water           
            if (_waterAmount + RefilSpeed * 60 * Time.deltaTime >= 1)
            {
                _waterAmount = 1;
                _state = FireFighterState.Idle;
            }            
            _waterAmount += RefilSpeed * 60 * Time.deltaTime;
            if (HealthBarTransform != null)
            {
                HealthBarTransform.localScale = new Vector3(_waterAmount, 1, 1);
            }            
        }

        // Next target is being selected.
        //GameObject targetOfMovement = null;
        //if (TargetsOfMovementList.Count != 0)
        //{
        //    targetOfMovement = TargetsOfMovementList.First();
        //    IsOnMove = true;
        //    if(targetOfMovement == WaterPointingTargetOfMovement)
        //    {
        //        IsMovingToWater = true;
        //    }
        //    else
        //    {
        //        IsMovingToWater = false;
        //    }
        //}
        //else
        //{
        //    IsOnMove = false;
        //}             

        // Helicopter moves and rotates.
        //if (IsOnMove)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetOfMovement.transform.position.x, 
        //        transform.position.y, targetOfMovement.transform.position.z), MovementSpeed * Time.deltaTime);
        //    if (new Vector3(targetOfMovement.transform.position.x, transform.position.y,
        //        targetOfMovement.transform.position.z) != transform.position)
        //    {
        //        var q = Quaternion.LookRotation(new Vector3(targetOfMovement.transform.position.x, transform.position.y,
        //                targetOfMovement.transform.position.z) - transform.position);
        //        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, RotationSpeed * Time.deltaTime);
        //    }
        //}

        // Helicopter sprays water.
        //if (IsOnMove && Mathf.Abs(transform.position.x - targetOfMovement.transform.position.x) < 0.1 && Mathf.Abs(transform.position.z - targetOfMovement.transform.position.z) < 0.1)
        //{
        //    IsOnMove = false;
        //    if (!IsMovingToWater && _waterAmount >= AmountsOfOneSpray)
        //    {
        //        var water = Instantiate(WaterSpray);
        //        if (Type == FireFighterType.AerialChemicalBombing)
        //        {
        //            water.transform.position = targetOfMovement.transform.position;
        //        }
        //        else
        //        {
        //            water.transform.position = transform.position;
        //        }
        //        var waterController = water.GetComponent<WaterController>();
        //        _waterAmount -= AmountsOfOneSpray;
        //        //GetComponents<AudioSource>()[2].Play();
        //    }
        //    if (HealthBarTransform != null)
        //    {
        //        HealthBarTransform.localScale = new Vector3(_waterAmount, 1, 1);
        //    }
        //}

        // Helicopter refils water.
        //if (!IsOnMove && IsMovingToWater && _waterAmount < 1)
        //{
        //    _waterAmount += RefilSpeed * 60 * Time.deltaTime;
        //    if (HealthBarTransform != null)
        //    {
        //        HealthBarTransform.localScale = new Vector3(_waterAmount, 1, 1);
        //    }
        //}

        // Destroy target sprite
        //if (!IsOnMove && targetOfMovement != null)
        //{
        //    Destroy(targetOfMovement);
        //    TargetsOfMovementList.RemoveAt(0);
        //    if (TargetsOfMovementList.Count != 0)
        //    {
        //        IsOnMove = true;
        //    }
        //}
    }
}
public enum FireFighterType : byte
{
    AerialWaterSpraying,    
    AerialChemicalSpraying,
    AerialChemicalBombing
}
public enum FireFighterState : byte
{
    Idle,
    Moving,
    Dropping,
    Refilling
}