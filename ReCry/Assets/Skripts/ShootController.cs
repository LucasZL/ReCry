using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ShootController : MonoBehaviour {

    public GameObject AssaultBullet;
    public float BulletSpeed = 30;
    private List<GameObject> AssaultBulletPool;
    private int pooledAmount = 30;
    bool fired = false;

    float firedelay = 1f;
	// Use this for initialization
	void Start ()
    {
        this.AssaultBulletPool = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject bullet = Instantiate(AssaultBullet) as GameObject;
            bullet.SetActive(false);
            AssaultBulletPool.Add(bullet);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        Fire();
        ActivateTimer();
	}

    void Fire()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (!fired)
            {
                foreach (var bullet in AssaultBulletPool)
                {
                    if (!bullet.activeInHierarchy)
                    {
                        fired = true;
                        bullet.SetActive(true);
                        bullet.transform.position = this.transform.position;
                        bullet.transform.rotation = Quaternion.identity;
                        bullet.GetComponent<Rigidbody>().AddForce(-bullet.GetComponent<Rigidbody>().velocity, ForceMode.Impulse);
                        bullet.GetComponent<Rigidbody>().AddForce(this.transform.forward * BulletSpeed, ForceMode.Impulse);
                        ActivateTimer();
                        return;
                    }
                }
            }
            
        }
    }

    void ActivateTimer()
    {
        
        firedelay -= Time.deltaTime;
        if (firedelay <= 0)
        {
            fired = false;
            firedelay = 1f;
        }
    }
}
