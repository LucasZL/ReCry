using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ShootController : MonoBehaviour {

    public GameObject AssaultBullet;
    public float BulletSpeed = 30;
    private List<GameObject> AssaultBulletPool;
    private int pooledAmount = 30;

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
	}

    void Fire()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            foreach (var bullet in AssaultBulletPool)
            {
                if (!bullet.activeInHierarchy)
                {
                    bullet.SetActive(true);
                    bullet.transform.position = this.transform.position;
                    bullet.GetComponent<Rigidbody>().AddForce(this.transform.forward * BulletSpeed, ForceMode.Impulse);
                }
            }
        }
    }
}
