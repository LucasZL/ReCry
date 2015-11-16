//
//  CharacterMovementMultiplayer.cs
//  ReCry
//  
//  Created by Kevin Holst on 22.09.2015
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//

using UnityEngine;
using System.Collections;

public class RayCastShoot : MonoBehaviour
{

    int munitionValue;
    private bool fired;
    private float firedelay = 0.15f;
    private float shotDistance = 25000f;
    CharacterStats stats;
    PhotonView ph;
    Camera mainCamera;

    void Start()
    {
        ph = PhotonView.Get(this.transform);
        if (ph.isMine)
        {
            mainCamera = this.transform.Find("Camera").GetComponent<Camera>();
            stats = GetComponentInParent<CharacterStats>();
            stats.ammunitionText.text = string.Format("{0} / {1}", stats.munition, stats.restmuni);
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (ph.isMine)
        {
            RayCastFire();
            ActivateTimer();
            Reload();
        }
    }


    void RayCastFire()
    {
        if (!stats.respawnModus)
        {
            if (!fired)
            {
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    fired = true;
                    RaycastHit hit;

                    Ray RayCast = new Ray(this.mainCamera.transform.position, this.mainCamera.transform.forward);
                    Debug.DrawRay(this.transform.position, this.transform.forward, Color.red);
                    if (Physics.Raycast(RayCast,out hit,shotDistance))
                    {
                        if (stats.munition > 0)
                        {
                            stats.munition--;
                            stats.ammunitionText.text = string.Format("{0} / {1}", stats.munition, stats.restmuni);
                            Debug.Log(hit.transform.tag);
                            if (hit.transform.gameObject.tag == "Player")
                            {
                                CharacterStats s = hit.transform.GetComponent<CharacterStats>();
                                if (s != null)
                                {
                                    s.GetComponent<PhotonView>().RPC("GetDamage", PhotonTargets.All, 100);
                                }
                            }
                            ActivateTimer();
                        }
                        else
                        {
                            Debug.Log("Magazin is empty");
                        }
                    }
                }
            }

        }

    }

    void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (stats.restmuni > 0)
            {

                if (stats.munition < 30)
                {
                    munitionValue = (stats.staticMunition - stats.munition);
                    if (munitionValue > stats.restmuni)
                    {
                        munitionValue = stats.restmuni;
                        stats.munition = munitionValue;
                        stats.restmuni = 0;
                        stats.ammunitionText.text = string.Format("{0} / {1}", stats.munition, stats.restmuni);
                        return;
                    }
                    stats.restmuni = stats.restmuni - munitionValue;
                    stats.munition = stats.munition + munitionValue;
                    stats.ammunitionText.text = string.Format("{0} / {1}", stats.munition, stats.restmuni);
                }
            }
            else
            {
                Debug.Log("Weapon Empty");
            }
        }
    }

    void ActivateTimer()
    {
        firedelay -= Time.deltaTime;
        if (firedelay <= 0)
        {
            fired = false;
            firedelay = 0.15f;
        }
    }
}
