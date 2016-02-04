//
//  CharacterStats.cs
//  ReCry
//  
//  Created by Kevin Holst, Lucas Zacharias-Langhans on 30.09.2015
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CharacterStats : MonoBehaviour
{
    public bool respawnModus = false;

    public float Armor;
    public float Life;
    public int munition = 30;
    public int restmuni = 120;
    public int staticMunition = 30;
    public int team;

    private Vector3 startPos;

    private Text lifeText;
    private Text armorText;
    public Text ammunitionText;
    public Image healthImage;
    public Image armorImage;

    private List<GameObject> islands;
    private List<GameObject> teamMember;
    private List<GameObject> otherPlayer;
    private int teamRespawnPoints;
    private int aliveTeamMember;
    private int otherTeamRespawnPoints;
    private int otherAliveTeamMember;

    PhotonView ph;
    MapGenerator nmr;
    Color color;

    void Start()
    {
        ph = PhotonView.Get(this.transform.gameObject);

        if (ph.isMine)
        {
            PhotonNetwork.playerName = Utility.Username;
            this.gameObject.name = Utility.Username;
            GetTeamColor();
            this.Life = 100;
            this.Armor = 100;
            this.lifeText = GameObject.FindWithTag("LifeText").GetComponent<Text>() as Text;
            this.armorText = GameObject.FindWithTag("ArmorText").GetComponent<Text>() as Text;
            this.healthImage = GameObject.FindWithTag("HealthUI").GetComponent<Image>() as Image;
            this.armorImage = GameObject.FindWithTag("ArmorUI").GetComponent<Image>() as Image;
            this.ammunitionText = GameObject.FindWithTag("Ammunition").GetComponent<Text>() as Text;
            this.healthImage.sprite = Resources.Load<Sprite>("Sprites/First_aid");
            this.armorImage.sprite = Resources.Load<Sprite>("Sprites/shield_256");
            this.lifeText.text = this.Life.ToString();
            this.armorText.text = this.Armor.ToString();

            nmr = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();
            this.nmr.SpawnPlayer(this.gameObject);

            this.islands = new List<GameObject>();
            this.islands.AddRange(GameObject.FindGameObjectsWithTag("island_wood"));
            this.islands.AddRange(GameObject.FindGameObjectsWithTag("island_sand"));

            this.teamMember = new List<GameObject>();
            this.otherPlayer = new List<GameObject>();
            this.teamMember.AddRange(GameObject.FindGameObjectsWithTag("Player"));

            foreach (var player in teamMember)
            {
                if (player.GetComponent<CharacterStats>().team != this.team)
                {
                    teamMember.Remove(player);
                    otherPlayer.Add(player);
                }
            }
        }
    }

    void Update()
    {
        if (ph.isMine)
        {
            UpdateLifeText();
            UpdateArmorText();
            if (Life <= 0)
            {
                RespawnPlayer();
            }
            if (respawnModus)
            {
                OnMiniMapClick();
            }

            teamRespawnPoints = 0;
            aliveTeamMember = 0;
            otherTeamRespawnPoints = 0;
            otherAliveTeamMember = 0;

            foreach (var island in islands)
            {
                IslandOwner islandOwner = island.GetComponent<IslandOwner>();
                if(islandOwner.owner == this.team)
                {
                    teamRespawnPoints += islandOwner.respawnTickets;
                }
                else
                {
                    otherTeamRespawnPoints += islandOwner.respawnTickets;
                }
            }

            foreach (var player in teamMember)
            {
                CharacterStats characterStats = player.GetComponent<CharacterStats>();
                if (characterStats.Life != 0)
                {
                    aliveTeamMember += 1;
                }
            }

            foreach (var player in otherPlayer)
            {
                CharacterStats characterStats = player.GetComponent<CharacterStats>();
                if (characterStats.Life != 0)
                {
                    otherAliveTeamMember += 1;
                }
            }

            if (teamRespawnPoints == 0 && aliveTeamMember == 0)
            {
                //Das Team hat verloren
            }
            else if(otherTeamRespawnPoints == 0 && otherAliveTeamMember == 0)
            {
                //Das Team hat gewonnen
            }
        }
    }

    [PunRPC]
    public void GetDamage(int damage)
    {
        if (this.Armor > 0)
        {
            this.Armor -= damage / 30;
        }
        else
        {
            this.Life -= damage;
        }

    }

    void UpdateLifeText()
    {
        if (this.Life <= 0)
        {
            this.Life = 0;
            this.lifeText.text = string.Format("{0}", Life);
        }
        else if (this.Life > 0)
        {
            this.lifeText.text = string.Format("{0}", Life);
        }
    }

    [PunRPC]
    void GetTeamColor()
    {
        if (PhotonNetwork.player.GetTeam() == PunTeams.Team.darkblue)
        {
            this.team = 4;
            ph.RPC("SetDarkBlue", PhotonTargets.AllBuffered, null);
        }
        else if (PhotonNetwork.player.GetTeam() == PunTeams.Team.cyan)
        {
            this.team = 3;
            ph.RPC("SetCyan", PhotonTargets.AllBuffered, null);
        }
        else if (PhotonNetwork.player.GetTeam() == PunTeams.Team.red)
        {
            this.team = 2;
            ph.RPC("SetRed", PhotonTargets.AllBuffered, null);
        }
        else if (PhotonNetwork.player.GetTeam() == PunTeams.Team.green)
        {
            this.team = 1;
            ph.RPC("SetGreen", PhotonTargets.AllBuffered, null);
        }
        Debug.Log(PhotonNetwork.player.GetTeam());
    }

    [PunRPC]
    void SetDarkBlue()
    {
        color = new Color32(26, 35, 126, 1);
        gameObject.GetComponent<Renderer>().material.color = color;
        transform.Find("Bazooka_1").GetComponent<Renderer>().material.color = color;
        transform.Find("Bazooka_2").GetComponent<Renderer>().material.color = color;
    }
    [PunRPC]
    void SetRed()
    {
        color = new Color32(244, 67, 54, 1);
        gameObject.GetComponent<Renderer>().material.color = color;
        transform.Find("Bazooka_1").GetComponent<Renderer>().material.color = color;
        transform.Find("Bazooka_2").GetComponent<Renderer>().material.color = color;
    }

    [PunRPC]
    void SetCyan()
    {
        color = new Color32(11, 188, 201, 1);
        gameObject.GetComponent<Renderer>().material.color = color;
        transform.Find("Bazooka_1").GetComponent<Renderer>().material.color = color;
        transform.Find("Bazooka_2").GetComponent<Renderer>().material.color = color;
    }

    [PunRPC]
    void SetGreen()
    {
        color = new Color32(100, 221, 23, 1);
        gameObject.GetComponent<Renderer>().material.color = color;
        transform.Find("Bazooka_1").GetComponent<Renderer>().material.color = color;
        transform.Find("Bazooka_2").GetComponent<Renderer>().material.color = color;
    }

    void UpdateArmorText()
    {
        if (this.Armor <= 0)
        {
            this.Armor = 0;
            this.armorText.text = string.Format("{0}", Armor);
        }
        else if (this.Life > 0)
        {
            this.armorText.text = string.Format("{0}", Armor);
        }
    }

    void RespawnPlayer()
    {
        respawnModus = true;
        startPos = this.gameObject.transform.position;

        GameObject[] minimap = GameObject.FindGameObjectsWithTag("minimapIsland");
        int i = (minimap.Length + 1) / 2;
        GameObject respawn = minimap[i];

        startPos = new Vector3(respawn.transform.position.x - 300, respawn.transform.position.y, respawn.transform.position.z);

    }

    void OnMiniMapClick()
    {
        this.gameObject.transform.position = startPos;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray RayCast = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            if (Physics.Raycast(RayCast, out hit))
            {
                if (hit.transform.gameObject.tag == "minimapIsland")
                {
                    if (hit.transform.gameObject.GetComponent<MinimapIslandStats>().owner == this.team)
                    {
                        foreach (var island in this.nmr.minimapIslands)
                        {
                            if (island == hit.transform.gameObject)
                            {
                                int mapIndex = this.nmr.minimapIslands.IndexOf(island);
                                nmr.RespawnPlayer(this.gameObject, mapIndex);
                                respawnModus = false;
                            }
                        }
                    }
                }
            }
        }
    }
}
