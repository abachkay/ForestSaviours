using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    public GameObject FireObject;    
    public GameObject TerrainObject;
    public GameObject BurntSpot;

    public UnityEngine.UI.Text TreesPercentageText;
    public GameObject ResultMenu;
    public UnityEngine.UI.Text ResultText;
    public GameObject WindArrow;
    public UnityEngine.UI.Text WindSpeedText;    

    public float RadiationCoeficient = 10;
    public float DamageCoeficient = 1;
    public int[] InitialFireTreeIndexes;
    public float[] InitialFireTreeRadiuses;        

    public float WindSpeed = 5;
    public int WindAngle = 0;
    public float WindAngleAmplitude = 20f;
    public float WindSpeedAmplitude = 0.3f;
    public float WindAngleChangeDelay = 3f;

    public int LevelIndex = 0;

    private TreeInstance[] _trees;
    private GameObject[] _fires;
    private float[] _treesHealth;
    private float[] _treesFireResistance;
    private TreeState[] _treesStates; 
    private Dictionary<int, float>[] _nearTrees;
    private float _pollingTimeStep = 0.1f;
    private float _fireDistance = 0.04f;
    private float _pollingCounter = 0;
    private float _waterRadius = 20;
    private GameObject[] _spots;
    private float _windCounter = 0;

    void Start ()
    {                
        if (WindArrow != null && WindSpeedText != null)
        {
            WindSpeedText.text = WindSpeed.ToString() + "m/s";
            WindArrow.transform.rotation = Quaternion.Euler(new Vector3( 0,0,-WindAngle));            
        }
        var newTerrainData = Instantiate(TerrainObject.GetComponent<Terrain>().terrainData);
        TerrainObject.GetComponent<Terrain>().terrainData = newTerrainData;
        _trees = TerrainObject.GetComponent<Terrain>().terrainData.treeInstances;
        //Debug.Log(_trees.Length);
        _fires = new GameObject[_trees.Length];
        _spots = new GameObject[_trees.Length];
        _treesHealth = new float[_trees.Length];
        _treesFireResistance = new float[_trees.Length];
        _treesStates = new TreeState[_trees.Length];
        _nearTrees = new Dictionary<int, float>[_trees.Length];
        for (int i=0; i<_trees.Length; i++)
        {

            _treesHealth[i] = Random.Range(100, 120);
            _treesFireResistance[i] = Random.Range(100, 120);
            _treesStates[i] = TreeState.Ok;            
            for(int j=0;j<_trees.Length;j++)
            {
                Vector2 position2d = new Vector2(_trees[i].position.z, _trees[i].position.x);
                position2d.x += WindSpeed / 1000 * Mathf.Cos(WindAngle * Mathf.PI / 180);
                position2d.y += WindSpeed / 1000 * Mathf.Sin(WindAngle * Mathf.PI / 180);
                var distance = Vector3.Distance(new Vector3(position2d.y, _trees[i].position.y, position2d.x), _trees[j].position);
                if (i != j && distance < _fireDistance)
                {
                    if (_nearTrees[i] == null)
                    {
                        _nearTrees[i] = new Dictionary<int,float>();
                    }
                    _nearTrees[i].Add(j, distance);
                }
            }
        }
        for (int i = 0; i < InitialFireTreeIndexes.Length; i++)
        {
            for (int j = 0; j < _trees.Length; j++)
            {
                if (Vector3.Distance(_trees[InitialFireTreeIndexes[i]].position, _trees[j].position) < InitialFireTreeRadiuses[i])
                {
                    _treesStates[j] = TreeState.FirePeak;
                    PutOnFire(j);
                }
            }        
        }
    }
		
	void Update ()
    {
        _pollingCounter += Time.deltaTime;
        if(_pollingCounter > _pollingTimeStep)
        {
            bool isGameOver = true;
            float savedTreesCount = 0f;
            _pollingCounter = 0;            
            for(int i=0;i<_trees.Length;i++)
            {                                
                if (_treesStates[i] != TreeState.Dead && _treesStates[i] != TreeState.Ok)
                {
                    isGameOver = false;
                    _treesHealth[i] -= DamageCoeficient;
                    if (_treesHealth[i] < 0)
                    {
                        PutOutFire(i);
                        _treesStates[i] = TreeState.Dead;
                        ConvertToDeadTree(i);
                    }                    
                    else if (_nearTrees[i] != null)
                    {
                        foreach(var j in _nearTrees[i])
                        {
                            if(_treesStates[j.Key]==TreeState.Ok)
                            {
                                _treesFireResistance[j.Key] -= RadiationCoeficient * 0.00005f / _nearTrees[i][j.Key] / _nearTrees[i][j.Key];
                            }
                        }
                    }
                    if (_treesHealth[i]<80 && _treesStates[i] == TreeState.FireStart)
                    {
                        _treesStates[i] = TreeState.FirePeak;
                        _fires[i].GetComponent<ParticleSystem>().startSize = 20;
                        _fires[i].GetComponent<ParticleSystem>().playbackSpeed = 4;                   
                    }
                    else if (_treesHealth[i]<20 && _treesStates[i] == TreeState.FirePeak)
                    {
                        _treesStates[i] = TreeState.FireEnd;                     
                        _fires[i].GetComponent<ParticleSystem>().startSize = 10;
                        _fires[i].GetComponent<ParticleSystem>().playbackSpeed = 1.5f;                       
                    }
                }
            }
            for (int i = 0; i < _trees.Length; i++)
            {
                if(_treesStates[i] == TreeState.Ok && _treesFireResistance[i]<=0)
                {
                    _treesStates[i] = TreeState.FireStart;
                    PutOnFire(i);                    
                }
            }
            // Calculating saved trees count
            for (int i = 0; i < _trees.Length; i++)
            {
                if (_treesStates[i] != TreeState.Dead)
                {
                    savedTreesCount++;
                }
                if (TreesPercentageText != null)
                {
                    TreesPercentageText.text = ((int)(savedTreesCount / _trees.Length * 100)).ToString() + "%";
                }
            }
            // Game over ui respond
            if (isGameOver == true && ResultMenu != null && ResultText != null)
            {
                foreach (var audio in Camera.main.GetComponents<AudioSource>())
                {
                    audio.Pause();
                }
                ResultMenu.SetActive(true);
                ResultText.text = "Result: " + ((int)(savedTreesCount / _trees.Length * 100)).ToString() + "%";
                if (PlayerPrefs.GetInt(LevelIndex.ToString() + "%") < (int)(savedTreesCount / _trees.Length * 100))
                {
                    PlayerPrefs.SetInt(LevelIndex.ToString() + "%", (int)(savedTreesCount / _trees.Length * 100));
                }
                Time.timeScale = 0;
            }
            // Check house on 6 level
            if (LevelIndex == 5)
            {
                if(_fires.Length == 1238 && _fires[1237] != null)
                {
                    ResultMenu.SetActive(true);
                    ResultText.text = "Game over";
                    Time.timeScale = 0;
                }
            }
        }

        // Wind speed and angle random change
        _windCounter += Time.deltaTime;
        if (_windCounter > WindAngleChangeDelay)
        {
            _windCounter = 0;
            var angle = WindAngle + Random.Range(-WindAngleAmplitude, WindAngleAmplitude);
            var speed =  WindSpeed + Random.Range(-WindSpeedAmplitude, WindSpeedAmplitude);
            if (WindArrow != null && WindSpeedText != null)
            {
                WindSpeedText.text = string.Format("{0:0.0} м/с", speed);
                WindArrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));
            }
        }       
    }

    private void PutOnFire(int index)
    {
        var terrainSize = TerrainObject.GetComponent<Terrain>().terrainData.size;
        _fires[index] = Instantiate(FireObject, TerrainObject.GetComponent<Transform>());
        _fires[index].transform.position = new Vector3(_trees[index].position.x * terrainSize.x, 
            _trees[index].position.y*terrainSize.y, _trees[index].position.z * terrainSize.z);
        _fires[index].GetComponent<ParticleSystem>().Play();
    }

    private void PutOutFire(int index)
    {
        Destroy(_fires[index]);
        _fires[index] = null;        
    }
    public void PutOutFire(GameObject water)
    {
        for (int i = 0; i < _trees.Length; i++)
        {
            if (_fires[i] && Vector3.Distance(_fires[i].transform.position, new Vector3(water.transform.position.x, _fires[i].transform.position.y, water.transform.position.z)) <= _waterRadius)
            {
                PutOutFire(i);
                _treesStates[i] = TreeState.Ok;
                _treesFireResistance[i] = 1000;
            }
        }
    }
    private void ConvertToDeadTree(int index)
    {
        // Scale tree down to 0 to make him gone
        var list = new List<TreeInstance>(_trees);
        var tree = _trees[index];
        tree.widthScale = 0;
        tree.heightScale = 0;
        list[index] = tree;
        TerrainObject.GetComponent<Terrain>().terrainData.treeInstances = list.ToArray();
        _trees = TerrainObject.GetComponent<Terrain>().terrainData.treeInstances;
        // Make burnt ground spot
        if (BurntSpot != null)
        {
            _spots[index] = Instantiate(BurntSpot, TerrainObject.transform);
            var terrainSize = TerrainObject.GetComponent<Terrain>().terrainData.size;
            _spots[index].transform.position = new Vector3(_trees[index].position.x * terrainSize.x,
            _trees[index].position.y * terrainSize.y + 0.01f, _trees[index].position.z * terrainSize.z);
            _spots[index].transform.Rotate(new Vector3(0, 0, Random.Range(0, 360)));
        }
    }
    private enum TreeState : int
    {
        Ok = 0,
        FireStart = 1,
        FirePeak = 2,
        FireEnd = 3,
        Dead = 4,
    }
}
