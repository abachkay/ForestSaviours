using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FireFightersManagementController : MonoBehaviour
{
    public float TargetOffset = 1;
    public int SelectedHelicoterIndex = 0;
    public FireFighterController[] Helicopters;
    public NoResourceTextController NoResourceTextController;

    private Vector2 firstTouchPosition;    
    private bool isTouchMoving = false;
    private RaycastHit lastButtonDownPosition;

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Input from windows.        
//#if UNITY_EDITOR
        if ((Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && 
            //!UnityEditor.EditorApplication.isRemoteConnected && 
            (UnityEngine.EventSystems.EventSystem.current == null || UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null))
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
                        HandleTouch(hit);
                    }
                }
            }
        }
//#endif        
        // Input from mobile.
        if (Input.touchCount == 2 && (UnityEngine.EventSystems.EventSystem.current == null ||
            UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null))
        {
            isTouchMoving = false;
        }
        if (Input.touchCount == 1 && (UnityEngine.EventSystems.EventSystem.current == null ||
            UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null))
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                isTouchMoving = true;
                firstTouchPosition = touch.position;                                
            }          
            else if (touch.phase == TouchPhase.Ended && isTouchMoving &&
                !(Mathf.Abs(firstTouchPosition.x - touch.position.x) > Screen.height * 0.04 || Mathf.Abs(touch.position.y - firstTouchPosition.y) > Screen.height * 0.04))
            { 
                if (Physics.Raycast(ray, out hit))
                {
                    HandleTouch(hit);
                }                                
            }
        }
    }
    private void HandleTouch(RaycastHit hit)
    {
        if (hit.transform.tag == "Target" && Helicopters[SelectedHelicoterIndex].TargetsOfMovementList.Contains(hit.transform.gameObject))
        {
            Helicopters[SelectedHelicoterIndex].TargetsOfMovementList.Remove(hit.transform.gameObject);
            Destroy(hit.transform.gameObject);
        }
        else if (hit.transform.tag == "Water" && Helicopters[SelectedHelicoterIndex].Type == FireFighterType.AerialWaterSpraying)
        {
            if (Helicopters[SelectedHelicoterIndex].RefillingTarget != null)
            {
                Destroy(Helicopters[SelectedHelicoterIndex].RefillingTarget);
                Helicopters[SelectedHelicoterIndex].RefillingTarget = null;
            }
            Helicopters[SelectedHelicoterIndex].RefillingTarget = Instantiate(Helicopters[SelectedHelicoterIndex].TargetForRefillPrefab);
            Helicopters[SelectedHelicoterIndex].RefillingTarget.transform.position = new Vector3(hit.point.x, hit.point.y + TargetOffset, hit.point.z);
        }
        else if (Helicopters[SelectedHelicoterIndex].CanDrop)
        {
            Helicopters[SelectedHelicoterIndex].TargetsOfMovementList.Add(Instantiate(Helicopters[SelectedHelicoterIndex].TargetOfMovementPrefab));
            Helicopters[SelectedHelicoterIndex].TargetsOfMovementList.Last().transform.position = new Vector3(hit.point.x, hit.point.y + TargetOffset, hit.point.z);                       
        }
        else if (NoResourceTextController != null)
        {
            NoResourceTextController.Show();
        }
    }
    public void SelectFireFighter(int index)
    {
        SelectedHelicoterIndex = index;
    }
    public void SendToBase()
    {
        Helicopters[SelectedHelicoterIndex].TargetsOfMovementList.Add(Instantiate(Helicopters[SelectedHelicoterIndex].TargetOfMovementPrefab));
        Helicopters[SelectedHelicoterIndex].TargetsOfMovementList.Last().transform.position = new Vector3(-20, 0, -20);        
        //Helicopters[SelectedHelicoterIndex].WaterPointingTargetOfMovement = Helicopters[SelectedHelicoterIndex].TargetsOfMovementList.Last();
    }
}
