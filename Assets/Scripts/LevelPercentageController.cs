using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPercentageController : MonoBehaviour
{
    public UnityEngine.UI.Text[] PercentageTexts;	
	void Start ()
    {
	    for(int i=0;i<PercentageTexts.Length;i++)
        {            
            PercentageTexts[i].text = PlayerPrefs.GetInt(i.ToString()+"%").ToString()+"%";
        }
	}		
}
