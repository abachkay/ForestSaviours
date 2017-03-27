using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FiringOld : MonoBehaviour 
{    
	public GameObject Flame;
    public double TreeGettingOnFireTimeSec = 1;
    public double TreeDestroyPollingIntervalSec = 0.1;    
    public double FireDistance = 0.03;
    public double fireTtl = 7;
    public double fireTtlRandomizeRange = 3;
    public double WaterRadius = 10;
    public int deadTreeIndex = 0;
    public int[] InitialFireTreeIndexes = { 0,1 };
    public UnityEngine.UI.Text PercentageText;
    public Canvas GameOverMenu;
    public UnityEngine.UI.Text GameOverText;
    public int levelIndex=0;

    private double secondCounter2Destroy = 0;
    private double secondCounter2GetOnFire = 0;
    private Terrain currentTerrain;    
    private bool[] isOnFire;
    private bool[] isPassedFire;
    private TreeInstance[] trees;
    private GameObject[] fires;
    private List<int>[] nearTreesIndexes;
    private double[] fireTtls;
    private int TreesCount
    {
        get
        {
            return trees.Length;
        }
    }        
	void Start ()
    {        
        currentTerrain = Terrain.activeTerrain;
        trees = currentTerrain.terrainData.treeInstances;        
        isOnFire = new bool[TreesCount];
        isPassedFire = new bool[TreesCount];
        fires = new GameObject[TreesCount];
        fireTtls = new double[TreesCount];
        nearTreesIndexes = new List<int>[TreesCount];
        var rand = new System.Random();
        Debug.Log($"Tree count: {TreesCount}");
        for(int i=0;i<TreesCount;i++)
        {
            fireTtls[i] = fireTtl+rand.Next(0,(int)(fireTtlRandomizeRange*10))/10.0;
            for (int j = 0; j < TreesCount; j++)
            {
                if (Vector3.Distance(trees[i].position, trees[j].position) < FireDistance && i != j)
                {
                    if (nearTreesIndexes[i] == null)
                    {
                        nearTreesIndexes[i] = new List<int>();
                    }
                    nearTreesIndexes[i].Add(j);
                }
            }
        }
        foreach (var i1 in InitialFireTreeIndexes)
        {
            MakeFlame(i1);
            foreach (var i2 in nearTreesIndexes[i1])
            {
                MakeFlame(i2);
            }
        }
    }
    void MakeFlame(int treeIndex)
    {
        if (!isOnFire[treeIndex])
        {
            var treeInstance = trees[treeIndex];
            fires[treeIndex] = Instantiate(Flame);
            fires[treeIndex].transform.position = new Vector3(treeInstance.position.x * currentTerrain.terrainData.size.x, treeInstance.position.y * currentTerrain.terrainData.size.y, treeInstance.position.z * currentTerrain.terrainData.size.z);
            fires[treeIndex].transform.localScale = new Vector3(treeInstance.widthScale, treeInstance.heightScale, treeInstance.widthScale);
            isOnFire[treeIndex] = true;
        }
    }		
    void KillTree(int treeIndex)
    {
        var list = new List<TreeInstance>(currentTerrain.terrainData.treeInstances);
        var tree = trees[treeIndex];
        tree.prototypeIndex = deadTreeIndex;
        list[treeIndex] = tree;
        currentTerrain.terrainData.treeInstances = list.ToArray();       
    }    
    void KillFire(int treeIndex)
    {
        Destroy(fires[treeIndex]);
        fires[treeIndex] = null;
        isOnFire[treeIndex] = false;
    }
    void Update()
    {     
        secondCounter2GetOnFire += Convert.ToDouble(Time.deltaTime);
        secondCounter2Destroy += Convert.ToDouble(Time.deltaTime);
        if (secondCounter2Destroy >= TreeDestroyPollingIntervalSec)
        {            
            secondCounter2Destroy = 0;            
            PercentageText.text = ((int)(100 * fireTtls.Count(x => x > 0) / TreesCount)).ToString() + "%";
            if (fires.Count(x => x != null) == 0)
            {
                int percentage = ((int)(100 * fireTtls.Count(x => x > 0) / TreesCount));
                if (PlayerPrefs.GetInt(levelIndex.ToString() + "%") < percentage)
                {
                    PlayerPrefs.SetInt(levelIndex.ToString() + "%", percentage);
                }
                GameOverMenu.enabled = true;
                GameOverText.text = percentage.ToString() + "%";
                foreach(var audio in Camera.main.GetComponents<AudioSource>())
                {
                    audio.Pause();
                }
            }
            for (int i = 0; i < isOnFire.Length; i++)
            {                
                if (isOnFire[i])
                {                    
                    fireTtls[i] -= TreeDestroyPollingIntervalSec;
                    if (fireTtls[i] <= 0)
                    {
                        KillTree(i);
                        KillFire(i);
                    }
                }
            }            
        }
        if (secondCounter2GetOnFire >= TreeGettingOnFireTimeSec)
        {                       
            secondCounter2GetOnFire = 0;
            var treeToFireIndexesSet = new HashSet<int>();            
            for (int i = 0; i < isOnFire.Length; i++)
            {                           
                if (isOnFire[i]&&!isPassedFire[i])
                {
                    foreach (var index in nearTreesIndexes[i])
                    {
                        if (!isOnFire[index])
                        {
                            treeToFireIndexesSet.Add(index);
                        }
                    }
                    isPassedFire[i] = true;
                }
            }
            foreach (var i in treeToFireIndexesSet)
            {
                MakeFlame(i);
            }
        }
    }
    public void PutOutFire(GameObject water)
    {                
        for(int i=0;i<TreesCount;i++)
        {            
            if(fires[i]&&Vector3.Distance(fires[i].transform.position,new Vector3(water.transform.position.x,fires[i].transform.position.y,water.transform.position.z))<=WaterRadius)
            {
                secondCounter2GetOnFire = 0;
                isOnFire[i] = false;
                KillFire(i);
            }
        }
    }
}
