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
    public GameObject RefillingTarget;    
    public GameObject TargetOfMovementPrefab;
    public GameObject WaterSpray;
    public GameObject TargetForRefillPrefab;
    public GameObject TubePrefab;

    private GameObject _tube;
    private float _waterAmount = 1;
    private FireFighterState _state = FireFighterState.Idle;
    private bool _noDrop = false;
    private void Start()
    {
        TargetsOfMovementList = new List<GameObject>();
    }
    private void Update()
    {       
        // Destroy tube
        if (_state != FireFighterState.Refilling && Type == FireFighterType.AerialWaterSpraying && _tube != null)
        {
            Destroy(_tube);
            _tube = null;
        }

        if (_state == FireFighterState.Idle && TargetsOfMovementList.Count > 0 || RefillingTarget != null)
        {
            _state = FireFighterState.Moving;
        }

        if (_state == FireFighterState.Moving && TargetsOfMovementList.Count > 0 || RefillingTarget != null)
        {
            // Helicopter moves and rotates.
            GameObject targetOfMovement = null;
            if (TargetsOfMovementList.Count > 0)
            {
                targetOfMovement = TargetsOfMovementList.First();
            }
            else
            {
                targetOfMovement = RefillingTarget;
            }
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
                if (RefillingTarget != null)
                {
                    _state = FireFighterState.Refilling;
                }
                if (targetOfMovement.activeSelf && _waterAmount >= AmountsOfOneSpray && TargetsOfMovementList.Count > 0 && _noDrop == false)
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
                    // Send to base
                    if (Type != FireFighterType.AerialWaterSpraying && _waterAmount < AmountsOfOneSpray)
                    {                                                
                        RefillingTarget = Instantiate(TargetOfMovementPrefab);
                        RefillingTarget.transform.position = new Vector3(-15,0,-15);
                        RefillingTarget.SetActive(false);
                    }

                }
                else if (RefillingTarget == null)
                {
                    _state = FireFighterState.Idle;
                }

                // Destroy target sprite.
                Destroy(targetOfMovement);
                if (TargetsOfMovementList.Any())
                {                    
                    TargetsOfMovementList.RemoveAt(0);
                }
                else
                {
                    RefillingTarget = null;
                }
            }
        }

        if (_state == FireFighterState.Refilling)
        {           
            // Make tube;
            if (Type == FireFighterType.AerialWaterSpraying && TubePrefab != null && _tube == null)
            {
                _tube = Instantiate(TubePrefab,transform);                
            }
            // Refils water           
            if (_waterAmount + RefilSpeed * 60 * Time.deltaTime >= 1)
            {
                _waterAmount = 1;
                _state = FireFighterState.Idle;
                if (TargetsOfMovementList.Count == 0 && Type != FireFighterType.AerialWaterSpraying)
                {
                    TargetsOfMovementList.Add(Instantiate(TargetOfMovementPrefab));
                    TargetsOfMovementList.Last().transform.position = new Vector3(100, 0, 100);
                    TargetsOfMovementList.Last().SetActive(false);                                            
                }
                      
            }
            _waterAmount += RefilSpeed * 60 * Time.deltaTime;
            if (HealthBarTransform != null)
            {
                HealthBarTransform.localScale = new Vector3(_waterAmount, 1, 1);
            }
        }
    }

    public bool CanDrop
    {
        get
        {
            return (_waterAmount >= AmountsOfOneSpray * (TargetsOfMovementList.Count + 1) || (_state == FireFighterState.Refilling && 1 >= AmountsOfOneSpray * (TargetsOfMovementList.Count + 1)));
        }
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