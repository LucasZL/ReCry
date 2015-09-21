using UnityEngine;
using System.Collections;

public class JumpDetection : MonoBehaviour {

    public bool isGrounded;
    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Env")
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision other)
    {
        isGrounded = false;
    }
}
