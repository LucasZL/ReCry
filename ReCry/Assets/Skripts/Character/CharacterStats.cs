﻿#pragma warning disable 618
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterStats : MonoBehaviour
{

    //Respawn
    public bool respawnModus = false;




    public float Armor;
    public float Life;
    public int munition = 30;
    public int restmuni = 120;
    public int staticMunition = 30;
    public int team = 1;

    private Text lifeText;
    private Text armorText;
    public Text ammunitionText;
    public Image healthImage;
    public Image armorImage;

    PhotonView ph;
    NetworkManagerRandom nmr;

    // Use this for initialization
    void Start()
    {
        ph = PhotonView.Get(this.transform.gameObject);

        if (ph.isMine)
        {
            this.team = Random.Range(1, 5);
            Color color;
            if (this.team == 1)
            {
                //Team Green
                color = new Color32(100, 221, 23, 1);
                gameObject.GetComponent<Renderer>().material.color = color;
                transform.Find("Bazooka_1").GetComponent<Renderer>().material.color = color;
                transform.Find("Bazooka_2").GetComponent<Renderer>().material.color = color;
            }
            else if (this.team == 2)
            {
                //Team Red
                color = new Color32(244, 67, 54, 1);
                gameObject.GetComponent<Renderer>().material.color = color;
                transform.Find("Bazooka_1").GetComponent<Renderer>().material.color = color;
                transform.Find("Bazooka_2").GetComponent<Renderer>().material.color = color;
            }
            else if (this.team == 3)
            {
                //Team Cyan
                color = new Color32(11, 188, 201, 1);
                gameObject.GetComponent<Renderer>().material.color = color;
                transform.Find("Bazooka_1").GetComponent<Renderer>().material.color = color;
                transform.Find("Bazooka_2").GetComponent<Renderer>().material.color = color;
            }
            else if (this.team == 4)
            {
                //Team DarkBlue
                color = new Color32(26, 35, 126, 1);
                gameObject.GetComponent<Renderer>().material.color = color;
                transform.Find("Bazooka_1").GetComponent<Renderer>().material.color = color;
                transform.Find("Bazooka_2").GetComponent<Renderer>().material.color = color;
            }
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
            nmr = GameObject.Find("MapGeneratorNetwork").GetComponent<NetworkManagerRandom>();
            nmr.SpawnPlayer(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ph.isMine)
        {
            UpdateLifeText();
            UpdateArmorText();
            if (Life <= 0)
            {
                RespawnPlayer();
                //this.nmr.RespawnPlayer(this.gameObject);
            }
            if (respawnModus)
            {
                OnMiniMapClick();
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
    }

    void OnMiniMapClick()
    {
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
                                nmr.RespawnPlayer(this.gameObject,mapIndex);
                                respawnModus = false;
                            }
                        }
                    }
                }
            }
            
        }
    }
}
