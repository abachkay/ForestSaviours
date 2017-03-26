using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PinchZoom : MonoBehaviour
{        
    void Update()
    {
        if(Application.platform==RuntimePlatform.WindowsEditor|| Application.platform == RuntimePlatform.WindowsPlayer)
        {
            var zoomDelta = Input.GetAxis("Mouse ScrollWheel");
            if (zoomDelta != 0)
            {
                var newCameraY = Mathf.Clamp(transform.position.y - zoomDelta * 2000 * Time.deltaTime, 200, 300);
                transform.position = new Vector3(transform.position.x, newCameraY, transform.position.z);                
            }
        }        
        if (Input.touchCount == 2 && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
            var newCameraY = Mathf.Clamp(transform.position.y + deltaMagnitudeDiff * 20 * Time.deltaTime, 200, 300);            
            transform.position=new Vector3(transform.position.x, newCameraY, transform.position.z);
        }
    }
}