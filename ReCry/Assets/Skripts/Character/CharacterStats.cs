using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterStats : MonoBehaviour {

    public float Armor;
    public float Life;
    private Text lifeText;
    private Text armorText;
    public Image healthImage;
    public Image armorImage;

	// Use this for initialization
	void Start ()
    {
        this.Life = 100;
        this.lifeText = GameObject.FindWithTag("LifeText").GetComponent<Text>() as Text;
        this.armorText = GameObject.FindWithTag("ArmorText").GetComponent<Text>() as Text;
        this.healthImage = GameObject.FindWithTag("HealthUI").GetComponent<Image>() as Image;
        this.armorImage = GameObject.FindWithTag("ArmorUI").GetComponent<Image>() as Image;

        this.healthImage.sprite = Resources.Load<Sprite>("Sprites/First_aid");
        this.armorImage.sprite = Resources.Load<Sprite>("Sprites/shield_256");

    }

    // Update is called once per frame
    void Update ()
    {
        UpdateText();
        if (Life <= 0)
        {
            Debug.Log("TOT");
        }
	}


    public void GetDamage()
    {
        this.Life -= 100;
    }

    void UpdateText()
    {
        this.lifeText.text = string.Format("{0}", Life);
    }


}
