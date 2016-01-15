using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class TeamSelection : Photon.MonoBehaviour
{
    public Text BlueTeam;
    public Text RedTeam;
    public Text CyanTeam;
    public Text GreenTeam;
    MapWithoutConnectingtoMaster master;
    public GameObject PanelCamera;
    public GameObject buttonpanel;

    void Start()
    {
        master = GameObject.Find("MapGenerator").GetComponent<MapWithoutConnectingtoMaster>();
    }
    
    void Update()
    {
        foreach (var team in PunTeams.PlayersPerTeam)
        {
           
            switch (team.Key)
            {
                case PunTeams.Team.none:
                    break;
                case PunTeams.Team.red:
                    RedTeam.text = team.Value.Count.ToString();
                    break;
                case PunTeams.Team.darkblue:
                    BlueTeam.text = team.Value.Count.ToString();
                    break;
                case PunTeams.Team.cyan:
                    CyanTeam.text = team.Value.Count.ToString();
                    break;
                case PunTeams.Team.green:
                    GreenTeam.text = team.Value.Count.ToString();
                    break;
                default:
                    break;
            }
        }
    }
    
    public void JoinCyanTeam()
    {
        PhotonNetwork.player.SetTeam(PunTeams.Team.cyan);
        //PanelCamera.SetActive(false);
        buttonpanel.SetActive(false);
        master.playerSpawned = false;
    }

    public void JoinBlueTeam()
    {
        PhotonNetwork.player.SetTeam(PunTeams.Team.darkblue);
        buttonpanel.SetActive(false);
        master.playerSpawned = false;
    }

    public void JoinRedTeam()
    {
        PhotonNetwork.player.SetTeam(PunTeams.Team.red);
        buttonpanel.SetActive(false);
        master.playerSpawned = false;
    }

    public void JoinGreenTeam()
    {
        PhotonNetwork.player.SetTeam(PunTeams.Team.green);
        buttonpanel.SetActive(false);
        master.playerSpawned = false;
    }

    public virtual void OnPhotonSerializeView()
    {

    }


}
