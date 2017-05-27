using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoResourceTextController : MonoBehaviour {

    public float MessageTimeToLive = 2f;
    private float _counter;

    private void Start()
    {
        _counter = MessageTimeToLive;
    }

    private void Update ()
    {
        if (_counter > 0)
        {
            _counter -= Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
        }
	}
    public void Show()
    {
        _counter = MessageTimeToLive;
        gameObject.SetActive(true);
    }
}
