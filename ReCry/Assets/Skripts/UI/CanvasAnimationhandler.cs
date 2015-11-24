using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasAnimationhandler : MonoBehaviour {

    private Canvas gameSearchCanvas;
    public Animator[] animate;

	// Use this for initialization
	void Start ()
    {
        this.gameSearchCanvas = GameObject.Find("GameSearchCanvas").GetComponent<Canvas>();
        this.animate = this.gameSearchCanvas.GetComponentsInChildren<Animator>();
	}
	
	// Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            StartCoroutine(PlayAnimations(0.1f));
        }
    }

    IEnumerator PlayAnimations(float time)
    {
        yield return new WaitForSeconds(time);
        for (int i = 0; i < animate.Length; i++)
        {
            yield return new WaitForSeconds(time);
            this.animate[i].SetBool("Shown", true);
        }
    }
}
