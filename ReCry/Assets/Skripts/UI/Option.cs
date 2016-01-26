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
    public Toggle VSync;
    public Dropdown AnisotropicFiltering;
    public Slider ShadowDistance;
    public Slider MouseSensitivity;
    public Toggle FullScreen;
    public AudioSource source;

    public Slider Master;

    public Text MasterText;
    public Text ShadowText;
    public Text MouseSensitivityText;

    void Start()
    {
        FullScreen.isOn = PlayerPrefs.GetInt("Screenmanager Is Fullscreen mode") != 0;
        Resolution.value = PlayerPrefs.GetInt("ScreenmanagerSelectedResolution");
        Anti.value = PlayerPrefs.GetInt("QualityAntiAliasing");
        Texture.value = PlayerPrefs.GetInt("QualityTexture");
        MouseSensitivity.value = PlayerPrefs.GetFloat("MouseSensitivity");
        //Polishing
        VSync.isOn = PlayerPrefs.GetInt("VSync") != 0;
        AnisotropicFiltering.value = PlayerPrefs.GetInt("QualityAnisotropicFiltering");
        ShadowDistance.value = PlayerPrefs.GetFloat("QualityShadowDistance");
    }

    public void IsFullScreen(bool changed)
    {
        if (FullScreen.isOn)
        {
            Screen.fullScreen = changed;
        }
        else
        {
            Screen.fullScreen = changed;
        }
    }

    public void ChangeResolution(int changed)
    {
        changed = Resolution.value;
        if (FullScreen.isOn)
        {
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
        else
        {
            if (changed == 0)
            {
                Screen.SetResolution(1280, 720, false);
            }
            else if (changed == 1)
            {
                Screen.SetResolution(1360, 768, false);
            }
            else if (changed == 2)
            {
                Screen.SetResolution(1366, 768, false);
            }
            else if (changed == 3)
            {
                Screen.SetResolution(1600, 900, false);
            }
            else if (changed == 4)
            {
                Screen.SetResolution(1920, 1080, false);
            }
        }
        PlayerPrefs.SetInt("ScreenmanagerSelectedResolution", changed);

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
        PlayerPrefs.SetInt("QualityAntiAliasing",changed);
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
        PlayerPrefs.SetInt("QualityTexture", changed);
    }

    public void ChangeVSync()
    {
        int changed;
        if (VSync.isOn)
        {
            changed = 1;
            QualitySettings.vSyncCount = changed;
        }
        else
        {
            changed = 0;
            QualitySettings.vSyncCount = 0;
        }
        PlayerPrefs.SetInt("VSync", changed);
    }

    public void ChangeAnisotropicFiltering(int changed)
    {
        changed = AnisotropicFiltering.value;
        if (changed == 0)
        {
            QualitySettings.anisotropicFiltering = UnityEngine.AnisotropicFiltering.Disable;
        }
        else if (changed == 1)
        {
            QualitySettings.anisotropicFiltering = UnityEngine.AnisotropicFiltering.Enable;
        }
        PlayerPrefs.SetInt("QualityAnisotropicFiltering", changed);
    }


    public void ChangeShadowDistance(float changed)
    {
        QualitySettings.shadowDistance = changed;
        this.ShadowText.text = changed.ToString();
        PlayerPrefs.SetFloat("QualityShadowDistance", changed);
    }

    public void ChangeMouseSensitivity(float changed)
    {
        Utility.MouseSensitivity = changed;
        this.MouseSensitivityText.text = changed.ToString();
        PlayerPrefs.SetFloat("MouseSensitivity", changed);
    }

    public void SetMasterVolume()
    {
        
    }

}
