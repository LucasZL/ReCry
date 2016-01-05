using UnityEngine;
using System.Collections;

public class ButtonManager : MonoBehaviour {

    public Menu CurrentMenu;


	// Use this for initialization
	void Start ()
    {
        StartAnimation(CurrentMenu);
	}
	
    public void StartAnimation(Menu menu)
    {
        if (CurrentMenu != null)
        {
            CurrentMenu.Animate = false;
        }
        CurrentMenu = menu;
        CurrentMenu.Animate = true;
    }
}
