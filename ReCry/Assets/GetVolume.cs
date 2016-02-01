using UnityEngine;
using System.Collections;

public class GetVolume : MonoBehaviour {
    public AudioSource Audiosource;

	// Use this for initialization
	void Start () {
        Audiosource.volume = PlayerPrefs.GetFloat("Volume");
	}
}
