using UnityEngine;
using System.Collections;

public class RefillAmmoAndHealth : Photon.MonoBehaviour {

    public GameObject AmmoObject;
    public GameObject HealthObject;
    private NetworkManagerRandom random;
    private float timer;

	// Use this for initialization
	void Start ()
    {
        this.random = GameObject.Find("MapGeneratorNetwork").GetComponent<NetworkManagerRandom>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void Timer()
    {
        
    }

    void PlacePowerup()
    {

    }
}
