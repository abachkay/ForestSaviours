using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider slider;
    void Start()
    {
        slider.value = AudioListener.volume;
    }
    public void VolumeChanged()
    {
        AudioListener.volume=slider.value;
    }
}
