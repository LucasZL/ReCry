//
//  RayCastShoot.cs
//  ReCry
//  
//  Created by Kevin Holst on 22.09.2015
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RayCastShoot : MonoBehaviour
{
    int munitionValue;
    AudioSource source;
    private bool fired;
    private float firedelay = 0.15f;
    private float shotDistance = 25000f;
    CharacterStats stats;
    PhotonView ph;
    Camera mainCamera;
    public Text hitplayer;

    void Start()
    {
        ph = PhotonView.Get(this.transform);
        if (ph.isMine)
        {
            mainCamera = this.transform.Find("Camera").GetComponent<Camera>();
            this.source = transform.Find("Bazooka_1").GetComponent<AudioSource>();
            this.source.volume = PlayerPrefs.GetFloat("Volume");
            FindObject<Option>(FindObjectOfType<Canvas>().transform).source = this.source;
            stats = GetComponentInParent<CharacterStats>();
            stats.ammunitionText.text = string.Format("{0} / {1}", stats.munition, stats.restmuni);
            this.hitplayer = GameObject.FindWithTag("HitPlayer").GetComponent<Text>();
            this.hitplayer.text = "";
        }
    }

    static T FindObject<T>(Transform transform) where T : MonoBehaviour
    {
        var c = transform.GetComponent<T>();
        if (c)
            return c;
        foreach (Transform item in transform)
        {
            c = FindObject<T>(item);
            if (c)
                return c;
        }
        return default(T);
    }

    // Update is called once per frame
    void Update()
    {
        if (ph.isMine && !Utility.IsInGame)
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
                    if (Physics.Raycast(RayCast, out hit, shotDistance))
                    {

                        if (stats.munition > 0)
                        {
                            this.source.Play();
                            stats.munition--;
                            stats.ammunitionText.text = string.Format("{0} / {1}", stats.munition, stats.restmuni);
                            Debug.Log(hit.collider.name);
                            if (hit.transform.gameObject.tag == "Player")
                            {
                                CharacterStats s = hit.transform.GetComponent<CharacterStats>();
                                if (s != null)
                                {
                                    s.GetComponent<PhotonView>().RPC("GetDamage", PhotonTargets.All, 100);

                                    StartCoroutine(HitMarker());
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

    IEnumerator HitMarker()
    {
        hitplayer.color = Color.red;
        hitplayer.text = "Player hit";
        yield return new WaitForSeconds(0.1f);
        hitplayer.text = "";
    }
}
