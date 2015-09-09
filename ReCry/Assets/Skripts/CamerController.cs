using UnityEngine;
using System.Collections;

public class CamerController : MonoBehaviour {

    float mouseX;
    float mouseY;

	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
    }
}
