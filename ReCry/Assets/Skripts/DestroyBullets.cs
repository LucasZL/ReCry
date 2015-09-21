using UnityEngine;
using System.Collections;

public class DestroyBullets : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        Invoke("Destroy", 0.5f);
    }

    void Destroy()
    {
        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
        this.gameObject.SetActive(false);
    }

}
