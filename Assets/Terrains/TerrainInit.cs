using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainInit : MonoBehaviour
{
    public GameObject TerrainPrefab;    
	void Start ()
    {        
        var terrainGameObject = Instantiate(TerrainPrefab);        
        var terrain = terrainGameObject.GetComponent<Terrain>();        
        TerrainData newTerrainData = Instantiate(terrain.terrainData);
        terrain.terrainData = newTerrainData;                
    }	  
}