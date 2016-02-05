//
//  Trigger.cs
//  ReCry
//  
//  Created by Kevin Holst on 26.01.2016
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Trigger : MonoBehaviour
{
    private string[] triggersay = { "Welcome to the Tutorial Level", "Jump over the Edge, to do a Jump press <W + Space>.", "Well Done!!, you can also do a RunJump by pressing and hold down <Shift + Space>, You may have recognized, that the blue bar in the upper mid of the screen decreases. When the bar hits 0 you can't even jump or use the Jetpack when you fall down you will Spawn at the Start-Point", "The next Island is above you to get there you must use your Jetpack by first pressing <Space> and then holding down the <Shift Key>, when your energy hits Zero and you're to high and smash on the ground you will lost armor","Well, Done! On the right Island you will see enemies you can kill them by pressing the <Left Mouse> Button to Reload press the <R> Button ","Jump to the Last Island","Go into the House","When you go into a House, the island will mark with your Team Color on the Minimap in the Center of the whole map, see the image on the Right Side you will return to the Menu after 10 Seconds"};
    public Text PanelText;
    public GameObject MinimapImage;
    public GameObject panel;

    void Start()
    {
        panel.SetActive(false);
        MinimapImage.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            panel.SetActive(true);
            this.PanelText.text = triggersay[Utility.TutorialID];
            Utility.TutorialID++;
            if (Utility.TutorialID == 8)
            {
                MinimapImage.SetActive(true);
                StartCoroutine(BackToMenu(10));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        panel.SetActive(false);
        Destroy(this.gameObject);
    }

    IEnumerator BackToMenu(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        Application.LoadLevel("Menu");
    }
}
