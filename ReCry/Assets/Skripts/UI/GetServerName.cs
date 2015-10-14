using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GetServerName : MonoBehaviour {

    public Text text;

	// Update is called once per frame
	void Update ()
    {
        this.text.text = Utility.ServerName;
    }
}
