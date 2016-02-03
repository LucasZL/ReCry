//
//  MinimapIslandStats.cs
//  ReCry
//  
//  Created by Lucas Zacharias-Langhans on 04.10.2015
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//

using UnityEngine;
using System.Collections;

public class MinimapIslandStats : MonoBehaviour
{
    public int owner = 0;

    void Update()
    {
        Color color;
        if (this.owner == 0)
        {
            color = new Color32(255, 255, 255, 1);
            color.a = 0.05f;
            gameObject.GetComponent<Renderer>().material.color = color;
        }
        else if (this.owner == 1)
        {
            color = new Color32(100, 221, 23, 1);
            color.a = 1f;
            gameObject.GetComponent<Renderer>().material.color = color;
        }
        else if (this.owner == 2)
        {
            color = new Color32(244, 67, 54, 1);
            color.a = 1f;
            gameObject.GetComponent<Renderer>().material.color = color;
        }
        else if (this.owner == 3)
        {
            color = new Color32(11, 188, 201, 1);
            color.a = 1f;
            gameObject.GetComponent<Renderer>().material.color = color;
        }
        else if (this.owner == 4)
        {
            color = new Color32(26, 35, 126, 1);
            color.a = 1f;
            gameObject.GetComponent<Renderer>().material.color = color;
        }
        else if (this.owner == 5)
        {
            color = new Color32(255, 255, 255, 1);
            color.a = 0.015f;
            gameObject.GetComponent<Renderer>().material.color = color;
        }
    }
}
