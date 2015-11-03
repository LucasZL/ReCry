using UnityEngine;
using System.Collections;

public class FallDownCollider : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<CharacterStats>().Life = 0;
        }
    }
}
