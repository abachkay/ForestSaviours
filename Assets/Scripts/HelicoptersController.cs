using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicoptersController : MonoBehaviour
{
    public GameObject Water;
    public GameObject TerrainObject;
    public float TargetOffset = 1;
    public float CameraMinX = 100;
    public float CameraMaxX = 1000;
    public float CameraMinZ = 100;
    public float CameraMaxZ = 1000;
    public float CameraMoveSpeed = 5000;
    public int SelectedHelicoterIndex = 0;
    public Transform[] HelicoptersTransforms;
    public GameObject[] HelicoptersHealthBars;
    public GameObject[] HelicoptersTargetObjects;
    public float[] HelicoptersSpeeds;
    public float[] HelicoptersRotationSpeeds;
    public float[] HelicoptersRefilSpeeds;
    public float[] HelicoptersAmountsOfOneSpray;   

    private bool[] areHelicoptersOnMove;
    private bool[] areHelicoptersGoingToWater;
    private float[] helicoptersWaterAmounts;
    

    private float _lastMousePositionX = 0;
    private float _lastMousePositionY = 0;   
    private Vector2 firstTouchPosition;
    private Vector2 intermediateTouchPosition;
    private Vector2 lastTouchPosition;
    private RaycastHit lastButtonDownPosition;
    private bool isTouchMoving = false;

    void Start()
    {
        areHelicoptersOnMove = new bool[HelicoptersTransforms.Length];
        areHelicoptersGoingToWater = new bool[HelicoptersTransforms.Length];
        helicoptersWaterAmounts = new float[HelicoptersTransforms.Length];
        for (int i=0;i<HelicoptersTransforms.Length;i++)
        {
            helicoptersWaterAmounts[i] = 1;
        }
    }
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Inout from windows.
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
            }

            if (Input.GetMouseButton(0))
            {
                var translation = new Vector3(Input.GetAxis("Mouse Y") * 400 * Time.deltaTime, 0,
                                   Input.GetAxis("Mouse X") * -1 * 400 * Time.deltaTime);
                if (transform.position.x + translation.x > CameraMaxX || transform.position.x + translation.x < CameraMinX)
                    translation.x = 0;
                if (transform.position.z + translation.z > CameraMaxZ || transform.position.z + translation.z < CameraMinZ)
                    translation.z = 0;
                transform.position += translation;
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
        // Heli moves and rotates.
        for (int index = 0; index < HelicoptersTransforms.Length; index++)
        {
            if (areHelicoptersOnMove[index])
            {
                HelicoptersTransforms[index].position = Vector3.MoveTowards(HelicoptersTransforms[index].position, new Vector3(HelicoptersTargetObjects[index].transform.position.x, HelicoptersTransforms[index].position.y, HelicoptersTargetObjects[index].transform.position.z), HelicoptersSpeeds[index] * Time.deltaTime * 60);
                var q = Quaternion.LookRotation(new Vector3(HelicoptersTargetObjects[index].transform.position.x, HelicoptersTransforms[index].position.y, HelicoptersTargetObjects[index].transform.position.z) - HelicoptersTransforms[index].position);
                HelicoptersTransforms[index].rotation = Quaternion.RotateTowards(HelicoptersTransforms[index].rotation, q, HelicoptersRotationSpeeds[index] * Time.deltaTime * 60);
            }

            // Heli sprays water.
            if (areHelicoptersOnMove[index] && Mathf.Abs(HelicoptersTransforms[index].position.x - HelicoptersTargetObjects[index].transform.position.x) < 0.1 && Mathf.Abs(HelicoptersTransforms[index].position.z - HelicoptersTargetObjects[index].transform.position.z) < 0.1)
            {
                areHelicoptersOnMove[index] = false;
                if (!areHelicoptersGoingToWater[index] && helicoptersWaterAmounts[index] >= HelicoptersAmountsOfOneSpray[index])
                {
                    var water = Instantiate(Water);
                    water.transform.position = HelicoptersTransforms[index].position;
                    var waterController = water.GetComponent<WaterController>();
                    waterController.TerrainObject = TerrainObject;
                    helicoptersWaterAmounts[index] -= HelicoptersAmountsOfOneSpray[index];
                    GetComponents<AudioSource>()[2].Play();
                }
                HelicoptersHealthBars[index].transform.localScale = new Vector3(helicoptersWaterAmounts[index], 1, 1);
            }
            // Heli refils water.
            if (!areHelicoptersOnMove[index] && areHelicoptersGoingToWater[index] && helicoptersWaterAmounts[index] < 1)
            {
                helicoptersWaterAmounts[index] += HelicoptersRefilSpeeds[index] / 10 * 60 * Time.deltaTime;
                HelicoptersHealthBars[index].transform.localScale = new Vector3(helicoptersWaterAmounts[index], 1, 1);
            }
        }
    }
}
