using UnityEngine;
using System.Collections;

public class DestroyBullets : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        Invoke("Destroy", 5f);
    }

    void Destroy()
    {
        this.gameObject.SetActive(false);
    }

}
