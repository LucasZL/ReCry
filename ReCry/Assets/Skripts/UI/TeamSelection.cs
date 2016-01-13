using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class TeamSelection : Photon.MonoBehaviour {

    List<PhotonPlayer> blueteam;
    List<PhotonPlayer> redteam;
    List<PhotonPlayer> cyanteam;
    List<PhotonPlayer> greenteam;
    
    public Text BlueTeam;
    public Text RedTeam;
    public Text CyanTeam;
    public Text GreenTeam;

	// Use this for initialization
	void Start ()
    {
        if (PhotonNetwork.isMasterClient)
        {
            blueteam = new List<PhotonPlayer>();
            redteam = new List<PhotonPlayer>();
            cyanteam = new List<PhotonPlayer>();
            greenteam = new List<PhotonPlayer>();
            Invoke("SendTeams", 1);
        }
        photonView.RPC("CheckStats", PhotonTargets.OthersBuffered);
    }
    void SendTeams()
    {
        photonView.RPC("SendTeamList", PhotonTargets.OthersBuffered, blueteam, cyanteam, redteam, greenteam);
    }
    [PunRPC]
    void SendTeamLists(List<PhotonPlayer> blue, List<PhotonPlayer> cyan, List<PhotonPlayer> red, List<PhotonPlayer> green)
    {
        blueteam = blue;
        cyanteam = cyan;
        redteam = red;
        greenteam = green;
    }

    [PunRPC]
    public void JoinCyanTeam()
    {
        if (cyanteam.Count >= 8)
        {
            
        }
        else
        {
            cyanteam.Add(PhotonNetwork.player);
        }
    }
    [PunRPC]
    public void JoinBlueTeam()
    {
        if (blueteam.Count >= 8)
        {

        }
        else
        {
            blueteam.Add(PhotonNetwork.player);
        }
    }
    [PunRPC]
    public void JoinRedTeam()
    {
        if (redteam.Count >= 8)
        {

        }
        else
        {
            redteam.Add(PhotonNetwork.player);
        }
    }
    [PunRPC]
    public void JoinGreenTeam()
    {
        if (greenteam.Count >= 8)
        {

        }
        else
        {
            greenteam.Add(PhotonNetwork.player);
        }
    }


    [PunRPC]
    void CheckStats()
    {
        BlueTeam.text = string.Format("{0} / 8", blueteam.Count);
        RedTeam.text = string.Format("{0} / 8", redteam.Count);
        CyanTeam.text = string.Format("{0} / 8", cyanteam.Count);
        GreenTeam.text = string.Format("{0} / 8", greenteam.Count);
    }

    public virtual void OnPhotonSerializeView()
    {

    }
}
