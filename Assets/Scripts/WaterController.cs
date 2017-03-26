using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour {
    public GameObject TerrainObject;
    private bool isDead;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var ps = GetComponent<ParticleSystem>();
        if (ps.particleCount == 0)
        {
            var firingController = TerrainObject.GetComponent<FiringOld>();
            firingController.PutOutFire(gameObject);
            Destroy(gameObject);                        
        }
	}
}
