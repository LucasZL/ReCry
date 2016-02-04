//
//  WinningConditions.cs
//  ReCry
//  
//  Created by Lucas Zacharias-Langhans on 04.02.2016
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WinningConditions : MonoBehaviour
{
    private List<GameObject> islands;
    private List<GameObject> teamMember;
    private List<GameObject> otherPlayer;
    private int teamRespawnPoints;
    private int aliveTeamMember;
    private int otherTeamRespawnPoints;
    private int otherAliveTeamMember;
    public int team;

    PhotonView ph;
    
    void Start ()
    {
        ph = PhotonView.Get(this.transform.gameObject);

        this.team = this.gameObject.GetComponent<CharacterStats>().team;

        if (ph.isMine)
        {
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
	
	void Update ()
    {
        if (ph.isMine)
        {
            teamRespawnPoints = 0;
            aliveTeamMember = 0;
            otherTeamRespawnPoints = 0;
            otherAliveTeamMember = 0;

            foreach (var island in islands)
            {
                IslandOwner islandOwner = island.GetComponent<IslandOwner>();
                if (islandOwner.owner == this.team)
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
            else if (otherTeamRespawnPoints == 0 && otherAliveTeamMember == 0)
            {
                //Das Team hat gewonnen
            }
        } 
    }
}