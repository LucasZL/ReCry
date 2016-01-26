using UnityEngine;
using System.Collections;

public class PortPlayer : MonoBehaviour {

    public GameObject Spawn;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.position = new Vector3(Spawn.transform.position.x, Spawn.transform.position.y, Spawn.transform.position.z);
        }
    }
}
