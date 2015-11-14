using UnityEngine;
using System.Collections;

public class CanvasMainMenuScript : MonoBehaviour
{
    public Animator[] animator;
    private GameObject Canvas;
    void Start()
    {
        this.Canvas = GameObject.Find("GameSearchCanvas");
        this.animator = this.Canvas.GetComponentsInChildren<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        StartCoroutine(StartAnimation(0.25f));
    }


    IEnumerator StartAnimation(float delay)
    {
        
        for (int i = 0; i < animator.Length; i++)
        {
            yield return new WaitForSeconds(delay);
            this.animator[i].SetBool("Shown", true);
        }
    }
}
