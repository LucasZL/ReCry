//
//  CharacterMovementMultiplayer.cs
//  ReCry
//  
//  Created by Kevin Holst on 15.01.2015
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Option : MonoBehaviour
{

    public Dropdown Resolution;
    public Dropdown Anti;
    public Dropdown Texture;
    public AudioSource source;

    public Slider Master;

    public Text MasterText;

    void Start()
    {
        Master.value = source.volume;
    }

    public void ChangeResolution(int changed)
    {
        changed = Resolution.value;
        if (changed == 0)
        {
            Screen.SetResolution(1280, 720, true);
        }
        else if (changed == 1)
        {
            Screen.SetResolution(1360, 768, true);
        }
        else if (changed == 2)
        {
            Screen.SetResolution(1366, 768, true);
        }
        else if (changed == 3)
        {
            Screen.SetResolution(1600, 900, true);
        }
        else if (changed == 4)
        {
            Screen.SetResolution(1920, 1080, true);
        }

    }

    public void ChangeAntiAliasing(int changed)
    {
        changed = Anti.value;
        if (changed == 0)
        {
            QualitySettings.antiAliasing = 0;
        }
        else if (changed == 1)
        {
            QualitySettings.antiAliasing = 2;
        }
        else if (changed == 2)
        {
            QualitySettings.antiAliasing = 4;
        }
        else if (changed == 3)
        {
            QualitySettings.antiAliasing = 8;
        }
    }

    public void ChangeTextureQuality(int changed)
    {
        changed = Texture.value;
        if (changed == 0)
        {
            QualitySettings.masterTextureLimit = 0;
        }
        else if (changed == 1)
        {
            QualitySettings.masterTextureLimit = 1;

        }
        else if (changed == 2)
        {
            QualitySettings.masterTextureLimit = 2;

        }
        else if (changed == 3)
        {
            QualitySettings.masterTextureLimit = 3;

        }
    }

    public void SetMasterVolume()
    {
        this.MasterText.text = (int)Master.value * 100 + " % ";
    }

}
