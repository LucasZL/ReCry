using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

    public Animator Animator;


    public bool Animate
    {
        get { return Animator.GetBool("IsOpen"); }
        set { Animator.SetBool("IsOpen",value); }
    }


    // Use this for initialization
    void Awake ()
    {
        Animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update()
    {
	
	}
}
