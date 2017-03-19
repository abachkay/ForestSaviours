using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FiringOld : MonoBehaviour 
{    
	public GameObject Flame;
    public double TreeGettingOnFireTimeSec = 1;
    public double TreeDestroyPollingIntervalSec = 1;    
    public double FireDistance = 0.03;
    public double fireTtl = 10;
    public double fireTtlRandomizeRange = 3;

    private double secondCounter2Destroy = 0;
    private double secondCounter2GetOnFire = 0;
    private Terrain currentTerrain;    
    private bool[] isOnFire;
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
        fires = new GameObject[TreesCount];
        fireTtls = new double[TreesCount];
        nearTreesIndexes = new List<int>[TreesCount];
        var rand = new System.Random();
        for(int i=0;i<TreesCount;i++)
        {
            fireTtls[i] = fireTtl+(rand.Next(0,(int)(fireTtlRandomizeRange*10))/10.0);
            for(int j=0;j<TreesCount;j++)
            {
                if (Vector3.Distance(trees[i].position, trees[j].position) < FireDistance&&i!=j)
                {
                    if (nearTreesIndexes[i] == null)
                    {
                        nearTreesIndexes[i] = new List<int>();
                    }
                    nearTreesIndexes[i].Add(j);                    
                }
            }
        }          
        MakeFlame(0);                            
    }
    void MakeFlame(int treeIndex)
    {
        var treeInstance = trees[treeIndex];
        fires[treeIndex] = Instantiate(Flame);        
        fires[treeIndex].transform.position = new Vector3(treeInstance.position.x * currentTerrain.terrainData.size.x, treeInstance.position.y * currentTerrain.terrainData.size.y, treeInstance.position.z * currentTerrain.terrainData.size.z);
        fires[treeIndex].transform.localScale = new Vector3(treeInstance.widthScale, treeInstance.heightScale, treeInstance.widthScale);
        isOnFire[treeIndex] = true;
    }		
    void KillTree(int treeIndex)
    {
        var list = new List<TreeInstance>(currentTerrain.terrainData.treeInstances);
        var tree = trees[treeIndex];
        tree.prototypeIndex = 1;
        list[treeIndex] = tree;
        currentTerrain.terrainData.treeInstances = list.ToArray();
        Destroy(fires[treeIndex]);
        fires[treeIndex] = null;
    }
    void Update()
    {     
        secondCounter2GetOnFire += Convert.ToDouble(Time.deltaTime);
        secondCounter2Destroy += Convert.ToDouble(Time.deltaTime);
        if (secondCounter2Destroy >= TreeDestroyPollingIntervalSec)
        {
            secondCounter2Destroy = 0;            
            for (int i = 0; i < isOnFire.Length; i++)
            {
                if (isOnFire[i])
                {
                    fireTtls[i] -= TreeDestroyPollingIntervalSec;
                    if (fireTtls[i] <= 0)
                        KillTree(i);
                }
            }            
        }
        if (secondCounter2GetOnFire >= TreeGettingOnFireTimeSec)
        {
            secondCounter2GetOnFire = 0;
            var treeToFireIndexesSet = new HashSet<int>();
            for (int i = 0; i < isOnFire.Length; i++)
            {
                if (isOnFire[i])
                {
                    foreach (var index in nearTreesIndexes[i])
                    {
                        if (!isOnFire[index])
                        {
                            treeToFireIndexesSet.Add(index);
                        }
                    }
                }
            }
            foreach (var i in treeToFireIndexesSet)
            {
                MakeFlame(i);
            }
        }
    }
}
