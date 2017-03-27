using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliSelectController : MonoBehaviour
{
    public UnityEngine.UI.Button[] HeliSelectButtons;
    public void SelectHeli(int index)
    {           
        foreach (var b in HeliSelectButtons)
        {
            var colors1 = b.colors;
            var normalColor1 = colors1.normalColor;
            normalColor1.r = 0.33f;
            normalColor1.g = 0.33f;
            normalColor1.b = 0.33f;
            colors1.normalColor = normalColor1;
            b.colors = colors1;
        }        
        var colors = HeliSelectButtons[index].colors;
        var normalColor = colors.normalColor;
        normalColor.r = 255;
        normalColor.g = 255;
        normalColor.b = 255;
        colors.normalColor = normalColor;
        HeliSelectButtons[index].colors = colors;
        var helicoptersController = GetComponent<HelicoptersController>();
        helicoptersController.SelectedHelicoterIndex = index;
    }
}
