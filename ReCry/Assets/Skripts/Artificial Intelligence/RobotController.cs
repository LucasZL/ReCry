//
//  CharacterMovementMultiplayer.cs
//  ReCry
//  
//  Created by Max Mulert on 20.01.2016
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon;

public class RobotController : Photon.MonoBehaviour
{
    public bool walking = false;
    public int MinDistance = 15;
    public WayPoint[] WayPoints;
    public Vector3 TargetVector;
    public GameObject TargetObject;
    public AudioClip AudioClip;
    GameObject primeEnemy;
    AudioSource source;
    PhotonView pv;
    Transform trans;
    Transform primeEnemyTrans;
    public int speed = 1;
    public int Health = 200;
    int shootingTimer;
    int kitingTimer;
    int kitingDuration = 3000;
    int reloadDuration = 700;
    float shotDistance = 25000f;
    public bool crossedBridge = true;
    public List<GameObject> Enemys;
    public List<GameObject> Allies;

    void Start()
    {
        trans = gameObject.GetComponent<Transform>();
        pv = PhotonView.Get(trans.gameObject);
        this.source = transform.Find("Bazooka_1").GetComponent<AudioSource>();
        this.source.volume = PlayerPrefs.GetFloat("Volume");
    }

    void Update()
    {
        if (CheckForEnemys())
        {
            SearchFightMode();
        }
        else if (walking)
        {
            Walk(TargetVector);

            if (Arrived(0.4f, TargetVector))
            {
                walking = false;
            }
        }
        else
        {
            TargetVector = FindNewTarget();
        }
    }


    void Walk(Vector3 target)
    {
        Vector3 moveDirection = new Vector3();
        moveDirection = GetMoveDirection(target);
        trans.Translate(moveDirection.normalized * Time.deltaTime * speed);
    }

    void SpawnPlayer()
    {
        //PhotonNetwork.Instantiate("Robby",,, 0);
    }

    void SearchFightMode()
    {
        float minDis = 0;
        float actualDis;

        foreach (var Enemy in Enemys)
        {
            actualDis = Vector3.Distance(Enemy.transform.position, trans.position);

            if (actualDis < minDis || minDis == 0)
            {
                primeEnemy = Enemy;
                primeEnemyTrans = primeEnemy.GetComponent<Transform>();
                minDis = actualDis;
            }
        }

        if (minDis < MinDistance)
        {
            Kite();
        }
        else
        {
            Pressure();
        }
    }

    Vector3 FindNewTarget()
    {
        if (crossedBridge)
        {
            crossedBridge = false;
            return FindNewIslandTarget();
        }
        else
        {
            int rnd = Random.Range(0, 2);

            if (rnd == 1)
            {
                if (TargetObject.GetComponent<WayPoint>().bridgeSpwaned == true)
                {
                    crossedBridge = true;
                    WayPoint wp;
                    wp = TargetObject.GetComponent<WayPoint>();
                    TargetObject = wp.otherBridgePoint;
                    return wp.otherBridgePoint.transform.position;
                }
                else
                {
                    return FindNewIslandTarget();
                }
            }
            else
            {
                return FindNewIslandTarget();
            }
        }
    }

    Vector3 FindNewIslandTarget()
    {
        RaycastHit ray;
        Vector3 startPoint = trans.position;

        if (Physics.Raycast(startPoint, Vector3.down, out ray, 5))
        {
            Debug.Log("IslandHit");
            WayPoints = ray.transform.GetComponentsInChildren<WayPoint>();
        }

        Debug.Log("GetMyWaypoint");
        return GetWayPoint();
    }

    Vector3 GetWayPoint()
    {
        int rnd = Random.Range(0, WayPoints.GetLength(0) - 1);
        TargetObject = WayPoints[rnd].gameObject;

        if (TargetObject != null)
        {
            walking = true;
        }
        return WayPoints[rnd].gameObject.transform.position;
    }

    void FindNewDefensiveTarget()
    {
        TargetVector = FindNewIslandTarget();

        if (Vector3.Distance(TargetVector, primeEnemyTrans.position) < Vector3.Distance(trans.position, primeEnemyTrans.position))
        {
            FindNewDefensiveTarget();
        }
    }

    Vector3 GetMoveDirection(Vector3 target)
    {
        Vector3 direction = target - trans.position;
        return direction;
    }


    //I am fucking retarded.....
    bool Arrived()
    {
        if (trans.position.x - 0.3f < TargetVector.x)
        {
            if (trans.position.z - 0.3f < TargetVector.z)
            {
                return true;
            }
            else if (trans.position.z + 0.3f < TargetVector.z)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (trans.position.x + 0.3f < TargetVector.x)
        {
            if (trans.position.z + 0.3f < TargetVector.z)
            {
                return true;
            }
            else if (trans.position.z - 0.3f < TargetVector.z)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    //bool Arrived(float AccDis, Vector3 goalVector)
    //{
    //    if (trans.position.x > goalVector.x - AccDis &&
    //        trans.position.x < goalVector.x + AccDis &&
    //        trans.position.z > goalVector.z - AccDis &&
    //        trans.position.z < goalVector.z + AccDis)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    bool Arrived(float AccDis, Vector3 goalVector)
    {
        if (Vector3.Distance(trans.position, goalVector) <= AccDis)
            return true;
        else
            return false;
    }

    bool CheckForEnemys()
    {
        if (Enemys.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    [PunRPC]
    public void GetDamage(int damage)
    {
        Health -= damage;
        Debug.Log(Health);

        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void Kite()
    {
        Walk(TargetVector);

        if (kitingTimer > kitingDuration)
        {
            walking = false;
            kitingDuration = Random.Range(1500, 4000);
            kitingTimer = 0;

        }
        else
        {
            kitingTimer += (int)(Time.deltaTime * 1000);
            FindNewDefensiveTarget();
        }

        Shoot();
    }

    void Pressure()
    {
        Walk(primeEnemyTrans.position);
        Shoot();
    }


    void Shoot()
    {
        trans.LookAt(primeEnemy.transform);

        if (shootingTimer > reloadDuration)
        {
            //Shoot
            shootingTimer = 0;
            Fire();

        }
        else
        {
            shootingTimer += (int)(Time.deltaTime * 1000);
        }
    }

    void Fire()
    {
        RaycastHit hit;

        //this.source.PlayOneShot(AudioClip, this.source.volume);
        Ray RayCast = new Ray(this.trans.FindChild("Gun").transform.position, trans.FindChild("Gun").up);
        if (Physics.Raycast(RayCast, out hit, shotDistance))
        {
            Debug.Log(hit.collider.name);
            if (hit.transform.gameObject.tag == "Player")
            {
                CharacterStats s = hit.transform.GetComponent<CharacterStats>();
                if (s != null)
                {
                    s.GetComponent<PhotonView>().RPC("GetDamage", PhotonTargets.All, 100);

                }
            }
            if (hit.transform.gameObject.tag == "Agent")
            {
                RobotController rc = hit.transform.GetComponent<RobotController>();
                if (rc != null)
                {
                    rc.GetComponent<PhotonView>().RPC("GetDamage", PhotonTargets.All, 100);

                }
            }
        }
    }
}
