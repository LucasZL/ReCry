//
//  PLayerIndicatorUpdater.cs
//  ReCry
//  
//  Created by Kevin Holst on 04.02.2016
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//


using UnityEngine;
using System.Collections;

public class PlayerIndicatorUpdater : MonoBehaviour
{

    [SerializeField]
    private RectTransform imagetransform;
    [SerializeField]
    private Vector2 map;
    [SerializeField]
    private Camera minimapCamera;

    public Camera MiniMapCamera
    {
        get { return minimapCamera; }
    }

    public Transform PlayerTransform { get; set; }

    public Vector2 Map
    {
        get { return map; }
    }

    public Vector2 InvMap { get { return new Vector2(1 / Map.x, 1 / Map.y); } }

    public RectTransform ImageTransform
    {
        get { return imagetransform; }
    }


    void Update()
    {
        if (!PlayerTransform)
        {
            return;
        }
        var viewport = MiniMapCamera.WorldToViewportPoint(PlayerTransform.position);
        ((RectTransform)transform).anchoredPosition = new Vector2(viewport.x * imagetransform.rect.width, viewport.y * imagetransform.rect.height);
    }
}
