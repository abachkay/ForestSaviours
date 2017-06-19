using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{        
	void Update ()
    {
        var ps = GetComponent<ParticleSystem>();
        if (ps.particleCount == 0)
        {
            var fireController = Terrain.activeTerrain.gameObject.GetComponent<FireController>();
            fireController.PutOutFire(gameObject);
            Destroy(gameObject);                        
        }
	}
}
