﻿using UnityEngine;
using System.Collections;

public class RayCastShoot : MonoBehaviour
{

    float maxRange;

    // Update is called once per frame
    void Update()
    {
        RayCastFire();
    }

    void RayCastFire()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            RaycastHit hit;
            Ray RayCast = new Ray(this.transform.position, this.transform.forward);
            Debug.DrawRay(this.transform.position, this.transform.forward, Color.red);
            if (Physics.Raycast(RayCast, out hit))
            {
                maxRange = hit.distance;
                Debug.Log("Hit");
                Debug.Log(hit.transform.tag.ToString());
                if (hit.collider.gameObject.tag == "Player")
                {
                    Debug.Log("Penis");
                }
            }
        }
    }
}
