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

    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position

    void Update()
    {        
        //    var translation = Vector3.zero;
        //    // Zoom in or out
        //    //var zoomDelta = Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed * Time.deltaTime;
        //    //if (zoomDelta != 0)
        //    //{
        //    //    translation -= Vector3.up * ZoomSpeed * zoomDelta;
        //    //}
        //    if (Input.GetMouseButton(0))
        //    {
        //        translation -= new Vector3(Input.GetAxis("Mouse Y") * -1 * DragSpeed * Time.deltaTime, 0,
        //                           Input.GetAxis("Mouse X") * DragSpeed * Time.deltaTime);
        //    }
        //    var desiredPosition = GetComponent<Camera>().transform.position + translation;
        //    if (desiredPosition.x < DragXMin || DragXMax < desiredPosition.x)
        //    {
        //        translation.x = 0;
        //    }
        //    if (desiredPosition.y < ZoomMin || ZoomMax < desiredPosition.y)
        //    {
        //        translation.y = 0;
        //    }
        //    if (desiredPosition.z < DragYMin || DragYMax < desiredPosition.z)
        //    {
        //        translation.z = 0;
        //    }
        //    transform.position += translation;

        if (Input.touchCount == 1) 
        {
            Touch touch = Input.GetTouch(0); 
            if (touch.phase == TouchPhase.Began)
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Ended) 
            {
                fp = lp;
                lp = touch.position;
                if (Mathf.Abs(lp.x - fp.x) > Screen.height * 0.1 || Mathf.Abs(lp.y - fp.y) > Screen.height * 0.1)
                {
                    var vector = lp - fp;
                    transform.position += new Vector3(vector.y*Time.deltaTime, 0, -vector.x* Time.deltaTime);
                }
            }                             
        }
    }
}