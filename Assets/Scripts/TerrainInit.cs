using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainInit : MonoBehaviour
{
    public GameObject TerrainPrefab;
    public UnityEngine.UI.Text PercentageText;
    public Canvas PauseMenu;
    public Canvas GameOverMenu;
    public UnityEngine.UI.Text GameOverText;
    void Start ()
    {
        Time.timeScale = 1;
        PauseMenu.enabled = false;
        GameOverMenu.enabled = false;
        var terrainGameObject = Instantiate(TerrainPrefab);        
        var terrain = terrainGameObject.GetComponent<Terrain>();        
        TerrainData newTerrainData = Instantiate(terrain.terrainData);
        terrain.terrainData = newTerrainData;        
        var heliController = GetComponent<HeliController>();
        heliController.TerrainObject = terrainGameObject;
        Debug.Log(terrain.GetComponent<FiringOld>());
        var firingController = terrain.GetComponent<FiringOld>();
        firingController.PercentageText = PercentageText;
        firingController.GameOverMenu = GameOverMenu;
        firingController.GameOverText = GameOverText;
    }	
}