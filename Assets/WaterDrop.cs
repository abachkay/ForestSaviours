using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrop : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    public GameObject prefab;
    private float _lastMousePositionX=0;
    private float _lastMousePositionY = 0;
    void Update()
    {
        if (Input.GetMouseButton(0))
        {        
            _lastMousePositionX = Input.GetAxis("Mouse X");
            _lastMousePositionY = Input.GetAxis("Mouse Y");            
        }
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonUp(0)&& Input.GetAxis("Mouse X")==_lastMousePositionX&& Input.GetAxis("Mouse X")==_lastMousePositionY)
            {
                GameObject obj = Instantiate(prefab, new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.identity) as GameObject;
            }
        }
    }
}
