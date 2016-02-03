//
//  MapRotation.cs
//  ReCry
//  
//  Created by Lucas Zacharias-Langhans on 05.10.2015
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//

using UnityEngine;
using System.Collections;

public class MapRotation : MonoBehaviour {
    public float speed = 10;
    public GameObject parent;

    void Start()
    {
        parent = GameObject.Find("PivotPoint");
    }

    void Update()
    {
        this.transform.RotateAround(parent.transform.position, new Vector3(0, speed, 0),Time.deltaTime*speed);
    }
}
