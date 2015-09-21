using UnityEngine;
using System.Collections;

public class RayCastShoot : MonoBehaviour {

    float maxRange = 80;
    RaycastHit hit;
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void RayCastFire()
    {
        Ray RayCast = new Ray(this.transform.position, Vector3.forward);

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Physics.Raycast(RayCast, out hit, maxRange))
            {
                if (hit.collider.tag == "Player")
                {

                }
            }
        }
    }
}
