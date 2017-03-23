using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliController : MonoBehaviour
{ 
    public GameObject TargetObject;        
    public Transform HeliTransform;
    public GameObject Water;
    public GameObject TerrainObject;
    public GameObject HealthBar;
    public float HeliSpeed = 0.5f;
    public float HeliRotationSpeed = 1;
    public float TargetOffset = 1;
    public float RefilSpeed = 0.01f;
    public float CameraMinX = 200;
    public float CameraMaxX = 650;
    public float CameraMinZ = 300;
    public float CameraMaxZ = 650;
    public float CameraMoveSpeed = 10;

    private float _lastMousePositionX = 0;
    private float _lastMousePositionY = 0;
    private bool isHeliOnMove = false;
    private float waterAmount = 1;
    private bool isGoingToWater = false;
    private Vector3 firstTouchPosition;
    private Vector3 intermediateTouchPosition;
    private Vector3 lastTouchPosition;  

    void Update()
    {                
        RaycastHit hit;        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        //{
        //    if (Input.GetMouseButton(0))
        //    {
        //        transform.position -= new Vector3(Input.GetAxis("Mouse Y") * -1 * 200 * Time.deltaTime, 0,
        //                           Input.GetAxis("Mouse X") * 200 * Time.deltaTime);
        //    }
            
        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        if (Input.GetMouseButtonUp(0))
        //        {
        //            TargetObject.transform.position = new Vector3(hit.point.x, hit.point.y + TargetOffset, hit.point.z);
        //            isHeliOnMove = true;
        //            if (hit.transform.tag == "Water")
        //                isGoingToWater = true;
        //            else
        //                isGoingToWater = false;
        //        }
        //    }

        //    if (Input.GetMouseButton(0))
        //    {
        //        var translation = new Vector3(Input.GetAxis("Mouse Y") * -1 * 400 * Time.deltaTime, 0,
        //                           Input.GetAxis("Mouse X") * 400 * Time.deltaTime);
        //        var newCameraPos = transform.position - translation;
        //        if (newCameraPos.z < CameraMaxZ && newCameraPos.z > CameraMinZ && newCameraPos.x < CameraMaxX && newCameraPos.x > CameraMinX)
        //            transform.position = newCameraPos;
        //    }
        //}

        if (Input.touchCount == 1 && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                firstTouchPosition = touch.position;
                intermediateTouchPosition = touch.position;
                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {                
                lastTouchPosition = touch.position;
                var vector = (lastTouchPosition - intermediateTouchPosition) * CameraMoveSpeed;
                var newCameraPos = transform.position + new Vector3(vector.y * Time.deltaTime, 0, -vector.x * Time.deltaTime);
                if (newCameraPos.z < CameraMaxZ && newCameraPos.z > CameraMinZ && newCameraPos.x < CameraMaxX && newCameraPos.x > CameraMinX)
                    transform.position = newCameraPos;
                intermediateTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                lastTouchPosition = touch.position;
                if (Mathf.Abs(lastTouchPosition.x - firstTouchPosition.x) > Screen.height * 0.04 || Mathf.Abs(lastTouchPosition.y - firstTouchPosition.y) > Screen.height * 0.04)
                {
                    var vector = (lastTouchPosition - intermediateTouchPosition)* CameraMoveSpeed;
                    var newCameraPos = transform.position + new Vector3(vector.y * Time.deltaTime, 0, -vector.x * Time.deltaTime);
                    if (newCameraPos.z < CameraMaxZ && newCameraPos.z > CameraMinZ && newCameraPos.x < CameraMaxX && newCameraPos.x > CameraMinX)
                        transform.position = newCameraPos;
                }
                else
                {
                    if (Physics.Raycast(ray, out hit))
                    {
                        TargetObject.transform.position = new Vector3(hit.point.x, hit.point.y + TargetOffset, hit.point.z);
                        isHeliOnMove = true;
                        if (hit.transform.tag == "Water")
                        {
                            isGoingToWater = true;
                        }
                        else
                        {
                            isGoingToWater = false;
                        }
                    }
                }
                intermediateTouchPosition = lastTouchPosition;
            }
        }
    
        if (isHeliOnMove)
        {
            HeliTransform.position = Vector3.MoveTowards(HeliTransform.position, new Vector3(TargetObject.transform.position.x, HeliTransform.position.y, TargetObject.transform.position.z), HeliSpeed*Time.deltaTime*60);           
            var q = Quaternion.LookRotation(new Vector3(TargetObject.transform.position.x, HeliTransform.position.y, TargetObject.transform.position.z) - HeliTransform.position);
            HeliTransform.rotation = Quaternion.RotateTowards(HeliTransform.rotation, q, HeliRotationSpeed*Time.deltaTime * 60);
        }
        if(isHeliOnMove&& Mathf.Abs(HeliTransform.position.x- TargetObject.transform.position.x)<0.1&& Mathf.Abs(HeliTransform.position.z- TargetObject.transform.position.z)<0.1)
        {
            isHeliOnMove = false;
            if (!isGoingToWater && waterAmount >= 0.25f)
            {
                var water = Instantiate(Water);
                water.transform.position = HeliTransform.position;
                var waterController = water.GetComponent<WaterController>();
                Debug.Log(TerrainObject);
                waterController.TerrainObject = TerrainObject;
                waterAmount -= 0.25f;
            }                      
            HealthBar.transform.localScale = new Vector3(waterAmount, 1,1);
        }
        if(!isHeliOnMove && isGoingToWater && waterAmount<1)
        {
            waterAmount += RefilSpeed/10*60*Time.deltaTime;
            HealthBar.transform.localScale = new Vector3(waterAmount, 1, 1);
        }
    }
}
