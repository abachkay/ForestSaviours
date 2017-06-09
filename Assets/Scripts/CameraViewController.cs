using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraViewController : MonoBehaviour
{      
    private Vector2 _firstTouchPosition;
    private Vector2 _intermediateTouchPosition;
    private Vector2 _lastTouchPosition;
    private bool _isSwipeInitiated = false;
    private float _terrainAverageHeight = 5;

    private void Update()
    {
        //For Windows.
//#if UNITY_EDITOR
        if ((Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && 
            //!UnityEditor.EditorApplication.isRemoteConnected &&
            (UnityEngine.EventSystems.EventSystem.current == null || UnityEngine.EventSystems.EventSystem.current != null && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()))
        {
            if (Input.GetMouseButton(0))
            {
                var translation = new Vector3(Input.GetAxis("Mouse X") * -300 * Time.deltaTime, 0, Input.GetAxis("Mouse Y") * -300 * Time.deltaTime);
                if (GetCameraOutOfGameAreaDistanceX(translation) != 0)
                {
                    translation.x = 0;
                }
                if (GetCameraOutOfGameAreaDistanceZ(translation) != 0)
                {
                    translation.z = 0;
                }
                transform.position += translation;
            }
        }
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            var zoomDelta = Input.GetAxis("Mouse ScrollWheel");
            if (zoomDelta != 0)
            {
                var deltaY = zoomDelta * -1500 * Time.deltaTime;
                var translation = new Vector3(0, deltaY, 0);
                if (GetCameraOutOfGameAreaDistanceZ(translation) != 0)
                {                    
                    translation += new Vector3(0, 0, -GetCameraOutOfGameAreaDistanceZ(translation));
                }
                if (GetCameraOutOfGameAreaDistanceX(translation) != 0)
                {
                    translation += new Vector3(-GetCameraOutOfGameAreaDistanceX(translation), 0, 0);
                }
                if (GetCameraOutOfGameAreaDistanceX(translation) != 0 || GetCameraOutOfGameAreaDistanceZ(translation) != 0 || (transform.position + translation).y < 100)
                {
                    translation = new Vector3(0, 0, 0);
                }
                transform.position += translation;
            }
        }
//#endif
        //Input from mobile.
        if (Input.touchCount == 2 && (UnityEngine.EventSystems.EventSystem.current == null ||
            UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null))
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
            var deltaY = deltaMagnitudeDiff * 20 * Time.deltaTime;
            var translation = new Vector3(0, deltaY, 0);
            if (GetCameraOutOfGameAreaDistanceZ(translation) != 0)
            {
                translation += new Vector3(0, 0, -GetCameraOutOfGameAreaDistanceZ(translation));
            }
            if (GetCameraOutOfGameAreaDistanceX(translation) != 0)
            {
                translation += new Vector3(-GetCameraOutOfGameAreaDistanceX(translation), 0, 0);
            }
            if (GetCameraOutOfGameAreaDistanceX(translation) != 0 || GetCameraOutOfGameAreaDistanceZ(translation) != 0 || (transform.position + translation).y < 150)
            {
                translation = new Vector3(0, 0, 0);
            }
            transform.position += translation;
            _isSwipeInitiated = false;
        }        
        if (Input.touchCount == 1 && (UnityEngine.EventSystems.EventSystem.current == null || 
            UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null))
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
                var vector = (_lastTouchPosition - _intermediateTouchPosition) * 5000f;
                vector.Scale(new Vector2(1.0f / Screen.width, 1.0f / Screen.height));
                var translation = new Vector3(-vector.x * Time.deltaTime, 0, -vector.y * Time.deltaTime);
                if (GetCameraOutOfGameAreaDistanceX(translation) != 0)
                {
                    translation.x = 0;
                }
                if (GetCameraOutOfGameAreaDistanceZ(translation) != 0)
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
                    var vector = (_lastTouchPosition - _intermediateTouchPosition) * 5000f;                    
                    vector.Scale(new Vector2(1.0f / Screen.width, 1.0f / Screen.height));
                    var translation = new Vector3(-vector.x * Time.deltaTime, 0, -vector.y * Time.deltaTime);
                    if (GetCameraOutOfGameAreaDistanceX(translation) != 0)
                    {
                        translation.x = 0;
                    }
                    if (GetCameraOutOfGameAreaDistanceZ(translation) != 0)
                    {
                        translation.z = 0;
                    }
                    transform.position += translation;
                }
                _intermediateTouchPosition = _lastTouchPosition;
            }
        }
    }

    private float GetCameraOutOfGameAreaDistanceZ(Vector3 translation)
    {
        var terrainMinZ = Terrain.activeTerrain.transform.position.z;
        var terrainMaxZ = terrainMinZ + Terrain.activeTerrain.terrainData.size.z;        
        var cameraVerticalViewAngle = Camera.main.fieldOfView * Mathf.PI / 180f;
        var cameraTransformAngleX = Camera.main.transform.rotation.eulerAngles.x * Mathf.PI / 180f;
        var cameraPosition = Camera.main.transform.position + translation;
        var cameraMaxZ = cameraPosition.z + (cameraPosition.y - _terrainAverageHeight) * Mathf.Tan(((Mathf.PI / 2 - cameraTransformAngleX) + cameraVerticalViewAngle / 2));
        var cameraMinZ = cameraPosition.z + (cameraPosition.y - _terrainAverageHeight) * Mathf.Tan(((Mathf.PI / 2 - cameraTransformAngleX) - cameraVerticalViewAngle / 2));
        //Debug.Log(cameraMaxZ + " " + cameraMinZ);
        if (cameraMaxZ > terrainMaxZ)
        {
            return +cameraMaxZ - terrainMaxZ;
        }
        else if (cameraMinZ < terrainMinZ)
        {
            return +cameraMinZ - terrainMinZ;
        }
        else
        {
            return 0;
        }        
    }

    private float GetCameraOutOfGameAreaDistanceX(Vector3 translation)
    {
        var terrainMinX = Terrain.activeTerrain.transform.position.x;
        var terrainMaxX = terrainMinX + Terrain.activeTerrain.terrainData.size.x;
        var cameraTransformAngleX = Camera.main.transform.rotation.eulerAngles.x * Mathf.PI / 180f;
        var cameraVerticalViewAngle = Camera.main.fieldOfView * Mathf.PI / 180f;     
        var cameraPosition = Camera.main.transform.position + translation;
        var l = (cameraPosition.y - _terrainAverageHeight) / Mathf.Cos((Mathf.PI / 2 - cameraTransformAngleX) + cameraVerticalViewAngle / 2);
        var l1 = l * Mathf.Sin(cameraVerticalViewAngle / 2);
        var l2 = l1 * Screen.width / Screen.height;
        var cameraMaxX = cameraPosition.x + l2;
        var cameraMinX = cameraPosition.x - l2;
        //Debug.Log(cameraMaxX.ToString() + " " + cameraMinX.ToString()+" "+ cameraPosition.x);
        if (cameraMaxX > terrainMaxX)
        {
            return cameraMaxX - terrainMaxX;
        }
        else if (cameraMinX < terrainMinX)
        {
            return cameraMinX - terrainMinX;
        }
        else
        {
            return 0;
        }        
    }
}