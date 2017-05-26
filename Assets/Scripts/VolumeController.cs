using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public UnityEngine.UI.Image image;
    public Sprite ImageOn;
    public Sprite ImageOff;
    void Start()
    {
        if (AudioListener.volume == 0)
        {
            image.sprite = ImageOff;
        }
        else
        {
            image.sprite = ImageOn;
        }

    }
    public void SwitchVolume()
    {
        if (AudioListener.volume == 0)
        {
            AudioListener.volume = 1;
            image.sprite = ImageOn;
        }
        else
        {
            AudioListener.volume = 0;
            image.sprite = ImageOff;
        }
    }
}
