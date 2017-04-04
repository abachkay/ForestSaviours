using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraMoveController : MonoBehaviour
{
    public float CameraMoveSpeed = 5000;
    public float MinX = 100;
    public float MaxX = 1000;
    public float MinZ = 100;
    public float MaxZ = 1000;

    private Vector2 _firstTouchPosition;
    private Vector2 _intermediateTouchPosition;
    private Vector2 _lastTouchPosition;
    private bool _isSwipeInitiated = false;

    private void Update()
    {
        //For Windows.
#if UNITY_EDITOR
        if ((Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && !UnityEditor.EditorApplication.isRemoteConnected && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {            
            if (Input.GetMouseButton(0))
            {
                var translation = new Vector3(Input.GetAxis("Mouse Y") * 400 * Time.deltaTime, 0,
                                   Input.GetAxis("Mouse X") * -1 * 400 * Time.deltaTime);
                if (transform.position.x + translation.x > MaxX || transform.position.x + translation.x < MinX)
                {
                    translation.x = 0;
                }
                if (transform.position.z + translation.z > MaxZ || transform.position.z + translation.z < MinZ)
                {
                    translation.z = 0;
                }
                transform.position += translation;
            }
        }
#endif
        // Input from mobile.
        if (Input.touchCount == 2 && UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null)
        {
            _isSwipeInitiated = false;
        }
        if (Input.touchCount == 1 && UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null)
        {            
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                _isSwipeInitiated = true;
                _firstTouchPosition = touch.position;
                _intermediateTouchPosition = touch.position;
                _lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved && _isSwipeInitiated)
            {
                _lastTouchPosition = touch.position;
                var vector = (_lastTouchPosition - _intermediateTouchPosition) * CameraMoveSpeed;
                vector.Scale(new Vector2(1.0f / Screen.width, 1.0f / Screen.height));
                var translation = new Vector3(vector.y * Time.deltaTime, 0, -vector.x * Time.deltaTime);
                if (transform.position.x + translation.x > MaxX || transform.position.x + translation.x < MinX)
                {
                    translation.x = 0;
                }
                if (transform.position.z + translation.z > MaxZ || transform.position.z + translation.z < MinZ)
                {
                    translation.z = 0;
                }
                transform.position += translation;
                _intermediateTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended && _isSwipeInitiated)
            {
                _isSwipeInitiated = false;
                _lastTouchPosition = touch.position;
                if (Mathf.Abs(_lastTouchPosition.x - _firstTouchPosition.x) > Screen.height * 0.04 || Mathf.Abs(_lastTouchPosition.y - _firstTouchPosition.y) > Screen.height * 0.04)
                {
                    var vector = (_lastTouchPosition - _intermediateTouchPosition) * CameraMoveSpeed;
                    vector.Scale(new Vector2(1.0f / Screen.width, 1.0f / Screen.height));
                    var translation = new Vector3(vector.y * Time.deltaTime, 0, -vector.x * Time.deltaTime);
                    if (transform.position.x + translation.x > MaxX || transform.position.x + translation.x < MinX)
                    {
                        translation.x = 0;
                    }
                    if (transform.position.z + translation.z > MaxZ || transform.position.z + translation.z < MinZ)
                    {
                        translation.z = 0;
                    }
                    transform.position += translation;
                }               
                _intermediateTouchPosition = _lastTouchPosition;
            }
        }
    }
}