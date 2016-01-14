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
        PhotonNetwork.LoadLevel("JoinGame");
    }

    public void JoinBlueTeam()
    {
        PhotonNetwork.player.SetTeam(PunTeams.Team.darkblue);
        PhotonNetwork.LoadLevel("JoinGame");
    }

    public void JoinRedTeam()
    {
        PhotonNetwork.player.SetTeam(PunTeams.Team.red);
        PhotonNetwork.LoadLevel("JoinGame");
    }

    public void JoinGreenTeam()
    {
        PhotonNetwork.player.SetTeam(PunTeams.Team.green);
        PhotonNetwork.LoadLevel("JoinGame");
    }

    public virtual void OnPhotonSerializeView()
    {

    }


}
