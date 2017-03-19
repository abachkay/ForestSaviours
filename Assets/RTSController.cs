using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSController : MonoBehaviour
{
    public int ZoomSpeed = 400;
    public int DragSpeed = 100;    

    public int ZoomMin = 100;
    public int ZoomMax = 400;

    public int DragXMin = 400;
    public int DragXMax = 1000;
    public int DragYMin = 200;
    public int DragYMax = 900;

    // Update is called once per frame
    void Update()
    {
        // Init camera translation for this frame.
        var translation = Vector3.zero;

        // Zoom in or out
        var zoomDelta = Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed * Time.deltaTime;       
        if (zoomDelta != 0)
        {
            translation -= Vector3.up * ZoomSpeed * zoomDelta;
        }

        // Move camera with mouse
        if (Input.GetMouseButton(0)) 
        {                
            translation -= new Vector3(Input.GetAxis("Mouse Y") * -1*DragSpeed * Time.deltaTime, 0,
                               Input.GetAxis("Mouse X") * DragSpeed * Time.deltaTime);
        }

       // Keep camera within level and zoom area
        var desiredPosition = GetComponent<Camera>().transform.position + translation;
        if (desiredPosition.x < DragXMin || DragXMax < desiredPosition.x)
        {
            translation.x = 0;
        }
        if (desiredPosition.y < ZoomMin || ZoomMax < desiredPosition.y)
        {
            translation.y = 0;
        }
        if (desiredPosition.z < DragYMin || DragYMax < desiredPosition.z)
        {
            translation.z = 0;
        }
        // Finally move camera parallel to world axis
        GetComponent<Camera>().transform.position += translation;
    }
}