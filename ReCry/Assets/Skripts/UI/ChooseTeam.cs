using UnityEngine;
using System.Collections;

public class ChooseTeam : MonoBehaviour {

    CharacterStats stats;
    NetworkManagerRandom rnd;
    public GameObject TeamPanel;

    void Start()
    {
                
    }


    public void BlueTeam()
    {
        Utility.Team = 4;

    }

    public void RedTeam()
    {

    }

    public void CyanTeam()
    {

    }
    
    public void GreenTeam()
    {

    }
}
