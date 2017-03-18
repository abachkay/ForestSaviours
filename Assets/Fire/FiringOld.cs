using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firing : MonoBehaviour 
{
	public GameObject Flame;
    private int delayCounter = 0;
    private bool[] isOnFire;
    private TreeInstance[] trees;
	void Start ()
    {
        trees = Terrain.activeTerrain.terrainData.treeInstances;
        var startTree = trees[0];
        isOnFire = new bool[trees.Length];
        isOnFire[0] = true;
        MakeFlame(startTree);        
	}

    void MakeFlame(TreeInstance treeTransform)
    {
        var flame = Instantiate(Flame);
        flame.transform.localScale = new Vector3(2, 2, 2);
        flame.transform.position = new Vector3(treeTransform.position.x * 500, treeTransform.position.y * 500, treeTransform.position.z * 500);
        flame.transform.localScale = new Vector3(treeTransform.widthScale, treeTransform.heightScale, treeTransform.widthScale);
        
    }		
	void Update ()
    {
        delayCounter++;
        
        if (delayCounter > 60)
        {
            var set = new HashSet<int>();
            delayCounter = 0;
            for (int i = 0; i < isOnFire.Length; i++)
            {
                if (isOnFire[i])
                {                    
                    for (int j = 0; j < isOnFire.Length; j++)
                    {                        
                        if (!isOnFire[j] && Vector3.Distance(trees[i].position, trees[j].position) < 0.04)
                        {
                            set.Add(j);                            
                        }
                    }
                }
            }
            foreach(var i in set)
            {
                isOnFire[i] = true;
                MakeFlame(trees[i]);
            }
        }
    }
}
