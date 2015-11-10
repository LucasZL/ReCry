using UnityEngine;
using System.Collections;

public class EXITGAME : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			Application.Quit();
		}
	}
}
