using UnityEngine;
using System.Collections;

public class MinimapIslandStats : MonoBehaviour
{
    public int owner = 0;
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        Color color;
        if (this.owner == 0)
        {
            color = Color.white;
            color.a = 0.3f;
            gameObject.GetComponent<Renderer>().material.color = color;
        }
        else if (this.owner == 1)
        {
            color = Color.green;
            color.a = 0.3f;
            gameObject.GetComponent<Renderer>().material.color = color;
        }
        else if (this.owner == 2)
        {
            color = Color.red;
            color.a = 0.3f;
            gameObject.GetComponent<Renderer>().material.color = color;
        }
        else if (this.owner == 3)
        {
            color = Color.cyan;
            color.a = 0.3f;
            gameObject.GetComponent<Renderer>().material.color = color;
        }
        else if (this.owner == 4)
        {
            color = Color.yellow;
            color.a = 0.3f;
            gameObject.GetComponent<Renderer>().material.color = color;
        }
    }
}
