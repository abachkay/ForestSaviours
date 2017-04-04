using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicoptersManagementController : MonoBehaviour
{     
    public int SelectedHelicoterIndex = 0;
    public HelicopterController[] Helicopters;
    public float TargetOffset = 1;

    private Vector2 firstTouchPosition;
    private Vector2 intermediateTouchPosition;
    private Vector2 lastTouchPosition;
    private bool isTouchMoving = false;
    private RaycastHit lastButtonDownPosition;
    
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Input from windows.
#if UNITY_EDITOR         
        if ((Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && !UnityEditor.EditorApplication.isRemoteConnected && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    lastButtonDownPosition = hit;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    if (hit.Equals(lastButtonDownPosition))
                    {                       
                        Helicopters[SelectedHelicoterIndex].HelicoptersTargetObjects.transform.position = new Vector3(hit.point.x, hit.point.y + TargetOffset, hit.point.z);
                        Helicopters[SelectedHelicoterIndex].IsOnMove= true;
                        if (hit.transform.tag == "Water")
                        {
                            Helicopters[SelectedHelicoterIndex].IsGoingToWater = true;
                        }
                        else
                        {
                            Helicopters[SelectedHelicoterIndex].IsGoingToWater = false;
                        }
                    }
                }
            }
        }
#endif
        // Input from mobile.
        if (Input.touchCount == 2 && UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null)
        {
            isTouchMoving = false;
        }
        if (Input.touchCount == 1 && UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null)
        {
            Debug.Log("touch");
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                isTouchMoving = true;
                firstTouchPosition = touch.position;
                intermediateTouchPosition = touch.position;
                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved && isTouchMoving)
            {
                lastTouchPosition = touch.position;
                var vector = (lastTouchPosition - intermediateTouchPosition) * CameraMoveSpeed;
                vector.Scale(new Vector2(1.0f / Screen.width, 1.0f / Screen.height));
                var translation = new Vector3(vector.y * Time.deltaTime, 0, -vector.x * Time.deltaTime);
                if (transform.position.x + translation.x > CameraMaxX || transform.position.x + translation.x < CameraMinX)
                {
                    translation.x = 0;
                }
                if (transform.position.z + translation.z > CameraMaxZ || transform.position.z + translation.z < CameraMinZ)
                {
                    translation.z = 0;
                }
                transform.position += translation;
                intermediateTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended && isTouchMoving)
            {
                isTouchMoving = false;
                lastTouchPosition = touch.position;
                if (Mathf.Abs(lastTouchPosition.x - firstTouchPosition.x) > Screen.height * 0.04 || Mathf.Abs(lastTouchPosition.y - firstTouchPosition.y) > Screen.height * 0.04)
                {
                    var vector = (lastTouchPosition - intermediateTouchPosition) * CameraMoveSpeed;
                    vector.Scale(new Vector2(1.0f / Screen.width, 1.0f / Screen.height));
                    var translation = new Vector3(vector.y * Time.deltaTime, 0, -vector.x * Time.deltaTime);
                    if (transform.position.x + translation.x > CameraMaxX || transform.position.x + translation.x < CameraMinX)
                    {
                        translation.x = 0;
                    }
                    if (transform.position.z + translation.z > CameraMaxZ || transform.position.z + translation.z < CameraMinZ)
                    {
                        translation.z = 0;
                    }
                    transform.position += translation;
                }
                else
                {
                    if (Physics.Raycast(ray, out hit))
                    {
                        HelicoptersTargetObjects[SelectedHelicoterIndex].transform.position = new Vector3(hit.point.x, hit.point.y + TargetOffset, hit.point.z);
                        areHelicoptersOnMove[SelectedHelicoterIndex] = true;
                        if (hit.transform.tag == "Water")
                        {
                            areHelicoptersGoingToWater[SelectedHelicoterIndex] = true;
                        }
                        else
                        {
                            areHelicoptersGoingToWater[SelectedHelicoterIndex] = false;
                        }
                    }
                }
                intermediateTouchPosition = lastTouchPosition;
            }
        }       
    }
}
