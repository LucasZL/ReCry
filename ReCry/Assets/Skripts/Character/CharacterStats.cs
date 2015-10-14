#pragma warning disable 618
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterStats : MonoBehaviour
{

    public float Armor;
    public float Life;
    public int munition = 30;
    public int restmuni = 120;
    public int staticMunition = 30;
    private Text lifeText;
    private Text armorText;
    public Text ammunitionText;
    public Image healthImage;
    public Image armorImage;

    PhotonView ph;

    // Use this for initialization
    void Start()
    {
        ph = PhotonView.Get(this.transform.gameObject);

        if (ph.isMine)
        {
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
                Debug.Log("TOT");
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
}
